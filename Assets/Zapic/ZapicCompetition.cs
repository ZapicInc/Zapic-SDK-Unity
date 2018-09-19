using System;

/// <summary>
/// Information about a specific competition.
/// </summary>
public sealed class ZapicCompetition
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
    /// The description.
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    /// The developer define metadata.
    /// </summary>
    public string Metadata { get; private set; }

    /// <summary>
    /// Flag indicating if the competition is currently active.
    /// </summary>
    public bool? Active { get; private set; }

    /// <summary>
    /// When did the competition start.
    /// </summary>
    public DateTime? Start { get; private set; }

    /// <summary>
    /// When will/did the competition end.
    /// </summary>
    public DateTime? End { get; private set; }

    /// <summary>
    /// The total number of players in the competition.
    /// </summary>
    public long? TotalUsers { get; private set; }

    /// <summary>
    /// The current player's status
    /// </summary>
    public ZapicCompetitionStatus? Status { get; private set; }

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
    /// The player's rank on the leaderboard (ex. Top 100). If the player
    /// is not on the leaderboard this value will be nil.
    /// </summary>
    public long? LeaderboardRank { get; private set; }

    /// <summary>
    ///  The player's rank in their league. If the player
    ///  is not in a league yet this value will be nil.
    /// </summary>
    public long? LeagueRank { get; private set; }

    public ZapicCompetition(string id,
        string title,
        string description,
        string metadata,
        bool? active,
        DateTime? start,
        DateTime? end,
        long? totalUsers,
        ZapicCompetitionStatus? status,
        string formattedScore,
        double? score,
        long? leaderboardRank,
        long? leagueRank
    )
    {
        Id = id;
        Title = title;
        Description = description;
        Metadata = metadata;
        Active = active;
        Start = start;
        End = end;
        TotalUsers = totalUsers;
        Status = status;
        FormattedScore = formattedScore;
        Score = score;
        LeaderboardRank = leaderboardRank;
        LeagueRank = leagueRank;
    }

}