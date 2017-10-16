using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
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
        throw new NotImplementedException("Android is not currently supported");
#endif
    }

    /// <summary>
    /// Starts zapic. This should be called
    /// as soon as possible during app startup.
    /// </summary>
    /// <param name="version">App version id.</param>
    public static void Start(string version)
    {
        _interface.Start(version);
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
    /// Gets the current players unique id.
    /// </summary>
    /// <returns>The unique id.</returns>
    public static Guid? PlayerId()
    {
        return _interface.PlayerId();
    }

    /// <summary>
    /// Submit a new in-game event to zapic.
    /// </summary>
    /// /// <param name="param">Collection of parameter names and associate values (numeric, string, bool)</param>
    public static void SubmitEvent(Dictionary<string, object> param)
    {
        _interface.SubmitEvent(param);
    }
}