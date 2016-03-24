using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class hourglass_manager : MonoBehaviour {
	const float eps = 0.0000001f;

	public delegate void callback();

	public class hourglass {
		float target;
		callback cb;
		bool _pause = false;

		public hourglass(float target, callback cb) {
			this.target = target;
			this.cb = cb;
		}

		public bool update_time_and_rise_if_necessary() {
			if (_pause)
				return false;
			target -= Time.deltaTime;
			if (target <= 0) {
				pause ();
				cb ();
				return true;
			}
			return false;
		}

		public void pause() {
			_pause = true;
		}
	}

	public class dot
	{
		float target;
		float duration;
		float reach;
		callback cb_each;
		callback when_end;
		bool _pause = false;

		public dot(float target, float duration, callback cb_each, callback when_end) {
			this.target = target;
			this.reach = 0;
			this.duration = duration;
			this.cb_each = cb_each;
			this.when_end = when_end;
		}

		public bool update_time_and_rise_if_necessary() {
			if (_pause)
				return false;
			reach += Time.deltaTime;
			target -= Time.deltaTime;
			if (reach >= duration) {
				reach -= duration;
				cb_each ();
			}
			if (target <= 0) {
				when_end ();
				return true;
			}
			return false;
		}
	}

	private Dictionary<System.Guid, hourglass> hourglasses { get; set; }
	private Dictionary<System.Guid, dot> dots { get; set; }

	public hourglass_manager() {
		this.hourglasses = new Dictionary<System.Guid, hourglass> ();
		this.dots = new Dictionary<System.Guid, dot> ();
	}

	void Update () {
		List<System.Guid> lst_remove = new List<System.Guid> ();

		foreach (KeyValuePair<System.Guid, hourglass> kp in hourglasses) {
			if (kp.Value == null || !kp.Value.update_time_and_rise_if_necessary ()) {
				//hourglasses [kp.Key] = null;
				lst_remove.Add (kp.Key);
			}
		}

		foreach (System.Guid id in lst_remove) {
			hourglasses.Remove (id);
		}

		lst_remove.Clear ();

		foreach (KeyValuePair<System.Guid, dot> kp in dots) {
			if (kp.Value == null || !kp.Value.update_time_and_rise_if_necessary ()) {
				dots [kp.Key] = null;
				lst_remove.Add (kp.Key);
			}
		}

		foreach (System.Guid id in lst_remove) {
			dots.Remove (id);
		}
	}

	public void cancle_hourglass(System.Guid id) {
		if (hourglasses.ContainsKey (id)) {
			hourglasses [id].pause ();
			hourglasses.Remove (id);
		}
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
		System.Guid id = System.Guid.NewGuid ();
		dots.Add(id, new dot(total_time, duration, cb_each, when_end));
		return id;
	}

	public System.Guid add_dot(int total_times, float duration, callback cb_each, callback when_end) {
		return add_dot (total_times * duration + eps, duration, cb_each, when_end);
	}
}
