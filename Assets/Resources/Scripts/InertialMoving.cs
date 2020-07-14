using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InertialMoving : MonoBehaviour {

	private Quaternion SmoothDamp(Quaternion rot, Quaternion target, ref Quaternion deriv, float time) {
		if (Time.deltaTime < Mathf.Epsilon) return rot;
		// account for double-cover
		var Dot = Quaternion.Dot(rot, target);
		var Multi = Dot > 0f ? 1f : -1f;
		target.x *= Multi;
		target.y *= Multi;
		target.z *= Multi;
		target.w *= Multi;
		// smooth damp (nlerp approx)
		var Result = new Vector4(
			Mathf.SmoothDamp(rot.x, target.x, ref deriv.x, time),
			Mathf.SmoothDamp(rot.y, target.y, ref deriv.y, time),
			Mathf.SmoothDamp(rot.z, target.z, ref deriv.z, time),
			Mathf.SmoothDamp(rot.w, target.w, ref deriv.w, time)
		).normalized;

		// ensure deriv is tangent
		var derivError = Vector4.Project(new Vector4(deriv.x, deriv.y, deriv.z, deriv.w), Result);
		deriv.x -= derivError.x;
		deriv.y -= derivError.y;
		deriv.z -= derivError.z;
		deriv.w -= derivError.w;		

		return new Quaternion(Result.x, Result.y, Result.z, Result.w);
	}

	Quaternion previousRotation;
	Transform myTransform;
	Vector3 origin;
	Quaternion originRotation;

	public float rotationPower = 2.0f;
	public float decreasingFactor = 0.02f;

	void Start(){
		myTransform = transform;
		origin = myTransform.localPosition;
		originRotation = myTransform.localRotation;
	}

	void Update () {
		myTransform.rotation = previousRotation;
		Quaternion deriv = new Quaternion ();
		Quaternion result = SmoothDamp (myTransform.localRotation, originRotation, ref deriv, Time.deltaTime * rotationPower);
		result.x = Mathf.Clamp(result.x, -decreasingFactor, decreasingFactor);
		result.y = Mathf.Clamp(result.y, -decreasingFactor, decreasingFactor);
		result.z = Mathf.Clamp(result.z, -decreasingFactor, decreasingFactor);
		Quaternion currentRotation = myTransform.localRotation;
		currentRotation.x += result.x;
		currentRotation.y += result.y;
		currentRotation.z += result.z;
		myTransform.localRotation = result;
		previousRotation = myTransform.rotation;
	}
}
