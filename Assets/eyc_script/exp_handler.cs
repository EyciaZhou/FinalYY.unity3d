using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class exp_handler : MonoBehaviour , controller_interface, buff_interface
{
	public delegate void t_lv_up_callback ();

	public float current { get; private set; }
	public long limit { get; private set; }
	public int lv { get; private set; }

	List<t_lv_up_callback> callbacks = new List<t_lv_up_callback> ();

	public attributes_manager am{ get; set; }
	private view_interface view;

	private bool modified = false;

	public void calculate (attributes_manager.t_mid_attributes mid)
	{
		//TODO:
		mid.intelligence += 10 * lv;
		mid.agility += 10 * lv;
		mid.strength += 10 * lv;
	}

	public void bind_view (view_interface v)
	{
		this.view = v;
	}

	public attributes_manager.t_buff_change_callback change_callback { get; set; }
	public int priority { get; set; }

	public bool vaild {
		get {
			return !com.p.hp.is_dead ();
		}
	}

	public void update_controller ()
	{
		//pass
	}

	public System.Guid guid { get; private set; }

	public exp_handler() {
		this.guid = System.Guid.NewGuid ();
		clear ();
		priority = 0; //TODO:
	}

	private long exp_calu(int lv) {
		return 10 * lv * lv; //TODO:
	}

	public void clear ()
	{
		lv = 3;
		current = 0;
		limit = exp_calu (lv);
	}

	void Update ()
	{
	
	}

	public void init ()
	{
		clear ();
	}

	void _lvup ()
	{
		lv++;

		limit = exp_calu (lv);
		foreach (t_lv_up_callback cb in callbacks) {
			cb ();
		}
	}

	public void lv_up (long mach)
	{
		for (int i = 0; i < mach; i++) {
			_lvup ();
		}
		modified = true;
	}

	public void gain_exp (int exp)
	{
		current += exp * am.attr.exp_mutiply + am.attr.exp_extra;
		while (current >= limit) {
			current -= limit;
			_lvup ();
		}
		modified = true;
	}

	public void add_lv_up_callback (t_lv_up_callback cb)
	{
		callbacks.Add(cb);
	}

	public void remove_lv_up_callback (t_lv_up_callback cb)
	{
		callbacks.Remove (cb);
	}

	void LateUpdate() {
		if (modified) {
			modified = false;
			view.update_view ();
		}
	}
}
