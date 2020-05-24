/// <summary>
///     Provides constant values that identify the different error codes returned in a <see cref="ZapicException"/>.
/// </summary>
/// <remarks>Added in 1.3.0.</remarks>
public static class ZapicErrorCode
{
    /// <summary>Identifies an error code that indicates the Zapic application failed to start.</summary>
    /// <remarks>Added in 1.3.0.</remarks>
    public static readonly int FAILED_TO_START = 2600;

    /// <summary>Identifies an error code that indicates the Zapic application received an invalid message.</summary>
    /// <remarks>Added in 1.3.0.</remarks>
    public static readonly int INVALID_QUERY = 2651;

    /// <summary>Identifies an error code that indicates the Zapic application sent an invalid message.</summary>
    /// <remarks>Added in 1.3.0.</remarks>
    public static readonly int INVALID_RESPONSE = 2601;

    /// <summary>
    ///     Identifies an error code that indicates the Zapic application encountered a message that requires a logged-in
    ///     user.
    /// </summary>
    /// <remarks>Added in 1.3.0.</remarks>
    public static readonly int LOGIN_REQUIRED = 2653;

    /// <summary>
    ///     Identifies an error code that indicates the Zapic application encountered a network-related error (offline,
    ///     timeout, etc).
    /// </summary>
    /// <remarks>Added in 1.3.0.</remarks>
    public static readonly int NETWORK_ERROR = 2652;

    /// <summary>
    ///     Identifies an error code that indicates the version of the Zapic SDK is not supported by the Zapic
    ///     application.
    /// </summary>
    /// <remarks>Added in 1.3.0.</remarks>
    public static readonly int VERSION_NOT_SUPPORTED = 2650;
}
