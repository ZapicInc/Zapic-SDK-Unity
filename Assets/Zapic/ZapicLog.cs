#if !UNITY_EDITOR
using ZapicSDK;
#endif

#if !UNITY_EDITOR && UNITY_IOS
using UnityEngine;
#endif

/// <summary>Provides static methods to enable or disable verbose SDK logging.</summary>
/// <remarks>Added in 1.3.0.</remarks>
public static class ZapicLog
{
    /// <summary>Disables verbose SDK logging.</summary>
    /// <remarks>Added in 1.3.0.</remarks>
    public static void Disable()
    {
#if !UNITY_EDITOR && UNITY_ANDROID
        ZapicAndroidLog.Disable();
#elif !UNITY_EDITOR && UNITY_IOS
        Debug.Log("Zapic: SDK logging is not supported on iOS");
#endif
    }

    /// <summary>Enables verbose SDK logging.</summary>
    /// <remarks>Added in 1.3.0.</remarks>
    public static void Enable()
    {
#if !UNITY_EDITOR && UNITY_ANDROID
        ZapicAndroidLog.Enable();
#elif !UNITY_EDITOR && UNITY_IOS
        Debug.Log("Zapic: SDK logging is not supported on iOS");
#endif
    }
}
