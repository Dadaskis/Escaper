using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoonSharp.Interpreter;
using UnityEngine.SceneManagement;

public class LevelTransitionData {
	public string gameObjectName;
	public string targetMap;
}

public class MapData {
	public string name;
	public string mainModelName;
	public List<LevelTransitionData> levelTransitions = new List<LevelTransitionData>();
}

[LuaApi(
	luaName = "MapLoader",
	description = "C# side of map registering")]
public class ResourceMapLoader : LuaAPIBase {

	public static ResourceMapLoader instance;

	public Dictionary<string, MapData> maps = new Dictionary<string, MapData>();
	public string onNewGameMap = "";

	public ResourceMapLoader() : base("MapLoader") {
		instance = this;
	}

	protected override void InitialiseAPITable(){
		m_ApiTable ["SetNewGameMap"] = (System.Func<DynValue, bool>) SetNewGameMap;
		m_ApiTable ["RegisterMap"] = (System.Func<DynValue, bool>) RegisterMap;
	}

	string GetStringFromTable(Table table, string key) {
		DynValue value = table.Get (key);
		if (!value.IsNil ()) {
			return value.CastToString ();
		}
		return "";
	}

	[LuaApiFunction(
		name = "SetNewGameMap", 
		description = "Sets map on new game"
	)]
	public bool SetNewGameMap(DynValue value) {
		if (!value.IsNil ()) {
			return SetNewGameMap (value.CastToString());
		}
		return false;
	}

	public bool SetNewGameMap(string name) {
		onNewGameMap = name;
		return true;
	}

	[LuaApiFunction(
		name = "RegisterMap", 
		description = "Register new map"
	)]
	public bool RegisterMap(DynValue value) {
		if(!value.IsNil() && value.Type == DataType.Table) {
			MapData data = new MapData ();
			Table table = value.Table;
			data.name = GetStringFromTable (table, "Name");
			data.mainModelName = GetStringFromTable (table, "MainModelName");
			DynValue levelTransitions = table.Get ("LevelTransitions");
			if (!levelTransitions.IsNil () && levelTransitions.Type == DataType.Table) {
				Table levelsTable = levelTransitions.Table;
				foreach (DynValue transitionData in levelsTable.Values) {
					if (!transitionData.IsNil () && transitionData.Type == DataType.Table) {
						Table transitionTable = transitionData.Table;
						if (!transitionTable.Get ("GameObjectName").IsNil () && !transitionTable.Get ("TargetMap").IsNil ()) {
							string gameObjectName = transitionTable.Get ("GameObjectName").CastToString ();
							string targetMap = transitionTable.Get ("TargetMap").CastToString ();
							LevelTransitionData transitionDataResult = new LevelTransitionData();
							transitionDataResult.gameObjectName = gameObjectName;
							transitionDataResult.targetMap = targetMap;
							data.levelTransitions.Add (transitionDataResult);
						}
					}
				}
			}
			return RegisterMap (data);
		}
		return false;
	}

	public bool RegisterMap(MapData data) {
		maps [data.name] = data;
		return true;
	}

	public void ChangeMap(string name) {
		MapObject[] map = GameObject.FindObjectsOfType<MapObject> ();
		if (map != null && map.GetLength (0) > 0) {
			foreach (MapObject mapObj in map) { 
				GameObject.Destroy (mapObj.gameObject);
			}
		}
		MapData data = maps [name];
		GameObject mainModel = GameObject.Instantiate (ResourceModelLoader.instance.models [data.mainModelName]);
		Debug.Log ("Instantiated " + data.mainModelName);
		mainModel.SetActive (true);
		mainModel.transform.parent = null;
		mainModel.transform.position = Vector3.zero;
		mainModel.AddComponent<MapObject> ();
		foreach (MeshRenderer renderer in mainModel.GetComponentsInChildren<MeshRenderer> ()) {
			renderer.gameObject.AddComponent<MeshCollider> ();
		}

		foreach (LevelTransitionData LTData in data.levelTransitions) {
			Transform LTTransform = mainModel.transform.Find (LTData.gameObjectName);
			if (LTTransform != null) {
				GameObject LTObject = LTTransform.gameObject;
				//Rigidbody LTBody = LTObject.AddComponent<Rigidbody> ();
				//LTBody.isKinematic = true;
				//LTBody.useGravity = false;
				BoxCollider LTCollider = LTObject.AddComponent<BoxCollider> ();
				LTCollider.isTrigger = true;
				MapLevelTransition LTComponent = LTObject.AddComponent<MapLevelTransition> ();
				LTComponent.target = LTData.targetMap;
				GameObject.Destroy (LTObject.GetComponent<MeshRenderer> ());
				GameObject.Destroy (LTObject.GetComponent<MeshCollider> ());
			}
		}
		GameObject.DontDestroyOnLoad (mainModel);
		SceneManager.LoadScene ("Empty");
		//SceneManager.MoveGameObjectToScene (mainModel, SceneManager.GetActiveScene ());
	}

}
