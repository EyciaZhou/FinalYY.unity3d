using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class hp_handler : MonoBehaviour, controller_interface
{
	public delegate void t_dead_callback ();
	public delegate bool t_cannot_dead_callback ();

	public int point { get; private set; }
	public int max_point { get; private set; }
	public float recovery_per_second { get; private set; }

	private float recovery_last;
	private bool modified = false;
	
	List<t_dead_callback> dead_callback = new List<t_dead_callback> ();
	List<t_cannot_dead_callback> cannot_dead_callback = new List<t_cannot_dead_callback> ();

	view_interface view;

	public attributes_manager am{ get; set; }
	public System.Guid guid { get; private set; }

	public hp_handler() {
		this.guid = System.Guid.NewGuid ();
	}

	public void update_controller() {
//		int lst_mx = this.max_point;
		this.max_point = am.attr.max_hp;

//		if (this.max_point - lst_mx > 0) {
//			recovery (this.max_point - lst_mx);
//		}

		if (point > this.max_point) {
			point = this.max_point;
		}

		this.recovery_per_second = am.attr.recovery_per_second;

		modified = true;
		Debug.Log (this.point);
		Debug.Log ("hp update_controller");
	}

	public float getRate() {
		return (float)point / max_point;
	}

	public void bind_view(view_interface v) {
		this.view = v;
	}

	public bool is_dead ()
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
		modified = true;

	}

	public void hurt (int point/*, Color c1, Color c2*/)
	{
		if (point < 0) {
			return;
		}
		if (this.point <= 0) {
			return;
		}
		this.point -= point;
		/*
		 * pos = Camera.main.WorldToScreenPoint (gameObject.transform.position + Vector3.up * (mode / 2 + 1) + Vector3.left * (mode % 2 - 0.5f));
		mode = (mode + 1) % 4;
		GUI_Game.GUI_Add_Hurt (pos.x, Screen.height - pos.y, (-point).ToString (), c1, c2);
		*/

		modified = true;

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

	public void add_dead_callback (t_dead_callback callback)
	{
		dead_callback.Add (callback);
	}

	public void reborn (int hp = -1)
	{
		CancelInvoke ("recovery_deamon");
		InvokeRepeating ("recovery_deamon", 0, 1);
		if (hp == -1) {
			recovery (max_point);
		} else {
			recovery (hp);
		}
	}

	void recovery_deamon ()
	{
		recovery_last += recovery_per_second;
		recovery ((int)(recovery_last));
		recovery_last -= (int)(recovery_last);
	}
		
	// Use this for initialization
	public void init (int hp)
	{
		this.point = this.max_point = hp;
		InvokeRepeating ("recovery_deamon", 0, 1);
	}

	void LateUpdate() {
		if (point == 0) {
			hurt (0);
		}

		if (modified) {
			//Debug.Log ("hp");

			modified = false;

			if (view != null) {
				view.update_view ();
			}
		}
	}
}
