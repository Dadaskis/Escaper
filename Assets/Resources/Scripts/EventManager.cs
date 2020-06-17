using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventArguments {
	private List<object> objects;

	public EventArguments(params object[] args) {
		objects = new List<object> ();
		for (int index = 0; index < args.GetLength(0); index++) {
			objects.Add (args [index]);
		}
	}

	public void Add(object arg) {
		objects.Add (arg);
	}

	public void Remove(object arg) {
		objects.Remove (arg);
	}

	public void RemoveAt(int index) {
		objects.RemoveAt (index);
	}

	public Type Get<Type>(int index) {
		object data = objects [index];
		if (data is Type) {
			return (Type)data;
		}
		try {
			return (Type)Convert.ChangeType(data, typeof(Type));
		} catch (InvalidCastException) {
			return default(Type);
		}
	}

	public int Count {
		get {
			return objects.Count;
		}
	}

}

public class EventManager : MonoBehaviour {

	class Event : UnityEvent<EventArguments> {}

	private Dictionary<string, Event> events;

	private static EventManager manager;

	public static EventManager instance {
		get{
			if (!manager) {
				manager = FindObjectOfType (typeof(EventManager)) as EventManager;
				if (!manager) {
					Debug.LogError ("Error: EventManager is not exist.");
				} else {
					manager.Init ();
				}
			}
			return manager;
		}
	}

	void Init() {
		if (events == null) {
			events = new Dictionary<string, Event>();
		}
	}

	public static void AddEventListener(string name, UnityAction<EventArguments> listener) {
		Event thisEvent = null;
		if (instance.events.TryGetValue (name, out thisEvent)) {
			thisEvent.AddListener (listener);
		} else {
			thisEvent = new Event ();
			thisEvent.AddListener (listener);
			instance.events.Add (name, thisEvent);
		}
	}

	public static void RemoveEventListener(string name, UnityAction<EventArguments> listener) {
		if (manager == null) {
			return;
		}

		Event thisEvent = null;

		if(instance.events.TryGetValue(name, out thisEvent)) {
			thisEvent.RemoveListener (listener);			
		}
	}

	public static void RunEventListeners(string name, params object[] args) {
		Event thisEvent = null;

		if (instance.events.TryGetValue (name, out thisEvent)) {
			thisEvent.Invoke (new EventArguments(args));
		}
	}

}
