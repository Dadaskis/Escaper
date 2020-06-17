using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerSpawner : MonoBehaviour {

	public GameObject player;
	public GameObject uiOnStart;
	public GameObject worldStuff;

	private GameObject followingUI;

	void Awake () {
		if (FindObjectOfType<Player> () == null) {
			GameObject obj = Instantiate (player);
			obj.transform.position = transform.position + new Vector3(0.0f, 1.0f, 0.0f);
			if (uiOnStart != null) {
				followingUI = Instantiate (uiOnStart, Player.instance.uiCanvas.transform);
			}
			if (worldStuff != null) {
				Instantiate (worldStuff);
			}
		}
	}

	void Update() {
		/*if (followingUI != null) {
			FirstPersonController controller = Player.instance.GetComponent<FirstPersonController> ();
			if (controller != null) {
				controller.enableMouseLook = false;
				controller.mouseLook.SetCursorLock (false);
			} else {
				Cursor.lockState = CursorLockMode.None;
				Cursor.visible = true;
			}
		} else {
			FirstPersonController controller = Player.instance.GetComponent<FirstPersonController> ();
			if (controller != null) {
				controller.enableMouseLook = true;
				controller.mouseLook.SetCursorLock (true);
			} else {
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
			}
			Destroy (this);
		}*/
	}

	private Color gizmosColor = new Color(255, 250, 50, 10);

	void OnDrawGizmos() {
		Gizmos.color = gizmosColor;
		Gizmos.DrawWireSphere (transform.position, 0.3f);
	}
}
