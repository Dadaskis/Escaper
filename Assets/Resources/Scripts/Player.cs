using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

[RequireComponent(typeof(Character))]
public class Player : MonoBehaviour {

	private GameObject weaponObject;
	public GameObject weaponHolder;
	public Camera camera;
	public Character character;
	public GameObject deathPlayer;
	public Image damagePanel;
	public Canvas uiCanvas;
	public GameObject usagePanel;
	public Text usageText;
	public float pickupDistance = 3.0f;
	public GUIInventory inventory;
	public PlayerWeaponSlots weaponSlots;
	public FirstPersonController controller;

	public GameObject WeaponObject {
		set {
			weaponObject = value;
		}
		get {
			return weaponObject;
		}
	}

	public static Player instance;

	public bool killed = false;
	void OnDeath() {
		if(!killed) {
			GameObject player = Instantiate (deathPlayer, transform);
			player.transform.SetParent (null);
			GetComponentInChildren<Camera> ().enabled = false;
			Destroy (this.gameObject);
			//gameObject.transform.position = new Vector3(10000.0f, 10000.0f, 10000.0f);
			killed = true;
		}
	}

	void Awake() {
		instance = this;
		character.onDeath.AddListener (OnDeath);
		Time.timeScale = 1.0f;
	}

	void Update() {
		RaycastHit hit = character.Raycast ();
		usageText.text = "";
		usagePanel.SetActive (false);
		if (hit.transform != null) {
			if (hit.distance < pickupDistance) {
				IUsableObject obj = hit.transform.GetComponent<IUsableObject> ();
				if (obj != null) {
					usagePanel.SetActive (true);
					usageText.text = obj.ShowText ();
					if (InputManager.GetButtonDown ("Use")) {
						obj.Use ();
					}
				}
			}
		}
	}

}
