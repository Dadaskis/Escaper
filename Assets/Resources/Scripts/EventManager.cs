using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventData {
	public List<object> objects;

	public EventData(params object[] args) {
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


	public delegate EventData EventMethodType(EventData args);

	public class LocalEvent {
		public List<EventMethodType> methods = new List<EventMethodType>();

		public void AddListener (EventMethodType method) {
			methods.Add (method);
		}

		public void RemoveListener(EventMethodType method) {
			methods.Remove (method);
		}

		public void Invoke(EventData args, ref EventData returned) {
			foreach (EventMethodType method in methods) {
				try{
					EventData data = method (args);
					if (data != null) {
						returned.objects.AddRange (data.objects);
					}
				} catch (System.Exception ex) {
					Debug.LogError (ex);
				}
			}
		}

		public EventData Invoke(EventData args) {
			EventData data = new EventData ();
			Invoke (args, ref data);
			return data;
		}

		public EventData Invoke(params object[] args) {
			return Invoke (new EventData(args));
		}
	}


	private Dictionary<string, LocalEvent> events;

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
			events = new Dictionary<string, LocalEvent>();
		}
	}

	public static void AddEventListener(string name, EventMethodType listener) {
		LocalEvent thisEvent = null;
		if (instance.events.TryGetValue (name, out thisEvent)) {
			thisEvent.AddListener (listener);
		} else {
			thisEvent = new LocalEvent ();
			thisEvent.AddListener (listener);
			instance.events.Add (name, thisEvent);
		}
	}

	public static void AddEventListener<EventType>(EventMethodType listener) {
		string name = typeof(EventType).Name;
		AddEventListener (name, listener);
	}

	public static void RemoveEventListener(string name, EventMethodType listener) {
		if (manager == null) {
			return;
		}

		LocalEvent thisEvent = null;

		if(instance.events.TryGetValue(name, out thisEvent)) {
			thisEvent.RemoveListener (listener);			
		}
	}

	public static void RemoveEventListener<EventType> (EventMethodType listener) {
		string name = typeof(EventType).Name;
		RemoveEventListener (name, listener);
	}

	public static EventData RunEventListeners(string name, params object[] args) {
		LocalEvent thisEvent = null;
		EventData returnValues = new EventData ();
		if (instance.events.TryGetValue (name, out thisEvent)) {
			thisEvent.Invoke (new EventData(args), ref returnValues);
		}
		return returnValues;
	}

	public static EventData RunEventListeners<EventType>(params object[] args) {
		string name = typeof(EventType).Name;
		return RunEventListeners (name, args);
	}

}
