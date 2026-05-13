using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using TwentyOz.VivenSDK.Scripts.Editor.Build.VAvatar;
using TwentyOz.VivenSDK.Scripts.Editor.Build.VMap;
using TwentyOz.VivenSDK.Scripts.Editor.Core;
using UnityEditor;
using UnityEngine;

namespace TwentyOz.VivenSDK.Scripts.Editor.Build
{
    /// <summary>
    /// Viven 빌드 데이터를 JSON 형식으로 직렬화하는 유틸리티 클래스입니다.
    /// VMap, VObject, VAvatar의 빌드 데이터를 JSON 형식으로 변환하는 기능을 제공합니다.
    /// </summary>
    public static class VivenBuildDataWriter
    {
    
        private const string PackageJsonRelativePath = "TwentyOz/VivenSDK/Scripts/Editor/_Developer/package.json";

        private static string GetSdkVersionFromPackageJson()
        {
            var path = Path.Combine(Application.dataPath, PackageJsonRelativePath);
            if (!File.Exists(path))
                return "0.0.0";
            try
            {
                var json = File.ReadAllText(path);
                var wrapper = JsonUtility.FromJson<PackageVersionWrapper>(json);
                return !string.IsNullOrEmpty(wrapper?.version) ? wrapper.version : "0.0.0";
            }
            catch
            {
                return "0.0.0";
            }
        }

        [Serializable]
        private class PackageVersionWrapper
        {
            public string version;
        }

        private static string GetContentVersionFromBuildData(VivenBuildData buildData)
        {
            if (buildData == null) return "1.0.0";
            return "1.0.0";
        }

        /// <summary>
        /// 맵 빌드 데이터를 JSON 형식으로 변환합니다.
        /// 빌드 시간, 생성자 정보, 맵 이름, 지원 플랫폼 등의 정보를 포함합니다.
        /// </summary>
        /// <param name="buildData">변환할 맵 빌드 데이터</param>
        /// <returns>JSON 형식으로 직렬화된 맵 빌드 데이터</returns>
        public static string WriteMapBuildData(VivenMapBuildData buildData)
        {
            var availablePlatforms = VivenPlatformExtension.Platforms
                .Where(platform => buildData.GetPlatformSceneWrapper(platform).enabled)
                .Select(platform => platform.GetPlatformName())
                .ToArray();

            var assetPath = AssetDatabase.GetAssetPath(buildData);
            var assetGuid = string.IsNullOrEmpty(assetPath) ? "" : AssetDatabase.AssetPathToGUID(assetPath);

            var dataObject = new Dictionary<string, object>
            {
                { "assetGuid", assetGuid },
                { "relativePath", assetPath ?? "" },
                { "dateAndTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss zzz") },
                { "creator_mbrId", VivenLauncher.GetUserInfo().mbrId },
                { "creator_nickname", VivenLauncher.GetUserInfo().nickname },
                { "buildType", buildData.BuildType.ToString() },
                { "mapName", buildData.GetBuildName() },
                { "availablePlatforms", availablePlatforms },
                { "contentVersion", GetContentVersionFromBuildData(buildData) },
                { "SDKVersion", GetSdkVersionFromPackageJson() },
                { "uniqueGuid", Guid.NewGuid().ToString() }
            };

            return JsonConvert.SerializeObject(dataObject);
        }

        /// <summary>
        /// 오브젝트 빌드 데이터를 JSON 형식으로 변환합니다.
        /// 빌드 시간, 생성자 정보, 오브젝트 이름, 지원 플랫폼 등의 정보를 포함합니다.
        /// </summary>
        /// <param name="objectBuildData">변환할 오브젝트 빌드 데이터</param>
        /// <returns>JSON 형식으로 직렬화된 오브젝트 빌드 데이터</returns>
        public static string WriteVObjectBuildData(VivenObjectBuildData objectBuildData)
        {
            var availablePlatforms = VivenPlatformExtension.Platforms
                .Where(platform => objectBuildData.GetPlatformSceneWrapper(platform).enabled)
                .Select(platform => platform.GetPlatformName())
                .ToArray();

            var assetPath = AssetDatabase.GetAssetPath(objectBuildData);
            var assetGuid = string.IsNullOrEmpty(assetPath) ? "" : AssetDatabase.AssetPathToGUID(assetPath);

            var dataObject = new Dictionary<string, object>
            {
                { "assetGuid", assetGuid },
                { "relativePath", assetPath ?? "" },
                { "dateAndTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss zzz") },
                { "creator_mbrId", VivenLauncher.GetUserInfo().mbrId },
                { "creator_nickname", VivenLauncher.GetUserInfo().nickname },
                { "buildType", objectBuildData.BuildType.ToString() },
                { "objectName", objectBuildData.GetBuildName() },
                { "availablePlatforms", availablePlatforms },
                { "contentVersion", GetContentVersionFromBuildData(objectBuildData) },
                { "SDKVersion", GetSdkVersionFromPackageJson() },
                { "uniqueGuid", Guid.NewGuid().ToString() }
            };

            return JsonConvert.SerializeObject(dataObject);
        }

        /// <summary>
        /// 아바타 빌드 데이터를 JSON 형식으로 변환합니다.
        /// 빌드 시간, 생성자 정보, 아바타 이름, 지원 플랫폼 등의 정보를 포함합니다.
        /// </summary>
        /// <param name="avatarBuildData">변환할 아바타 빌드 데이터</param>
        /// <returns>JSON 형식으로 직렬화된 아바타 빌드 데이터</returns>
        public static string WriteVAvatarBuildData(VivenAvatarBuildData avatarBuildData)
        {
            var availablePlatforms = VivenPlatformExtension.Platforms
                .Where(platform => avatarBuildData.GetPlatformSceneWrapper(platform).enabled)
                .Select(platform => platform.GetPlatformName())
                .ToArray();

            var assetPath = AssetDatabase.GetAssetPath(avatarBuildData);
            var assetGuid = string.IsNullOrEmpty(assetPath) ? "" : AssetDatabase.AssetPathToGUID(assetPath);

            var dataObject = new Dictionary<string, object>
            {
                { "assetGuid", assetGuid },
                { "relativePath", assetPath ?? "" },
                { "dateAndTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss zzz") },
                { "creator_mbrId", VivenLauncher.GetUserInfo().mbrId },
                { "creator_nickname", VivenLauncher.GetUserInfo().nickname },
                { "buildType", avatarBuildData.BuildType.ToString() },
                { "avatarName", avatarBuildData.GetBuildName() },
                { "availablePlatforms", availablePlatforms },
                { "contentVersion", GetContentVersionFromBuildData(avatarBuildData) },
                { "SDKVersion", GetSdkVersionFromPackageJson() },
                { "uniqueGuid", Guid.NewGuid().ToString() }
            };

            return JsonConvert.SerializeObject(dataObject);
        }
    }
}