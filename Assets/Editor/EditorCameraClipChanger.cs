using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EditorCameraClipChanger : EditorWindow {
	float near = 0.001f;
	float far = 1000.0f;

	// Add menu named "My Window" to the Window menu
	[MenuItem("Window/EditorCameraClipChanger")]
	static void Init()
	{
		// Get existing open window or if none, make a new one:
		EditorCameraClipChanger window = (EditorCameraClipChanger)EditorWindow.GetWindow(typeof(EditorCameraClipChanger));
		window.Show();
	}

	void OnGUI()
	{
		/*GUILayout.Label("Base Settings", EditorStyles.boldLabel);
		myString = EditorGUILayout.TextField("Text Field", myString);

		groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
		myBool = EditorGUILayout.Toggle("Toggle", myBool);
		myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
		EditorGUILayout.EndToggleGroup();*/
		near = EditorGUILayout.FloatField (near);
		far = EditorGUILayout.FloatField (far);
		if (GUILayout.Button ("Apply clip to camera")) {
			//var drawing = SceneView.currentDrawingSceneView;
			//foreach (Camera camera in SceneView.GetAllSceneCameras()) {
				//var camera = drawing.camera;
			Camera camera = Camera.current;
			if (camera != null) {
				camera.nearClipPlane = near;
				camera.farClipPlane = far;
			}
			//}
		}
	}
}
