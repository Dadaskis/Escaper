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

namespace Events.IWeapon {
	class PrimaryFire {}
	class Reload {}
	class Shot {}
}
	
public class IWeapon : MonoBehaviour {
	public string weaponClass = "";
	public int slot = -1;
	public IAmmoFiller ammoFiller = null;
	public string ammoType = "";
	public int currentDurability = 600;
	public int maxDurability = 600;
	public bool animationPlaying = false;
	public Animator animator = null;
	public AnimatorOverrideController animatorOverride;
	public AnimationClipOverrides animationOverrides;
	public Character owner;
	public bool firstPerson = true;
	public int maxAmmo = 30;
	public int currentAmmo = 30;
	public int damage = 2;
	public bool restrictUsing = false;
	public string animatorSpeedFloatName = "AnimationSpeed";
	public float overallWeaponsAnimationSpeedModifier = 1.0f;
	public float currentAnimationSpeed = 1.0f;
	public GameObject target;
	public Vector3 overrideTargetHitPosition = Vector3.zero;

	public delegate void AnimationOverMethodType ();
	public delegate void AnimationOverrideMethodType(ref AnimationClipOverrides overrides);

	public IEnumerator PlayAnimation(string name, float duration) {
		if (animationPlaying) {
			yield break;
		}
		animationPlaying = true;
		animator.SetTrigger (name);
		yield return new WaitForSeconds (duration / currentAnimationSpeed);
		animationPlaying = false;
		SetCurrentAnimationSpeed (1.0f);
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
			if (firstPerson) {
				animator = GetComponent<Animator> ();
				if (animator == null) {
					animator = GetComponentInChildren<Animator> ();
				}
			} else {
				animator = GetComponentInParent <Animator> ();
			}
		}
		if (animator != null) {
			animatorOverride = new AnimatorOverrideController (animator.runtimeAnimatorController);
			animator.runtimeAnimatorController = animatorOverride;
			animationOverrides = new AnimationClipOverrides (animatorOverride.overridesCount);
			UpdateAnimations ();
		}
		CustomStartOnEnd ();
	}

	public virtual void PrimaryFire () {
		EventManager.RunEventListeners<Events.IWeapon.PrimaryFire> (this);
	}

	public virtual void SinglePrimaryFire () {}
	public virtual void SecondaryFire () {}
	public virtual void SingleSecondaryFire () {}

	public virtual void Reload () {
		EventManager.RunEventListeners<Events.IWeapon.Reload> (this);
	}

	public virtual void Punch () {}
	public virtual void Save () {}
	public virtual void UnSave () {}
	public virtual void CustomStartOnBegin () {}
	public virtual void CustomStartOnEnd () {}
	public virtual void MagCheck () {}
	public virtual void ApplyAnimationOverrides (ref AnimationClipOverrides overrides) {}
	public virtual float Takeout() { return 1.0f; }
	public virtual float Draw() { return 1.0f; }

}