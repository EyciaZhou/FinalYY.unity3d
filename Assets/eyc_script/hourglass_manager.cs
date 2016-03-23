using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class hourglass_manager : MonoBehaviour {
	const float eps = 0.0000001;

	delegate void callback();

	public class hourglass {

		float target;
		float reach;
		callback cb;

		public hourglass(float target, callback cb) {
			this.target = target;
			this.reach = 0;
			this.cb = cb;
		}

		public bool update_time_and_rise_if_necessary() {
			reach += Time.deltaTime;
			if (reach >= target) {
				cb ();
				Destroy (this);
				return true;
			}
			return false;
		}
	}

	private Dictionary<System.Guid, hourglass> hourglasses = new Dictionary<System.Guid, hourglass> ();

	void Update () {
		List<System.Guid> lst_remove = new List<System.Guid> ();

		foreach (KeyValuePair<System.Guid, hourglass> kp in hourglasses) {
			if (kp.Value == null || !kp.Value.update_time_and_rise_if_necessary ()) {
				hourglasses [kp.Key] = null;

				lst_remove.Add (kp.Key);
			}
		}

		foreach (System.Guid id in lst_remove) {
			hourglasses.Remove (id);
		}
	}

	public void cancle_hourglass(System.Guid id) {
		hourglasses.Remove (id);
	}

	public System.Guid add_hourglass(float total_time, callback cb) {
		System.Guid id = System.Guid.NewGuid ();
		hourglasses.Add(id, new hourglass(total_time, cb));
		return id;
	}

	public System.Guid add_dot_start_now(float total_time, float duration, callback cb_each, callback when_end) { 
		cb_each ();
		return add_dot (total_time, duration, cb_each, when_end);
	}

	public System.Guid add_dot_start_now(int total_times, float duration, callback cb_each, callback when_end) {
		cb_each ();
		return add_dot (total_times, duration, cb_each, when_end);
	}

	public System.Guid add_dot(float total_time, float duration, callback cb_each, callback when_end) { 
	
	}

	public System.Guid add_dot(int total_times, float duration, callback cb_each, callback when_end) {
		return add_dot (total_times * duration + eps, duration, cb_each, when_end);
	}
}
