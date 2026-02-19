using FluentResults;

namespace Flowvale.Template.Application.Errors;

public class BadRequestError(string message) : Error(message);