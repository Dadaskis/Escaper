using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

public class IWeapon : MonoBehaviour {
	[Header("IWeapon")]
	public bool animationPlaying = false;
	public Animator animator = null;
	public AnimatorOverrideController animatorOverride;
	public AnimationClipOverrides animationOverrides;
	public Character owner;
	public bool firstPerson = true;
	public int maxAmmo = 30;
	public int currentAmmo = 30;
	public bool restrictUsing = false;
	public string animatorSpeedFloatName = "AnimationSpeed";
	public float overallWeaponsAnimationSpeedModifier = 1.0f;
	public float currentAnimationSpeed = 1.0f;
	public int currentAnimationsCount = 0;

	public delegate void AnimationOverMethodType ();
	public delegate void AnimationOverrideMethodType(ref AnimationClipOverrides overrides);

	private IEnumerator PlayAnimationWaitPart(float duration, AnimationOverMethodType onOver = null) {
		yield return new WaitForSeconds (duration / currentAnimationSpeed);
		animationPlaying = false;
		SetCurrentAnimationSpeed (1.0f);
		if (onOver != null) {
			onOver ();
		}
		currentAnimationsCount--;
		Debug.LogError (currentAnimationsCount + " " + animationPlaying);
	}

	public void PlayAnimation(string name, float duration, AnimationOverMethodType onOver = null) {
		if (currentAnimationsCount > 0) {
			return;
		}
		animationPlaying = true;
		currentAnimationsCount++;
		Debug.LogError ("Animations: " + currentAnimationsCount);
		animator.SetTrigger (name);
		StartCoroutine(PlayAnimationWaitPart(duration, onOver));
	}

	public void UpdateAnimations() {
		animatorOverride.GetOverrides (animationOverrides);
		ApplyAnimationOverrides (ref animationOverrides);
		animatorOverride.ApplyOverrides (animationOverrides);
	}

	public void UpdateAnimations(AnimationOverrideMethodType method) {
		animatorOverride.GetOverrides (animationOverrides);
		method(ref animationOverrides);
		animatorOverride.ApplyOverrides (animationOverrides);
	}

	public void ChangeAnimation(string name, AnimationClip clip) {
		UpdateAnimations (delegate(ref AnimationClipOverrides overrides) {
			overrides[name] = clip;
		});
	}

	public void SetCurrentAnimationSpeed(float speed) {
		animator.SetFloat (animatorSpeedFloatName, speed * overallWeaponsAnimationSpeedModifier);
		currentAnimationSpeed = speed * overallWeaponsAnimationSpeedModifier;
	}

	void Start() {
		CustomStartOnBegin ();
		if (animator == null) {
			animator = GetComponent<Animator> ();
			if (animator == null) {
				animator = GetComponentInChildren<Animator> ();
			}
		}
		animatorOverride = new AnimatorOverrideController (animator.runtimeAnimatorController);
		animator.runtimeAnimatorController = animatorOverride;
		animationOverrides = new AnimationClipOverrides (animatorOverride.overridesCount);
		UpdateAnimations ();
		CustomStartOnEnd ();
	}

	public virtual void PrimaryFire () {}
	public virtual void SinglePrimaryFire () {}
	public virtual void SecondaryFire () {}
	public virtual void SingleSecondaryFire () {}
	public virtual void Reload () {}
	public virtual void Punch () {}
	public virtual void Save () {}
	public virtual void UnSave () {}
	public virtual void CustomStartOnBegin () {}
	public virtual void CustomStartOnEnd () {}
	public virtual void MagCheck () {}
	public virtual void ApplyAnimationOverrides (ref AnimationClipOverrides overrides) {}

}
