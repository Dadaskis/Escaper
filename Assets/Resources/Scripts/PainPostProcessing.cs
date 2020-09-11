using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PainPostProcessing : MonoBehaviour {

	public PostProcessingCaller caller;

	public float speed = 5.0f;
	public float healthMultiply = 2.0f;

	public float effectPowerTarget = 0.0f;
	public float effectPower = 0.0f;

	public List<string> soundsOnHit = new List<string> ();

	public void SetEffectPower(float power) {
		caller.material.SetFloat ("_EffectPower", power);
		effectPower = power;
	}

	public void SetEffectPowerTarget(float power) {
		effectPowerTarget = power;
	}

	EventData OnCharacterHealthChanged(EventData args) {
		Character character = args.Get<Character> (0);
		if (character == Player.instance.character) {
			int currentHealth = args.Get<int> (1);
			int previousHealth = args.Get<int> (2);

			if (previousHealth > currentHealth) {
				SetEffectPower (1.0f);
			}

			float power = ((float)(currentHealth * healthMultiply)) / ((float)character.MaxHealth);
			power = Mathf.Clamp (power, 0.0f, 1.0f);
			power = 1.0f - power;
			SetEffectPowerTarget (power);			
		}

		return new EventData();
	}

	EventData OnHumanoidCharacterShoot(EventData args) {
		Character target = args.Get<Character> (0);
		HumanoidCharacterShootStatus status = args.Get<HumanoidCharacterShootStatus> (1);
		if (status == HumanoidCharacterShootStatus.HIT && Player.instance.character == target) {
			SoundObjectData data = SoundManager.instance.GetBasicSoundObjectData(soundsOnHit[Random.Range (0, soundsOnHit.Count - 1)]);
			data.spatialBlend = 0.0f;
			SoundManager.instance.CreateSound (data);
		}

		return new EventData ();
	}

	void Update() {
		SetEffectPower (Mathf.Lerp (effectPower, effectPowerTarget, speed * Time.deltaTime));
		caller.enabled = effectPower > 0.03f;
	}

	void Start(){
		EventManager.AddEventListener<Events.Character.HealthChanged> (OnCharacterHealthChanged);
		EventManager.AddEventListener<Events.HumanoidCharacter.Shoot> (OnHumanoidCharacterShoot);
	}

}
