using FluentResults;

namespace Flowvale.Template.Application.Errors;

public class NotFoundError(string message) : Error(message)
{
    public const string ErrorCode = "404";
}
