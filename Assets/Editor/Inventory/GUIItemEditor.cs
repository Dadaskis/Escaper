using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GUIItem))]
[CanEditMultipleObjects]
public class GUIItemEditor : Editor {
	void OnEnable() {

	}

	public override void OnInspectorGUI() {
		/*serializedObject.Update();
		SerializedProperty iterator = serializedObject.GetIterator ();
		while(iterator.NextVisible(true)) {
			EditorGUILayout.PropertyField (iterator);
		}
		serializedObject.ApplyModifiedProperties();*/
		base.OnInspectorGUI ();

		GUIItem item = target as GUIItem;

		if (GUILayout.Button ("Resize", EditorStyles.miniButton)) {
			item.Resize ();
		}
	}

}
