using Flowvale.Template.API.Filters;
using Flowvale.Template.Application.Common;
using Flowvale.Template.Application.Handlers.Authors;
using Mediator;

namespace Flowvale.Template.API.Endpoints;

public static class Authors
{
    public static void MapAuthorsEndpoints(this WebApplication app)
    {
        var group = app
        .MapGroup(nameof(Authors))
        .WithTags("Authors")
        .AddEndpointFilter<ResultFilter>();

        group.MapGet("/",
            async (IMediator mediator,
                CancellationToken cancellationToken,
                int page = 1,
                int pageSize = 20,
                SortOrder order = SortOrder.Asc) =>
            mediator.Send(new List.Query(page, pageSize, order), cancellationToken)
            );

        group.MapGet("/{id}/books",
            async (IMediator mediator,
                CancellationToken cancellationToken,
                string id,
                int page = 1,
                int pageSize = 20) =>
            mediator.Send(new BooksList.Query(page, pageSize, id), cancellationToken));
    }
}
