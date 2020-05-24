using System;

/// <summary>
///     Represents the different pages that may be shown using <see cref="Zapic.ShowPage(ZapicPages)"/>.
/// </summary>
/// <remarks>Added in 1.2.0.</remarks>
[Obsolete("Replaced with enum ZapicPage")]
public enum ZapicPages
{
    /// <summary>Identifies a page that shows the list of challenges.</summary>
    /// <remarks>Added in 1.2.0.</remarks>
    Challenges,

    /// <summary>Identifies a page that shows a form to create a new challenge.</summary>
    /// <remarks>Added in 1.2.0.</remarks>
    CreateChallenge,

    /// <summary>Identifies a page that shows the profile.</summary>
    /// <remarks>Added in 1.2.0.</remarks>
    Profile,

    /// <summary>Identifies a page that shows the list of statistics.</summary>
    /// <remarks>Added in 1.2.0.</remarks>
    Stats,
}
