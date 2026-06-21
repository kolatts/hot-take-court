using System.Security.Cryptography;
using System.Text;

namespace Verdict.Functions.Domain;

public static class SideAssigner
{
    public const string Prosecution = "PROSECUTION";
    public const string Defense     = "DEFENSE";

    /// <summary>
    /// Deterministically assigns a side to a player for a given round, guaranteeing
    /// at least one player on each side. Players are ranked by their individual hashes
    /// and assigned alternately (rank 0 → PROSECUTION, rank 1 → DEFENSE, …), so with
    /// ≥ 2 players both sides are always represented.
    /// </summary>
    public static string Assign(string randSeed, int round, string playerGuid, IEnumerable<string> allPlayerGuids)
    {
        static string HashKey(string seed, int r, string guid) =>
            Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes($"{seed}|{r}|{guid}")));

        var ordered = allPlayerGuids
            .OrderBy(g => HashKey(randSeed, round, g))
            .ToList();

        var rank = ordered.IndexOf(playerGuid);
        return rank % 2 == 0 ? Prosecution : Defense;
    }
}
