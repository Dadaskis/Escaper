using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Create item data", menuName = "New item data")]
public class ItemData : ScriptableObject {
	public GameObject prefabUI;
	public GameObject physicalPrefab;
	public Sprite icon;
	public string actionType;
	public string nameText;
	public string tag;
	public string additionalText;
	public string showName;
	public string fullName;
	public string description;
	public Vector2 size;
}
