﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class mana_handler : MonoBehaviour
{
	public delegate void TmanaOutCallback ();

	public long point = 100;
	float recovery_point;
	float recovery_last;
	public long max_point = 400;

	List<TmanaOutCallback> manaOutCallback = new List<TmanaOutCallback> ();

	public float getRate ()
	{
		return (float)point / max_point;
	}

	public bool IsManaOut ()
	{
		return point == 0;
	}

	public void restart ()
	{
		point = max_point;
	}

	public void setMaxPoint (long maxp)
	{
		max_point = maxp;
	}

	public void setRecovery (float recp)
	{
		recovery_point = recp;
	}

	public void recovery (long point)
	{
		this.point += point;
		if (this.point > max_point) {
			this.point = max_point;
		}
	}

	public bool cost (long point)
	{
		if (point < 0) {
			return false;
		}
		
		if (point > this.point) {
			for (int i = 0; i < manaOutCallback.Count; i++) {
				manaOutCallback [i] ();
			}
			return false;
		}
		this.point -= point;
		return true;
	}

	public void setManaoutCallBack (TmanaOutCallback callback)
	{
		manaOutCallback.Add (callback);
	}

	public void reborn (long mp = -1)
	{
		CancelInvoke ("recovery_deamon");
		InvokeRepeating ("recovery_deamon", 0, 1);
		if (mp == -1) {
			point = max_point;
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
		recovery_last += recovery_point;
		recovery ((long)(recovery_last));
		recovery_last -= (long)(recovery_last);
	}

	// Use this for initialization
	public void init ()
	{
		InvokeRepeating ("recovery_deamon", 0, 1);
		//com.hp.addDeadCallBack (dead);
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
