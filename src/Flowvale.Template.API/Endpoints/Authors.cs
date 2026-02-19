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

        group.MapGet("/", async (IMediator mediator, CancellationToken cancellationToken, int page = 1, int pageSize = 20, SortOrder order = SortOrder.Ascending) =>
        {
            var result = await mediator.Send(new List.Query(page, pageSize, order), cancellationToken);
            return result.IsSuccess ? Results.Ok(result.Value) : Results.Problem(result.Errors.FirstOrDefault()?.Message);
        });

    }
}
