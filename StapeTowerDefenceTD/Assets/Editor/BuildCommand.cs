using UnityEditor;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEditor.Build.Reporting;

public class BuildScript
{
    public static void BuildiOS()
    {
        string buildPath = Path.Combine(Directory.GetCurrentDirectory(), "iOSBuild");
        Debug.Log("Build path: " + buildPath);

        string[] enabledScenes = EditorBuildSettings.scenes
            .Where(s => s.enabled)
            .Select(s => s.path)
            .ToArray();
        if (enabledScenes.Length == 0)
        {
            Debug.LogError("No enabled scenes found in Build Settings. Build cannot proceed.");
            return;
        }

        if (!Directory.Exists(buildPath))
        {
            Directory.CreateDirectory(buildPath);
            Debug.Log("Created directory: " + buildPath);
        }

        // Build
        BuildPlayerOptions buildOptions = new BuildPlayerOptions
        {
            scenes = enabledScenes,
            locationPathName = buildPath,
            target = BuildTarget.iOS,
            options = BuildOptions.None
        };

        Debug.Log("Starting build with scenes: " + string.Join(", ", enabledScenes));

        BuildReport report = BuildPipeline.BuildPlayer(buildOptions);
        if (report.summary.result != BuildResult.Succeeded)
        {
            Debug.LogError("❌ Build failed: " + report.summary.ToString());
            return;
        }

        Debug.Log("✅ Build succeeded: " + report.summary.ToString());

        // Create xcconfig to enforce Release + disable dev code signing
        string xcconfigPath = Path.Combine(buildPath, "CodemagicRelease.xcconfig");
        File.WriteAllText(xcconfigPath,
@"CODE_SIGN_STYLE = Automatic
DEVELOPMENT_TEAM = " + System.Environment.GetEnvironmentVariable("APP_STORE_TEAM_ID") + @"
PRODUCT_BUNDLE_IDENTIFIER = " + System.Environment.GetEnvironmentVariable("BUNDLE_IDENTIFIER") + @"
CONFIGURATION = Release
");

        Debug.Log("✅ Generated CodemagicRelease.xcconfig");

        // Optionally: copy manually prepared auth.xcconfig (if any)
        string sourceAuth = Path.Combine(Directory.GetCurrentDirectory(), "iOSBuild", "auth.xcconfig");
        string targetAuth = Path.Combine(buildPath, "auth.xcconfig");
        if (File.Exists(sourceAuth))
        {
            File.Copy(sourceAuth, targetAuth, true);
            Debug.Log("📄 Copied custom auth.xcconfig.");
        }
    }
}
