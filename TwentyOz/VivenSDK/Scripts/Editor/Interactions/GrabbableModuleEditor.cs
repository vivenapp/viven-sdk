using Twoz.Viven.Interactions;
using UnityEditor;
using UnityEngine;

namespace TwentyOz.VivenSDK.Scripts.Editor.Interactions
{
    [CustomEditor(typeof(VivenGrabbableModule))]
    public class GrabbableModuleEditor : UnityEditor.Editor
    {
        private bool showAdvanced;

        private SerializedProperty _grabType;
        private SerializedProperty _parentToHandOnGrab;
        private SerializedProperty _holdTimeThreshold;
        private SerializedProperty _throwForce;
        private SerializedProperty _grabPoints;
        private SerializedProperty _attachPoints;
        private SerializedProperty _excludeLayerObjects;

        private GUIStyle _boldItalicStyle;

        private void OnEnable()
        {
            _grabType = serializedObject.FindProperty("grabType");
            _parentToHandOnGrab = serializedObject.FindProperty("parentToHandOnGrab");
            _holdTimeThreshold = serializedObject.FindProperty("holdTimeThreshold");
            _throwForce = serializedObject.FindProperty("throwForce");
            _grabPoints = serializedObject.FindProperty("grabPoints");
            _attachPoints = serializedObject.FindProperty("attachPoints");
            _excludeLayerObjects = serializedObject.FindProperty("excludeLayerObjects");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            _boldItalicStyle ??= new GUIStyle(EditorStyles.boldLabel) { fontStyle = FontStyle.BoldAndItalic };

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Grab Type", _boldItalicStyle);
            EditorGUILayout.PropertyField(_grabType, new GUIContent("Object Grab Type"));
            EditorGUILayout.PropertyField(_parentToHandOnGrab, new GUIContent("Parent To Hand On Grab"));

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Interaction", _boldItalicStyle);
            EditorGUILayout.PropertyField(_holdTimeThreshold, new GUIContent("Hold Time Threshold"));
            EditorGUILayout.PropertyField(_throwForce, new GUIContent("Viven Throw Force"));

            EditorGUILayout.Space();

            showAdvanced = EditorGUILayout.Foldout(showAdvanced, "Advanced");

            if (showAdvanced)
            {
                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Grab Point", _boldItalicStyle);
                EditorGUILayout.PropertyField(_grabPoints, new GUIContent("Grab Points"));

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Attach", _boldItalicStyle);
                EditorGUILayout.PropertyField(_attachPoints, new GUIContent("Viven Attach Points"));

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Layer", _boldItalicStyle);
                EditorGUILayout.PropertyField(_excludeLayerObjects, new GUIContent("Exclude Layer Game Objects"));
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}