namespace Flowvale.Template.Domain.Models;

public record Book(
    string Id,
    string Title,
    Uri URL,
    Uri Thumbnail,
    IReadOnlyCollection<Author> Authors);
