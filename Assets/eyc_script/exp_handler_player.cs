﻿using UnityEngine;
using System.Collections;

public class exp_handler_player : exp_handler, buff_interface {
	public attributes_manager am{ get; set; }

	public System.Guid guid { get; private set; }

	public delegate void ExpValueChangeAction ();
	public event ExpValueChangeAction OnExpValueChange;

	public void calculate (attributes_manager.t_mid_attributes mid)
	{
		//TODO:
		mid.intelligence += 10 * lv;
		mid.agility += 10 * lv;
		mid.strength += 10 * lv;

		mid.speed_base += 5 + lv / 2;
		mid.speed_mutiply += 1 + 0.5f / lv;

		mid.exp_mutiply += 1;

		mid.coin_raidus += 1;
	}

	public exp_handler_player() : base() {
		this.guid = System.Guid.NewGuid ();
		OnLvUp += (() => {
			if (am != null) {
				am.buff_changed_from("ehib");
			}
		});
	}

	public new void gain_exp(int exp) {
		base.gain_exp ((int)(exp*am.attr.exp_mutiply + am.attr.exp_extra));
		OnExpValueChange ();
	}
}