using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDamagablePart : IDamagable {

	public Character character;
	public float multiplier = 1.0f;
	
	public override void Damage(int damage, Character shooter){
		character.Damage ((int)(damage * multiplier), shooter);
	}
}
