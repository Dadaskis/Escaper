using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IUsableObject : SerializableMonoBehaviour {

	public abstract string ShowText();

	public abstract void Use ();

}
