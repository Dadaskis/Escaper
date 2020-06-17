using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InputKey {
	public KeyCode key;
	public string name;
	public string normalName;
}

[CreateAssetMenu(fileName = "Create input data", menuName = "New input data")]
public class InputData : ScriptableObject {
	public List<InputKey> keys;
}

