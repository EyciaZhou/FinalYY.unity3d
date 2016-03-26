using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * map buffs to attributes value
 * and tell controllers when attribute changes
 * 
 * buff effect the mid-attributes
 * mid-attributes effect the attributes
 * mid-attribute can minus or plus with independent order
 * and calculate attributes from mid_attributes use
 * series t_attr_calc.
 * 
 * if the attributes is not the field of this class(not built-in)
 * can add such attributei into attr_dic.
 * 
 * for example, the attribute is speed of player, 
 * so the mid-attribute of it may the 
 * speed_base, speed_addition and speed_mutiply
 * and the function to calculate speed with type of t_attr_calc is
 * 
 * void calc_speed(t_mid_attributes mid, t_attributes attr) {
 * 		attr.speed = attr.attr_dic["speed_in_dic"] = 
 * 			(mid.speed_base + mid.speed_addition) * mid.speed_mutiply;
 * }
 * 
 * API:
 * 
 * 		void re_calculate();
 * 
 * 		buff_interface add_buff(buff_interface buff, int priority);
 * 		buff_interface add_buff(buff_interface buff); -> use priority in the "buff"
 * 		bool remove_buff(buff_interface to_remove);
 * 
 * 		controller_interface bind_controller(controller_interface c);
 * 		bool remove_controller(controller_interface c);
 * 
 * 		System.Guid add_attr_calc(t_attr_calc, string name);
 * 		bool remove_attr_calc(System.Guid to_remove);
 * 
 */
public class attributes_manager : MonoBehaviour {
	public delegate void t_attr_calc (t_mid_attributes mid, t_attributes attr);
	public delegate void t_buff_change_callback ();

	public t_attributes attr { get; private set; }

	public class t_attributes {
		public int max_hp;
		public int max_mp;
		public float speed;

		public float recovery_per_second;
		public float recovery_mana_per_second;

		public float exp_mutiply;
		public int exp_extra;

		public float duration_hatch_fireball_in_ring;

		public Dictionary<string, Object> attr_dic = new Dictionary<string, Object> ();
	}

	public class t_mid_attributes {
		public int strength;
		public int agility;
		public int intelligence;

		public float speed_base;
		public int speed_addition;
		public float speed_mutiply;

		public float exp_mutiply;
		public int exp_extra;
	}

	private t_mid_attributes mid;
	private Dictionary<System.Guid, string> attr_calc_name = new Dictionary<System.Guid, string>();
	private Dictionary<System.Guid, t_attr_calc> attr_calc = new Dictionary<System.Guid, t_attr_calc> ();

	private Dictionary<System.Guid, controller_interface> controllers = new Dictionary<System.Guid, controller_interface>();
	//private Dictionary<System.Guid, buff_interface> buff_sort_with_guid = new Dictionary<System.Guid, buff_interface>();
	private List<buff_interface> buff_sort_with_priority = new List<buff_interface>();

	private bool buff_changed;

	//---------------------calc progress------------------

	private void _calculate_buff_to_mid() {
		t_mid_attributes mid = new t_mid_attributes ();
		foreach (buff_interface buff in buff_sort_with_priority) {
			buff.calculate (mid);
		}
		this.mid = mid;
	}

	private void _calculate_mid_to_attr() {
		t_attributes attr = new t_attributes ();
		foreach (t_attr_calc calc in attr_calc.Values) {
			calc (this.mid, attr);
		}
		this.attr = attr;
	}

	private void _update_controllers() {
		foreach (controller_interface ci in controllers.Values) {
			ci.update_controller ();
		}
	}

	public void re_calculate() {
		_calculate_buff_to_mid ();
		_calculate_mid_to_attr ();
		_update_controllers ();
	}

	//-------------------------buff----------------------------

	public buff_interface add_buff(buff_interface bi, int priority) {
		bi.priority = priority;
		return add_buff (bi);
	}

	public buff_interface add_buff(buff_interface bi) {
		bi.am = this;
		bi.change_callback = on_buff_change;

		//buff_sort_with_guid.Add (bi.guid, bi);
		buff_sort_with_priority.Add (bi);
		buff_sort_with_priority.Sort ((buff_interface x, buff_interface y) => x.priority.CompareTo(y.priority));
		return bi;
	}

	public bool remove_buff(buff_interface bi) {
		if (controllers.ContainsKey(bi.guid)) {
			buff_sort_with_priority.Remove (bi);
			//buff_sort_with_guid.Remove (bi.guid);
			return true;
		}
		return false;
	}

	//-------------------------controller----------------------------


	public controller_interface bind_controller(controller_interface ci) {
		ci.am = this;
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

	//-------------------------attr_calc----------------------------

	public System.Guid add_attr_calc(t_attr_calc ac, string name) {
		System.Guid id = System.Guid.NewGuid ();

		attr_calc.Add (id, ac);
		attr_calc_name.Add (id, name);

		return id;
	}

	public bool remove_attr_calc(System.Guid to_remove){
		if (attr_calc.ContainsKey(to_remove)) {
			attr_calc.Remove (to_remove);
			attr_calc_name.Remove (to_remove);
			return true;
		}
		return false;
	}
		
	//-----------------------etc---------------------------

	private void on_buff_change() {
		buff_changed = true;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (buff_changed) {
			re_calculate ();
			buff_changed = false;
		}
	}
}
