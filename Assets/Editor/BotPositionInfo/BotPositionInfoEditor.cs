using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(BotPositionsInfo), true)]
[CanEditMultipleObjects]
public class BotPositionInfoEditor : Editor {

	public override void OnInspectorGUI() {
		base.OnInspectorGUI ();

		BotPositionsInfo botPositionsInfo = target as BotPositionsInfo;

		if (GUILayout.Button ("Place voxels", EditorStyles.miniButton)) {
			botPositionsInfo.PlaceVoxels ();
		}

		if (GUILayout.Button ("Clear voxels", EditorStyles.miniButton)) {
			botPositionsInfo.ClearVoxels ();
		}

		if (GUILayout.Button ("Identify voxels", EditorStyles.miniButton)) {
			botPositionsInfo.IdentifyVoxels ();
		}

		if (GUILayout.Button ("Enable voxels gizmos", EditorStyles.miniButton)) {
			botPositionsInfo.EnableVoxelsGizmos ();
		}

		if (GUILayout.Button ("Disable voxels gizmos", EditorStyles.miniButton)) {
			botPositionsInfo.DisableVoxelsGizmos ();
		}

		if (GUILayout.Button ("Initialize as singleton", EditorStyles.miniButton)) {
			botPositionsInfo.InitializeSingleton ();
		}

	}
}
