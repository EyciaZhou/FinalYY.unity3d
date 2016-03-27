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

	static public void slow_follow(Transform transform, Vector3 target_pos, Vector3 traget_ea, float speed) {
		if (speed * Time.deltaTime > 1) {
			transform.position = target_pos;
			transform.eulerAngles = traget_ea;
		} else {
			transform.position = Vector3.Slerp (transform.position, target_pos, speed * Time.deltaTime);
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.Euler (traget_ea), speed * Time.deltaTime);
		}
	}

	static public void slow_follow_localpostion(Transform transform, Vector3 target_pos, float speed) {
		if (speed * Time.deltaTime > 1) {
			transform.localPosition = target_pos;
		} else {
			transform.localPosition = Vector3.Slerp (transform.localPosition, target_pos, speed * Time.deltaTime);
		}
	}
}
