using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Create item data", menuName = "New item data")]
public class ItemData : ScriptableObject {
	public GameObject prefab;
	public Sprite icon;
	public string nameText;
	public string tag;
	public string additionalText;
	public string showName;
	public Vector2 size;
}
