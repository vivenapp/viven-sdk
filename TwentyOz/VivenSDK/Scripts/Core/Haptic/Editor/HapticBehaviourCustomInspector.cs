using UnityEditor;
using UnityEngine;

namespace TwentyOz.VivenSDK.Scripts.Core.Haptic.Editor
{
    [CustomEditor(typeof(HapticBehaviour))]
    public class HapticBehaviourCustomInspector : UnityEditor.Editor
    {
        private SerializedProperty _hardness;
        private SerializedProperty _smoothness;
        private SerializedProperty _warmness;
        private SerializedProperty _friction;
        private SerializedProperty _autoPopulateOnAwake;
        private SerializedProperty _hapticIntensity;

        private MeshRenderer _meshRenderer;
        private Collider     _collider;

        private GUIStyle _headerStyle;

        private void OnEnable()
        {
            _hardness            = serializedObject.FindProperty(nameof(HapticBehaviour.hardness));
            _smoothness          = serializedObject.FindProperty(nameof(HapticBehaviour.smoothness));
            _warmness            = serializedObject.FindProperty(nameof(HapticBehaviour.warmness));
            _friction            = serializedObject.FindProperty(nameof(HapticBehaviour.friction));
            _autoPopulateOnAwake = serializedObject.FindProperty(nameof(HapticBehaviour.autoPopulateOnAwake));
            _hapticIntensity     = serializedObject.FindProperty(nameof(HapticBehaviour.hapticIntensity));

            _meshRenderer = ((HapticBehaviour)target).GetComponent<MeshRenderer>();
            _collider     = ((HapticBehaviour)target).GetComponent<Collider>();
        }

        public override void OnInspectorGUI()
        {
            _headerStyle ??= new GUIStyle(GUI.skin.label) { fontSize = 16, fontStyle = FontStyle.Bold };
            EditorGUILayout.LabelField("Viven Haptic Behaviour", _headerStyle);

            bool hasValidMaterial = _meshRenderer != null && _meshRenderer.sharedMaterial != null;

            if (_meshRenderer == null)
                EditorGUILayout.HelpBox("MeshRenderer 컴포넌트가 없습니다.", MessageType.Error);
            else if (_meshRenderer.sharedMaterial == null)
                EditorGUILayout.HelpBox("MeshRenderer 컴포넌트의 Material이 없습니다.", MessageType.Error);
            else if (_meshRenderer.sharedMaterial.shader.name != "Standard")
                EditorGUILayout.HelpBox("MeshRenderer 컴포넌트의 Material의 Shader가 Standard가 아닙니다.", MessageType.Error);
            else
                EditorGUILayout.HelpBox("Haptic 장비 연동을 위한 컴포넌트입니다.", MessageType.Info);

            EditorGUILayout.BeginVertical();

            Slider(EditorGUILayout.GetControlRect(), _hardness, 0, 1, new GUIContent("Hardness"));
            Slider(EditorGUILayout.GetControlRect(), _smoothness, 0, 1, new GUIContent("Smoothness"));
            Slider(EditorGUILayout.GetControlRect(), _warmness, 0, 1, new GUIContent("Warmness"));
            if (_collider == null)
                EditorGUILayout.HelpBox("Collider 컴포넌트가 없습니다.", MessageType.Error);
            if (_collider != null && _collider.material == null)
                EditorGUILayout.HelpBox("Collider 컴포넌트의 Material이 없습니다.", MessageType.Error);
            Slider(EditorGUILayout.GetControlRect(), _friction, 0, 1, new GUIContent("Friction"));
            Slider(EditorGUILayout.GetControlRect(), _hapticIntensity, 0, 1, new GUIContent("Haptic Intensity"));
            EditorGUILayout.PropertyField(_autoPopulateOnAwake);

            EditorGUILayout.Space(5);

            using (new EditorGUI.DisabledScope(!hasValidMaterial))
            {
                if (GUILayout.Button("Populate"))
                {
                    _hardness.floatValue   = _meshRenderer.sharedMaterial.GetFloat("_Glossiness");
                    _smoothness.floatValue = _meshRenderer.sharedMaterial.GetFloat("_Glossiness");
                    _warmness.floatValue   = _meshRenderer.sharedMaterial.GetFloat("_Metallic");
                }
            }

            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.EndVertical();
        }

        private static void Slider(Rect position, SerializedProperty property, float leftValue, float rightValue, GUIContent label)
        {
            label = EditorGUI.BeginProperty(position, label, property);

            EditorGUI.BeginChangeCheck();
            var newValue = EditorGUI.Slider(position, label, property.floatValue, leftValue, rightValue);

            if (EditorGUI.EndChangeCheck()) property.floatValue = newValue;

            EditorGUI.EndProperty();
        }
    }
}