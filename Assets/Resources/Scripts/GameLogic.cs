using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

[System.Serializable]
public class EquipmentItem {
	public string name = "";
	public int price = 0;
}

[System.Serializable]
public class LocationStartSettings { 
	public string locationName;
	public string sceneName;
	public List<EquipmentItem> items = new List<EquipmentItem>();
	public string nextLocationName = "";
}

[System.Serializable]
public class PlayerBoostData {
	public string name = "";
	public int price = 0;
	public int points = 0;
	public int maximumPoints = 1;
}

[System.Serializable]
public class PlayerStartData {
	public List<int> items = new List<int>();
}

[System.Serializable]
public class PlayerBoostHandler {
	public string boostName;
	public IPlayerBoost boostApplier;
}

[System.Serializable]
public class LastRaidResult { 
	public int killed = 0;
	public int walkDistance = 0;
	public int earnedEXP = 0;
}

public enum PlayerStatus {
	DEAD,
	ESCAPING
}

namespace Events.GameLogic {
	class AddedItemToThePlayer {}
	class NewLocationLoaded {}
	class BoostPointChanged {}
	class PlayerDeath {}
}

[System.Serializable]
public class GameLogicSaveData {
	public Dictionary<string, LocationStartSettings> locationStartSettings = new Dictionary<string, LocationStartSettings> ();
	public Dictionary<string, PlayerBoostData> boostData = new Dictionary<string, PlayerBoostData> ();
	public LocationStartSettings currentLocationSettings;
	public int currentEXP;
	public PlayerStatus playerStatus;
	public LastRaidResult lastRaidResult;
}

public class GameLogic : MonoBehaviour {

	public static GameLogic instance;

	public Dictionary<string, LocationStartSettings> locationStartSettings = new Dictionary<string, LocationStartSettings> ();
	public Dictionary<string, PlayerBoostData> boostData = new Dictionary<string, PlayerBoostData> ();
	public List<LocationStartSettings> locationStartSettingsList = new List<LocationStartSettings> ();
	public List<PlayerBoostData> boostDataList = new List<PlayerBoostData> ();
	public List<PlayerBoostHandler> boostHandlers = new List<PlayerBoostHandler>();
	public LocationStartSettings currentLocationSettings;
	public int currentEXP;
	public GameObject playerObject;
	public int killedByPlayer = 0;
	public string afterDeathScene = "";
	public PlayerStatus playerStatus;
	public LastRaidResult lastRaidResult;

	public void Save() {
		GameLogicSaveData data = new GameLogicSaveData ();
		data.locationStartSettings = locationStartSettings;
		data.boostData = boostData;
		data.currentLocationSettings = currentLocationSettings;
		data.currentEXP = currentEXP;
		data.playerStatus = playerStatus;
		data.lastRaidResult = lastRaidResult;
		string json = JsonConvert.SerializeObject(data);
		System.IO.File.WriteAllText("Saves/GameLogic.save", json);
	}

	public void Load() {
		string json = System.IO.File.ReadAllText("Saves/GameLogic.save");
		GameLogicSaveData data = JsonConvert.DeserializeObject<GameLogicSaveData>(json);
		locationStartSettings = data.locationStartSettings;
		boostData = data.boostData;
		currentLocationSettings = data.currentLocationSettings;
		currentEXP = data.currentEXP;
		playerStatus = data.playerStatus;
		lastRaidResult = data.lastRaidResult;
	}

	EventData CharacterKilled(EventData args) {
		if (Player.instance == null) {
			return new EventData ();
		}
		Character virgin = args.Get<Character> (0);
		Character killer = args.Get<Character> (1);
		if (killer == Player.instance.character) {
			killedByPlayer++;
		}
		return new EventData();
	}

	void Awake() {
		instance = this;
		foreach (LocationStartSettings settings in locationStartSettingsList) {
			locationStartSettings [settings.locationName] = settings; 
		}
		foreach (PlayerBoostData boostData in boostDataList) {
			this.boostData [boostData.name] = boostData; 
		}
		EventManager.AddEventListener<Events.Character.Killed> (CharacterKilled);
		Load ();
	}

	void Start() {
		EventManager.RunEventListeners<Events.GameLogic.NewLocationLoaded> ();
	}

	public static LocationStartSettings SetCurrentLocationSettings(string locationName) {
		LocationStartSettings settings;
		if (instance.locationStartSettings.TryGetValue (locationName, out settings)) {
			foreach (KeyValuePair<string, PlayerBoostData> boostData in instance.boostData) {
				boostData.Value.points = 0;
			}
			instance.currentLocationSettings = settings;
			return settings;
		}
		return null;
	}

	public static LocationStartSettings GetCurrentLocationSettings() {
		return instance.currentLocationSettings;
	}

	public static int GetStartPrice(PlayerStartData data) {
		int startPrice = 0;

		LocationStartSettings startSettings = instance.currentLocationSettings;

		foreach (int itemIndex in data.items) {
			startPrice += startSettings.items [itemIndex].price;
		}

		startPrice += GetBoostsFinalPrice ();

		return startPrice;
	}

	IEnumerator SetLocationScene(PlayerStartData data) {
		AsyncOperation operation = SceneManager.LoadSceneAsync (currentLocationSettings.sceneName);
		while (true) {
			Debug.LogError (operation.progress);
			if (operation.isDone) {
				break;
			}
			yield return new WaitForSeconds (0.1f);
		}
		yield return operation;

		GameObject player = Instantiate (playerObject);
		player.transform.position = Vector3.zero;

		yield return new WaitForEndOfFrame ();

		foreach (int itemIndex in data.items) {
			EquipmentItem item = currentLocationSettings.items [itemIndex];
			GUIItem itemUI = Player.instance.inventory.AddItem (ItemManager.GetItem(item.name));
			EventManager.RunEventListeners<Events.GameLogic.AddedItemToThePlayer> (itemUI);
		}

		EventManager.RunEventListeners<Events.GameLogic.NewLocationLoaded> ();

		foreach (PlayerBoostHandler boostHandler in boostHandlers) {
			PlayerBoostData playerData;
			if(boostData.TryGetValue(boostHandler.boostName, out playerData)) {
				boostHandler.boostApplier.Apply (playerData);
			}
		}
	}

	public static bool LaunchLocation(PlayerStartData data) {
		int startPrice = GetStartPrice (data);

		if (startPrice <= instance.currentEXP) {
			instance.currentEXP -= startPrice;
			instance.killedByPlayer = 0;
			instance.StartCoroutine (instance.SetLocationScene (data));
			return true;
		}

		return false;
	}

	public static void AddBoostPoint(string boostName) {
		PlayerBoostData data;
		if (instance.boostData.TryGetValue (boostName, out data)) {
			if (data.points < data.maximumPoints) {
				data.points++;
				EventManager.RunEventListeners<Events.GameLogic.BoostPointChanged> (data);
			}
		}
	}

	public static void DecreaseBoostPoint(string boostName) {
		PlayerBoostData data;
		if (instance.boostData.TryGetValue (boostName, out data)) {
			if (data.points > 0) {
				data.points--;
				EventManager.RunEventListeners<Events.GameLogic.BoostPointChanged> (data);
			}
		}
	}

	public static int GetBoostFinalPrice(string boostName) {
		PlayerBoostData data;
		if (instance.boostData.TryGetValue (boostName, out data)) {
			return data.price * data.points;
		}
		return 0;
	}

	public static int GetBoostPrice(string boostName) {
		PlayerBoostData data;
		if (instance.boostData.TryGetValue (boostName, out data)) {
			return data.price;
		}
		return 0;
	}

	public static int GetBoostPoints(string boostName) {
		PlayerBoostData data;
		if (instance.boostData.TryGetValue (boostName, out data)) {
			return data.points;
		}
		return 0;
	}

	public static int GetBoostPointsMaximum(string boostName) {
		PlayerBoostData data;
		if (instance.boostData.TryGetValue (boostName, out data)) {
			return data.maximumPoints;
		}
		return 0;
	}

	public static int GetBoostsFinalPrice() {
		int finalPrice = 0;
		foreach (KeyValuePair<string, PlayerBoostData> pairData in instance.boostData) {
			int price = pairData.Value.points * pairData.Value.price;
			finalPrice += price;
		}
		return finalPrice;
	}

	public static int GetRaidReward() {
		int reward = 0;
		reward += Mathf.Min (instance.killedByPlayer * 100, 500);
		reward += Mathf.Min ((int) Player.instance.walkDistance, 1000);
		if (instance.playerStatus == PlayerStatus.ESCAPING) {
			reward *= 2;
		}
		return Mathf.Min(reward, 9999 - instance.currentEXP);
	}

	public void InitializeLastRaidResult() {
		LastRaidResult result = new LastRaidResult ();
		result.earnedEXP = GetRaidReward ();
		result.killed = killedByPlayer;
		result.walkDistance = (int)Player.instance.walkDistance;
		lastRaidResult = result;
	}

	public static LastRaidResult GetLastRaidResult() {
		return instance.lastRaidResult;
	}

	public static PlayerStatus GetPlayerStatus() {
		return instance.playerStatus;
	}

	IEnumerator SetSceneAfterDeath() {
		playerStatus = PlayerStatus.DEAD;
		InitializeLastRaidResult ();
		instance.currentEXP += GetRaidReward ();
		instance.currentEXP = Mathf.Min (instance.currentEXP, 9999);
		GUIInventories.instance.DisableMouseLook ();

		AsyncOperation operation = SceneManager.LoadSceneAsync (afterDeathScene);
		while (true) {
			if (operation.isDone) {
				break;
			}
			yield return new WaitForSeconds (0.1f);
		}
		yield return operation;

		Destroy (Player.instance.gameObject);
		Save ();
	}

	public static void PlayerDeath() {
		EventData data = EventManager.RunEventListeners<Events.GameLogic.PlayerDeath> ();
		if (data.Count > 0) {
			return;
		}
		instance.StartCoroutine (instance.SetSceneAfterDeath ());
	}

	IEnumerator SetSceneAfterEscape() {
		playerStatus = PlayerStatus.ESCAPING;
		InitializeLastRaidResult ();
		instance.currentEXP += GetRaidReward ();
		instance.currentEXP = Mathf.Min (instance.currentEXP, 9999);
		GUIInventories.instance.DisableMouseLook ();

		AsyncOperation operation = SceneManager.LoadSceneAsync (afterDeathScene);
		while (true) {
			if (operation.isDone) {
				break;
			}
			yield return new WaitForSeconds (0.1f);
		}
		yield return operation;

		Destroy (Player.instance.gameObject);
		Save ();
	}

	public static void PlayerEscaping() {
		instance.StartCoroutine (instance.SetSceneAfterEscape ());
	}
}
