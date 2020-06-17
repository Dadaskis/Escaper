using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableRigidbodyData {
	public bool isKinematic;
	public bool useGravity;
	public bool detectCollisions;
	public Vector3 velocity = Vector3.zero;
	public float mass;
}

[RequireComponent(typeof(SerializableTransform))]
public class SerializableRigidbody : SerializableMonoBehaviour {

	private Rigidbody body;
	private SerializableTransform transform;

	void Awake() {
		body = GetComponent<Rigidbody> ();
		transform = GetComponent<SerializableTransform> ();
	}

	void Start () {
		saveName = saveName + transform.saveName + "Rigidbody";
	}

	public override SerializableData GetSerializableData () {
		SerializableData rawData = base.GetSerializableData ();
		SerializableRigidbodyData data = new SerializableRigidbodyData ();
		data.detectCollisions = body.detectCollisions;
		data.isKinematic = body.isKinematic;
		data.useGravity = body.useGravity;
		data.velocity = body.velocity;
		data.mass = body.mass;
		rawData.target = data;
		rawData.type = typeof(SerializableRigidbody);
		return rawData;
	}

	public override void SetSerializableData (SerializableData rawData) {
		SerializableRigidbodyData data = ConvertTargetObject<SerializableRigidbodyData> (rawData.target);
		body.velocity = data.velocity;
		body.useGravity = data.useGravity;
		body.detectCollisions = data.detectCollisions;
		body.isKinematic = data.isKinematic;
		body.mass = data.mass;
	}

}
