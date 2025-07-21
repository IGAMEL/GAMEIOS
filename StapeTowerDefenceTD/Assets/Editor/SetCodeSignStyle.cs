#if UNITY_IOS
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.IO;

public class SetCodeSignStyle
{
    [PostProcessBuild(999)]
    public static void SetAutomaticSigning(BuildTarget buildTarget, string pathToBuiltProject)
    {
        if (buildTarget != BuildTarget.iOS)
            return;

        string projPath = PBXProject.GetPBXProjectPath(pathToBuiltProject);
        PBXProject proj = new PBXProject();
        proj.ReadFromFile(projPath);

        string target = proj.GetUnityMainTargetGuid();
        string frameworkTarget = proj.GetUnityFrameworkTargetGuid();

        proj.SetBuildProperty(target, "CODE_SIGN_STYLE", "Automatic");
        proj.SetBuildProperty(frameworkTarget, "CODE_SIGN_STYLE", "Automatic");

        proj.SetBuildProperty(target, "DEVELOPMENT_TEAM", "YOUR_TEAM_ID");
        proj.SetBuildProperty(frameworkTarget, "DEVELOPMENT_TEAM", "YOUR_TEAM_ID");

        proj.WriteToFile(projPath);
    }
}
#endif
