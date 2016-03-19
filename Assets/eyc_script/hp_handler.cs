using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class hp_handler : MonoBehaviour
{
	public delegate void t_dead_callback ();
	public delegate bool t_cannot_dead_callback ();

	public int point = 40;
	float recovery_point = 4;
	float recovery_last;
	public int max_point = 400;
	
	List<t_dead_callback> dead_callback = new List<t_dead_callback> ();
	List<t_cannot_dead_callback> cannot_dead_callback = new List<t_cannot_dead_callback> ();

	view_interface view;

	public void bind_view(view_interface v) {
		this.view = v;
	}

	public float getRate ()
	{
		return (float)point / max_point;
	}

	public bool is_dead ()
	{
		return point == 0;
	}

	public void restart ()
	{
		point = max_point;
	}

	public void set_max_point (int maxp)
	{
		max_point = maxp;
		if (view != null) {
			view.update_view ();
		}
	}

	public void set_recovery_per_second (float recp)
	{
		recovery_point = recp;
	}

	public void recovery (int point)
	{
		if (point < 0) {
			return;
		}
		if (is_dead ()) {
			return;
		}
		this.point += point;
		if (this.point > max_point) {
			this.point = max_point;
		}
		if (view != null) {
			view.update_view ();
		}
	}

	public void hurt (int point/*, Color c1, Color c2*/)
	{
		if (point < 0) {
			return;
		}
		this.point -= point;
		/*
		 * pos = Camera.main.WorldToScreenPoint (gameObject.transform.position + Vector3.up * (mode / 2 + 1) + Vector3.left * (mode % 2 - 0.5f));
		mode = (mode + 1) % 4;
		GUI_Game.GUI_Add_Hurt (pos.x, Screen.height - pos.y, (-point).ToString (), c1, c2);
		*/

		if (view != null) {
			view.update_view ();
		}

		if (this.point <= 0) {
			this.point = 0;
			foreach (t_cannot_dead_callback cb in cannot_dead_callback) {
				if (cb ()) {
					return;
				}
			}
			CancelInvoke ("recovery_deamon");
			foreach (t_dead_callback cb in dead_callback) {
				cb ();
			}
		}
	}

	public void add_cannot_dead_callback (t_cannot_dead_callback callback)
	{
		cannot_dead_callback.Add (callback);
	}

	public void addDeadCallBack (t_dead_callback callback)
	{
		dead_callback.Add (callback);
	}

	public void reborn (int hp = -1)
	{
		CancelInvoke ("recovery_deamon");
		InvokeRepeating ("recovery_deamon", 0, 1);
		if (hp == -1) {
			point = max_point;
		} else {
			recovery (hp);
		}
	}

	void recovery_deamon ()
	{
		recovery_last += recovery_point;
		recovery ((int)(recovery_last));
		recovery_last -= (int)(recovery_last);
	}
		
	// Use this for initialization
	public void init ()
	{
		InvokeRepeating ("recovery_deamon", 0, 1);
	}
	
	// Update is called once per frame
	void Update ()
	{
	}
}
