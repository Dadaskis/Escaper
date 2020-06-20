using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GUIDragable : MonoBehaviour, IDragHandler  {
	RectTransform rectTransform;
	Canvas canvas;

	public void FindNeededData() {
		if (rectTransform == null) {
			rectTransform = GetComponent<RectTransform> ();
		}

		if (canvas == null) {
			canvas = GetComponentInParent<Canvas> ();
		}
	}

	public void OnDrag (PointerEventData eventData) {
		FindNeededData ();

		rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
		transform.SetAsLastSibling ();
	}
}
