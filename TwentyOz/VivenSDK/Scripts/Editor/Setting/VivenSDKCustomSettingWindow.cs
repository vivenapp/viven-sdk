using System;
using System.IO;
using System.Linq;
using TwentyOz.VivenSDK.Scripts.Editor.Core;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.Build;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace TwentyOz.VivenSDK.Scripts.Editor.Setting
{
    public class VivenSDKCustomSettingsWindow : EditorWindow
    {
        // SDK 권장 ProjectSettings 복사 대상 파일 목록입니다.
        private static readonly string[] RequiredProjectSettingFiles =
        {
            "GraphicsSettings.asset",
            "QualitySettings.asset",
            "TagManager.asset",
        };

        private const string SettingsTargetPath = "ProjectSettings/";
        private const string ManifestPath = "Packages/manifest.json";
        private const string EnableJsonCatalogSymbol = "ENABLE_JSON_CATALOG";
        private const string RemoteBuildPathName = "Remote.BuildPath";
        private const string RemoteLoadPathName = "Remote.LoadPath";

        private bool _isProjectSettingsApplied;
        private bool _isAddressableSettingsApplied;
        private bool _isPackageDepsApplied;

        private static string PrefKeyProjectSettings =>
            $"VivenSDK.ProjectSettingsApplied.{Application.dataPath.GetHashCode():X}";

        private static string PrefKeyAddressableSettings =>
            $"VivenSDK.AddressableSettingsApplied.{Application.dataPath.GetHashCode():X}";

        private static string PrefKeyPackageDeps =>
            $"VivenSDK.PackageDepsApplied.{Application.dataPath.GetHashCode():X}";

        private AddAndRemoveRequest _addRequest;
        private static bool _isPackageInstallInProgress;
        // 프로젝트 시작 직후와 패키지 등록 직후가 겹쳐도 한 세션에서 한 번만 자동 표시합니다.
        private static bool _hasAutoPopupShownThisSession;

        private static string SettingsSourcePath =>
            VivenSDKPaths.Combine("TwentyOz/Settings/ProjectSettings/");

        public Texture2D logo;

        private GUIStyle _buttonGUIStyle;
        private readonly GUILayoutOption[] _buttonStyle = { GUILayout.Width(350), GUILayout.Height(50) };

        [MenuItem("VIVEN SDK/Settings")]
        public static void ShowWindow()
        {
            var window = GetWindow<VivenSDKCustomSettingsWindow>("VIVEN SDK Settings");
            window.minSize = new Vector2(400, 500);
            window.maxSize = new Vector2(400, 501);
            window.Show();
        }

        [MenuItem("VIVEN SDK/Settings", true)]
        private static bool ValidateShowWindow() => !_isPackageInstallInProgress;

        private void OnEnable()
        {
            logo = AssetDatabase.LoadAssetAtPath<Texture2D>(
                VivenSDKPaths.Combine("TwentyOz/VivenSDK/Logo/Logo_horizontal.png"));

            // 에디터를 다시 열었을 때도 실제 상태를 다시 계산해 버튼 상태를 보여줍니다.
            RefreshAppliedStates();
        }

        private void OnGUI()
        {
            EditorGUILayout.Space(50);

            if (logo != null)
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Label(logo, GUILayout.Width(300), GUILayout.Height(100));
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.Space(50);

            var prevEnabled = GUI.enabled;
            GUI.enabled = prevEnabled && !_isPackageInstallInProgress;

            DrawButton("Package Dependencies 설치", InstallPackageDependencies, _isPackageDepsApplied);
            EditorGUILayout.Space();
            DrawButton("Project Settings 적용", ApplyProjectSettings, _isProjectSettingsApplied);
            EditorGUILayout.Space();
            DrawButton("Addressable Settings 적용", ApplyAddressableSettings, _isAddressableSettingsApplied);
            EditorGUILayout.Space();

            DrawButton("Close", Close);

            GUI.enabled = prevEnabled;
        }

        private void DrawButton(string label, Action action, bool? isApplied = null)
        {
            if (isApplied.HasValue)
            {
                if (isApplied.Value)
                {
                    _buttonGUIStyle = new GUIStyle(GUI.skin.button);
                    _buttonGUIStyle.normal.background = MakeTexture(1, 1, Color.green);
                    _buttonGUIStyle.normal.textColor = Color.black;
                    _buttonGUIStyle.fontStyle = FontStyle.Bold;
                    _buttonGUIStyle.fontSize = 14;
                }
                else
                {
                    _buttonGUIStyle = new GUIStyle(GUI.skin.button);
                    _buttonGUIStyle.fontStyle = FontStyle.Bold;
                    _buttonGUIStyle.fontSize = 14;
                }
            }
            else
            {
                _buttonGUIStyle = new GUIStyle(GUI.skin.button);
                _buttonGUIStyle.fontStyle = FontStyle.Bold;
                _buttonGUIStyle.fontSize = 14;
            }

            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();

            if (GUILayout.Button(label, _buttonGUIStyle, _buttonStyle))
            {
                action();
            }

            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        private Texture2D MakeTexture(int width, int height, Color color)
        {
            var texture = new Texture2D(width, height);
            var pixels = new Color[width * height];
            for (var i = 0; i < pixels.Length; i++)
            {
                pixels[i] = color;
            }

            texture.SetPixels(pixels);
            texture.Apply();
            return texture;
        }

        private void ApplyProjectSettings()
        {
            if (!Directory.Exists(SettingsSourcePath))
            {
                Debug.LogError("Recommended settings not found at " + SettingsSourcePath);
                return;
            }

            foreach (var fileName in RequiredProjectSettingFiles)
            {
                CopySettingsFile(fileName);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            EditorPrefs.SetBool(PrefKeyProjectSettings, true);
            RefreshAppliedStates();
            Debug.Log("Recommended settings applied.");
        }

        private void CopySettingsFile(string fileName)
        {
            var sourceFile = Path.Combine(SettingsSourcePath, fileName);
            var targetFile = Path.Combine(SettingsTargetPath, fileName);

            if (File.Exists(sourceFile))
            {
                File.Copy(sourceFile, targetFile, true);
                Debug.Log($"Copied {fileName} to {SettingsTargetPath}");
            }
            else
            {
                Debug.LogWarning($"{fileName} not found in {SettingsSourcePath}");
            }
        }

        private void ApplyAddressableSettings()
        {
            var settings = AddressableAssetSettingsDefaultObject.GetSettings(true);
            if (settings == null)
            {
                Debug.LogError("Failed to create or find Addressables settings.");
                return;
            }

            settings.EnableJsonCatalog = true;
            settings.BuildRemoteCatalog = true;
            settings.RemoteCatalogBuildPath.SetVariableByName(settings, RemoteBuildPathName);
            settings.RemoteCatalogLoadPath.SetVariableByName(settings, RemoteLoadPathName);
            AddScriptingDefineSymbol(EnableJsonCatalogSymbol);

            EditorUtility.SetDirty(settings);
            AssetDatabase.SaveAssets();

            // 심볼과 Addressables 설정이 모두 반영됐는지 다시 검사합니다.
            RefreshAppliedStates();
            Debug.Log("Addressables settings applied successfully.");
        }

        private void InstallPackageDependencies()
        {
            _isPackageInstallInProgress = true;
            // RequiredGitPackages를 그대로 사용해 필요한 Git 패키지를 설치합니다.
            _addRequest = Client.AddAndRemove(VivenSDKPackageDependencies.RequiredGitPackages, Array.Empty<string>());
            EditorApplication.update += OnAddAndRemoveComplete;
        }

        private void OnAddAndRemoveComplete()
        {
            if (!_addRequest.IsCompleted)
            {
                EditorUtility.DisplayProgressBar("Package Dependencies", "Installing package dependencies...", 0.5f);
                return;
            }

            EditorApplication.update -= OnAddAndRemoveComplete;
            EditorUtility.ClearProgressBar();
            _isPackageInstallInProgress = false;

            if (_addRequest.Status == StatusCode.Success)
            {
                Debug.Log("Package dependencies installed.");
            }
            else
            {
                Debug.LogError($"Package dependencies install failed: {_addRequest.Error?.message}");
            }

            RefreshAppliedStates();
        }

        public static bool HasMissingRequiredSettings()
        {
            // 세 항목 중 하나라도 누락되면 시작 시 설정 창을 다시 띄웁니다.
            return !EvaluateProjectSettingsApplied()
                   || !EvaluateAddressableSettingsApplied()
                   || !EvaluatePackageDependenciesApplied();
        }

        public static void TryShowWindowForMissingSettings()
        {
            if (_hasAutoPopupShownThisSession || _isPackageInstallInProgress)
            {
                return;
            }

            if (!HasMissingRequiredSettings())
            {
                return;
            }

            // 자동 팝업은 이 세션에서 한 번만 수행합니다.
            _hasAutoPopupShownThisSession = true;
            ShowWindow();
        }

        public static bool EvaluateProjectSettingsApplied()
        {
            return EditorPrefs.GetBool(PrefKeyProjectSettings, false);
        }

        public static bool EvaluateAddressableSettingsApplied()
        {
            var settings = AddressableAssetSettingsDefaultObject.GetSettings(false);
            if (settings == null)
            {
                return false;
            }

            // Addressables 옵션, 원격 카탈로그 경로, define symbol이 모두 맞아야 완료입니다.
            return settings.EnableJsonCatalog
                   && settings.BuildRemoteCatalog
                   && settings.RemoteCatalogBuildPath.GetName(settings) == RemoteBuildPathName
                   && settings.RemoteCatalogLoadPath.GetName(settings) == RemoteLoadPathName
                   && HasScriptingDefineSymbol(EnableJsonCatalogSymbol);
        }

        public static bool EvaluatePackageDependenciesApplied()
        {
            if (!File.Exists(ManifestPath))
            {
                return false;
            }

            var manifestContents = File.ReadAllText(ManifestPath);
            // manifest의 dependency 값에 RequiredGitPackages의 Git URL이 모두 있어야 완료입니다.
            return VivenSDKPackageDependencies.RequiredGitPackages.All(packageUrl =>
                manifestContents.Contains(packageUrl, StringComparison.Ordinal));
        }

        private void RefreshAppliedStates()
        {
            // 실제 상태를 다시 계산하고, 기존 EditorPrefs 캐시도 같은 결과로 맞춰 둡니다.
            _isProjectSettingsApplied = EvaluateProjectSettingsApplied();
            _isAddressableSettingsApplied = EvaluateAddressableSettingsApplied();
            _isPackageDepsApplied = EvaluatePackageDependenciesApplied();

            CacheAppliedStates(_isProjectSettingsApplied, _isAddressableSettingsApplied, _isPackageDepsApplied);
        }

        private static void CacheAppliedStates(bool isProjectSettingsApplied, bool isAddressableSettingsApplied,
            bool isPackageDepsApplied)
        {
            EditorPrefs.SetBool(PrefKeyProjectSettings, isProjectSettingsApplied);
            EditorPrefs.SetBool(PrefKeyAddressableSettings, isAddressableSettingsApplied);
            EditorPrefs.SetBool(PrefKeyPackageDeps, isPackageDepsApplied);
        }

        private void AddScriptingDefineSymbol(string symbol)
        {
            var buildTarget = EditorUserBuildSettings.activeBuildTarget;
            var targetGroup = BuildPipeline.GetBuildTargetGroup(buildTarget);
            var namedBuildTarget = NamedBuildTarget.FromBuildTargetGroup(targetGroup);

            PlayerSettings.GetScriptingDefineSymbols(namedBuildTarget, out var symbols);
            if (symbols.Contains(symbol))
            {
                return;
            }

            var newSymbols = symbols.Append(symbol).ToArray();
            PlayerSettings.SetScriptingDefineSymbols(namedBuildTarget, newSymbols);
        }

        private static bool HasScriptingDefineSymbol(string symbol)
        {
            var buildTarget = EditorUserBuildSettings.activeBuildTarget;
            var targetGroup = BuildPipeline.GetBuildTargetGroup(buildTarget);
            var namedBuildTarget = NamedBuildTarget.FromBuildTargetGroup(targetGroup);

            PlayerSettings.GetScriptingDefineSymbols(namedBuildTarget, out var symbols);
            return symbols.Contains(symbol);
        }
    }
}

