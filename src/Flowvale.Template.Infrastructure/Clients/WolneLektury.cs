using Flowvale.Template.Infrastructure.Interfaces.Clients;
using System.Net.Http.Json;

namespace Flowvale.Template.Infrastructure.Clients;

// It's better to handle all errors of the client returning Result<T> instead of T, but want to finish the task quick
internal class WolneLektury(HttpClient httpClient) : IWolneLektury
{
    public async Task<IReadOnlyCollection<AuthorDetailedDto>> GetAuthorsAsync(CancellationToken cancellationToken)
    {
        var response = await httpClient.GetAsync("authors", cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            return [];
        }

        var authors = await response.Content.ReadFromJsonAsync<IEnumerable<AuthorDetailedDto>>(cancellationToken: cancellationToken)
                   ?? [];

        return authors.ToList();
    }

    public async Task<BookDetailedDto?> GetBookAsync(string id, CancellationToken cancellationToken)
    {
        var response = await httpClient.GetAsync($"books/{id}/", cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response.Content.ReadFromJsonAsync<BookDetailedDto>(cancellationToken: cancellationToken);
    }
    public async Task<IReadOnlyCollection<BookDto>> GetBooksAsync(CancellationToken cancellationToken)
    {
        var response = await httpClient.GetAsync("books", cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            return Array.Empty<BookDto>();
        }

        var list = await response.Content.ReadFromJsonAsync<IEnumerable<BookDto>>(cancellationToken: cancellationToken)
                   ?? Array.Empty<BookDto>();

        return list.ToList();
    }

    public async Task<IReadOnlyCollection<BookDto>> GetBooksByAuthorAsync(string authorId, CancellationToken cancellationToken)
    {
        var response = await httpClient.GetAsync($"authors/{authorId}/books", cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            return Array.Empty<BookDto>();
        }

        var list = await response.Content.ReadFromJsonAsync<IEnumerable<BookDto>>(cancellationToken: cancellationToken)
                   ?? Array.Empty<BookDto>();
        return list.ToList();
    }
}
