using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(ToggleImage))]
public class ToggleImageCustomInspector : ImageEditor
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		EditorGUI.BeginChangeCheck();
		EditorGUILayout.PropertyField(serializedObject.FindProperty("_toggleOnSprite"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("_toggleOffSprite"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("_isOn"));
		EditorGUILayout.PropertyField(serializedObject.FindProperty("onValueChanged"));

		if (EditorGUI.EndChangeCheck())
			serializedObject.ApplyModifiedProperties();
	}
}
