using System;

/// <summary>An exception thrown by the Zapic SDK.</summary>
/// <remarks>Added in 1.3.0.</remarks>
public sealed class ZapicException : Exception
{
    /// <summary>The error code.</summary>
    private readonly int code;

    /// <summary>Initializes a new instance of the <see cref="ZapicException"/> class.</summary>
    /// <param name="code">The error code.</param>
    public ZapicException(int code)
    {
        this.code = code;
    }

    /// <summary>Initializes a new instance of the <see cref="ZapicException"/> class.</summary>
    /// <param name="code">The error code.</param>
    /// <param name="message">The error message.</param>
    public ZapicException(int code, string message)
        : base(message)
    {
        this.code = code;
    }

    /// <summary>Initializes a new instance of the <see cref="ZapicException"/> class.</summary>
    /// <param name="code">The error code.</param>
    /// <param name="message">The error message.</param>
    /// <param name="innerException">
    ///     The exception that is the cause of the current exception or <c>null</c> if no inner exception is specified.
    /// </param>
    public ZapicException(int code, string message, Exception innerException)
        : base(message, innerException)
    {
        this.code = code;
    }

    /// <summary>Gets the error code (see <see cref="ZapicErrorCode"/> for more information).</summary>
    /// <remarks>Added in 1.3.0.</remarks>
    public int Code
    {
        get
        {
            return code;
        }
    }
}
