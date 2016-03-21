//Copyright (c) 2013-2015 Eycia Zhou <zhou.eycia@gmail.com>
//
//许可权特此免费授与获得本软件的副本以及相关文档（“软件”）的任何人不受限制地处理本软件的权利，
//包括不受限制地使用、复制、修改、合并、出版、分发、分许可、和／或销售此软件的副本的权利，同时
//把这样的权利授与获得本软件副本的人，但必须遵循以下条件：
//
//本软件的所有副本或实质性部分必须包含以上版权声明和本许可声明。
//
//本软件以“按现状”的基础提供，不附有任何形式的（无论是明示的还是默示的）保证，包括（但不限于）
//适销性或适用于某特定用途的默示保证。无论何种情况，无论是合同诉讼、侵权还是其他诉讼，作者或版
//权所有者对于任何由于本软件导致的或与本软件的使用或其他处理有关的任何索赔、损失或其他责任，均
//不负任何法律责任。

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*BUFF控制器
 * ---------------------------------------------------------------------------
 * 使用：
 *	将本与其他公共脚本放在一个物体上，本脚本依赖com
 * ---------------------------------------------------------------------------
 * 作用：
 *	通过init函数初始化buff
 *	通过add_buff函数添加buff，函数返回唯一id
 *	通过remove_buff函数根据id删除buff
 *	
 *	然后本组件会在每个fix中计算所有buff等级，判断buff过期情况，并自动清理，
 *	将buff等级登记至com组件。
 * ---------------------------------------------------------------------------
 */

public class buff_manager : MonoBehaviour
{

	class buff_time
	{
		public t.buff buff;
		public float time;

		public buff_time (t.buff buff, float time)
		{
			this.buff = buff;
			this.time = time;
		}
	}

	static Dictionary<string, List<buff_time> > buffs = new Dictionary<string, List<buff_time>> ();

	public string add_buff (t.buff buff, float time = -1)
	{
		string uuid;
		do {
			uuid = System.Guid.NewGuid ().ToString ();
		} while (buffs.ContainsKey (uuid));

		List<buff_time> temp = new List<buff_time> ();
		temp.Add (new buff_time (buff, time));

		buffs.Add (uuid, temp);
		return uuid;
	}

	public string add_buff (Dictionary<t.buff, float> _buffs)
	{
		List<buff_time> temp = new List<buff_time> ();
		foreach (KeyValuePair<t.buff, float> buff in _buffs) {
			temp.Add (new buff_time (buff.Key, buff.Value));
		}
		string uuid;
		do {
			uuid = System.Guid.NewGuid ().ToString ();
		} while (buffs.ContainsKey (uuid));
		buffs.Add (uuid, temp);
		return uuid;
	}

	public void change_buff (string uid, t.buff buff, float time = -1)
	{
		if (buffs.ContainsKey (uid)) {
			List<buff_time> temp = new List<buff_time> ();
			temp.Add (new buff_time (buff, time));
			buffs [uid] = temp;
		}
	}

	public void change_buff (string uid, Dictionary<t.buff, float> _buffs)
	{
		if (buffs.ContainsKey (uid)) {
			List<buff_time> temp = new List<buff_time> ();
			foreach (KeyValuePair<t.buff, float> buff in _buffs) {
				temp.Add (new buff_time (buff.Key, buff.Value));
			}
			buffs [uid] = temp;
		}
	}

	public void remove_buff (string uuid)
	{
		if (buffs.ContainsKey (uuid)) {
			buffs.Remove (uuid);
		}
	}

	public void init ()
	{
		buffs.Clear ();
	}

	public void cal_buff ()
	{
		List<string> rem = new List<string> ();
		foreach (string key in buffs.Keys) {
			foreach (buff_time buff in buffs[key]) {
				if (buff.time == -1) {
					continue;
				}
				buff.time -= Time.fixedDeltaTime;
			}
			buffs [key].RemoveAll (item => item.time != -1 && item.time < 0);

			if (buffs [key].Count == 0) {
				rem.Add (key);
			}
		}

		foreach (string key in rem) {
			buffs.Remove (key);
		}

		t.buff_set bs = new t.buff_set ();
		foreach (List<buff_time> bufft in buffs.Values) {
			foreach (buff_time buff in bufft) {
				buff.buff.buff_func (bs);
			}
		}
		com.b = bs;
		//com.calu_buff_c ();
	}

	void FixedUpdate ()
	{
		cal_buff ();
	}
}
