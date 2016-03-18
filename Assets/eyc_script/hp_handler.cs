using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class hp_handler : MonoBehaviour
{
	public delegate void TdeadCallback ();

	public delegate bool Tcannot_dead_callback ();

	public int point = 40;
	float recovery_point = 4;
	float recovery_last;
	public int max_point = 400;
	/*
	bool show;
	string val;
	Vector3 pos;*/

	//public int mode = 0;
	
	List<TdeadCallback> deadCallback = new List<TdeadCallback> ();
	List<Tcannot_dead_callback> cannot_dead_callback = new List<Tcannot_dead_callback> ();

	public float getRate ()
	{
		return (float)point / max_point;
	}

	public bool isDead ()
	{
		return point == 0;
	}

	public void restart ()
	{
		point = max_point;
	}

	public void setMaxPoint (int maxp)
	{
		max_point = maxp;
	}

	public void setRecovery (float recp)
	{
		recovery_point = recp;
	}

	public void recovery (int point)
	{
		if (point < 0) {
			return;
		}
		if (isDead ()) {
			return;
		}
		this.point += point;
		if (this.point > max_point) {
			this.point = max_point;
		}
	}

	public void hurt (int point)
	{
		hurt (point, Color.yellow, Color.red);
	}

	public void hurt (int point, Color c1, Color c2)
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

		if (this.point <= 0) {
			this.point = 0;
			CancelInvoke ("recovery_deamon");
			foreach (Tcannot_dead_callback cb in cannot_dead_callback) {
				if (cb ()) {
					return;
				}
			}
			foreach (TdeadCallback cb in deadCallback) {
				cb ();
			}
		}
	}

	public void add_cannot_dead_callback (Tcannot_dead_callback callback)
	{
		cannot_dead_callback.Add (callback);
	}

	public void addDeadCallBack (TdeadCallback callback)
	{
		deadCallback.Add (callback);
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
