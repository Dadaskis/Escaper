using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GUIInventorySlots))]
[CanEditMultipleObjects]
public class GUIInventorySlotsEditor : Editor {

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

		GUIInventorySlots slots = target as GUIInventorySlots;

		if (GUILayout.Button ("Generate slots", EditorStyles.miniButton)) {
			slots.GenerateSlots ();
		}
	}

}
