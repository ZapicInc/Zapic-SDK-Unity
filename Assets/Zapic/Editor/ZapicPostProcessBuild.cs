#if UNITY_IOS
using System.Collections;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode.Custom;
using UnityEditor.iOS.Xcode.Custom.Extensions;
using UnityEngine;

namespace Zapic
{
    public class ZapicPostProcessBuild
    {
        [PostProcessBuild(999)]
        public static void OnPostprocessBuild(BuildTarget buildTarget, string pathToBuiltProject)
        {
            ConfigureXcodeSettings(buildTarget, pathToBuiltProject);
            ConfigureXcodePlist(buildTarget, pathToBuiltProject);
        }

        private static void ConfigureXcodeSettings(BuildTarget buildTarget, string pathToBuiltProject)
        {
            if (buildTarget != BuildTarget.iOS)
                return;

            Debug.Log("Running Zapic Xcode Build Scripts");

            string projPath = PBXProject.GetPBXProjectPath(pathToBuiltProject);

            //Find and load the xcode project
            var proj = new PBXProject();
            proj.ReadFromString(File.ReadAllText(projPath));

            //Find the unity target
            string targetGuid = proj.TargetGuidByName(PBXProject.GetUnityTargetName());

            //Enable modules so @import can be used to resolve frameworks
            proj.AddBuildProperty(targetGuid, "CLANG_ENABLE_MODULES", "YES");

            Debug.Log("Done with Zapic Xcode Build Scripts");
        }

        private static void ConfigureXcodePlist(BuildTarget buildTarget, string pathToBuiltProject)
        {
            if (buildTarget != BuildTarget.iOS)
                return;

            Debug.Log("Zapic:Configuring plist");

            var plistPath = Path.Combine(pathToBuiltProject, "Info.plist");
            var plist = new PlistDocument();
            plist.ReadFromFile(plistPath);

            // Add string setting
            plist.root.SetString("NSPhotoLibraryUsageDescription", "Zapic will only use the Photos you select.");
            plist.root.SetString("NSCameraUsageDescription", "Zapic allows you to take photos.");

            // Apply editing settings to Info.plist
            plist.WriteToFile(plistPath);

            Debug.Log("Zapic:Done configuring plist");
        }
    }
}
#endif