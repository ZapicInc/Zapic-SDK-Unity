using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using ZapicSDK;

public static class Zapic
{
    [DllImport("__Internal")]
    private static extern void z_connect();

    /**
    * Connect to Zapic
    */
    public static void Connect()
    {
        z_connect();
    }

    public static void ShowMenu()
    {
        Debug.LogError("Not supported yet");
    }

    public static void ShowMenu(Views view)
    {
        Debug.LogError("Not supported yet");
    }
}
