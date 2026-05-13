using System;
using TwentyOz.VivenSDK.Scripts.Editor.Build.VMap;
using TwentyOz.VivenSDK.Scripts.Editor.Core;
using UnityEditor;
using UnityEngine;

namespace TwentyOz.VivenSDK.Scripts.Editor.Build
{
    /// <summary>
    /// VivenSDK의 빌드 설정을 관리하는 ScriptableObject 클래스입니다.
    /// 전역 빌드 설정과 VMap 빌드 설정을 제공합니다.
    /// </summary>
    [CreateAssetMenu(fileName = "BuildSettings", menuName = "VivenSDK/BuildSettings")]
    [Serializable]
    public class VivenBuildSetting : ScriptableObject
    {
        /// SDK 설치 방식(git UPM / Assets 직접 설치)에 따라
        /// VivenSDKPaths.Combine 으로 절대 경로가 결정됩니다.
        private const string BuildSettingsRelativePath =
            "TwentyOz/VivenSDK/Scripts/Editor/Build/Datas/BuildSettings.asset";

        /// <summary>
        /// 플랫폼별 빌드 프로파일 정보를 담고 있는 설정
        /// </summary>
        [SerializeField] public VivenContentBuildProfiles contentBuildProfiles;

        /// <summary>
        /// VMap 빌드 관련 설정
        /// </summary>
        [SerializeField] public VMapBuildSetting vMapBuildSetting;

        /// <summary>
        /// VivenBuildSetting의 싱글톤 인스턴스
        /// </summary>
        private static VivenBuildSetting _instance;

        /// <summary>
        /// 전역 빌드 설정 인스턴스를 가져옵니다.
        /// SDK 설치 방식(git UPM / Assets 직접 설치)과 무관하게 BuildSettings.asset을 탐색합니다.
        /// </summary>
        public static VivenBuildSetting Global
        {
            get
            {
                if (_instance != null) return _instance;

                var path = VivenSDKPaths.Combine(BuildSettingsRelativePath);
                _instance = AssetDatabase.LoadAssetAtPath<VivenBuildSetting>(path);

                if (_instance == null)
                {
                    Debug.LogError(
                        $"[VivenSDK] BuildSettings.asset을 찾을 수 없습니다.\n" +
                        $"탐색 경로: {path}\n" +
                        "SDK 패키지가 정상적으로 설치되었는지 확인하세요.");
                }

                return _instance;
            }
        }

        /// <summary>
        /// VMap 빌드 설정을 가져옵니다. Global이 누락된 경우 null을 반환합니다.
        /// </summary>
        public static VMapBuildSetting VMapBuildSetting => Global != null ? Global.vMapBuildSetting : null;
    }
}