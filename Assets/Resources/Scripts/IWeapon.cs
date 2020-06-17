using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationClipOverrides : List<KeyValuePair<AnimationClip, AnimationClip>> {
	public AnimationClipOverrides(int capacity) : base(capacity) {}

	public AnimationClip this[string name] {
		get {
			return this.Find (x => x.Key.name.Equals (name)).Value; 
		}
		set {
			int index = this.FindIndex (x => x.Key.name.Equals (name));
			if (index != -1) {
				this [index] = new KeyValuePair<AnimationClip, AnimationClip> (this [index].Key, value);
			}
		}
	}
}

public abstract class IWeapon : MonoBehaviour {

	public bool animationPlaying = false;
	public Animator animator;
	public AnimatorOverrideController animatorOverride;
	public AnimationClipOverrides animationOverrides;
	public Character owner;
	public bool firstPerson = true;
	public int maxAmmo = 1;
	public int currentAmmo = 1;

	public IEnumerator PlayAnimation(string name, float duration) {
		animationPlaying = true;
		animator.SetBool (name, true);
		yield return new WaitForSeconds (duration);
		animationPlaying = false;
		animator.SetBool (name, false);
	}

	public Vector3 CalculateSightPosition(Transform weapon, Transform sight) {
		Vector3 sightPosition = new Vector3 ();
		Vector3 sightPrePosition = sight.position;
		Vector3 weaponPrePosition = weapon.position;
		sightPosition = sightPrePosition - weaponPrePosition;
		//sightPosition = weapon.localPosition - sightPosition;
		sightPosition.z = weapon.localPosition.z;
		sightPosition.y = -sightPosition.y;
		return sightPosition;
	}

	void Start() {
		CustomStartOnBegin ();
		animatorOverride = new AnimatorOverrideController (animator.runtimeAnimatorController);
		animator.runtimeAnimatorController = animatorOverride;
		animationOverrides = new AnimationClipOverrides (animatorOverride.overridesCount);
		animatorOverride.GetOverrides (animationOverrides);
		ApplyAnimationOverrides (ref animationOverrides);
		animatorOverride.ApplyOverrides (animationOverrides);
		CustomStartOnEnd ();
	}

	public abstract void PrimaryFire ();
	public abstract void SinglePrimaryFire ();
	public abstract void SecondaryFire ();
	public abstract void SingleSecondaryFire ();
	public abstract void Reload ();
	public abstract void CustomStartOnBegin();
	public abstract void CustomStartOnEnd();
	public abstract void ApplyAnimationOverrides(ref AnimationClipOverrides overrides);

}
