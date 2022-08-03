using LanguageExt.Common;
using Microsoft.AspNetCore.Mvc;

namespace JobScheduler.Api.Extentions;

internal static class ResponseConverter
{
    public static ActionResult<R> ToResponse<R, T>(this Result<T> response, Func<T, R> onSuccess) =>
        response.Match(
           success => new OkObjectResult(onSuccess(success)),
           fail => fail.ExceptionHandler<R>()
           );

    public static ActionResult<T> ToResponse<T>(this Result<T> response) =>
        response.ToResponse(q => q);
}
