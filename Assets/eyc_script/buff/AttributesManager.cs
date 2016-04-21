using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 * process buffs to attributes value
 * and tell controllers when attribute changes
 * 
 * buff effect the mid-attributes
 * mid-attributes effect the attributes
 * mid-attribute can minus or plus with independent order
 * and calculate attributes from mid_attributes use
 * series t_attr_calc.
 * 
 * if the attributes is not the field of this class(not built-in)
 * you may add such attributei into attr_dic.
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
public class AttributesManager : MonoBehaviour {
	public delegate void CalcMidToAttrAction (MidAttributes mid, Attributes attr);
	public delegate void AttributesChangeAction ();

	public Attributes Attr { get; private set; }
	private MidAttributes Mid = new MidAttributes();

	/* Attributes Class
	 * 
	 * including any attributes number of this game(or player)
	 * if not built-in or genrate during runtime, can add it to Etc(Dictionary)
	 */ 
	public class Attributes {
		public int HpUpperLimit;
		public int MpUpperLimit;
		public float MoveSpeed; //per second

		public float HpRecovery; //per second
		public float MpRecovery; //per second

		public float FireRingHatchDuration;

		public float SmallFireball_Hurt;
		public int SmallFireball_Cost;
		public float SmallFireball_Cooldown;

		public Dictionary<string, Object> Etc = new Dictionary<string, Object> ();
	}

	public int Calc_ReallyExp(int raw) {
		return (int)(raw * Mid.ExpMutiply / 100 + Mid.ExpExtra);
	}

	public int Calc_ReallyMpCost(int cost) {
		if (cost < 0)
			return 0;

		return (int)((cost - Mid.MpCostMinus) * (100 - Mid.MpCostPercentageMinus) / 100.0);
	}

	public float Calc_CoinRaidus() {
		return Mid.CollectRaidus;
	}

	private void Calc_Builtin_MidToAttr(MidAttributes Mid, Attributes Attr) {

		Attr.HpUpperLimit = Mid.Strength * 20 + Mid.HpAddtion;
		Attr.HpRecovery = Mid.Strength * 0.5f;

		Attr.MpUpperLimit = Mid.Intelligence * 20 + Mid.MpAddition;
		Attr.MpRecovery = Mid.Intelligence * 0.5f;

		Attr.MoveSpeed = (Mid.SpeedBase + Mid.SpeedAddition) * Mid.SpeedMutiply / 100f;

		Attr.SmallFireball_Cost = Mid.SmallFireball_Level * 5 + 30;
		Attr.SmallFireball_Cooldown = 1.0f / (Mid.SmallFireball_Level + 1);
		Attr.SmallFireball_Hurt = 10 * Mid.SmallFireball_Level + 0.1f * Mid.Intelligence;
	
		Attr.FireRingHatchDuration = 10;
	}

	public class MidAttributes {
		public int Strength;
		public int Agility;
		public int Intelligence;

		public float SpeedBase;
		public int SpeedAddition;
		public int SpeedMutiply; //100

		public int ExpMutiply; //100
		public int ExpExtra;
		public int HpAddtion;
		public int MpAddition;
		public int MpCostMinus;
		public int MpCostPercentageMinus; //100

		public int SmallFireball_Level;

		public float CollectRaidus;

	}

	private event CalcMidToAttrAction _onCalcMidToAttr;
	public event CalcMidToAttrAction OnCalcMidToAttr {
		add {
			_onCalcMidToAttr += value;
			re_calculate ();
		}
		remove {
			_onCalcMidToAttr -= value;
			re_calculate ();
		}
	}

	public event AttributesChangeAction OnAttributesChange;

	private Dictionary<System.Guid, controller_interface> controllers = new Dictionary<System.Guid, controller_interface>();
	private Dictionary<System.Guid, IBuff> buff_sort_with_guid = new Dictionary<System.Guid, IBuff>();

	private bool changed;

	public AttributesManager() {
		re_calculate ();
		OnCalcMidToAttr += Calc_Builtin_MidToAttr;
	}

	//---------------------calc progress------------------

	private void _CalcBuffToMid() {
		MidAttributes mid = new MidAttributes ();
		foreach (IBuff buff in buff_sort_with_guid.Values) {
			buff.calculate (mid);
		}
		this.Mid = mid;
	}

	private void _CalcMidToAttr() {
		Attributes attr = new Attributes ();
		if (_onCalcMidToAttr != null) {
			_onCalcMidToAttr (this.Mid, attr);
		}
		this.Attr = attr;
	}

	public void re_calculate() {
		_CalcBuffToMid ();
		_CalcMidToAttr ();
		if (OnAttributesChange != null) {
			OnAttributesChange ();
		}
	}

	//-------------------------buff----------------------------

	public IBuff AddBuff(IBuff bi) {
		bi.AM = this;
		buff_sort_with_guid.Add (bi.Guid, bi);
		changed = true;
		return bi;
	}

	public bool RemoveBuff(IBuff bi) {
		if (buff_sort_with_guid.ContainsKey(bi.Guid)) {
			bi.AM = null;
			buff_sort_with_guid.Remove (bi.Guid);
			changed = true;
			return true;
		}
		return false;
	}

	//-------------------------controller----------------------------

	/*
	public controller_interface bind_controller(controller_interface ci) {
		ci.am = this;
		controllers.Add (ci.guid, ci);
		changed = true;
		return ci;
	}

	public bool remove_controller(controller_interface ci) {
		if (controllers.ContainsKey (ci.guid)) {
			controllers.Remove (ci.guid);
			changed = true;
			return true;
		}
		return false;
	}
	*/

	//-----------------------etc---------------------------

	public void NotifyBuffChange() {
		changed = true;
	}

	public void NotifyBuffChange(string name) {
		changed = true;
		Debug.Log (name);
	}

	public void ReCalc() {
		re_calculate ();
	}

	public void NotifyCalcChange() {
		changed = true;
	}

	public void NotifyCalcChange(string name) {
		changed = true;
		Debug.Log (name);
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (changed) {
			re_calculate ();
			changed = false;
		}
	}
}
