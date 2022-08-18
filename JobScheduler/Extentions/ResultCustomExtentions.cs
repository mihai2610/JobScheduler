using LanguageExt.Common;


namespace JobScheduler.Extentions
{
    /// <summary>
    /// Aditional extentions for Restul package
    /// </summary>
    public static class ResultCustomExtentions
    {
        public static Result<KResponse> MapC<KResponse, TResponse>(this Result<TResponse> request, Func<TResponse, KResponse> func) =>
            request.Match(
                some => func(some),
                none => new Result<KResponse>(none)
                );

        public static async Task<Result<KResponse>> BindT<KResponse, TResponse>(this Task<Result<TResponse>> requestTask, Func<TResponse, Task<Result<KResponse>>> func)
        {
            return await (await requestTask)
                .Match(
                    async response => await func(response),
                    fail => Task.FromResult(new Result<KResponse>(fail))
                );
        }
    }
}
