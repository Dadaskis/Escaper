﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Reflection;

/// <summary>
/// Broadcast messages between objects and components, including inactive ones (which Unity doesn't do)
/// </summary>
public static class MessageUtility {

	/// <summary>
	/// Determine if the object has the given method
	/// </summary>
	private static void InvokeIfExists(this object objectToCheck, string methodName, params object[] parameters)
	{
		Type type = objectToCheck.GetType();
		MethodInfo methodInfo = type.GetMethod (methodName);
		if (type.GetMethod (methodName) != null) {
			methodInfo.Invoke(objectToCheck, parameters);
		}
	}

	/// <summary>
	/// Invoke the method if it exists in any component of the game object, even if they are inactive
	/// </summary>
	public static void BroadcastToAll(this GameObject gameobject, string methodName, params object[] parameters) {
		MonoBehaviour[] components = gameobject.GetComponents<MonoBehaviour> ();
		foreach (MonoBehaviour m in components) {
			m.InvokeIfExists(methodName, parameters);
		}
	}
	/// <summary>
	/// Invoke the method if it exists in any component of the component's game object, even if they are inactive
	/// </summary>
	public static void BroadcastToAll(this Component component, string methodName, params object[] parameters) {
		component.gameObject.BroadcastToAll (methodName, parameters);
	}

	/// <summary>
	/// Invoke the method if it exists in any component of the game object and its children, even if they are inactive
	/// </summary>
	public static void SendMessageToAll(this GameObject gameobject, bool onlyDisabled, string methodName, params object[] parameters) {
		MonoBehaviour[] components = gameobject.GetComponentsInChildren<MonoBehaviour> (true);
		foreach (MonoBehaviour m in components) {
			try {
				if (onlyDisabled && m.gameObject != null) {
					if (m.gameObject.activeInHierarchy == false) {
						m.InvokeIfExists (methodName, parameters);
					}
				} else if (!onlyDisabled) {
					m.InvokeIfExists (methodName, parameters);
				}
			} catch(System.Exception ex) {
				Debug.LogError (ex);
			}
		}
	}

	/// <summary>
	/// Invoke the method if it exists in any component of the component's game object and its children, even if they are inactive
	/// </summary>
	public static void SendMessageToAll(this Component component, bool onlyDisabled, string methodName, params object[] parameters) {
		component.gameObject.SendMessageToAll (onlyDisabled, methodName, parameters);
	}

	/// <summary>
	/// Invoke the method if it exists in any component of the game object and its ancestors, even if they are inactive
	/// </summary>
	public static void SendMessageUpwardsToAll(this GameObject gameobject, string methodName, params object[] parameters) {
		Transform tranform = gameobject.transform;
		while (tranform != null) {
			tranform.gameObject.BroadcastToAll(methodName, parameters);
			tranform = tranform.parent;
		}
	}
	/// <summary>
	/// Invoke the method if it exists in any component of the component's game object and its ancestors, even if they are inactive
	/// </summary>
	public static void SendMessageUpwardsToAll(this Component component, string methodName, params object[] parameters) {
		component.gameObject.SendMessageUpwardsToAll (methodName, parameters);
	}
}
