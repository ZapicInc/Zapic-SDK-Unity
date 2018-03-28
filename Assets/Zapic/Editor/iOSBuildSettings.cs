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
    public class iOSBuildSettings
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

            Debug.Log("Running Zapic Xcode Scripts");

            string projPath = PBXProject.GetPBXProjectPath(pathToBuiltProject);

            //Find and load the xcode project
            var proj = new PBXProject();
            proj.ReadFromString(File.ReadAllText(projPath));

            //Find the unity target
            string targetGuid = proj.TargetGuidByName(PBXProject.GetUnityTargetName());

            //Force swift to be included
            Debug.Log("Zapic: Including Swift Libraries");
            proj.SetBuildProperty(targetGuid, "ALWAYS_EMBED_SWIFT_STANDARD_LIBRARIES", "YES");

            //Include the frameworks directory in search path to find Zapic.framework
            Debug.Log("Zapic: Setting search path");
            proj.SetBuildProperty(targetGuid, "LD_RUNPATH_SEARCH_PATHS", "$(inherited) @executable_path/Frameworks");

            //Find the id for Zapic.framework fild
            var frameworkId = proj.FindFileGuidByProjectPath("Frameworks/Plugins/iOS/Zapic.framework");

            if (string.IsNullOrEmpty(frameworkId))
            {
                Debug.LogError("Zapic: Unable to find iOS framework");
            }

            //Get or Add a copy files build phase
            Debug.Log("Zapic:Adding embedded framework");

            proj.AddFileToEmbedFrameworks(targetGuid, frameworkId);

            //Stip the framework to the propery build type
            proj.AddStripFatFrameworkScript(targetGuid);

            //Save the project
            proj.WriteToFile(projPath);

            Debug.Log("Zapic:Done configuring xcode settings");
        }

        private static void ConfigureXcodePlist(BuildTarget buildTarget, string pathToBuiltProject)
        {
            if (buildTarget != BuildTarget.iOS)
                return;

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
    }

    internal static class PBXExtensions
    {
        /// <summary>
        /// This is a hack to access the internal method. Replace this one unity makes it public
        /// </summary>
        /// <param name="project"></param>
        /// <param name="targetGuid"></param>
        /// <param name="name"></param>
        /// <param name="shellPath"></param>
        /// <param name="shellScript"></param>
        public static void AppendShellScriptBuildPhase(this PBXProject project, string targetGuid, string name, string shellPath, string shellScript)
        {
            Debug.Log("Zapic: Adding Shell script");

            MethodInfo dynMethod = project.GetType().GetMethod(
                "AppendShellScriptBuildPhase",
                BindingFlags.NonPublic | BindingFlags.Instance,
                null,
                CallingConventions.Any,
                new System.Type[] { typeof(string), typeof(string), typeof(string), typeof(string) },
                null);

            dynMethod.Invoke(project, new object[] { targetGuid, name, shellPath, shellScript });
        }

        public static string ShellScriptByName(this PBXProject project, string targetGuid, string name)
        {
            Debug.Log("Zapic: Getting Shell script");

            MethodInfo dynMethod = project.GetType().GetMethod(
                "ShellScriptByName",
                BindingFlags.NonPublic | BindingFlags.Instance);

            var result = dynMethod.Invoke(project, new object[] { targetGuid, name });

            return result as string;
        }

        public static void AddStripFatFrameworkScript(this PBXProject proj, string targetGuid)
        {
            Debug.Log("Zapic: Adding strip framework");

            const string ScriptName = "Strip Fat Library";

            var scriptId = proj.ShellScriptByName(targetGuid, ScriptName);

            if (string.IsNullOrEmpty(scriptId))
                proj.AppendShellScriptBuildPhase(targetGuid, ScriptName, "/bin/sh", StripArchScript);
        }

        private static readonly string StripArchScript =
            @"
echo ""Target architectures: $ARCHS""

APP_PATH=""${TARGET_BUILD_DIR}/${WRAPPER_NAME}""

find ""$APP_PATH"" -name '*.framework' -type d | while read -r FRAMEWORK
do
FRAMEWORK_EXECUTABLE_NAME=$(defaults read ""$FRAMEWORK/Info.plist"" CFBundleExecutable)
FRAMEWORK_EXECUTABLE_PATH=""$FRAMEWORK/$FRAMEWORK_EXECUTABLE_NAME""
echo ""Executable is $FRAMEWORK_EXECUTABLE_PATH""
echo $(lipo -info ""$FRAMEWORK_EXECUTABLE_PATH"")

FRAMEWORK_TMP_PATH=""$FRAMEWORK_EXECUTABLE_PATH-tmp""

# remove simulator's archs if location is not simulator's directory
case ""${TARGET_BUILD_DIR}"" in
*""iphonesimulator"")
echo ""No need to remove archs""
;;
*)
if $(lipo ""$FRAMEWORK_EXECUTABLE_PATH"" -verify_arch ""i386"") ; then
lipo -output ""$FRAMEWORK_TMP_PATH"" -remove ""i386"" ""$FRAMEWORK_EXECUTABLE_PATH""
echo ""i386 architecture removed""
rm ""$FRAMEWORK_EXECUTABLE_PATH""
mv ""$FRAMEWORK_TMP_PATH"" ""$FRAMEWORK_EXECUTABLE_PATH""
fi
if $(lipo ""$FRAMEWORK_EXECUTABLE_PATH"" -verify_arch ""x86_64"") ; then
lipo -output ""$FRAMEWORK_TMP_PATH"" -remove ""x86_64"" ""$FRAMEWORK_EXECUTABLE_PATH""
echo ""x86_64 architecture removed""
rm ""$FRAMEWORK_EXECUTABLE_PATH""
mv ""$FRAMEWORK_TMP_PATH"" ""$FRAMEWORK_EXECUTABLE_PATH""
fi
;;
esac

echo ""Completed for executable $FRAMEWORK_EXECUTABLE_PATH""
echo $(lipo -info ""$FRAMEWORK_EXECUTABLE_PATH"")

done
            ";

    }
}
#endif
