using FluentAssertions;
using JobScheduler.Models;

namespace JobScheduler.Tests
{
    public class SortListJobTests
    {
        public static IEnumerable<object[]> GetInput()
        {
            yield return new object[] {
                new List<long> { 8, 7, 6, 5, 4, 3, 2, 1 },
                new List<long> { 1, 2, 3, 4, 5, 6, 7, 8 }
            };
            yield return new object[] {
                new List<long> { 1,2,1,2 },
                new List<long> { 1, 1,2,2 }
            };
            yield return new object[] {
                new List<long> { 1 },
                new List<long> { 1 }
            };
        }

        [Theory]
        [MemberData(nameof(GetInput))]
        public async Task CheckSorting(IReadOnlyCollection<long> input, IReadOnlyCollection<long> output)
        {
            //Arrange
            var job = new SortListJob() { Input = input, Output = output };

            //Act
            var response = await job.Execute(input);

            //Assert
            response.Should().NotBeNull();
            response.SequenceEqual(output).Should().BeTrue();
        }
    }
}