using Azure;
using Azure.Data.Tables;

namespace Verdict.Functions.Domain.Entities;

/// <summary>
/// One entity per (reactor, arg) per round.
/// PartitionKey = roomCode, RowKey = "R{round}-RX-{reactorGuid}-{argAuthorGuid}".
/// Upserted on each tap so a player can change their emoji reaction.
/// </summary>
public class ReactionEntity : ITableEntity
{
    public string PartitionKey { get; set; } = string.Empty; // roomCode
    public string RowKey { get; set; } = string.Empty;       // R{round}-RX-{reactorGuid}-{argAuthorGuid}
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }

    public int Round { get; set; }
    public string ReactorGuid { get; set; } = string.Empty;
    public string ArgAuthorGuid { get; set; } = string.Empty;
    public string Emoji { get; set; } = string.Empty;

    public static string BuildRowKey(int round, string reactorGuid, string argAuthorGuid)
        => $"R{round}-RX-{reactorGuid}-{argAuthorGuid}";
}
