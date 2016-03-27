using UnityEngine;
using System.Collections;

public class skill : MonoBehaviour, controller_interface {
	public GameObject bar_go;

	private bool small_fireball_colddown = true;

	public void small_fireball_from_bar(Quaternion rotation) {
		if (small_fireball_colddown) {
			if (com.p.mp.cost (am.attr.small_fireball_cost)) {
				com.ts.add_hourglass (
					am.attr.small_fireball_coldtime, 
					() => {
						small_fireball_colddown = true;
					}
				);
				com.fires.new_fireball_default (bar_go.transform, rotation, am.attr.small_fireball_hurt);
				small_fireball_colddown = false;
			}
		}
	}

	#region controller_interface implementation
	public void update_controller () {}
	public void bind_view (view_interface v) {}
	public attributes_manager am { get ; set ; }
	public System.Guid guid { get ; private set; }
	#endregion

	public skill() {
		guid = System.Guid.NewGuid ();
	}

	void Start() {
		am = com.p.am;
		am.bind_controller (this);

		bar_go = GameObject.Find ("bar_local");

		am.add_attr_calc((mid, attr) => {
			attr.small_fireball_cost = mid.small_fireball_level * 5 + 30;
			attr.small_fireball_coldtime = 1.0f / (mid.small_fireball_level+1);
			attr.small_fireball_hurt = 10 * mid.small_fireball_level + 0.1f * mid.intelligence;
		}, "small_fireball");
	}
}
