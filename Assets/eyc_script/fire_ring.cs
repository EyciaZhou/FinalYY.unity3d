using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class fire_ring : MonoBehaviour {
	const float ANG_V_DEG = 100;
	const float R = 1.2f;

	public enum ring_status {
		full,
		hatch
	}

	ring_status status = ring_status.full;
	player p;

	//if status is not full, last is in progress of hatch
	LinkedList<fireball> fireball_in_ring = new LinkedList<fireball>();
	List<Vector3> fireball_in_ring_local_should = new List<Vector3> ();

	fireball hatch;
	float hatched_time;
	hourglass_manager.dot dot;

	private GameObject ring;

	// Use this for initialization
	void Start () {
		p = com.p;
		p.am.add_attr_calc ((mid, attr)=>{
			attr.duration_hatch_fireball_in_ring = 10;
		},"duration_hatch_fireball_in_ring");

		ring = new GameObject ("ring");

		ring.transform.localPosition = Vector3.back * 0.5f + Vector3.up * 1.4f;
		ring.transform.localEulerAngles = Vector3.zero;
	}

	void _update_postion() {
		float deg_each = 360f / fireball_in_ring.Count;

		fireball_in_ring_local_should = new List<Vector3> (fireball_in_ring.Count);

		int k = 0;
		for (LinkedListNode<fireball> fb = fireball_in_ring.First; fb != null; fb = fb.Next) {
			fireball_in_ring_local_should.Add(new Vector3 (f.sin_deg (deg_each * k) * R, f.cos_deg (deg_each * k) * R));
			k++;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (status == ring_status.full) {
			if (fireball_in_ring.Count < p.exp.lv) {
				status = ring_status.hatch;

				hatch = com.fires.new_fire_ball_default ();//TODO: not default
				hatch.scale = 0;
				hatch.transform.SetParent (ring.transform);
				hatch.transform.localPosition = Vector3.zero;

				fireball_in_ring.AddLast(hatch);
				_update_postion ();

				dot = com.ts.add_dot (p.am.attr.duration_hatch_fireball_in_ring,
					0.1f, 
					() => {
						hatch.scale = dot.reach_total / dot.target;
					},
					() => {
						hatch.scale = 1;
						status = ring_status.full;
						hatch = null;
						dot = null;
					}
				);
			}
		}

		int k = 0;
		for (LinkedListNode<fireball> fb = fireball_in_ring.First; fb != null; fb = fb.Next) {
			f.slow_follow_localpostion (fb.Value.transform, fireball_in_ring_local_should [k], 0.1f);
			k++;	
		}//TODO: stop when finish

		f.slow_follow (ring.transform, p.transform.TransformPoint (0, 1.4f, -0.8f), p.transform.rotation.eulerAngles + Vector3.forward * Time.time * ANG_V_DEG, 5f);
	}
}
