using Flowvale.Template.Application.Errors;
using Flowvale.Template.Application.Repositories;
using FluentResults;
using FluentValidation;
using Mediator;

namespace Flowvale.Template.Application.Handlers.Books;

public abstract class GetById
{
    public record Query(string Id) : IQuery<Result<BookDto?>>;

    public record BookDto(string Id, string Title, Uri URL, Uri Thumbnail, IReadOnlyCollection<AuthorDto> Authors);

    public record AuthorDto(string Id, string Name);

    public class Validator : AbstractValidator<Query>
    {
        public Validator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Book ID cannot be empty");
        }
    }

    public class Handler(ILibraryRepository libraryRepository) : IQueryHandler<Query, Result<BookDto?>>
    {
        public async ValueTask<Result<BookDto?>> Handle(Query query, CancellationToken cancellationToken)
        {
            var book = await libraryRepository.GetBookAsync(query.Id, cancellationToken);

            if (book is null)
            {
                return new NotFoundError("Book not found");
            }

            return new BookDto(
                book.Id,
                book.Title,
                book.URL,
                book.Thumbnail,
                book.Authors.Select(a => new AuthorDto(a.Id, a.Name)).ToList());
        }
    }
}
