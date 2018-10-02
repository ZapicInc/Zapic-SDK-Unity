/// <summary>A Zapic statistic.</summary>
/// <remarks>Added in 1.3.0.</remarks>
public sealed class ZapicStatistic
{
    /// <summary>The current score as a formatted string.</summary>
    private readonly string formattedScore;

    /// <summary>The unique identifier.</summary>
    private readonly string id;

    /// <summary>The custom metadata.</summary>
    private readonly string metadata;

    /// <summary>The percentile of the current rank.</summary>
    private readonly int? percentile;

    /// <summary>The current score.</summary>
    private readonly double? score;

    /// <summary>The title.</summary>
    private readonly string title;

    /// <summary>Initializes a new instance of the <see cref="ZapicStatistic"/> class.</summary>
    /// <param name="id">The unique identifier.</param>
    /// <param name="title">The title.</param>
    /// <param name="metadata">The custom metadata.</param>
    /// <param name="score">The current score.</param>
    /// <param name="formattedScore">The current score as a formatted string.</param>
    /// <param name="percentile">The percentile of the current rank.</param>
    public ZapicStatistic(
        string id,
        string title,
        string metadata,
        double? score,
        string formattedScore,
        int? percentile)
    {
        this.formattedScore = formattedScore;
        this.id = id;
        this.metadata = metadata;
        this.percentile = percentile;
        this.score = score;
        this.title = title;
    }

    /// <summary>
    ///     Gets the current score as a formatted string or <c>null</c> if the player has not submitted an applicable
    ///     event to generate a score.
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
    ///     Gets the percentile of the current score or <c>null</c> if the player has not submitted an applicable event
    ///     to generate a score.
    /// </summary>
    /// <remarks>Added in 1.3.0.</remarks>
    public int? Percentile
    {
        get
        {
            return percentile;
        }
    }

    /// <summary>
    ///     Gets the current score or <c>null</c> if the player has not submitted an applicable event to generate a
    ///     score.
    /// </summary>
    /// <remarks>Added in 1.3.0.</remarks>
    public double? Score
    {
        get
        {
            return score;
        }
    }

    /// <summary>
    ///     <para>Gets the title.</para>
    ///     <para>This is used as a shorter, summary description of the statistic.</para>
    /// </summary>
    /// <remarks>Added in 1.3.0.</remarks>
    public string Title
    {
        get
        {
            return title;
        }
    }
}
