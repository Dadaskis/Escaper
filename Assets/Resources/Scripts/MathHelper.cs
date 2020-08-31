using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathHelper {

	public static Vector3 RandomVector3(float minimum = 0.0f, float maximum = 1.0f) {
		float X = Random.Range (-maximum, maximum);
		float Y = Random.Range (-maximum, maximum);
		float Z = Random.Range (-maximum, maximum);

		if (Mathf.Abs (X) < minimum) {
			if (X > 0) {
				X = -minimum;
			} else {
				X = minimum;
			}
		}

		if (Mathf.Abs (Y) < minimum) {
			if (Y > 0) {
				Y = -minimum;
			} else {
				Y = minimum;
			}
		}

		if (Mathf.Abs (Z) < minimum) {
			if (Z > 0) {
				Z = -minimum;
			} else {
				Z = minimum;
			}
		}

		Vector3 result = new Vector3 (X, Y, Z);
		return result;
	}

}
