using Flowvale.Template.API.Endpoints;
using Flowvale.Template.API.Middlewares;
using Flowvale.Template.Infrastructure;
using FluentValidation;
using System.Text.Json.Nodes;
using static Flowvale.Template.Application.Handlers.Books.GetById;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi(options =>
{
    options.AddSchemaTransformer((schema, context, cancellationToken) =>
    {
        if (context.JsonTypeInfo.Type.IsEnum)
        {
            var names = Enum.GetNames(context.JsonTypeInfo.Type);
            schema.Type = Microsoft.OpenApi.JsonSchemaType.String;
            schema.Enum = [.. names.Select(n => JsonValue.Create(n))];
        }
        return Task.CompletedTask;
    });
});

builder.Services.AddValidatorsFromAssemblyContaining<Query>(); ;
builder.Services.AddMediator(config =>
{
    config.ServiceLifetime = ServiceLifetime.Scoped;
    config.PipelineBehaviors = [typeof(ValidationPipelineBehaviour<,>)];
});

builder.Services.AddHttpClient();

builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "API v1");
    });
}

app.UseHttpsRedirection();

app.MapBooksEndpoints();
app.MapAuthorsEndpoints();

app.Run();