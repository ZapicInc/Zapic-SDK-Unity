﻿using System.IO;
using UnityEditor;
using UnityEngine;

class ZapicEditorWindow : EditorWindow
{
	private const string ManifestPath = @"Assets/Zapic/Plugins/Android/ZapicManifest.plugin/AndroidManifest.xml";

	private const string TemplateFile = @"<?xml version = ""1.0"" encoding=""utf-8""?>
<!-- This file was automatically generated by the Zapic SDK for Unity.Do not edit. -->
<manifest xmlns:android=""http://schemas.android.com/apk/res/android""
    package=""com.zapic.sdk.android.unity""
    android:versionCode=""1""
    android:versionName=""1.0"" >

    <uses-sdk android:minSdkVersion=""8"" android:targetSdkVersion=""16"" />

    <application>
        <meta-data android:name=""com.google.android.gms.games.WEB_CLIENT_ID"" android:value=""{0}"" />
    </application>
</manifest>";

	[MenuItem("Window/Zapic/Configure Android...")]
    public static void ShowZapicMenu()
    {
		EditorWindow window = EditorWindow.GetWindow(typeof(ZapicEditorWindow), true, "Zapic");
		window.minSize = new Vector2(400, 250);
    }

    void OnGUI()
    {
        GUI.skin.label.wordWrap = true;
        GUILayout.BeginVertical();

        GUIStyle link = new GUIStyle(GUI.skin.label);
        link.normal.textColor = new Color(0f, 0f, 1f);

        GUILayout.Space(10);

        GUILayout.Label("Zapic uses Google Play Games Services to authenticate users on Android. After importing and configuring the Google Play Games plugin for Unity, select the \"Configure\" button below to import the required settings for Zapic.");

        GUILayout.Space(20);

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();

        if (GUILayout.Button("Configure", GUILayout.Width(100)))
        {
            Configure();
        }

        if (GUILayout.Button("Cancel", GUILayout.Width(100)))
        {
            Close();
        }

        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

		GUILayout.Space(20);

		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();

		if (GUILayout.Button("Open Zapic Documentation", link, GUILayout.ExpandWidth(false)))
		{
			Application.OpenURL("https://www.zapic.com/docs/android");
		}

		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();

		GUILayout.Space(10);

        GUILayout.EndVertical();
    }

	private string GetManifest(string webClientId)
	{
		return string.Format(TemplateFile, webClientId);
	}

    private void Configure()
    {
		var webClientId = GooglePlayGames.GameInfo.WebClientId;
		if (string.IsNullOrEmpty(webClientId)) {
			Debug.LogError("Please import and configure the Google Play Games plugin for Unity.");
			return;
		}

		var manifest = GetManifest(webClientId);
		WriteFile(ManifestPath, manifest);

		EditorUtility.DisplayDialog("Zapic", "Successfully configured Zapic for Android.", "OK");
		Close();
    }

	private static void WriteFile(string path, string body)
	{
		FileInfo fi = new FileInfo(path);
		Directory.CreateDirectory(fi.DirectoryName);

		using(var wr = new StreamWriter(path, false))
		{
			wr.Write(body);
		}
	}
}