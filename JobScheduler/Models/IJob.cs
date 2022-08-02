namespace JobScheduler.Models
{
    /// <summary>
    /// Generic model for a job that will performa an action <see cref="Execute"/> 
    /// over an input <typeparam name="TInput"> and will store the output <typeparam name="TOutput"
    /// </summary>
    /// <typeparam name="TInput">Jon input type</typeparam>
    /// <typeparam name="TOutput">Jon output type</typeparam>
    public interface IJob<TInput, TOutput>
    {
        public long JobId { get; set; }
        public DateTime StartingTime { get; set; }
        public TimeSpan? Duration { get; set; }
        public JobStatusType Status { get; set; }
        public TInput Input { get; set; }
        public TOutput? Output { get; set; }
        public abstract Task<TOutput> Execute(TInput input);
    }

}
