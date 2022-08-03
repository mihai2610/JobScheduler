using JobScheduler.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace JobScheduler.Api.Extentions
{
    internal static class ExceptionsHandler
    {
        public static ActionResult<R> ExceptionHandler<R>(this Exception exception) =>
            exception switch
            {
                BadRequestException => new BadRequestObjectResult(exception.Message),
                ConflictException => new ConflictObjectResult(exception.Message),
                _ => new StatusCodeResult(StatusCodes.Status500InternalServerError)
            };
    }
}
