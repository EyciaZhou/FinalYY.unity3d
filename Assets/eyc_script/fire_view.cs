using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class fire_view : MonoBehaviour {
	private static GameObject fire_ball_prefab;

	private Dictionary<System.Guid, fireball> fireballs = new Dictionary<System.Guid, fireball>();

	void Start() {
		fire_ball_prefab = AssetDatabase.LoadAssetAtPath<GameObject> ("Assets/fireball/normal_hit.prefab");
	}

	public fireball new_fire_ball_default() {
		fireball fb = fireball.new_fireball(
			fire_ball_prefab,
			Vector3.one,
			20,
			null,
			com.p.transform.position,
			10,
			3
		);

		fireballs.Add (fb.uuid, fb);
		return fb;
	}

	public void add_fireball(fireball fb) {
		fireballs.Add(fb.uuid, fb);
	}

	public void remove_fire_ball(fireball fb) {
		Destroy (fb.gameObject);
		fireballs.Remove (fb.uuid);
	}
}
