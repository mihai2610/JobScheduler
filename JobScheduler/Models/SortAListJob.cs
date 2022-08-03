namespace JobScheduler.Models
{
    /// <inheritdoc/>
    [Serializable]
    public class SortListJob : IJob<IReadOnlyCollection<long>, IReadOnlyCollection<long>>
    {
        public long JobId { get; set; }
        public DateTime StartingTime { get; set; }
        public TimeSpan? Duration { get; set; }
        public JobStatusType Status { get; set; }
        public IReadOnlyCollection<long> Input { get; set; } = default!;
        public IReadOnlyCollection<long>? Output { get; set; } = default!;

        public async Task<IReadOnlyCollection<long>> Execute(IReadOnlyCollection<long> input) =>
           await Task.FromResult(input.OrderBy(x => x).ToList());
    }
}
