using System.Collections.Generic;
using System.Linq;
using TwentyOz.VivenSDK.Scripts.Core.Lua;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TwentyOz.VivenSDK.Scripts.Editor.Lua
{
    /// <summary>
    /// .lua 파일(VivenScript)이 재임포트되었을 때,
    /// 해당 스크립트를 참조하는 모든 VivenLuaBehaviour의 InjectedField를 자동으로 갱신합니다.
    /// </summary>
    public class VivenScriptPostprocessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            var importedLuaPaths = new HashSet<string>(
                importedAssets.Where(path => path.EndsWith(".lua")));

            if (importedLuaPaths.Count == 0) return;

            var pathCache = new Dictionary<VivenScript, string>();

            // 열려있는 모든 씬에서 VivenLuaBehaviour를 찾아 갱신
            RefreshBehavioursInLoadedScenes(importedLuaPaths, pathCache);

            // 프리팹 스테이지가 열려 있으면 그 안의 VivenLuaBehaviour도 갱신
            RefreshBehavioursInPrefabStage(importedLuaPaths, pathCache);
        }

        private static void RefreshBehavioursInLoadedScenes(HashSet<string> importedLuaPaths,
            Dictionary<VivenScript, string> pathCache)
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (!scene.isLoaded) continue;

                foreach (var rootGo in scene.GetRootGameObjects())
                {
                    RefreshBehavioursInHierarchy(rootGo, importedLuaPaths, pathCache);
                }
            }
        }

        private static void RefreshBehavioursInPrefabStage(HashSet<string> importedLuaPaths,
            Dictionary<VivenScript, string> pathCache)
        {
#if UNITY_2021_1_OR_NEWER
            var prefabStage = UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();
#else
            var prefabStage = UnityEditor.Experimental.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();
#endif
            if (prefabStage == null) return;

            var prefabRoot = prefabStage.prefabContentsRoot;
            if (prefabRoot != null)
            {
                RefreshBehavioursInHierarchy(prefabRoot, importedLuaPaths, pathCache);
            }
        }

        private static void RefreshBehavioursInHierarchy(GameObject root, HashSet<string> importedLuaPaths,
            Dictionary<VivenScript, string> pathCache)
        {
            var behaviours = root.GetComponentsInChildren<VivenLuaBehaviour>(true);
            foreach (var behaviour in behaviours)
            {
                if (behaviour.luaScript == null) continue;

                if (!pathCache.TryGetValue(behaviour.luaScript, out var scriptPath))
                {
                    scriptPath = AssetDatabase.GetAssetPath(behaviour.luaScript);
                    pathCache[behaviour.luaScript] = scriptPath;
                }

                if (string.IsNullOrEmpty(scriptPath) || !importedLuaPaths.Contains(scriptPath)) continue;

                // 재임포트 후 인스턴스가 교체되었을 수 있으므로 최신 에셋을 로드
                var refreshedScript = AssetDatabase.LoadAssetAtPath<VivenScript>(scriptPath);
                if (refreshedScript == null) continue;

                Undo.RecordObject(behaviour, "Auto Refresh Parsed Injections");
                behaviour.RefreshParsedInjectionInfos(refreshedScript);
                EditorUtility.SetDirty(behaviour);
            }
        }
    }
}
