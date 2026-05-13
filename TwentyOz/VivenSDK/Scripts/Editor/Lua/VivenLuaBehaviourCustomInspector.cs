using System;
using System.Reflection;
using TwentyOz.VivenSDK.Scripts.Core.Lua;
using UnityEditor;
using UnityEngine;

namespace TwentyOz.VivenSDK.Scripts.Editor.Lua
{
    [CustomEditor(typeof(VivenLuaBehaviour))]
    public class VivenLuaBehaviourCustomInspector : UnityEditor.Editor
    {
        private SerializedProperty _luaScript;
        private SerializedProperty _injection;
        private SerializedProperty _injectionInfoList;
        private UnityEngine.Object _lastLuaScriptObject;
        private UnityEngine.Object _luaScriptBeforePicker;
        private int _luaScriptPickerControlId = -1;
        private bool _isLuaScriptPickerActive;

        private GUIStyle _titleStyle;
        private GUIStyle _subtitleStyle;
        private GUIStyle _scriptLabelStyle;

        private string _cachedMigrationPreview;
        private bool _migrationPreviewDirty = true;

        private void OnEnable()
        {
            _luaScript = serializedObject.FindProperty("luaScript");
            _injection = serializedObject.FindProperty("injection");

            _injectionInfoList = serializedObject.FindProperty("uiInjectionInfoList");
            _lastLuaScriptObject = _luaScript.objectReferenceValue;

            // 역직렬화 후 uiInjectionInfoList ↔ newInjection 간 참조가 끊어져 있으므로
            // 재임포트 없이 메모리상의 VivenScript로 sync만 수행한다.
            var scriptObject = _luaScript.objectReferenceValue as VivenScript;
            if (scriptObject != null)
            {
                var behaviour = (VivenLuaBehaviour)target;
                behaviour.RefreshParsedInjectionInfos(scriptObject);
                serializedObject.Update();
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            var behaviour = serializedObject.targetObject as VivenLuaBehaviour;

            EnsureStyles();
            GUILayout.Label("VIVEN 기본 스크립트", _titleStyle);
            GUILayout.Label("VivenLuaBehaviour는 VivenSDK에서 사용하는 기본 스크립트입니다.", _subtitleStyle);
            GUILayout.Space(10);

            DrawLuaScriptControls();
            HandleLuaScriptObjectPickerEvent();

            GUILayout.Space(5);

            EditorGUI.BeginChangeCheck();

            DrawInjectionInfoList();

            int originalIndent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = originalIndent + 1;
            EditorGUILayout.PropertyField(_injection, new GUIContent("Injection (Legacy)"));
            EditorGUI.indentLevel = originalIndent;

            DrawLegacyMigrationButton(behaviour);

            serializedObject.ApplyModifiedProperties();

            if (EditorGUI.EndChangeCheck())
            {
                behaviour?.ApplyInjections();
            }

        }

        private void DrawLegacyMigrationButton(VivenLuaBehaviour behaviour)
        {
            if (behaviour?.injection == null || _luaScript.objectReferenceValue == null) return;

            if (_migrationPreviewDirty)
            {
                _cachedMigrationPreview = LegacyInjectionMigrator.Preview(behaviour);
                _migrationPreviewDirty = false;
            }

            if (string.IsNullOrEmpty(_cachedMigrationPreview)) return;

            GUILayout.Space(5);
            EditorGUILayout.HelpBox(
                "Legacy Injection에 마이그레이션되지 않은 필드가 있습니다.\n" +
                "아래 버튼으로 Lua 스크립트에 checkInject 코드를 자동 생성할 수 있습니다.",
                MessageType.Info);

            if (GUILayout.Button("Legacy → InjectedField 마이그레이션"))
            {
                // 버튼 클릭 시 최신 preview를 다시 계산
                var freshPreview = LegacyInjectionMigrator.Preview(behaviour);
                if (string.IsNullOrEmpty(freshPreview)) return;

                if (EditorUtility.DisplayDialog(
                        "Legacy Injection 마이그레이션",
                        "다음 코드가 Lua 스크립트에 추가됩니다:\n\n" + freshPreview +
                        "계속하시겠습니까?",
                        "마이그레이션",
                        "취소"))
                {
                    Undo.RecordObject(behaviour, "Migrate Legacy Injection");
                    var count = LegacyInjectionMigrator.Migrate(behaviour);
                    if (count > 0)
                    {
                        RefreshParsedInjectionInfosFromScript();
                        _migrationPreviewDirty = true;
                    }
                }
            }
        }

        private void DrawInjectionInfoList()
        {
            if (_injectionInfoList == null || _luaScript.objectReferenceValue == null)
            {
                EditorGUILayout.HelpBox("Lua Script가 지정되지 않았습니다.", MessageType.Info);
                return;
            }

            string scriptName = _luaScript.objectReferenceValue.name;
            EditorGUILayout.LabelField(scriptName, _scriptLabelStyle);

            GUILayout.Space(5);

            for (int i = 0; i < _injectionInfoList.arraySize; i++)
            {
                var element = _injectionInfoList.GetArrayElementAtIndex(i);
                EditorGUILayout.PropertyField(element, new GUIContent($"Element {i}"), true);
            }
        }

        private void DrawLuaScriptControls()
        {
            bool hasLuaScript = _luaScript.objectReferenceValue != null;

            if (!hasLuaScript)
            {
                GUILayout.BeginHorizontal();

                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(_luaScript, new GUIContent(string.Empty));
                if (EditorGUI.EndChangeCheck())
                {
                    InitializeInjectionInfoFromScript();
                }

                if (GUILayout.Button("VIVEN Script 만들기"))
                {
                    string currentFolderPath = "Assets";

                    // Reflection을 이용하여 ProjectWindowUtil.GetActiveFolderPath() 호출
                    Type projectWindowUtilType = typeof(ProjectWindowUtil);
                    MethodInfo getActiveFolderPath = projectWindowUtilType.GetMethod("GetActiveFolderPath",
                        BindingFlags.Static | BindingFlags.NonPublic);

                    // ProjectWindowUtil.GetActiveFolderPath() 호출 (internal method라서 reflection으로 호출해야 함)
                    if (getActiveFolderPath != null)
                    {
                        object obj = getActiveFolderPath.Invoke(null, null);
                        string pathToCurrentFolder = obj.ToString();
                        currentFolderPath = pathToCurrentFolder;
                    }

                    // 현재 에디터에서 열려있는 폴더 기준으로 Lua 파일 생성 패널 열기
                    var fileName = EditorUtility.SaveFilePanel("Create Lua File", currentFolderPath, "NewLuaFile", "lua");
                    if (string.IsNullOrEmpty(fileName))
                    {
                        GUILayout.EndHorizontal();
                        return;
                    }

                    using (var luaFile = new System.IO.StreamWriter(fileName))
                    {
                        luaFile.WriteLine("");
                    }
                    AssetDatabase.Refresh();
                    EditorApplication.ExecuteMenuItem("Assets/Refresh");

                    var assetPath = fileName.Substring(fileName.IndexOf("Assets", StringComparison.Ordinal));
                    var vivenScriptable = AssetDatabase.LoadAssetAtPath<VivenScript>(assetPath);
                    _luaScript.objectReferenceValue = vivenScriptable;
                    InitializeInjectionInfoFromScript();
                }

                GUILayout.EndHorizontal();
                return;
            }

            GUILayout.BeginHorizontal();
            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUILayout.PropertyField(_luaScript, new GUIContent(string.Empty));
            }

            if (GUILayout.Button("Script변경"))
            {
                if (ConfirmLuaScriptChange())
                {
                    _luaScriptBeforePicker = _luaScript.objectReferenceValue;
                    _luaScriptPickerControlId = GUIUtility.GetControlID(FocusType.Passive);
                    _isLuaScriptPickerActive = true;
                    EditorGUIUtility.ShowObjectPicker<VivenScript>(
                        _luaScript.objectReferenceValue as VivenScript,
                        false,
                        string.Empty,
                        _luaScriptPickerControlId);
                }
            }

            if (GUILayout.Button("Refresh"))
            {
                RefreshParsedInjectionInfosFromScript();
            }

            GUILayout.EndHorizontal();
        }

        private void HandleLuaScriptObjectPickerEvent()
        {
            if (!_isLuaScriptPickerActive)
            {
                return;
            }

            var currentEvent = Event.current;
            if (currentEvent.commandName == "ObjectSelectorCanceled")
            {
                _isLuaScriptPickerActive = false;
                _luaScriptPickerControlId = -1;
                _luaScript.objectReferenceValue = _luaScriptBeforePicker;
                return;
            }

            if (currentEvent.commandName != "ObjectSelectorClosed")
            {
                return;
            }

            if (EditorGUIUtility.GetObjectPickerControlID() != _luaScriptPickerControlId)
            {
                return;
            }

            _isLuaScriptPickerActive = false;
            _luaScriptPickerControlId = -1;
            var selectedScript = EditorGUIUtility.GetObjectPickerObject() as VivenScript;
            if (!selectedScript)
            {
                _luaScript.objectReferenceValue = null;
                _lastLuaScriptObject = null;
                return;
            }

            if (selectedScript == _luaScriptBeforePicker)
            {
                _luaScript.objectReferenceValue = _luaScriptBeforePicker;
                return;
            }

            _luaScript.objectReferenceValue = selectedScript;
            _migrationPreviewDirty = true;
            InitializeInjectionInfoFromScript();
        }

        private static bool ConfirmLuaScriptChange()
        {
            return EditorUtility.DisplayDialog(
                "Lua Script 변경 확인",
                "Lua Script를 변경하면 InjectionInfoList가 초기화됩니다.\n계속하시겠습니까?",
                "변경",
                "취소");
        }

        private void InitializeInjectionInfoFromScript()
        {
            var scriptObject = _luaScript.objectReferenceValue as VivenScript;
            if (!scriptObject || _injectionInfoList == null)
            {
                return;
            }

            if (_lastLuaScriptObject == _luaScript.objectReferenceValue)
            {
                return;
            }


            _lastLuaScriptObject = _luaScript.objectReferenceValue;

            var behaviour = (VivenLuaBehaviour)target;

            Undo.RecordObject(behaviour, "Sync Injection Info");
            behaviour.InitializeInjections((VivenScript)_lastLuaScriptObject);
            EditorUtility.SetDirty(behaviour);
            serializedObject.Update();
        }

        private void EnsureStyles()
        {
            if (_titleStyle == null)
            {
                _titleStyle = new GUIStyle(GUI.skin.label)
                {
                    alignment = TextAnchor.MiddleCenter,
                    fontStyle = FontStyle.Bold,
                    fontSize = 20,
                    wordWrap = true
                };
            }

            if (_subtitleStyle == null)
            {
                _subtitleStyle = new GUIStyle(GUI.skin.label)
                {
                    alignment = TextAnchor.MiddleCenter,
                    fontStyle = FontStyle.Bold,
                    fontSize = 15,
                    normal = { textColor = Color.gray },
                    wordWrap = true
                };
            }

            if (_scriptLabelStyle == null)
            {
                _scriptLabelStyle = new GUIStyle(EditorStyles.boldLabel)
                {
                    fontSize = Mathf.Max(EditorStyles.boldLabel.fontSize, 14)
                };
            }
        }

        private void RefreshParsedInjectionInfosFromScript()
        {
            var scriptObject = _luaScript.objectReferenceValue as VivenScript;
            if (!scriptObject) return;

            var assetPath = AssetDatabase.GetAssetPath(scriptObject);
            if (string.IsNullOrEmpty(assetPath)) return;

            AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
            AssetDatabase.Refresh();

            var refreshedScript = AssetDatabase.LoadAssetAtPath<VivenScript>(assetPath);
            if (!refreshedScript) return;

            _luaScript.objectReferenceValue = refreshedScript;
            _lastLuaScriptObject = refreshedScript;

            var behaviour = (VivenLuaBehaviour)target;
            Undo.RecordObject(behaviour, "Refresh Parsed Injections");
            behaviour.RefreshParsedInjectionInfos(refreshedScript);
            EditorUtility.SetDirty(behaviour);
            serializedObject.Update();
        }
    }
}