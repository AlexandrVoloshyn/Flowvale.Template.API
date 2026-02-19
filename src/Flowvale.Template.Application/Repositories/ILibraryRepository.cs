using Flowvale.Template.Application.Common;
using Flowvale.Template.Domain.Models;

namespace Flowvale.Template.Application.Repositories;

public interface ILibraryRepository
{
    Task<Book?> GetBookAsync(string id, CancellationToken cancellationToken);

    Task<(IReadOnlyCollection<Author> Items, int TotalCount)> GetAuthorsAsync(
        int page,
        int pageSize,
        SortOrder sortOrder,
        CancellationToken cancellationToken);

    Task<(IReadOnlyCollection<Book> Items, int TotalCount)> GetBooksAsync(
        int page,
        int pageSize,
        string? kind,
        string? genre,
        string? epoch,
        SortBy sortBy,
        SortOrder sortOrder,
        CancellationToken cancellationToken);

    Task<(IReadOnlyCollection<Book> Items, int TotalCount)> GetBooksByAuthorAsync(
        int page,
        int pageSize,
        string authorId,
        CancellationToken cancellationToken);
}
