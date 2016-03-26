using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class hourglass_manager : MonoBehaviour {
	const float eps = 0.0000001f;

	public delegate void callback();

	public class hourglass {
		public float target { get; private set; }
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
		public float target { get; private set; }
		public float duration { get; private set; }
		public float reach { get; private set; }
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

	public Dictionary<System.Guid, hourglass> hourglasses { get; private set; }
	public Dictionary<System.Guid, dot> dots { get; private set; }

	public hourglass_manager() {
		this.hourglasses = new Dictionary<System.Guid, hourglass> ();
		this.dots = new Dictionary<System.Guid, dot> ();
	}

	void Update () {
		List<System.Guid> lst_remove = new List<System.Guid> ();

		System.Guid[] keys = new System.Guid[hourglasses.Count];
		hourglasses.Keys.CopyTo (keys, 0);

		foreach (System.Guid k in keys) {
			if (hourglasses[k] == null || hourglasses[k].update_time_and_rise_if_necessary ()) {
				//hourglasses [kp.Key] = null;
				lst_remove.Add (k);
			}
		}

		foreach (System.Guid id in lst_remove) {
			hourglasses.Remove (id);
		}

		if (lst_remove.Count > 0) {
			Debug.Log (lst_remove.Count + "");
		}

		lst_remove.Clear ();

		foreach (KeyValuePair<System.Guid, dot> kp in dots) {
			if (kp.Value == null || kp.Value.update_time_and_rise_if_necessary ()) {
				//dots [kp.Key] = null;
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
		//Debug.Log ("add hourglass");

		System.Guid id = System.Guid.NewGuid ();
		hourglasses.Add(id, new hourglass(total_time, cb));

		//Debug.Log (hourglasses.Count + "");
		//Debug.Log (id + "");
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
