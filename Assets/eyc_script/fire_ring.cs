using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class fire_ring : MonoBehaviour {
	public enum ring_status {
		full,
		hatch
	}

	ring_status status = ring_status.full;

	//if status is not full, last is in progress of hatch
	public LinkedList<fireball> fireball_in_ring {get;private set;}

	fireball hatch;
	float hatched_time;
	hourglass_manager.dot dot;
	player p;

	private fire_ring_view view;

	public fire_ring() {
		fireball_in_ring = new LinkedList<fireball> ();
	}

	// Use this for initialization
	void Start () {
		p = com.p;
		view = com.p.fr_view;
	}
	
	// Update is called once per frame
	void Update () {
		if (status == ring_status.full) {
			if (fireball_in_ring.Count < p.exp.lv) {
				status = ring_status.hatch;

				hatch = com.fires.new_fireball_in_ring_default ();
				hatch.scale = 0;
				view.add_into_ring (hatch.gameObject);
				fireball_in_ring.AddLast(hatch);
				view.update_postion ();

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
	}
}
