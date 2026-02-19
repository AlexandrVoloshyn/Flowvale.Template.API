using Flowvale.Template.API.Endpoints;
using Flowvale.Template.API.Middlewares;
using Flowvale.Template.Infrastructure;
using FluentValidation;
using static Flowvale.Template.Application.Handlers.Books.GetById;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

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