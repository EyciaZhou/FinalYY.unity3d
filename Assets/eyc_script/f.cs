using UnityEngine;
using System.Collections;

public class f {
	static float DEG_TO_RAD = Mathf.PI / 180;
	static float RAD_TO_DEG = 180 / Mathf.PI;

	static public float vector2_to_angle_deg(Vector2 vec) {
		return Mathf.Atan2 (vec.y, vec.x) * RAD_TO_DEG;
	}

	static public float sin_deg(float deg) {
		return Mathf.Sin (deg * DEG_TO_RAD);
	}

	static public float cos_deg(float deg) {
		return Mathf.Cos (deg * DEG_TO_RAD);
	}

	static public float RandomFloat(float lh, float rh) {
		return Random.Range (lh, rh);
	}
}
