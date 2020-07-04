using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MaterialManager))]
[CanEditMultipleObjects]
public class MaterialManagerEditor : Editor {

	public override void OnInspectorGUI() {
		/*serializedObject.Update();
		SerializedProperty iterator = serializedObject.GetIterator ();
		while(iterator.NextVisible(true)) {
			EditorGUILayout.PropertyField (iterator);
		}
		serializedObject.ApplyModifiedProperties();*/
		base.OnInspectorGUI ();

		MaterialManager manager = target as MaterialManager;

		if (GUILayout.Button ("Change quality", EditorStyles.miniButton)) {
			manager.ChangeQuality ();
		}

		if (GUILayout.Button ("Change mode", EditorStyles.miniButton)) {
			manager.ChangeMode ();
		}

		if (GUILayout.Button ("Initialize", EditorStyles.miniButton)) {
			manager.Initialize ();
		}

		if (GUILayout.Button ("Reset materials", EditorStyles.miniButton)) {
			manager.ResetMaterials ();
		}

		if (GUILayout.Button ("Register new materials and textures", EditorStyles.miniButton)) {
			manager.RegisterMaterialsAndTextures ();
		}
	}

}
