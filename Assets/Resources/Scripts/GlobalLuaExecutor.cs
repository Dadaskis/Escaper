using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using MoonSharp.Interpreter;

public class GlobalLuaExecutor : MonoBehaviour {

	private static GlobalLuaExecutor instance;
	private LuaVM luaVM;

	public bool materialsLoaded = false;

	public LuaVM LuaState {
		get {
			return luaVM;
		}
	}

	//public string beginEntityRegisterScriptFileName = "";
	//public string endEntityRegisterScriptFileName = "";

	void ExecuteAllGlobalScripts(string path) {
		DirectoryInfo dirInfo = new DirectoryInfo(path);
		FileInfo[] filesInfo = dirInfo.GetFiles ();
		foreach (FileInfo fileInfo in filesInfo) {
			Debug.Log(fileInfo.FullName);
			if (fileInfo.Extension == ".lua") { 
				luaVM.ExecuteScript (fileInfo.FullName);
			}
		}
		DirectoryInfo[] directoriesInfo = dirInfo.GetDirectories ();
		if (directoriesInfo != null && directoriesInfo.GetLength (0) > 0) {
			foreach (DirectoryInfo directoryInfo in directoriesInfo) {
				ExecuteAllGlobalScripts (directoryInfo.FullName);
			}
		}
	}

	void ExecuteAllEntityScripts(string path) {
		DirectoryInfo dirInfo = new DirectoryInfo(path);
		FileInfo[] filesInfo = dirInfo.GetFiles ();
		foreach (FileInfo fileInfo in filesInfo) { 
			Debug.Log(fileInfo.FullName);
			if (fileInfo.Extension == ".lua") { 
				//luaVM.ExecuteScript (beginEntityRegisterScriptFileName);
				luaVM.ExecuteScript (fileInfo.FullName);				
				//luaVM.ExecuteScript (endEntityRegisterScriptFileName);
			}
		}
	}

	IEnumerator StartScripts() {
		while (ResourceTextureLoader.instance.loadingTextures > 0) {
			yield return null;
		}
		ExecuteAllGlobalScripts ("GameData/Materials");
		materialsLoaded = true;
		ExecuteAllGlobalScripts ("GameData/Maps");
		//ExecuteAllGlobalScripts ("GameData/SingleInternalLua");
		//ExecuteAllEntityScripts ("GameData/EntityLua");
		//ExecuteAllGlobalScripts ("GameData/GlobalLua");
	}

	void Awake() {
		instance = this;
		luaVM = new LuaVM ();
	}

	void Start() {
		//UserData.RegisterType<Vector2> ();
		//UserData.RegisterType<Vector3> ();
		//UserData.RegisterType<Vector4> ();
		//UserData.RegisterType<Quaternion> ();
		//UserData.RegisterType<KeyCode> ();
		//UserData.RegisterType<LuaInput> ();
		//MoonSharp.Interpreter.UserData.RegisterType<Transform> ();
		//UserData.RegisterProxyType<LuaTransform, Transform>(raw => new LuaTransform(raw));
		//UserData.RegisterProxyType<LuaGameObject, GameObject>(raw => new LuaGameObject(raw));
		//UserData.RegisterProxyType<LuaComponent, Component>(raw => new LuaComponent(raw));

		//luaVM.SetGlobal ("Vector2", typeof(Vector2));
		//luaVM.SetGlobal ("Vector3", typeof(Vector3));
		//luaVM.SetGlobal ("Vector4", typeof(Vector4));
		//luaVM.SetGlobal ("Quaternion", typeof(Quaternion));
		//luaVM.SetGlobal ("Transform", typeof(Transform));
		//luaVM.SetGlobal ("GameObject", typeof(GameObject));
		//luaVM.SetGlobal ("Component", typeof(Component));
		//luaVM.SetGlobal ("KeyCode", typeof(KeyCode));
		//luaVM.SetGlobal ("Input", typeof(LuaInput));

		StartCoroutine (StartScripts());

	}

	public static GlobalLuaExecutor GetInstance() {
		return instance;
	}


}

