using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace TwentyOz.VivenSDK.Scripts.Editor.Build
{
    /// <summary>
    /// 파이프라인 자동화를 위한 batchmode 빌드 진입점입니다.
    /// 커맨드라인 인수를 파싱하여 씬을 열고 Addressables 빌드를 실행합니다.
    /// </summary>
    /// <remarks>
    /// 사용법:
    /// <code>
    /// Unity.exe -batchmode -quit -projectPath "D:/UnityProjects/viven-client-sdk" \
    ///   -executeMethod TwentyOz.VivenSDK.Scripts.Editor.Build.PipelineBuildEntry.BuildForPipeline \
    ///   --scenePath "Assets/AgentTest/map-creation/01-single-rps/RpsGameRoot.unity" \
    ///   [--buildOutputPath "D:/builds/output"]
    /// </code>
    /// </remarks>
    public static class PipelineBuildEntry
    {
        /// <summary>
        /// 파이프라인에서 호출되는 빌드 진입점입니다.
        /// 커맨드라인에서 --scenePath, --buildOutputPath 인수를 파싱합니다.
        /// </summary>
        public static void BuildForPipeline()
        {
            var args = Environment.GetCommandLineArgs();
            var scenePath = GetArgValue(args, "--scenePath");
            var buildOutputPath = GetArgValue(args, "--buildOutputPath");

            Debug.Log($"[PipelineBuildEntry] 빌드 시작");
            Debug.Log($"[PipelineBuildEntry] scenePath: {scenePath}");
            Debug.Log($"[PipelineBuildEntry] buildOutputPath: {buildOutputPath}");

            try
            {
                // 빌드 출력 경로 설정 (지정된 경우)
                if (!string.IsNullOrEmpty(buildOutputPath))
                {
                    VivenBuildManager.CustomBuildOutputPath = buildOutputPath;
                    Debug.Log($"[PipelineBuildEntry] CustomBuildOutputPath 설정: {buildOutputPath}");
                }

                // 씬 열기 (지정된 경우)
                if (!string.IsNullOrEmpty(scenePath))
                {
                    if (!File.Exists(scenePath))
                    {
                        Debug.LogError($"[PipelineBuildEntry] 씬 파일을 찾을 수 없습니다: {scenePath}");
                        EditorApplication.Exit(1);
                        return;
                    }

                    Debug.Log($"[PipelineBuildEntry] 씬 열기: {scenePath}");
                    EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
                }

                // BuildVMapOnLocalTemp 실행
                var result = VivenBuildManager.BuildVMapOnLocalTemp();

                if (result.IsSuccess)
                {
                    var outputPath = VivenBuildManager.GetCurrentBuildOutputPath();
                    Debug.Log($"[PipelineBuildEntry] 빌드 성공! 출력 경로: {outputPath}");
                    Debug.Log($"[PipelineBuildEntry] BUILD_OUTPUT_PATH={outputPath}");
                    EditorApplication.Exit(0);
                }
                else
                {
                    Debug.LogError($"[PipelineBuildEntry] 빌드 실패: {result.BuildMessage}");
                    EditorApplication.Exit(1);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[PipelineBuildEntry] 빌드 중 예외 발생: {e}");
                EditorApplication.Exit(1);
            }
        }

        /// <summary>
        /// 커맨드라인 인수 배열에서 지정된 키의 값을 반환합니다.
        /// </summary>
        /// <param name="args">커맨드라인 인수 배열</param>
        /// <param name="key">찾을 인수 키 (예: "--scenePath")</param>
        /// <returns>인수 값. 없으면 null</returns>
        private static string GetArgValue(string[] args, string key)
        {
            for (var i = 0; i < args.Length - 1; i++)
            {
                if (args[i].Equals(key, StringComparison.OrdinalIgnoreCase))
                {
                    return args[i + 1];
                }
            }

            return null;
        }
    }
}
