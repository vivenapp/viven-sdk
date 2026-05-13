using System.IO;

namespace TwentyOz.VivenSDK.Scripts.Editor.Core
{
    /// <summary>
    /// SDK 설치 방식(git UPM / Assets 직접 설치)에 관계없이 패키지 루트 경로를 반환하는 헬퍼 클래스입니다.
    /// </summary>
    public static class VivenSDKPaths
    {
        private const string UpmPackageRoot    = "Packages/com.viven.sdk";
        private const string AssetsPackageRoot = "Assets";

        /// <summary>
        /// UPM(git) 설치 경로가 존재하면 반환하고, 없으면 Assets/ 경로를 반환합니다.
        /// </summary>
        private static string PackageRoot =>
            Directory.Exists(UpmPackageRoot) ? UpmPackageRoot : AssetsPackageRoot;

        /// <summary>
        /// PackageRoot 기준으로 상대 경로를 결합합니다.
        /// </summary>
        public static string Combine(string relativePath) =>
            $"{PackageRoot}/{relativePath}";
    }
}
