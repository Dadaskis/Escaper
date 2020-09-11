using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissedPostProcessing : MonoBehaviour {

	public PostProcessingCaller caller;

	public float workTime = 0.1f;
	public float timer = 999999f;

	public List<string> soundsOnMiss = new List<string> ();

	public void ApplyEffect() {
		timer = 0.0f;
	}

	EventData OnHumanoidCharacterShoot(EventData args) {
		Character target = args.Get<Character> (0);
		HumanoidCharacterShootStatus status = args.Get<HumanoidCharacterShootStatus> (1);
		if (status == HumanoidCharacterShootStatus.MISS && Player.instance.character == target) {
			ApplyEffect ();
			SoundObjectData data = SoundManager.instance.GetBasicSoundObjectData(soundsOnMiss[Random.Range (0, soundsOnMiss.Count - 1)]);
			data.spatialBlend = 0.0f;
			SoundManager.instance.CreateSound (data);
		}

		return new EventData ();
	}

	void Update() {
		caller.enabled = timer < workTime;
		timer += Time.deltaTime;
	}

	//void OnRenderImage(RenderTexture source, RenderTexture destination) {
	//	if (timer < workTime) {
	//		Graphics.Blit (source, destination, material);
	//	} else {
	//		Graphics.Blit (null, destination, null);
	//	}
	//}

	void Start(){
		//Camera cam = GetComponent<Camera>();
		//cam.depthTextureMode = cam.depthTextureMode | DepthTextureMode.Depth;
		EventManager.AddEventListener<Events.HumanoidCharacter.Shoot> (OnHumanoidCharacterShoot);
	}

}
