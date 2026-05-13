namespace TwentyOz.VivenSDK.Scripts.Editor.Core
{
    /// <summary>
    /// VIVEN SDK가 필요로 하는 외부 Git 패키지 의존성 목록입니다.
    /// VivenSDKCustomSettingWindow와 ExportSdkGitDeploy에서 공유합니다.
    /// </summary>
    public static class VivenSDKPackageDependencies
    {
        public static readonly string[] RequiredGitPackages =
        {
            "https://github.com/lilxyzw/lilToon.git?path=Assets/lilToon#1.7.3",
            "https://github.com/marijnz/unity-toolbar-extender.git",
        };
    }
}
