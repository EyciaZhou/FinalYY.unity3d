using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuffUtils
{
	/*
		 public class OneLineBuff<T> : buff_interface {
		private string field_name;
		private T val;

		public void calculate (attributes_manager.t_mid_attributes mid) {
			var field_info = typeof(attributes_manager.t_mid_attributes).GetField (field_name);
			if (field_info == null) {
				Debug.Log ("not existed field in mid attributes: " + field_name);
				return;
			}
			var tmp = (T)field_info.GetValue (mid);
			var result = tmp + val;
			field_info.SetValue (mid, (T)(result));
		}

		public System.Guid guid {
			get;
			private set;
		}

		public OneLineBuff_int(string field_name, T val) {
			guid = System.Guid.NewGuid();
			this.field_name = field_name;
			this.val = val;
		}
	} 
	*/
	public class OneAttrBuff_float : IBuff
	{
		private string field_name;
		private float val;


		public void calculate (attributes_manager.t_mid_attributes mid)
		{
			var field_info = typeof(attributes_manager.t_mid_attributes).GetField (field_name);
			if (field_info == null) {
				Debug.Log ("not existed field in mid attributes: " + field_name);
				return;
			}
			var tmp = (float)field_info.GetValue (mid);
			var result = tmp + val;
			field_info.SetValue (mid, result);
		}

		public System.Guid Guid {
			get;
			private set;
		}

		public OneAttrBuff_float (string field_name, float val)
		{
			Guid = System.Guid.NewGuid ();
			this.field_name = field_name;
			this.val = val;
		}
	}

	public class OneAttrBuff_int : IBuff
	{
		private string field_name;
		private int val;

		public void calculate (attributes_manager.t_mid_attributes mid)
		{
			var field_info = typeof(attributes_manager.t_mid_attributes).GetField (field_name);
			if (field_info == null) {
				Debug.Log ("not existed field in mid attributes: " + field_name);
				return;
			}
			var tmp = (int)field_info.GetValue (mid);
			var result = tmp + val;
			field_info.SetValue (mid, result);
		}

		public System.Guid Guid {
			get;
			private set;
		}

		public OneAttrBuff_int (string field_name, int val)
		{
			Guid = System.Guid.NewGuid ();
			this.field_name = field_name;
			this.val = val;
		}
	}

	public class HeapAttrBuff : IBuff {
		private List<IBuff> heap = new List<IBuff>();

		#region buff_interface implementation

		public void calculate (attributes_manager.t_mid_attributes mid)
		{
			foreach (IBuff bi in heap) {
				bi.calculate (mid);
			}
		}

		public System.Guid Guid { get; private set;}

		#endregion

		public HeapAttrBuff() {
			Guid = System.Guid.NewGuid();
		}

		public HeapAttrBuff Add(IBuff bi) {
			heap.Add (bi);
			return this;
		}

		void Remove(IBuff bi) {
			heap.RemoveAll(((IBuff obj) => obj == bi));
		}
	}
		
	public static OneAttrBuff_float BuildBuffRange (string field_name, float l, float r)
	{
		return new OneAttrBuff_float (field_name, Random.Range (l, r));
	}

	public static OneAttrBuff_int BuildBuffRange (string field_name, int l, int r)
	{
		return new OneAttrBuff_int (field_name, Random.Range (l, r));
	}


}
