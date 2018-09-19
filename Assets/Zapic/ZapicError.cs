public sealed class ZapicError
{
    public int ErrorCode { get; private set; }

    public string Message { get; private set; }

    public ZapicError(int code, string message)
    {
        ErrorCode = code;
        Message = string.Format("Zapic Error: [{0}] {1}", code, message);
    }

    public static ZapicError DeserializationError { get { return new ZapicError(2701, "Unable to process response data, please ensure your SDK is up to date."); } }
}