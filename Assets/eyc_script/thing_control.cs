using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class good
{
	public string typ;
}

public enum ring_rare
{
	//金色,昂贵的
	Expensive,
	//白色,平常的
	Common,
	//绿色,稀有的
	Rare,
	//蓝色,史诗的
	Epic,
	//紫色,传说的
	Legend,
	//红色,唯一的
	Unique,
};

public class ring : good
{
	//一坨buff
	public List<t.buff> buffs;
	//在界面显示的图片的id，由com中控制总数并根据总数分
	int pic_num;
	public bool _using = false;

	ring_rare rare;

	public void add_buff (t.buff _buff)
	{
		this.buffs.Add (_buff);
	}

	// Use this for initialization
	public ring (List<t.buff> buffs, int pic_num, ring_rare rare)
	{
		this.typ = "ring";
		this.buffs = buffs;
		this.pic_num = pic_num;
		this.rare = rare;
	}

	public static ring random_ring()
	{
		
	}
}

public class hp_bottle : good
{
	public hp_bottle ()
	{
		this.typ = "hp_bottle";
	}
}

public class mp_bottle : good
{
	public mp_bottle ()
	{
		this.typ = "mp_bottle";
	}
}

public class spring : good
{
	public void use ()
	{

	}

	public spring ()
	{
		this.typ = "spring";
	}
}


public class thing_control : MonoBehaviour
{
	List<ring> rings;
	Dictionary<int, string> used_rings;
	int spring_num;
	int hp_bottle_num;
	int mp_bottle_num;

	public void add_thing (good thing)
	{
		if (thing.typ == "spring") {
			spring_num++;
		}
		if (thing.typ == "hp_bottle") {
			hp_bottle_num++;
		}
		if (thing.typ == "mp_bottle") {
			mp_bottle_num++;
		}
		if (thing.typ == "ring") {
			rings.Add ((ring)thing);
		}
	}

	public void use_hp ()
	{
		if (hp_bottle_num > 0) {
			com.p.hp.recovery (100);
		}
	}

	public void use_mp ()
	{
		if (mp_bottle_num > 0) {

		}
	}

	public void use_ring (int id)
	{
		if (!rings [id]._using) {
			Dictionary<t.buff, float> temp = new Dictionary<t.buff, float> ();
			rings [id]._using = true;
			foreach (t.buff buff in rings[id].buffs) {
				temp.Add (buff, -1);
			}
			string uuid = com.buffs.add_buff (temp);
			used_rings.Add (id, uuid);
		}
	}

	public void unuse_ring (int id)
	{
		if (rings [id]._using) {
			rings [id]._using = false;
			com.buffs.remove_buff (used_rings [id]);
			used_rings.Remove (id);
		}
	}

	bool spring_reborn ()
	{
		if (spring_num > 0) {
			spring_num--;
			com.reborn ();
			return true;
		}
		return false;
	}	

	public void init ()
	{
		com.p.hp.add_cannot_dead_callback (spring_reborn);
	}

	void Update ()
	{

	}
}