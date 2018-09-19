using System;

/// <summary>
/// Information about a specific Zapic challenge.
/// </summary>
public sealed class ZapicChallenge
{
    /// <summary>
    /// The unique id
    /// </summary>
    public string Id { get; private set; }

    /// <summary>
    /// The title for the challenge
    /// </summary>
    public string Title { get; private set; }

    /// <summary>
    /// Flag indicating if the challenge is currently active.
    /// </summary>
    public bool? Active { get; private set; }

    /// <summary>
    /// The challenge description.
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    /// When did the challenge start.
    /// </summary>
    public DateTime? Start { get; private set; }

    /// <summary>
    /// When will/did the challenge end.
    /// </summary>
    public DateTime? End { get; private set; }

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
    ///The developer defined metadata for this type of challenge.
    /// </summary>
    public string Metdata { get; private set; }

    /// <summary>
    /// The current players's status.
    /// </summary>
    public ZapicChallengeStatus? Status { get; private set; }

    /// <summary>
    /// The current player's rank.
    /// </summary>
    public long? Rank { get; private set; }

    /// <summary>
    /// The total number of players in the challenge.
    /// </summary>
    public long? TotalUsers { get; private set; }

    public ZapicChallenge(string id,
        string title,
        bool? active,
        string description,
        DateTime? start,
        DateTime? end,
        string formattedScore,
        double? score,
        string metadata,
        ZapicChallengeStatus? status,
        long? rank,
        long? totalUsers)
    {
        Id = id;
        Title = title;
        Active = active;
        Description = description;
        Start = start;
        End = end;
        FormattedScore = formattedScore;
        Score = score;
        Metdata = metadata;
        Status = status;
        Rank = rank;
        TotalUsers = totalUsers;

    }
}