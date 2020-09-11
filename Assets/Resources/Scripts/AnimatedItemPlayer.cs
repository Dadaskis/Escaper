using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimatedItemTrigger {
	public string name;
	public AnimationClip clip;
	public string animationSound;
}

[System.Serializable]
public class AnimatedItem {
	public string name;
	public GameObject prefab;
	public string defaultTriggerName;
	public List<AnimatedItemTrigger> triggersList;
	public Dictionary<string, AnimatedItemTrigger> triggers;

	public Dictionary<string, AnimatedItemTrigger> GetTriggersDictionary() {
		Dictionary<string, AnimatedItemTrigger> result = new Dictionary<string, AnimatedItemTrigger> ();
		foreach (AnimatedItemTrigger trigger in triggersList) {
			result [trigger.name] = trigger;
		}
		return result;
	}
};

public class AnimatedItemPlayer : MonoBehaviour {

	public static AnimatedItemPlayer instance;
	public List<AnimatedItem> itemsList = new List<AnimatedItem> ();
	public Dictionary<string, AnimatedItem> items = new Dictionary<string, AnimatedItem> ();
	public FirstPersonWeaponChanger weaponChanger;

	void Awake() {
		instance = this;

		foreach (AnimatedItem item in itemsList) {
			items [item.name] = item;
			item.triggers = item.GetTriggersDictionary ();
		}
	}

	IEnumerator AnimationPlayProcess(float seconds, string name, string triggerName, IWeapon previousWeapon) {
		instance.weaponChanger.restrictChanging = true;
		GUIInventories.instance.restrictInventoryOpening = true;

		yield return new WaitForSeconds (seconds);
		yield return new WaitForEndOfFrame ();

		AnimatedItem item = items [name];
		GameObject itemGameObject = Instantiate (item.prefab, weaponChanger.weaponHolder);
		Animator animator = itemGameObject.GetComponent<Animator> ();
		if (animator == null) {
			animator = itemGameObject.GetComponentInParent<Animator> ();
		}

		if (triggerName == "__DEFAULT") {
			animator.SetTrigger (item.defaultTriggerName);
			SoundObjectData data = SoundManager.instance.GetBasicSoundObjectData (item.triggers [item.defaultTriggerName].animationSound);
			data.spatialBlend = 1.0f;
			SoundManager.instance.CreateSound (data, transform.position, transform);
			yield return new WaitForSeconds (item.triggers [item.defaultTriggerName].clip.length);
		} else {
			animator.SetTrigger (triggerName);
			SoundObjectData data = SoundManager.instance.GetBasicSoundObjectData (item.triggers [triggerName].animationSound);
			data.spatialBlend = 1.0f;
			SoundManager.instance.CreateSound (data, transform.position, transform);
			yield return new WaitForSeconds (item.triggers [triggerName].clip.length);
		}

		Destroy (itemGameObject);

		instance.weaponChanger.restrictChanging = false;
		GUIInventories.instance.restrictInventoryOpening = false;

		if (previousWeapon != null) {
			previousWeapon.Draw ();
			//seconds = instance.weaponChanger.SetWeapon (previousWeapon.weaponClass);
			//Debug.LogError (seconds);
			//if (seconds != -1.0f) {
			//	yield return new WaitForSeconds (seconds);
			//	IWeapon newWeapon = instance.weaponChanger.GetWeapon ();
			//	newWeapon.currentAmmo = previousWeapon.currentAmmo;
			//	newWeapon.currentDurability = previousWeapon.currentDurability;
			//}
		}
	}

	public static bool PlayAnimation(string name, string triggerName = "__DEFAULT") {
		GUIInventories.Close ();
		IWeapon previousWeapon = instance.weaponChanger.GetWeapon ();
		float seconds = 0.0f;
		if (previousWeapon != null) {
			seconds = previousWeapon.Takeout ();
			if (seconds == -1.0f) {
				return false;
			}
		}
		instance.StartCoroutine(instance.AnimationPlayProcess(seconds, name, triggerName, previousWeapon));
		return true;
	}

}
