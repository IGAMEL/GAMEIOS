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
        Debug.Log("Enabled scenes: " + string.Join(", ", enabledScenes));

        if (!Directory.Exists(buildPath))
        {
            try
            {
                Directory.CreateDirectory(buildPath);
                Debug.Log("Created directory: " + buildPath);
            }
            catch (System.Exception e)
            {
                Debug.LogError("Failed to create directory: " + e.Message);
                return;
            }
        }
        else
        {
            Debug.Log("Directory already exists: " + buildPath);
        }

        BuildPlayerOptions buildOptions = new BuildPlayerOptions
        {
            scenes = enabledScenes,
            locationPathName = buildPath,
            target = BuildTarget.iOS,
            options = BuildOptions.None
        };

        Debug.Log("Starting build with options: " + buildOptions.ToString());

        BuildReport report = BuildPipeline.BuildPlayer(buildOptions);
        if (report.summary.result != BuildResult.Succeeded)
        {
            Debug.LogError("Build failed: " + report.summary.ToString());
        }
        else
        {
            Debug.Log("Build succeeded: " + report.summary.ToString());

            string sourceXcconfig = Path.Combine(Directory.GetCurrentDirectory(), "iOSBuild", "auth.xcconfig");
            string targetXcconfig = Path.Combine(buildPath, "auth.xcconfig");

            try
            {
                File.Copy(sourceXcconfig, targetXcconfig, true);
                Debug.Log("Copied auth.xcconfig to build output folder.");
            }
            catch (System.Exception e)
            {
                Debug.LogError("Failed to copy auth.xcconfig: " + e.Message);
            }
        }
    }
}
