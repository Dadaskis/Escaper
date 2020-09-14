using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class SpriteAnimation : MonoBehaviour {

	public string framesFolder = "";
	public Image image;
	public int framesPerSecond = 30;

	private List<Sprite> sprites = new List<Sprite>();
	private int minFrame = 999;
	private int maxFrame = 0;
	private int currentFrame = 0;

	IEnumerator PlayAnimation() {
		while (true) { 
			try {
				currentFrame++;

				if (currentFrame > maxFrame) {
					currentFrame = minFrame;
				}

				image.sprite = sprites [currentFrame];
			} catch (System.Exception ex) { 
				Debug.LogError (ex);
			}
			yield return new WaitForSeconds (((float)framesPerSecond) / 1000.0f);
		}
	}

	void Start () {
		SortedDictionary<int, Sprite> sortedSprites = new SortedDictionary<int, Sprite> ();
		Sprite[] objects = Resources.LoadAll (framesFolder, typeof(Sprite)).Cast<Sprite> ().ToArray ();
		
		foreach (Sprite obj in objects) {
			int frameNumber = System.Convert.ToInt32 (obj.name);
			sortedSprites [frameNumber] = obj;
			minFrame = Mathf.Min (minFrame, frameNumber);
			maxFrame = Mathf.Max (maxFrame, frameNumber);
		}

		currentFrame = minFrame;

		for (int frame = minFrame; frame <= maxFrame; frame++) {
			sprites.Add(sortedSprites [frame]);
		}

		StartCoroutine (PlayAnimation ());
	}

}
