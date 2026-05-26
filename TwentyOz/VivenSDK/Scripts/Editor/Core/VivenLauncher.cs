using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Win32;
using TwentyOz.VivenSDK.Scripts.Core.Common;
using TwentyOz.VivenSDK.Scripts.Editor.Util;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using Application = UnityEngine.Device.Application;
using Debug = UnityEngine.Debug;

namespace TwentyOz.VivenSDK.Scripts.Editor.Core
{
    /// <summary>
    /// Viven 사이트와 통신해 사용자 정보를 가져오거나, Viven 실행파일을 실행하는 클래스
    /// </summary>
    public static class VivenLauncher
    {
        /// <summary>
        /// Viven 실행파일 경로
        /// </summary>
        private static string _vivenPath;
        
        /// <summary>
        /// 로그인 여부
        /// </summary> 
        public static bool IsLogin()
        {
            // user-domain이 기존에 설정되었다면 CurrentDomain 불러오기
            if (EditorPrefs.HasKey("user-domain"))
            {
                // user-domain 이 LoginDomain에 정의된 값인 지 확인
                if (Enum.IsDefined(typeof(LoginDomain), EditorPrefs.GetInt("user-domain")))
                {
                    VivenDomain.SetDomain((LoginDomain)EditorPrefs.GetInt("user-domain"));
                }
            }
            return EditorPrefs.HasKey("user-token") && VivenDomain.CurrentDomain != LoginDomain.None;
        }

        /// <summary>
        /// 로그인된 사용자 정보
        /// </summary>
        private static UserInfo _userInfo;

        private static UnityWebRequest _request;
        public static UserInfo GetUserInfo() => _userInfo;

        
        /// <summary>
        /// 사용자 로그인 시키기
        /// </summary>
        public static void Login()
        {
            // 새로운 윈도우를 열어서 로그인을 하도록 사용자 유도를 함.
            VivenLoginWindow.ShowWindow();
        }

        /// <summary>
        /// 사용자 로그아웃 시키기
        /// </summary>
        public static void Logout()
        {
            var request = VivenAPI.Logout();
            request.SetRequestHeader("Authorization", "Bearer " + EditorPrefs.GetString("user-token"));
            request.SendWebRequest().completed += operation =>
            {
                EditorPrefs.DeleteKey("user-token");
                EditorPrefs.DeleteKey("user-domain");
                _userInfo = default;
            };
        }
        
        /// <summary>
        /// Viven 웹과 연결할 수 있는 지 확인합니다.
        /// </summary>
        /// <returns>로그인 여부</returns>
        public static bool ValidateVivenConnection()
        {
            // Check internet connection
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                Debug.LogError("인터넷 연결이 필요합니다.");
                EditorUtility.DisplayDialog("Error", "인터넷 연결이 필요합니다.", "OK");
                return false;
            }

            if (!IsLogin())
            {
                Debug.LogError("로그인이 필요합니다.");
                EditorUtility.DisplayDialog("Error", "로그인이 필요합니다.", "OK");
                return false;
            }

            return true;
        }
        
        /// <summary>
        /// Viven 프로세스가 실행 중인지 확인
        /// </summary>
        /// <returns>Viven 프로세스가 실행 중이면 true</returns>
        public static bool IsVivenRunning()
        {
            try
            {
#if UNITY_EDITOR_WIN
                // Windows: tasklist 명령어 사용
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = "tasklist",
                    Arguments = "/FI \"IMAGENAME eq Viven.exe\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                };
                
                using (var process = Process.Start(processStartInfo))
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();
                    
                    Debug.Log($"tasklist 결과: {output}");
                    return output.Contains("Viven.exe");
                }
#elif UNITY_EDITOR_OSX
                // macOS: ps 명령어 사용
                var processStartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/ps",
                    Arguments = "ax -o comm",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                };
                
                using (var process = Process.Start(processStartInfo))
                {
                    string output = process.StandardOutput.ReadToEnd();
                    process.WaitForExit();
                    
                    Debug.Log($"ps 결과: {output}");
                    return output.ToLower().Contains("viven");
                }
#else
                // Linux나 다른 플랫폼에서는 Process.GetProcesses() 사용
                var processes = Process.GetProcessesByName("Viven");
                return processes.Length > 0;
#endif
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"Viven 프로세스 확인 중 오류 발생: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Viven 클라이언트가 설치되어 있는지 확인합니다.
        /// </summary>
        public static bool IsVivenInstalled()
        {
            return TryGetVivenExecutablePath(out _);
        }

        /// <summary>
        /// 레지스트리에서 Viven 실행파일 경로를 가져옵니다. Viven이 설치되어 있지 않으면 false를 반환합니다.
        /// </summary>
        public static bool TryGetVivenExecutablePath(out string vivenPath)
        {
            vivenPath = null;
#if UNITY_EDITOR_WIN
            // viven:// URL 프로토콜 핸들러는 Viven 설치 시 등록됨. 키가 없으면 GetValue가 null을 반환한다.
            const string registryPath = @"HKEY_CLASSES_ROOT\viven\shell\open\command";
            var          val          = Registry.GetValue(registryPath, "", null) as string;
            if (string.IsNullOrEmpty(val) || val.Length < 5)
                return false;

            // 레지스트리 값은 `"C:\path\to\Viven.exe" "%1"` 형식이므로 끝의 ` "%1"`(5글자)를 제거하고 따옴표를 벗긴다.
            vivenPath = val.Remove(val.Length - 5, 5).Replace("\"", "");
            return !string.IsNullOrEmpty(vivenPath);
#else
            return false;
#endif
        }

        /// <summary>
        /// Viven 미설치 시 안내 다이얼로그를 띄웁니다.
        /// </summary>
        private static void ShowVivenNotInstalledDialog()
        {
            const string message = "Viven 클라이언트가 설치되어 있지 않습니다.\nhttps://viven.app 에서 다운로드 후 다시 시도해주세요.";
            Debug.LogError(message);
            EditorUtility.DisplayDialog("Viven 미설치", message, "OK");
        }

        /// <summary>
        /// Viven Launcher를 통해 Viven 실행
        /// </summary>
        public static void PlayVivenLocal()
        {
            if (!TryGetVivenExecutablePath(out var vivenPath))
            {
                ShowVivenNotInstalledDialog();
                return;
            }

            var processInfo = new ProcessStartInfo
            {
                Arguments       = $"viven://{VivenDomain.WebURL.GetDomainWebURL()}?d={VivenDomain.DTS.GetDomainDTS()}&t={EditorPrefs.GetString("user-token")}&s=true",
                UseShellExecute = true,
                FileName        = vivenPath,
            };
            Debug.Log(vivenPath);
            
            //Process 처리는 비동기로 진행
            Task.Run(() =>
            {
                try
                {
                    
                    Process.Start(processInfo);
                }
                catch (System.ComponentModel.Win32Exception ex)
                {
                    Debug.Log($"Viven 실행이 취소되었습니다: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Viven 실행 중 오류 발생: {ex.Message}");
                }
            });
        }

        /// <summary>
        /// 로그인 되어있다면 웹에서 User Profile을 가져와 User Info를 업데이트한다.
        /// </summary>
        public static void UpdateUserProfile()
        {
            // 로그인되어있고 인터넷 연결이 되어있다면
            if (IsLogin() && Application.internetReachability != NetworkReachability.NotReachable)
            {
                // 사용자 정보가 없다면 가져오기
                if (!Equals(_userInfo, default(UserInfo)))
                    return;
                
                if(_request != null && !_request.isDone)
                    return;
                // 설정된 User Profile을 가져온다.
                var profileRequest = VivenAPI.GetUserProfile(EditorPrefs.GetString("user-token"));
                _request = profileRequest;

                // 웹에서 User Profile을 가져온다.
                profileRequest.SendWebRequest().completed += _ => GetUserInfo(profileRequest);
            }
        }

        private static void GetUserInfo(UnityWebRequest request)
        {
            if (string.IsNullOrEmpty(request.downloadHandler.text))
            {
                Debug.LogError("User Profile을 가져오는 데 실패했습니다. 로그아웃합니다.");
                EditorPrefs.DeleteKey("user-token");
                VivenEditorUtil.RepaintToolbar();
                return;
            }
            _userInfo = JsonUtility.FromJson<UserInfo>(request.downloadHandler.text);
            VivenEditorUtil.RepaintToolbar();
        }
    }
}