using Flowvale.Template.API.Filters;
using Flowvale.Template.Application.Common;
using Flowvale.Template.Application.Handlers.Books;
using Mediator;
using static Flowvale.Template.Application.Handlers.Books.GetById;

namespace Flowvale.Template.API.Endpoints;

public static class Books
{
    public static void MapBooksEndpoints(this WebApplication app)
    {
        var group = app
            .MapGroup(nameof(Books))
            .WithTags(nameof(Books))
            .AddEndpointFilter<ResultFilter>();

        group.MapGet("/{id}", (string id, IMediator mediator) => mediator.Send(new GetById.Query(id)))
            .Produces<BookDto>()
            .Produces<DetailedError>(404);

        group.MapGet("/", async (IMediator mediator, CancellationToken cancellationToken,
                int page = 1,
                int pageSize = 20,
                string? kind = null,
                string? genre = null,
                string? epoch = null,
                SortBy sortBy = SortBy.Title,
                SortOrder order = SortOrder.Ascending) =>
        {
            var result = await mediator.Send(
                new List.Query(
                    page,
                    pageSize,
                    kind,
                    genre,
                    epoch,
                    sortBy,
                    order), cancellationToken);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.Problem(result.Errors.FirstOrDefault()?.Message);
        })
            .Produces<PagedResult<BookDto>>();
    }
}
