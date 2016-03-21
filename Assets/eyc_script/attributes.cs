using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class attributes : MonoBehaviour {
	public delegate void t_attr_calc (mid_attributes mid, attributes attr);
	public delegate void t_buff_change_callback ();

	public int max_hp;
	public int max_mp;
	public float speed;

	public float recovery_per_second;
	public float recovery_mana_per_second;

	public float exp_mutiply;
	public int exp_extra;

	private mid_attributes mid;

	public class mid_attributes {
		public int strength;
		public int agility;
		public int intelligence;

		public float speed_base;
		public int speed_extra;
		public float speed_mutiply;

		public float exp_mutiply;
		public int exp_extra;
	}

	public Dictionary<string, Object> attr_dic = new Dictionary<string, Object> ();
	private Dictionary<string, t_attr_calc> attr_calc = new Dictionary<string, t_attr_calc> ();

	private Dictionary<System.Guid, controller_interface> controllers = new Dictionary<System.Guid, controller_interface>();

	private Dictionary<System.Guid, buff_interface> buff_sort_with_guid = new Dictionary<System.Guid, buff_interface>();
	private List<buff_interface> buff_sort_with_priority = new List<buff_interface>();

	private bool buff_changed;

	private void _calculate_buff_to_mid() {
		mid_attributes mid = new mid_attributes ();
		foreach (buff_interface buff in buff_sort_with_priority) {
			buff.calculate (mid);
		}
		this.mid = mid;
	}

	private void _calculate_mid_to_attr() {
		attr_dic.Clear ();

		foreach (t_attr_calc calc in attr_calc.Values) {
			calc (this.mid, this);
		}
	}

	public void re_calculate() {
		_calculate_buff_to_mid ();
		_calculate_mid_to_attr ();

		foreach (controller_interface ci in controllers.Values) {
			ci.update_controller ();
		}
	}

	public buff_interface add_buff(buff_interface bi, int priority) {
		bi.priority = priority;
		return add_buff (bi);
	}

	public buff_interface add_buff(buff_interface bi) {
		bi.attr = this;
		bi.change_callback = on_buff_change;

		buff_sort_with_guid.Add (bi.guid, bi);
		buff_sort_with_priority.Add (bi);
		buff_sort_with_priority.Sort ((buff_interface x, buff_interface y) => x.priority.CompareTo(y.priority));
		return bi;
	}

	public bool remove_buff(buff_interface bi) {
		if (controllers.ContainsKey(bi.guid)) {
			buff_sort_with_priority.Remove (bi);
			buff_sort_with_guid.Remove (bi.guid);
			return true;
		}
		return false;
	}

	public controller_interface bind_controller(controller_interface ci) {
		ci.attr = this;
		controllers.Add (ci.guid, ci);
		return ci;
	}

	public bool remove_controller(controller_interface ci) {
		if (controllers.ContainsKey (ci.guid)) {
			controllers.Remove (ci.guid);
			return true;
		}
		return false;
	}

	private void on_buff_change() {
		buff_changed = true;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (buff_changed) {
			re_calculate ();
			buff_changed = false;
		}
	}
}
