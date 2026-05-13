using UnityEditor;
using UnityEditor.PackageManager;

namespace TwentyOz.VivenSDK.Scripts.Editor.Setting
{
    [InitializeOnLoad]
    public static class PackageInitializer
    {
        static PackageInitializer()
        {
            // 프로젝트가 열리면 한 프레임 뒤에 설정 누락 여부를 검사합니다.
            EditorApplication.delayCall += TryShowSettingsWindowOnStartup;
            // 패키지가 방금 등록된 경우에도 같은 검사 로직을 재사용합니다.
            Events.registeredPackages += OnPackagesRegistered;
        }

        private static void TryShowSettingsWindowOnStartup()
        {
            VivenSDKCustomSettingsWindow.TryShowWindowForMissingSettings();
        }

        private static void OnPackagesRegistered(PackageRegistrationEventArgs args)
        {
            foreach (var package in args.added)
            {
                // SDK 패키지가 추가된 직후에도 누락 설정이 있으면 창을 띄웁니다.
                if (package.name == "com.viven.sdk")
                {
                    VivenSDKCustomSettingsWindow.TryShowWindowForMissingSettings();
                    break;
                }
            }
        }
    }
}
