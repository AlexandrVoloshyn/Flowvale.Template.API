using Flowvale.Template.Application.Common;
using Flowvale.Template.Application.Repositories;
using FluentResults;
using Mediator;
using static Flowvale.Template.Application.Handlers.Books.GetById;

namespace Flowvale.Template.Application.Handlers.Books;

public abstract class List
{
    public record Query(
        int Page, 
        int PageSize, 
        string? Kind, 
        string? Genre, 
        string? Epoch, 
        SortBy SortBy, 
        SortOrder Order)
        : IQuery<Result<PagedResult<BookDto>>>;

    public class Handler(ILibraryRepository libraryRepository) : IQueryHandler<Query, Result<PagedResult<BookDto>>>
    {
        public async ValueTask<Result<PagedResult<BookDto>>> Handle(Query query, CancellationToken cancellationToken)
        {
            var books = await libraryRepository.GetBooksAsync(query.Page, query.PageSize, query.Kind, query.Genre, query.Epoch, query.SortBy, query.Order, cancellationToken);

            return new PagedResult<BookDto>(
                books.Items.Select(b => new BookDto(
                    b.Id,
                    b.Title,
                    b.URL,
                    b.Thumbnail,
                    b.Authors.Select(a => new AuthorDto(a.Id, a.Name)).ToList()
                )).ToList(),
                books.TotalCount,
                query.Page,
                query.PageSize);
        }
    }
}
