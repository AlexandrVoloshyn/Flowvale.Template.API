namespace Flowvale.Template.Infrastructure.Interfaces.Clients;

public record AuthorDetailedDto(
 string name,
 string slug);

public record BookDetailedDto(
string title,
string url,
string simple_thumb,
IReadOnlyList<AuthorDetailedDto> authors);

public record AuthorDto(
 string name);

public record BookDto(
string slug,
string kind,
string epoch,
string genre,
string title,
string url,
string simple_thumb,
string author);

public interface IWolneLektury
{
    Task<BookDetailedDto?> GetBookAsync(string id, CancellationToken cancellationToken);

    Task<IReadOnlyCollection<AuthorDetailedDto>> GetAuthorsAsync(CancellationToken cancellationToken);

    Task<IReadOnlyCollection<BookDto>> GetBooksAsync(CancellationToken cancellationToken);

    Task<IReadOnlyCollection<BookDto>> GetBooksByAuthorAsync(string authorId, CancellationToken cancellationToken);
}
