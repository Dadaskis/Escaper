using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapTriggerManager : MonoBehaviour {

	public static MapTriggerManager instance; 
	public Dictionary<string, List<IMapTriggerCallable>> callables = new Dictionary<string, List<IMapTriggerCallable>>();

	void Awake() {
		instance = this;
	}

	public static void AddCallable(IMapTriggerCallable callable) {
		List<IMapTriggerCallable> callablesList;
		if (instance.callables.TryGetValue (callable.triggerName, out callablesList)) {
			callablesList.Add (callable);
		} else {
			callablesList = new List<IMapTriggerCallable> ();
			callablesList.Add (callable);
			instance.callables [callable.triggerName] = callablesList;
		}
	}

	public static void RemoveCallable(IMapTriggerCallable callable) {
		List<IMapTriggerCallable> callablesList;
		if (instance.callables.TryGetValue (callable.triggerName, out callablesList)) {
			callablesList.Remove (callable);
		}
	}

	public static void Call(string triggerName) {
		List<IMapTriggerCallable> callablesList;
		if (instance.callables.TryGetValue (triggerName, out callablesList)) {
			foreach (IMapTriggerCallable callable in callablesList) {
				callable.Call ();
			}
		}
	}

}
