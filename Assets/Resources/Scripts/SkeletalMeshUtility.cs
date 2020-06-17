using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletalMeshUtility {

	public static void Combine(SkinnedMeshRenderer root, SkinnedMeshRenderer item) {
		Dictionary<string, Transform> boneMap = new Dictionary<string, Transform>();
		foreach (Transform bone in item.bones) {
			boneMap [bone.gameObject.name] = bone;
		}

		Transform[] newBones = new Transform[root.bones.Length];
		for (int i = 0; i < root.bones.Length; ++i)
		{
			GameObject bone = root.bones[i].gameObject;
			if (!boneMap.TryGetValue(bone.name, out newBones[i]))
			{
				Debug.Log("[SkeletalMeshUtility] Unable to map bone \"" + bone.name + "\" to target skeleton.");
				break;
			}
		}
		root.bones = newBones;
	}

}
