using System;
using TwentyOz.VivenSDK.Scripts.Core.Lua;
using UnityEditor;
using UnityEngine;

namespace TwentyOz.VivenSDK.Scripts.Editor.Lua
{
	[CustomPropertyDrawer(typeof(UIInjectionInfo))]
	public class UIInjectionInfoCustomPropertyDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var injectionInfo = GetInjectionInfo(property);

			EditorGUI.BeginProperty(position, label, property);

			if (injectionInfo == null)
			{
				EditorGUI.LabelField(position, "InjectionInfo not found");
			}
			else
			{
				var displayLabel = new GUIContent(injectionInfo.name ?? string.Empty);
				var valueRect = EditorGUI.PrefixLabel(position, displayLabel);
				var targetField = injectionInfo.TargetField;
				var pickerType = injectionInfo.PickerType;

				if (targetField != null)
				{
					EditorGUI.BeginChangeCheck();
					var newValue = DrawValueField(valueRect, pickerType, targetField.BoxedValue);
					if (EditorGUI.EndChangeCheck())
					{
						targetField.BoxedValue = newValue;
						EditorUtility.SetDirty(property.serializedObject.targetObject);
					}
				}
				else
				{
					EditorGUI.LabelField(valueRect, "Unsupported FieldType");
				}
			}

			EditorGUI.EndProperty();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return EditorGUIUtility.singleLineHeight;
		}

		private static object DrawValueField(Rect rect, Type type, object currentValue)
		{
			if (type == null)
			{
				EditorGUI.LabelField(rect, "None");
				return currentValue;
			}

			if (type == typeof(string))
			{
				return EditorGUI.TextField(rect, currentValue as string ?? string.Empty);
			}

			if (type == typeof(char))
			{
				var text = currentValue is char c ? c.ToString() : string.Empty;
				var result = EditorGUI.TextField(rect, text);
				return string.IsNullOrEmpty(result) ? default(char) : result[0];
			}

			if (type == typeof(int))
			{
				int value = currentValue is int intValue ? intValue : 0;
				return EditorGUI.IntField(rect, value);
			}

			if (type == typeof(long))
			{
				long value = currentValue is long longValue ? longValue : 0L;
				return EditorGUI.LongField(rect, value);
			}

			if (type == typeof(short))
			{
				short value = currentValue is short shortValue ? shortValue : (short)0;
				return (short)EditorGUI.IntField(rect, value);
			}

			if (type == typeof(byte))
			{
				byte value = currentValue is byte byteValue ? byteValue : (byte)0;
				return (byte)Mathf.Clamp(EditorGUI.IntField(rect, value), byte.MinValue, byte.MaxValue);
			}

			if (type == typeof(uint))
			{
				uint value = currentValue is uint uintValue ? uintValue : 0u;
				return (uint)Mathf.Max(0, EditorGUI.LongField(rect, value));
			}

			if (type == typeof(ulong))
			{
				ulong value = currentValue is ulong ulongValue ? ulongValue : 0ul;
				long input = EditorGUI.LongField(rect, (long)Mathf.Min(value, long.MaxValue));
				return (ulong)Mathf.Max(0, input);
			}

			if (type == typeof(ushort))
			{
				ushort value = currentValue is ushort ushortValue ? ushortValue : (ushort)0;
				return (ushort)Mathf.Clamp(EditorGUI.IntField(rect, value), ushort.MinValue, ushort.MaxValue);
			}

			if (type == typeof(sbyte))
			{
				sbyte value = currentValue is sbyte sbyteValue ? sbyteValue : (sbyte)0;
				return (sbyte)Mathf.Clamp(EditorGUI.IntField(rect, value), sbyte.MinValue, sbyte.MaxValue);
			}

			if (type == typeof(float))
			{
				float value = currentValue is float floatValue ? floatValue : 0f;
				return EditorGUI.FloatField(rect, value);
			}

			if (type == typeof(double))
			{
				double value = currentValue is double doubleValue ? doubleValue : 0d;
				return EditorGUI.DoubleField(rect, value);
			}

			if (type == typeof(bool))
			{
				bool value = currentValue is bool boolValue && boolValue;
				return EditorGUI.Toggle(rect, value);
			}

			if (type == typeof(Vector2))
			{
				Vector2 value = currentValue is Vector2 vectorValue ? vectorValue : Vector2.zero;
				return EditorGUI.Vector2Field(rect, GUIContent.none, value);
			}

			if (type == typeof(Vector3))
			{
				Vector3 value = currentValue is Vector3 vectorValue ? vectorValue : Vector3.zero;
				return EditorGUI.Vector3Field(rect, GUIContent.none, value);
			}

			if (type == typeof(Vector4))
			{
				Vector4 value = currentValue is Vector4 vectorValue ? vectorValue : Vector4.zero;
				return EditorGUI.Vector4Field(rect, GUIContent.none, value);
			}

			if (type == typeof(Vector2Int))
			{
				Vector2Int value = currentValue is Vector2Int vectorValue ? vectorValue : Vector2Int.zero;
				return EditorGUI.Vector2IntField(rect, GUIContent.none, value);
			}

			if (type == typeof(Vector3Int))
			{
				Vector3Int value = currentValue is Vector3Int vectorValue ? vectorValue : Vector3Int.zero;
				return EditorGUI.Vector3IntField(rect, GUIContent.none, value);
			}

			if (type == typeof(Quaternion))
			{
				Quaternion value = currentValue is Quaternion quaternionValue ? quaternionValue : Quaternion.identity;
				var vectorValue = new Vector4(value.x, value.y, value.z, value.w);
				var newVector = EditorGUI.Vector4Field(rect, GUIContent.none, vectorValue);
				return new Quaternion(newVector.x, newVector.y, newVector.z, newVector.w);
			}

			if (type == typeof(Color))
			{
				Color value = currentValue is Color colorValue ? colorValue : Color.white;
				return EditorGUI.ColorField(rect, value);
			}

			if (type == typeof(Rect))
			{
				Rect value = currentValue is Rect rectValue ? rectValue : new Rect();
				return EditorGUI.RectField(rect, value);
			}

			if (type == typeof(RectInt))
			{
				RectInt value = currentValue is RectInt rectValue ? rectValue : new RectInt();
				return EditorGUI.RectIntField(rect, value);
			}

			if (type == typeof(Bounds))
			{
				Bounds value = currentValue is Bounds boundsValue ? boundsValue : new Bounds();
				return EditorGUI.BoundsField(rect, value);
			}

			if (type == typeof(BoundsInt))
			{
				BoundsInt value = currentValue is BoundsInt boundsValue ? boundsValue : new BoundsInt();
				return EditorGUI.BoundsIntField(rect, value);
			}

			if (type == typeof(LayerMask))
			{
				LayerMask value = currentValue is LayerMask layerMaskValue ? layerMaskValue : default;
				value.value = EditorGUI.MaskField(rect, value.value, UnityEditorInternal.InternalEditorUtility.layers);
				return value;
			}

			if (type == typeof(AnimationCurve))
			{
				var value = currentValue as AnimationCurve ?? new AnimationCurve();
				return EditorGUI.CurveField(rect, value);
			}

			if (typeof(UnityEngine.Object).IsAssignableFrom(type))
			{
				var obj = currentValue as UnityEngine.Object;
				return EditorGUI.ObjectField(rect, obj, type, true);
			}

			EditorGUI.LabelField(rect, "ObjectPicker Not Supported");
			return currentValue;
		}

		private static UIInjectionInfo GetInjectionInfo(SerializedProperty property)
		{
			if (property?.serializedObject?.targetObject == null)
			{
				return null;
			}

			var targetObject = property.serializedObject.targetObject;
			UIInjectionInfo[] list = null;

			if (targetObject is VivenLuaBehaviour behaviour)
			{
				list = behaviour.uiInjectionInfoList;
			}
			if (list == null)
			{
				return null;
			}

			int index = GetArrayIndex(property.propertyPath);
			if (index < 0 || index >= list.Length)
			{
				return null;
			}

			return list[index];
		}

		private static int GetArrayIndex(string propertyPath)
		{
			int start = propertyPath.LastIndexOf('[');
			int end = propertyPath.LastIndexOf(']');
			if (start < 0 || end <= start) return -1;
			return int.TryParse(propertyPath.Substring(start + 1, end - start - 1), out int index)
				? index : -1;
		}
	}
}
