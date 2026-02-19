using Flowvale.Template.Application.Common;
using Flowvale.Template.Application.Repositories;
using FluentResults;
using Mediator;
using static Flowvale.Template.Application.Handlers.Books.GetById;

namespace Flowvale.Template.Application.Handlers.Authors;

public abstract class List
{
    public record Query(int Page, int PageSize, SortOrder Order) : IQuery<Result<PagedResult<AuthorDto>>>;

    public class Handler(ILibraryRepository libraryRepository) : IQueryHandler<Query, Result<PagedResult<AuthorDto>>>
    {
        public async ValueTask<Result<PagedResult<AuthorDto>>> Handle(Query query, CancellationToken cancellationToken)
        {
            var authors = await libraryRepository.GetAuthorsAsync(query.Page, query.PageSize, query.Order, cancellationToken);

            return new PagedResult<AuthorDto>(
                authors.Items.Select(x => new AuthorDto(x.Id, x.Name)).ToList(),
                authors.TotalCount,
                query.Page,
                query.PageSize);
        }
    }
}