using Flowvale.Template.Application.Common;
using Flowvale.Template.Application.Repositories;
using FluentResults;
using FluentValidation;
using Mediator;
using static Flowvale.Template.Application.Handlers.Books.GetById;

namespace Flowvale.Template.Application.Handlers.Authors;

public abstract class BooksList
{
    public record Query(int Page, int PageSize, string AuthorId) : IQuery<Result<PagedResult<BookDto>>>;

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.AuthorId)
                .NotEmpty()
                .WithMessage("Author ID cannot be empty")
                .Matches(@"^[A-Za-z0-9-]+$")
                .WithMessage("Author ID may only contain letters, numbers and hyphens");
        }
    }

    public class Handler(ILibraryRepository libraryRepository) : IQueryHandler<Query, Result<PagedResult<BookDto>>>
    {
        public async ValueTask<Result<PagedResult<BookDto>>> Handle(Query query, CancellationToken cancellationToken)
        {
            var books = await libraryRepository.GetBooksByAuthorAsync(query.Page, query.PageSize, query.AuthorId, cancellationToken);
            var result = new PagedResult<BookDto>(
                books.Items.Select(book => 
                    new BookDto(
                        book.Id,
                        book.Title,
                        book.URL,
                        book.Thumbnail,
                        book.Authors
                            .Select(a => new AuthorDto(a.Id, a.Name))
                        .ToList()))
                    .ToList(),
                books.TotalCount,
                query.Page,
                query.PageSize);
            return result;
        }
    }
}
