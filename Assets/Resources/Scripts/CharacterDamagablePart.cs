using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDamagablePart : MonoBehaviour {

	public Character character;
	public float multiplier = 1.0f;
	
	public void Damage(int damage, Character shooter){
		character.Damage ((int)(damage * multiplier), shooter);
	}
}
