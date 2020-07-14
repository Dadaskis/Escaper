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

public abstract class IWeapon : MonoBehaviour {

	public bool animationPlaying = false;
	public Animator animator;
	public AnimatorOverrideController animatorOverride;
	public AnimationClipOverrides animationOverrides;
	public Character owner;
	public bool firstPerson = true;
	public int maxAmmo = 1;
	public int currentAmmo = 1;
	public bool restrictUsing = false;

	public delegate void AnimationOverMethodType ();
	public delegate void AnimationOverrideMethodType(ref AnimationClipOverrides overrides);

	public IEnumerator PlayAnimation(string name, float duration) {
		if (animationPlaying) {
			yield break;
		}
		Debug.LogError ("Setting animation: " + name);
		animationPlaying = true;
		animator.SetTrigger (name);
		yield return new WaitForSeconds (duration);
		animationPlaying = false;
	}

	public IEnumerator PlayAnimation(string name, float duration, AnimationOverMethodType onOver = null) {
		yield return PlayAnimation (name, duration);
		onOver ();
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

	void Start() {
		CustomStartOnBegin ();
		animatorOverride = new AnimatorOverrideController (animator.runtimeAnimatorController);
		animator.runtimeAnimatorController = animatorOverride;
		animationOverrides = new AnimationClipOverrides (animatorOverride.overridesCount);
		UpdateAnimations ();
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
