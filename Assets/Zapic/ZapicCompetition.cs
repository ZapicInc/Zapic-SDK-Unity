using System;

/// <summary>A Zapic competition.</summary>
/// <remarks>Added in 1.3.0.</remarks>
public sealed class ZapicCompetition
{
    /// <summary>A value indicating whether the competition is active.</summary>
    private readonly bool active;

    /// <summary>The description.</summary>
    private readonly string description;

    /// <summary>The end date and time in UTC.</summary>
    private readonly DateTime end;

    /// <summary>The current score as a formatted string.</summary>
    private readonly string formattedScore;

    /// <summary>The unique identifier.</summary>
    private readonly string id;

    /// <summary>The current leaderboard rank.</summary>
    private readonly long? leaderboardRank;

    /// <summary>The current league rank.</summary>
    private readonly long? leagueRank;

    /// <summary>The custom metadata.</summary>
    private readonly string metadata;

    /// <summary>The current score.</summary>
    private readonly double? score;

    /// <summary>The start date and time in UTC.</summary>
    private readonly DateTime start;

    /// <summary>The current status.</summary>
    private readonly ZapicCompetitionStatus status;

    /// <summary>The title.</summary>
    private readonly string title;

    /// <summary>The total number of users that have accepted the invitation to join the competition.</summary>
    private readonly long totalUsers;

    /// <summary>Initializes a new instance of the <see cref="ZapicCompetition"/> class.</summary>
    /// <param name="id">The unique identifier.</param>
    /// <param name="title">The title.</param>
    /// <param name="description">The description.</param>
    /// <param name="metadata">The custom metadata.</param>
    /// <param name="active">A value indicating whether the competition is active.</param>
    /// <param name="start">The start date and time in UTC.</param>
    /// <param name="end">The end date and time in UTC.</param>
    /// <param name="totalUsers">
    ///     The total number of users that have accepted the invitation to join the competition.
    /// </param>
    /// <param name="status">The current status.</param>
    /// <param name="score">The current score.</param>
    /// <param name="formattedScore">The current score as a formatted string.</param>
    /// <param name="leaderboardRank">The current leaderboard rank.</param>
    /// <param name="leagueRank">The current league rank.</param>
    public ZapicCompetition(
        string id,
        string title,
        string description,
        string metadata,
        bool active,
        DateTime start,
        DateTime end,
        long totalUsers,
        ZapicCompetitionStatus status,
        double? score,
        string formattedScore,
        long? leaderboardRank,
        long? leagueRank)
    {
        this.active = active;
        this.description = description;
        this.end = end;
        this.formattedScore = formattedScore;
        this.id = id;
        this.leaderboardRank = leaderboardRank;
        this.leagueRank = leagueRank;
        this.metadata = metadata;
        this.score = score;
        this.start = start;
        this.status = status;
        this.title = title;
        this.totalUsers = totalUsers;
    }

    /// <summary>
    ///     <para>Gets a value indicating whether the competition is active.</para>
    ///     <para>
    ///         This is <c>true</c> when the current date and time is between the start and end dates and times. This
    ///         value should be used instead of comparing the current date and time to the start and end dates and times
    ///         to mitigate issues with client-server clock skew.
    ///     </para>
    /// </summary>
    /// <remarks>Added in 1.3.0.</remarks>
    public bool Active
    {
        get
        {
            return active;
        }
    }

    /// <summary>
    ///     <para>Gets the description or <c>null</c> if it has not been defined.</para>
    ///     <para>This is used as a longer, detailed description of the competition.</para>
    /// </summary>
    /// <remarks>Added in 1.3.0.</remarks>
    public string Description
    {
        get
        {
            return description;
        }
    }

    /// <summary>Gets the end date and time in UTC.</summary>
    /// <remarks>Added in 1.3.0.</remarks>
    public DateTime End
    {
        get
        {
            return end;
        }
    }

    /// <summary>
    ///     Gets the current score as a formatted string or <c>null</c> if the player has not accepted the invitation to
    ///     join the competition or has not submitted an applicable event to generate a score.
    /// </summary>
    /// <remarks>Added in 1.3.0.</remarks>
    public string FormattedScore
    {
        get
        {
            return formattedScore;
        }
    }

    /// <summary>Gets the unique identifier.</summary>
    /// <remarks>Added in 1.3.0.</remarks>
    public string Id
    {
        get
        {
            return id;
        }
    }

    /// <summary>
    ///     Gets the current leaderboard rank or <c>null</c> if the player has not accepted the invitation to join the
    ///     competition, has not submitted an applicable event to generate a score, or has not been ranked on the
    ///     leaderboard.
    /// </summary>
    /// <remarks>Added in 1.3.0.</remarks>
    public long? LeaderboardRank
    {
        get
        {
            return leaderboardRank;
        }
    }

    /// <summary>
    ///     Gets the current league rank or <c>null</c> if the player has not accepted the invitation to join the
    ///     competition.
    /// </summary>
    /// <remarks>Added in 1.3.0.</remarks>
    public long? LeagueRank
    {
        get
        {
            return leagueRank;
        }
    }

    /// <summary>Gets the custom metadata or <c>null</c> if it has not been defined.</summary>
    /// <remarks>Added in 1.3.0.</remarks>
    public string Metadata
    {
        get
        {
            return metadata;
        }
    }

    /// <summary>
    ///     Gets the current score or <c>null</c> if the player has not accepted the invitation to join the competition
    ///     or has not submitted an applicable event to generate a score.
    /// </summary>
    /// <remarks>Added in 1.3.0.</remarks>
    public double? Score
    {
        get
        {
            return score;
        }
    }

    /// <summary>Gets the start date and time in UTC.</summary>
    /// <remarks>Added in 1.3.0.</remarks>
    public DateTime Start
    {
        get
        {
            return start;
        }
    }

    /// <summary>Gets the current status.</summary>
    /// <remarks>Added in 1.3.0.</remarks>
    public ZapicCompetitionStatus Status
    {
        get
        {
            return status;
        }
    }

    /// <summary>
    ///     <para>Gets the title.</para>
    ///     <para>This is used as a shorter, summary description of the competition.</para>
    /// </summary>
    /// <remarks>Added in 1.3.0.</remarks>
    public string Title
    {
        get
        {
            return title;
        }
    }

    /// <summary>Gets the total number of users that have accepted the invitation to join the competition.</summary>
    /// <remarks>Added in 1.3.0.</remarks>
    public long TotalUsers
    {
        get
        {
            return totalUsers;
        }
    }
}
