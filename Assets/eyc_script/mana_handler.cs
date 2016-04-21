using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class mana_handler : MonoBehaviour, controller_interface
{
	public delegate void t_mana_out_callback ();

	List<t_mana_out_callback> mana_out_callbacks = new List<t_mana_out_callback> ();

	public int point { get; private set; }
	public float recovery_per_second { get; private set; }
	public int max_point { get; private set; }

	float recovery_last;

	view_interface view;

	bool modified = false;

	public AttributesManager am{ get; set; }
	public System.Guid guid{ get; private set; }

	public mana_handler() {
		this.guid = System.Guid.NewGuid ();
	}

	public void update_controller() {
		this.max_point = am.Attr.MpUpperLimit;

		if (point > this.max_point) {
			point = this.max_point;
		}

		this.recovery_per_second = am.Attr.MpRecovery;

		modified = true;
	}

	public void bind_view(view_interface v) {
		this.view = v;
	}

	public bool is_mana_out ()
	{
		return point == 0;
	}

	public void restart ()
	{
		point = max_point;
		modified = true;
	}

	public void recovery (int point)
	{
		this.point += point;
		if (this.point > max_point) {
			this.point = max_point;
		}
		modified = true;
	}

	public bool cost (int point)
	{
		point = com.p.am.Calc_ReallyMpCost (point);
		if (point < 0) {
			point = 0;
		}
		
		if (point > this.point) {
			foreach (t_mana_out_callback cb in mana_out_callbacks) {
				cb ();
			}
			return false;
		}
		this.point -= point;
		modified = true;
		return true;
	}

	public void add_mana_out_callback(t_mana_out_callback callback) {
		mana_out_callbacks.Add (callback);
	}

	public void remove_mana_out_callback(t_mana_out_callback callback) {
		mana_out_callbacks.Remove (callback);
	}

	public void reborn (int mp = -1)
	{
		CancelInvoke ("recovery_deamon");
		InvokeRepeating ("recovery_deamon", 0, 1);
		if (mp == -1) {
			recovery (max_point);
		} else {
			recovery (mp);
		}
	}

	public void dead ()
	{
		CancelInvoke ("recovery_deamon");
		point = 0;
	}

	void recovery_deamon ()
	{
		recovery_last += recovery_per_second;
		recovery ((int)recovery_last);
		recovery_last -= (int)recovery_per_second;
	}

	// Use this for initialization
	public void init ()
	{
		this.point = this.max_point = 0x7ffffff;
		InvokeRepeating ("recovery_deamon", 0, 1);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (modified) {
			modified = false;
			view.update_view ();
		}
	}
}
