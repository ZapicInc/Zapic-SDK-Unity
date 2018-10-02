/// <summary>Represents the status of the player in a Zapic competition.</summary>
/// <remarks>Added in 1.3.0.</remarks>
public enum ZapicCompetitionStatus
{
    /// <summary>Identifies when the player has received an invitation to join the competition.</summary>
    /// <remarks>Added in 1.3.0.</remarks>
    Invited = 0,

    /// <summary>Identifies when the player has ignored an invitation to join the competition.</summary>
    /// <remarks>Added in 1.3.0.</remarks>
    Ignored = 1,

    /// <summary>Identifies when the player has accepted an invitation to join the competition.</summary>
    /// <remarks>Added in 1.3.0.</remarks>
    Accepted = 2,
}
