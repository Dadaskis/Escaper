using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinecraftGun : MonoBehaviour {

	public RaycastFirearm gun;
	public int mode = 0; // 0 is static, 1 is dynamic, 2 is remover. Too lazy for ENum
	public GameObject staticCube;
	public GameObject dynamicCube;
	public List<Material> materials;
	public List<Sprite> iconsForMaterials;
	public Material currentMaterial;
	public Image modePanel;
	public Image materialPanel;

	void Start() {
		gun.afterShootEvent.AddListener (AfterShoot);
		//gun.onHitObjects = new List<GameObject> (); 
		switch (mode) {
		case 0:
			modePanel.color = new Color (0.0f, 0.3f, 0.0f);
			break;
		case 1:
			modePanel.color = new Color (0.2f, 0.5f, 0.0f);
			break;
		case 2:
			modePanel.color = new Color (0.5f, 0.0f, 0.0f);
			break;
		}
		//materialPanel.material = currentMaterial;
		materialPanel.sprite = iconsForMaterials[0];
	}

	void AfterShoot() {
		RaycastHit hit = gun.owner.Raycast ();
		switch(mode){
		case 0:
			GameObject newStaticCube = Instantiate (staticCube);
			MeshRenderer staticCubeRenderer = newStaticCube.GetComponent<MeshRenderer> ();
			staticCubeRenderer.sharedMaterial = currentMaterial;
			Vector3 point = hit.point + (hit.normal * 0.5f);
			point.x = Mathf.Round (point.x);
			point.y = Mathf.Round (point.y);
			point.z = Mathf.Round (point.z);
			newStaticCube.transform.position = point;
			break;
		case 1:
			GameObject newDynamicCube = Instantiate (dynamicCube);
			MeshRenderer dynamicCubeRenderer = newDynamicCube.GetComponent<MeshRenderer> ();
			dynamicCubeRenderer.sharedMaterial = currentMaterial;
			Rigidbody dynamicCubeBody = newDynamicCube.GetComponent<Rigidbody> ();
			newDynamicCube.transform.position = gun.owner.raycaster.transform.position + (gun.owner.raycaster.transform.forward * 0.5f);
			dynamicCubeBody.AddForce (gun.owner.raycaster.transform.forward * 30.0f);
			break;
		case 2:
			MinecraftObject obj = hit.transform.GetComponent<MinecraftObject> ();
			if (obj != null) {
				Destroy (obj.gameObject);
			}
			break;
		}
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.R)) {
			mode++;
			if (mode > 2) {
				mode = 0;
			}
			switch (mode) {
			case 0:
				modePanel.color = new Color (0.0f, 0.3f, 0.0f);
				break;
			case 1:
				modePanel.color = new Color (0.2f, 0.5f, 0.0f);
				break;
			case 2:
				modePanel.color = new Color (0.5f, 0.0f, 0.0f);
				break;
			}
		}

		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			currentMaterial = materials [0];
			materialPanel.sprite = iconsForMaterials [0];
		}
		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			currentMaterial = materials [1];
			materialPanel.sprite = iconsForMaterials [1];
		}
		if (Input.GetKeyDown (KeyCode.Alpha3)) {
			currentMaterial = materials [2];
			materialPanel.sprite = iconsForMaterials [2];
		}
		if (Input.GetKeyDown (KeyCode.Alpha4)) {
			currentMaterial = materials [3];
			materialPanel.sprite = iconsForMaterials [3];
		}
		if (Input.GetKeyDown (KeyCode.Alpha5)) {
			currentMaterial = materials [4];
			materialPanel.sprite = iconsForMaterials [4];
		}

	}
}
