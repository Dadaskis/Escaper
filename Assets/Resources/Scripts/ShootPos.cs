using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootPos : MonoBehaviour {
	private Color gizmosColor = new Color(255, 255, 0, 10);
	public AudioSource audioSource;
	public AudioClip[] shootSounds;

	void OnDrawGizmos() {
		Gizmos.color = gizmosColor;
		Gizmos.DrawWireSphere (transform.position, 0.05f);
		Gizmos.DrawRay(transform.position, transform.forward);
	}

	public void PlaySound() {
		audioSource.clip = shootSounds[Random.Range (0, shootSounds.Length - 1)];
		audioSource.PlayOneShot (audioSource.clip);
	}
}
