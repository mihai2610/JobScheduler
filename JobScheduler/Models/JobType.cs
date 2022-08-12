using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace JobScheduler.Models;

/// <summary>
/// Possyble job types
/// </summary>
[JsonConverter(typeof(JsonStringEnumMemberConverter))]
public enum JobType
{
    [EnumMember(Value = nameof(SortListOfLong))]
    SortListOfLong
};
