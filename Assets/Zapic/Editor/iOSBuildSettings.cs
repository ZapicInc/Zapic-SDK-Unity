using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.Collections;
using System.Text.RegularExpressions;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif
using System.IO;

public class iOSBuildSettings
{
    [PostProcessBuild]
    public static void OnPostprocessBuild(BuildTarget buildTarget, string pathToBuiltProject)
    {
#if UNITY_IOS
        ConfigureXcodeSettings(buildTarget, pathToBuiltProject);
        ConfigureXcodePlist(buildTarget, pathToBuiltProject);
#endif
    }

#if UNITY_IOS
    private static void ConfigureXcodeSettings(BuildTarget buildTarget, string pathToBuiltProject)
    {
        if (buildTarget == BuildTarget.iOS)
        {
            Debug.Log("Running Zapic Xcode Scripts");

            string projPath = pathToBuiltProject + "/Unity-iPhone.xcodeproj/project.pbxproj";

            var proj = new PBXProject();
            proj.ReadFromString(File.ReadAllText(projPath));

            string targetGuid = proj.TargetGuidByName("Unity-iPhone");

            Debug.Log("Zapic: Including Swift Libraries");
            proj.SetBuildProperty(targetGuid, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "YES");

            Debug.Log("Zapic: Setting search path");
            proj.SetBuildProperty(targetGuid, "LD_RUNPATH_SEARCH_PATHS", "$(inherited) @executable_path/Frameworks");

            //Find the existing framework id
            var frameworkId = proj.FindFileGuidByProjectPath("Frameworks/Plugins/iOS/Zapic.framework");

            //Try lowercase
            if(string.IsNullOrEmpty(frameworkId)){
                frameworkId = proj.FindFileGuidByProjectPath("Frameworks/Plugins/iOS/zapic.framework");
            }

            if (string.IsNullOrEmpty(frameworkId))
            {
                Debug.LogError("Zapic: Unable to find iOS framework");
            }

            Debug.Log("Zapic:Adding embedded frameworks");
            string embedPhase = proj.AddCopyFilesBuildPhase(targetGuid, "Embed Frameworks", "", "10");

            proj.AddFileToBuildSection(targetGuid, embedPhase, frameworkId);

            // Apply settings
            File.WriteAllText(projPath, proj.WriteToString());

            Debug.Log("Zapic:Done configuring xcode settings");
        }
    }

    private static void ConfigureXcodePlist(BuildTarget buildTarget, string pathToBuiltProject)
    {
        Debug.Log("Zapic:Configuring plist");

        // Samlpe of editing Info.plist
        var plistPath = Path.Combine(pathToBuiltProject, "Info.plist");
        var plist = new PlistDocument();
        plist.ReadFromFile(plistPath);

        // Add string setting
        plist.root.SetString("NSContactsUsageDescription", "We'll use your Contacts to find people you know on Zapic.");
        plist.root.SetString("NSPhotoLibraryUsageDescription", "Zapic will only use the Photos you select.");

        // Apply editing settings to Info.plist
        plist.WriteToFile(plistPath);

        Debug.Log("Zapic:Done configuring plist");
    }

#endif
}
