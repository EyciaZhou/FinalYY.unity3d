using UnityEngine;
using System.Collections;

public class t{
	public class buff_set {
		public float footstep_spd;
		public int lv;
	}

	public class buff {
		public delegate void t_buff_func (buff_set bs);
		public string name;
		public t_buff_func buff_func;

		public buff(string name, t_buff_func buff_func) {
			this.name = name;
			this.buff_func = buff_func;
		}
	}
}
