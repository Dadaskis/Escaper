using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;
using System.IO;

public class ResourceTextureLoader : MonoBehaviour {

	public static ResourceTextureLoader instance;

	public Dictionary<string, Texture2D> textures = new Dictionary<string, Texture2D> ();
	public int loadingTextures = 0;

	void Awake() {
		instance = this;
		LoadAllTextures ("GameData/Textures");
	}

	public static Texture2D GetTexture(string name) {
		Texture2D texture;
		if (instance.textures.TryGetValue (name, out texture)) {
			return texture;
		}
		return texture;
	}

	void LoadAllTextures(string path) {
		DirectoryInfo dirInfo = new DirectoryInfo(path);
		FileInfo[] filesInfo = dirInfo.GetFiles ();
		foreach (FileInfo fileInfo in filesInfo) {
			Debug.Log(fileInfo.FullName);
			//if (fileInfo.Extension == ".lua") { 
				//luaVM.ExecuteScript (fileInfo.FullName);
			//}
			if (fileInfo.Extension == ".jpg" || fileInfo.Extension == ".png") {
				LoadTexture (fileInfo.Name, fileInfo.FullName);
			}
		}
		DirectoryInfo[] directoriesInfo = dirInfo.GetDirectories ();
		if (directoriesInfo != null && directoriesInfo.GetLength (0) > 0) {
			foreach (DirectoryInfo directoryInfo in directoriesInfo) {
				LoadAllTextures (directoryInfo.FullName);
			}
		}
	}

	private IEnumerator LoadImage(string name, string path) {
		Debug.Log ("Loading image: " + name + ", " + path);
		/*Texture2D image = null;
		byte[] fileData;

		if (File.Exists (path)) {
			fileData = File.ReadAllBytes (path);
			image = new Texture2D (2, 2);
			image.LoadImage (fileData);
		}

		return image;*/

		WWW texture = new WWW ("file:///" + path);
		loadingTextures++;
		while (!texture.isDone) {
			yield return null;
		}
		loadingTextures--;
		textures [name] = texture.texture;

	}

	public bool LoadTexture(string name, string path) {
		if(File.Exists(path)){
			StartCoroutine (LoadImage (name, path));
			return true;
		} 
		return false;
	}

}
	