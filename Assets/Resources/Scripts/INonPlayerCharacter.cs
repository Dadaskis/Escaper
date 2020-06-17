using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class INonPlayerCharacter : MonoBehaviour {

	public Character data;

	public abstract void Move(Vector3 position);

}
