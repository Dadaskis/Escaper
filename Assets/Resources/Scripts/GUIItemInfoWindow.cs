using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIItemInfoWindow : MonoBehaviour {

	public Text name;
	public Text description;
	public Image icon;

	public void SetItem(GUIItem item) {
		name.text = item.fullName;
		description.text = item.description;
		icon.sprite = item.icon.sprite;
		transform.SetAsLastSibling ();
	}

}
