/// <summary>
/// Information about a specific statistic.
/// </summary>
public sealed class ZapicStatistic
{
    /// <summary>
    /// The unique id
    /// </summary>
    public string Id { get; private set; }

    /// <summary>
    /// The title for the stat
    /// </summary>
    public string Title { get; private set; }

    /// <summary>
    /// The current player's score, formatted in accordance
    /// with the developer portal.
    /// </summary>
    public string FormattedScore { get; private set; }

    /// <summary>
    /// The current player's score.
    /// </summary>
    public double? Score { get; private set; }

    /// <summary>
    /// The current player's percentile rank
    /// </summary>
    public double? Percentile { get; private set; }

    /// <summary>
    /// TThe player's rank on the leaderboard (ex. Top 100). If the player
    /// is not on the leaderboard this value will be null.
    /// </summary>
    public long? Rank { get; private set; }

    public ZapicStatistic(string id, string title, string formattedScore, double? score, double? percentile, long? rank)
    {
        Id = id;
        Title = title;
        FormattedScore = formattedScore;
        Score = score;
        Percentile = percentile;
        Rank = rank;
    }
}