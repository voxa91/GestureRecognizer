using UnityEngine;
using System.Collections;

public class Alignment : MonoBehaviour {
	public enum vAlignment { none, top, center, bottom }
	public enum hAlignment { none, left, center, right }

	public vAlignment vAlign = vAlignment.none;
	public hAlignment hAlign = hAlignment.none;

	public float xOffset = 0f;
	public float yOffset = 0f;

	private void SetAlignment () {
		Vector3 pos = transform.position;
		if (hAlign == hAlignment.left) {
			pos.x = GetScreen.left;
		} else if (hAlign == hAlignment.right) {
			pos.x = GetScreen.right;
		} else if (hAlign == hAlignment.center) {
			pos.x = (GetScreen.right + GetScreen.left) * 0.5f;
		}

		if (vAlign == vAlignment.top) {
			pos.y = GetScreen.top;
		} else if (vAlign == vAlignment.bottom) {
			pos.y = GetScreen.bottom;
		} else if (vAlign == vAlignment.center) {
			pos.y = (GetScreen.top + GetScreen.bottom) * 0.5f;
		}

		transform.position = pos + new Vector3 (xOffset, yOffset);
	}

	void Start () {
		SetAlignment ();
	}
}
