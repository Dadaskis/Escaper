using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Es.InkPainter;

public class LightVoxelColliderPainter : MonoBehaviour {
	[SerializeField]
	private Brush brush = null;

	[SerializeField]
	private int wait = 3;

	private int waitCount;

	public void Awake()
	{
		//GetComponent<MeshRenderer>().material.color = brush.Color;
		float distance = Vector3.Distance(transform.position, new Vector3(2.0f, -1.0f, 7.0f));
		distance = 1.0f / Mathf.Pow (distance * 0.01f, 2.0f);
		brush.Color = new Color (distance, 0.0f, 0.0f);
	}

	public void FixedUpdate()
	{
		++waitCount;
	}

	public void OnCollisionStay(Collision collision)
	{
		if(waitCount < wait)
			return;
		waitCount = 0;

		foreach(var p in collision.contacts)
		{
			var canvas = p.otherCollider.GetComponent<InkCanvas>();
			if(canvas != null)
				canvas.Paint(brush, p.point);
		}
		Destroy (this);
	}
}
