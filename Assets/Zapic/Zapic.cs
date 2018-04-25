using System;
using System.Collections.Generic;
using ZapicSDK;

public static class Zapic
{
    private static readonly IZapicInterface _interface;

    static Zapic()
    {
#if UNITY_EDITOR
        _interface = new ZapicEditorInterface();
#elif UNITY_IOS
        _interface = new ZapiciOSInterface();
#elif UNITY_ANDROID
        _interface = new ZapicAndroidInterface();
#endif
    }

    /// <summary>
    /// Gets or sets the callback invoked after the player has been logged in
    /// </summary>
    /// <remarks>
    /// The player that has been logged in is passed to the callback.
    /// </remarks>
    public static Action<ZapicPlayer> OnLogin
    {
        get
        {
            return _interface.OnLogin;
        }

        set
        {
            _interface.OnLogin = value;
        }
    }

    /// <summary>
    /// Gets or sets the callback invoked after the player has been logged out
    /// </summary>
    /// <remarks>
    /// The player that has been logged out is passed to the callback.
    /// </remarks>
    public static Action<ZapicPlayer> OnLogout
    {
        get
        {
            return _interface.OnLogout;
        }

        set
        {
            _interface.OnLogout = value;
        }
    }

    /// <summary>
    /// Starts zapic. This should be called
    /// as soon as possible during app startup.
    /// </summary>
    public static void Start()
    {
        _interface.Start();
    }

    /// <summary>
    /// Shows the given zapic window
    /// </summary>
    /// <param name="view">View to show.</param>
    public static void Show(Views view)
    {
        _interface.Show(view);
    }

    /// <summary>
    /// Gets the current player.
    /// </summary>
    /// <returns>The player</returns>
    public static ZapicPlayer Player()
    {
        return _interface.Player();
    }

    /// <summary>
    /// Handle Zapic data. Usually from an integration like push notifications.
    /// </summary>
    /// <param name="data">The data.</param>
    public static void HandleData(Dictionary<string, object> data)
    {
        _interface.HandleData(data);
    }

    /// <summary>
    /// Submit a new in-game event to zapic.
    /// </summary>
    /// <param name="param">Collection of parameter names and associate values (numeric, string, bool)</param>
    public static void SubmitEvent(Dictionary<string, object> param)
    {
        _interface.SubmitEvent(param);
    }
}
