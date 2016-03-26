using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class fire_ring : MonoBehaviour {
	public enum ring_status {
		full,
		hatch
	}

	ring_status status = ring_status.full;
	player p;

	LinkedList<fireball> fireball_in_ring = new LinkedList<fireball>();

	fireball hatch;
	float hatched_time;
	hourglass_manager.dot dot;

	// Use this for initialization
	void Start () {
		p = com.p;
		p.am.add_attr_calc ((mid, attr)=>{
			attr.duration_hatch_fireball_in_ring = 10;
		},"duration_hatch_fireball_in_ring");
	}
	
	// Update is called once per frame
	void Update () {
		if (status == ring_status.full) {
			if (fireball_in_ring.Count < p.exp.lv) {
				status = ring_status.hatch;
				hatch = com.fires.new_fire_ball_default ();//TODO: not default
				hatch.size = Vector3.zero;

				Debug.Log (p.am.attr.duration_hatch_fireball_in_ring);

				dot = com.ts.add_dot (p.am.attr.duration_hatch_fireball_in_ring,
					0.1f, 
					() => {
						hatch.size = Vector3.one * dot.reach_total / dot.target;
					},
					() => {
						hatch.size = Vector3.one;
						status = ring_status.full;
						fireball_in_ring.AddLast(hatch);
						hatch = null;
						dot = null;
					}
				);
			}
		}
	}
}
