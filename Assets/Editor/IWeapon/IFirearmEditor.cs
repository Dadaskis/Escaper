using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(IFirearm), true)]
[CanEditMultipleObjects]
public class IFirearmEditor : Editor {

	class NameIndexPair {
		public int index;
		public string name;
	}

	int GetClip(string name, List<NameIndexPair> nameByIndex){
		foreach (NameIndexPair pair in nameByIndex) {
			if (pair.name.Contains (name)) {
				return pair.index;
			}
		}
		return -1;
	}

	AnimationClip GetClipFromArray(string name, List<AnimationClip> clips, List<NameIndexPair> pairs) {
		int animationIndex = GetClip (name, pairs);
		if (animationIndex >= 0) {
			return clips [animationIndex];
		}
		return null;
	}

	public override void OnInspectorGUI() {
		/*serializedObject.Update();
		SerializedProperty iterator = serializedObject.GetIterator ();
		while(iterator.NextVisible(true)) {
			EditorGUILayout.PropertyField (iterator);
		}
		serializedObject.ApplyModifiedProperties();*/
		base.OnInspectorGUI ();

		IFirearm firearm = target as IFirearm;

		if (GUILayout.Button ("Assign animations by name (IFirearm)", EditorStyles.miniButton)) {
			List<AnimationClip> clips = new List<AnimationClip>();

			Object[] objects = AssetDatabase.LoadAllAssetRepresentationsAtPath (firearm.modelPath);
			foreach (Object obj in objects) {
				AnimationClip clip = obj as AnimationClip;
				if (clip != null) {
					clips.Add (clip);
				}
			}

			List<NameIndexPair> nameByIndex = new List<NameIndexPair> ();
			int index = 0;
			foreach (AnimationClip clip in clips) {
				NameIndexPair pair = new NameIndexPair ();
				pair.name = clip.name;
				pair.index = index;
				index++;
				nameByIndex.Add (pair);
			}

			firearm.idleClipFP = GetClipFromArray (firearm.idleAnimationFindNameFP, clips, nameByIndex);
			firearm.fireClipFP = GetClipFromArray (firearm.fireAnimationFindNameFP, clips, nameByIndex);
			firearm.drawClipFP = GetClipFromArray (firearm.drawAnimationFindNameFP, clips, nameByIndex);
			//firearm.fullReloadClipFP = GetClipFromArray (firearm.fullReloadAnimationFindNameFP, clips, nameByIndex); 
			//firearm.jammedIdleClipFP = GetClipFromArray (firearm.jammedIdleAnimationFindNameFP, clips, nameByIndex); 
			//firearm.jammingClipFP = GetClipFromArray (firearm.jammingAnimationFindNameFP, clips, nameByIndex); 
			firearm.punchHitClipFP = GetClipFromArray (firearm.punchHitAnimationFindNameFP, clips, nameByIndex); 
			firearm.punchNotHitClipFP = GetClipFromArray (firearm.punchNotHitAnimationFindNameFP, clips, nameByIndex); 
			firearm.reloadClipFP = GetClipFromArray (firearm.reloadAnimationFindNameFP, clips, nameByIndex); 
			firearm.saveClipFP = GetClipFromArray (firearm.saveAnimationFindNameFP, clips, nameByIndex); 
			//firearm.saveJammedClipFP = GetClipFromArray (firearm.saveJammedAnimationFindNameFP, clips, nameByIndex); 
			//firearm.unjammingClipFP = GetClipFromArray (firearm.unjammingAnimationFindNameFP, clips, nameByIndex); 
			firearm.checkMagClipFP = GetClipFromArray (firearm.checkMagAnimationFindNameFP, clips, nameByIndex);

		}
	}

}
