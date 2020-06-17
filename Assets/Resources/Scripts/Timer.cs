using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

class TimerData {
	public float time = 0;
	public float limit = 0;
	public int repetitions = -1;
	public UnityEvent listener;
	public bool worked = false;

	public void Update(float deltaTime) {
		time -= deltaTime;
		if (time < 0 && !worked) {
			listener.Invoke ();
			if (repetitions != -1) {
				repetitions -= 1;
				if (repetitions < 1) {
					worked = true;
				} else {
					time = limit;
				}
			} else {
				time = limit;
			}
		}
	}
}

public class Timer : MonoBehaviour {
	private Dictionary<string, TimerData> events = new Dictionary<string, TimerData>();
	private List<string> removeList = new List<string>();

	private static Timer manager;

	public static Timer instance {
		get{
			if (!manager) {
				manager = FindObjectOfType (typeof(Timer)) as Timer;
				if (!manager) {
					Debug.LogError ("Timer is not exist.");
				} else {
					manager.Init ();
				}
			}
			return manager;
		}
	}

	void Init() {
		
	}

	void Frame() {
		foreach (KeyValuePair<string, TimerData> pair in instance.events) {
			TimerData data = pair.Value;
			data.Update (Time.deltaTime);
			if (data.worked) {
				removeList.Add (pair.Key);
			}
		}
		foreach(string index2 in removeList){
			events.Remove (index2);
		}
		removeList.Clear ();
	}

	public static void Create(UnityAction listener, string name, float time, int repetitions = -1) {
		TimerData data = new TimerData ();
		data.limit = time;
		data.time = time;
		data.repetitions = repetitions;
		UnityEvent listener2 = new UnityEvent();
		listener2.AddListener (listener);
		data.listener = listener2;
		instance.events [name] = data;

	}

	public static void Remove(string name) {
		instance.events.Remove (name);
	}

	void Update() {
		instance.Frame ();
	}
}
