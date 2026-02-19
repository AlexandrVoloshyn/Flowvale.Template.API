using Flowvale.Template.Application.Common;
using Flowvale.Template.Application.Repositories;
using Flowvale.Template.Domain.Models;
using Flowvale.Template.Infrastructure.Interfaces.Clients;

namespace Flowvale.Template.Infrastructure.Repositories;

internal class LibraryRepository(IWolneLektury Client) : ILibraryRepository
{
    public async Task<(IReadOnlyCollection<Author> Items, int TotalCount)> GetAuthorsAsync(int page, int pageSize, SortOrder sortOrder, CancellationToken cancellationToken)
    {
        var authors = await Client.GetAuthorsAsync(cancellationToken);
        var totalCount = authors.Count();

        var filtered = sortOrder == SortOrder.Asc ? authors.OrderBy(a => a.name) : authors.OrderByDescending(a => a.name);

        var result = filtered.Skip((page - 1) * pageSize).Take(pageSize)
            .Select(a => new Author(a.slug, a.name))
            .ToArray();

        return (result, totalCount);
    }

    public async Task<Book?> GetBookAsync(string id, CancellationToken cancellationToken)
    {
        var book = await Client.GetBookAsync(id, cancellationToken);

        if (book is null)
        {
            return null;
        }

        return new Book
        (
            id,
            book.title,
            new Uri(book.url),
            new Uri(book.simple_thumb),
            book.authors.Select(a => new Author(a.slug, a.name)).ToList()
        );
    }

    public async Task<(IReadOnlyCollection<Book> Items, int TotalCount)> GetBooksAsync(int page, int pageSize, string? kind, string? genre, string? epoch, SortBy sortBy, SortOrder sortOrder, CancellationToken cancellationToken)
    {
        var authorsTask = Client.GetAuthorsAsync(cancellationToken);

        var books = await Client.GetBooksAsync(cancellationToken);

        var filtered = books.Where(book =>
            (string.IsNullOrWhiteSpace(kind) || string.Equals(book.kind, kind, StringComparison.InvariantCultureIgnoreCase)) &&
            (string.IsNullOrWhiteSpace(genre) || string.Equals(book.genre, genre, StringComparison.InvariantCultureIgnoreCase)) &&
            (string.IsNullOrWhiteSpace(epoch) || string.Equals(book.epoch, epoch, StringComparison.InvariantCultureIgnoreCase)));

        if (sortBy == SortBy.Title)
        {
            filtered = sortOrder == SortOrder.Asc ? filtered.OrderBy(b => b.title) : filtered.OrderByDescending(b => b.title);
        }
        else
        {
            filtered = sortOrder == SortOrder.Asc
                ? filtered.OrderBy(book => book.author)
                : filtered.OrderByDescending(book => book.author);
        }

        var totalCount = filtered.Count();

        var pageItems = filtered.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        var usedNames = pageItems
            .Select(book => book.author)
            .ToHashSet();

        var allAuthors = await authorsTask;
        var authorsByName = allAuthors
            .Where(a => usedNames.Contains(a.name))
            .ToDictionary(a => a.name, author => new Author(author.slug, author.name));

        var domainItems = pageItems.Select(book => new Book(
            book.slug,
            book.title,
            new(book.url),
            new(book.simple_thumb),
            [authorsByName[book.author]]
        )).ToList();

        return (domainItems, totalCount);
    }

    public async Task<(IReadOnlyCollection<Book> Items, int TotalCount)> GetBooksByAuthorAsync(int page, int pageSize, string authorId, CancellationToken cancellationToken)
    {
        var books = await Client.GetBooksByAuthorAsync(authorId, cancellationToken);

        var totalCount = books.Count();

        var pageItems = books.Skip((page - 1) * pageSize).Take(pageSize).ToList();

        var domainItems = pageItems.Select(book => new Book(
            book.slug,
            book.title,
            new(book.url),
            new(book.simple_thumb),
            [new(authorId, book.author)]
        )).ToList();

        return (domainItems, totalCount);
    }
}
