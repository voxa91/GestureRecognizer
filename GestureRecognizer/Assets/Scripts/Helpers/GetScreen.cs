using UnityEngine;
using System.Collections;

public static class GetScreen {
	public static float left {
		get {
			return Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x;
		}
	}

	public static float right {
		get {
			return Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0)).x;
		}
	}

	public static float bottom {
		get {
			return Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).y;
		}
	}

	public static float top {
		get {
			return Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0)).y;
		}
	}

	public static float width {
		get {
			return right - left;
		}
	}

	public static float height {
		get {
			return top - bottom;
		}
	}
}
