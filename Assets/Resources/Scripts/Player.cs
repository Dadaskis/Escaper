using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

[RequireComponent(typeof(Character))]
public class Player : MonoBehaviour {

	private GameObject weaponObject;
	private Vector3 previousPosition = Vector3.zero;
	public float walkDistance = 0.0f;
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
	public PostProcessingCaller underWaterPostProcessing;
	//public Rigidbody rigidBody;

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
			killed = true;
			GameLogic.PlayerDeath ();
		}
	}

	void Awake() {
		instance = this;
		character.onDeath.AddListener (OnDeath);
		Time.timeScale = 1.0f;
	}

	void Update() {
		walkDistance += Vector3.Distance (transform.position, previousPosition);
		previousPosition = transform.position;
		RaycastHit hit = character.Raycast ();
		if (usageText != null && usagePanel != null) {
			usageText.text = "";
			usagePanel.SetActive (false);
		}
		if (hit.transform != null) {
			if (hit.distance < pickupDistance) {
				IUsableObject obj = hit.transform.root.GetComponentInChildren<IUsableObject> ();
				if (obj != null) {
					if (usageText != null && usagePanel != null) {
						string text = obj.ShowText ();
						if (text.Length > 1) { 
							usagePanel.SetActive (true);
							usageText.text = text;
						}
					}
					if (InputManager.GetButtonDown ("Use")) {
						obj.Use ();
					}
				}
			}
		}
	}

}
