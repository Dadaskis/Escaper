using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MedkitUIIconData {
	
	public int applyOnUse = 0;
	public string spriteName = "";

}

[System.Serializable]
public class MedkitUIAnimationData {
	
	public int applyOnUse = 0;
	public string triggerName = "";
	public string modelName = "";

}
	
[CreateAssetMenu(fileName = "Create medkit item data", menuName = "New medkit item data")]
public class MedkitItemData : ItemData {
	
	public int maxUses = 1;
	public List<MedkitUIIconData> icons;
	public List<MedkitUIAnimationData> animations;
	public int addHealthSeconds = 10;
	public int addHealthPerHalfSecond = 3;

}
