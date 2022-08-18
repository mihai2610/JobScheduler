using JobScheduler.Exceptions;
using LanguageExt.Common;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace JobScheduler.Api.Extentions;

internal static class ResponseConverter
{
    public static ActionResult<R> ToResponse<R, T>(this Result<T> response, Func<T, R> onSuccess, ILogger logger) =>
        response.Match(
           success => new OkObjectResult(onSuccess(success)),
           fail =>
           {
               logger.LogError(fail, fail.Message);
               return fail.ExceptionHandler<R>();
           });

    public static ActionResult<T> ToResponse<T>(this Result<T> response, ILogger logger) =>
        response.ToResponse(q => q, logger);

    public static Result<T> ToModelTypeInput<T>(this object input) =>
        new Result<object>(input)
        .Map(q => JsonSerializer.Deserialize<T>((JsonElement)input))
        .Match(
            Some => Some,
            None => new Result<T>(new BadRequestException($"Cannot parse input {input}"))
        );
}
