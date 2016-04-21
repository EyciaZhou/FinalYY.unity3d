using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RingUtils
{
	//the number below is roll by face

	private class confForRandomAttr
	{
		public string FieldName;
		public float Least;
		//under mutiply of one
		public float Most;
		//under mutiply of one;
		public string Format;
		public string Prefix;

		public confForRandomAttr (string fieldName, float least, float most, string format, string prefix)
		{
			this.FieldName = fieldName;
			this.Least = least;
			this.Most = most;
			this.Format = format;
			this.Prefix = prefix;
		}
	}


	private class confForEachColor {
		public int Probability;
		public float AttrMutiply;
		public int LineCnt;
		public string PrefabName;
		public int MoneyWhenSale;
		public UnityEngine.Color color;
		public GameObject Prefab;

		public confForEachColor(int Probability, float AttrMutiply, int LineCnt, string PrefabName, int MoneyWhenSale, UnityEngine.Color color, GameObject Prefab) {
			this.Probability = Probability;
			this.AttrMutiply = AttrMutiply;
			this.LineCnt = LineCnt;
			this.PrefabName = PrefabName;
			this.MoneyWhenSale = MoneyWhenSale;
			this.color = color;
			this.Prefab = Prefab;
		}
	}

	const int ColorCnt = 6;
	private static readonly string prefabUILocation = "thing/ring/";

	private static readonly confForRandomAttr[] AttrConfig = new confForRandomAttr[] {
		new confForRandomAttr ("Strength", 3, 6, "力量+{0}", "强壮"),
		new confForRandomAttr ("Agility", 3, 6, "敏捷+{0}", "矫健"),
		new confForRandomAttr ("Intelligence", 3, 6, "智力+{0}", "聪慧"),
		new confForRandomAttr ("SpeedAddition", 1, 3, "移动速度+{0}", "迅捷"),
		new confForRandomAttr ("ExpMutiply", 40, 50, "经验获取+{0}%", "灵光"),
		new confForRandomAttr ("HpAddtion", 100, 150, "生命值+{0}", "红色"),
		new confForRandomAttr ("MpAddition", 100, 150, "魔法值+{0}", "蓝色"),
		new confForRandomAttr ("MpCostMinus", 3, 4, "魔法消耗-{0}", "节能"),
		new confForRandomAttr ("MpCostPercentageMinus", 10, 15, "魔法消耗-{0}%", "减排"),
	};

	private static confForEachColor[] ColorConf = new confForEachColor[ColorCnt] {
		new confForEachColor(5, 0, 0, "Expensive", 1000, UnityEngine.Color.yellow, null),
		new confForEachColor(60, 1, 1, "Common", 10, UnityEngine.Color.white, null),
		new confForEachColor(30, 1.5f, 2, "Rare", 50, UnityEngine.Color.green, null),
		new confForEachColor(10, 2, 3, "Epic", 100, UnityEngine.Color.blue, null),
		new confForEachColor(5, 3, 3, "Legend", 500, UnityEngine.Color.cyan, null),
		new confForEachColor(1, 5, 4, "Unique", 1000, UnityEngine.Color.red, null),
	};

	private static int ProbabilityAddTo = 111;

	private static readonly string[] SuffixOfEachWordInName = new string[4] {"的", "而又", "之", ""};


	public enum Color
	{
		//金色,昂贵的
		Expensive = 0,
		//白色,平常的
		Common = 1,
		//绿色,稀有的
		Rare = 2,
		//蓝色,史诗的
		Epic = 3,
		//紫色,传说的
		Legend = 4,
		//红色,唯一的
		Unique = 5,
	};

	public static void load ()
	{
		for (int i = 0; i < ColorCnt; i++) {
			ColorConf[i].Prefab = Resources.Load<GameObject> (prefabUILocation + ColorConf [i].PrefabName);
		}
	}

	private static void randomAttrsAndNameAndDescription (Color co, out BuffUtils.HeapAttrBuff buff, 
	                                                     out string name, out string description)
	{
		var co_i = (int)(co);
		var lineCnt = ColorConf [co_i].LineCnt;

		var choose = new bool[AttrConfig.Length];

		for (int i = 0; i < lineCnt;) {
			int n = Random.Range (0, AttrConfig.Length);
			if (!choose [n]) {
				choose [n] = true;
				i++;
			}
		}

		string prefix = "";
		string desc = "";
		BuffUtils.HeapAttrBuff b = new BuffUtils.HeapAttrBuff ();

		int p = 0;
		for (int i = 0; i < AttrConfig.Length; i++) {
			if (choose [i]) {
				prefix += AttrConfig [i].Prefix + SuffixOfEachWordInName[lineCnt-p-1];

				var fieldInfo = typeof(AttributesManager.MidAttributes).GetField (AttrConfig [i].FieldName);
				if (fieldInfo.FieldType == typeof(int)) {
					var val = (int)(Random.Range (AttrConfig [i].Least, AttrConfig [i].Most) * ColorConf [co_i].AttrMutiply);
					desc += string.Format (AttrConfig [i].Format, val) + "\n";
					b.Add (new BuffUtils.OneAttrBuff_int (AttrConfig [i].FieldName, val));
				}

				if (fieldInfo.FieldType == typeof(float)) {
					var val = Random.Range (AttrConfig [i].Least, AttrConfig [i].Most) * ColorConf [co_i].AttrMutiply;
					desc += string.Format (AttrConfig [i].Format, val) + "\n";
					b.Add (new BuffUtils.OneAttrBuff_float (AttrConfig [i].FieldName, val));
				}

				//TODO: etc

				p++;
			}
		}

		prefix += "指环";
		buff = b;
		name = prefix;
		description = desc;
	}

	public static RingDefault RandomOneRing ()
	{
		float r = Random.value * ProbabilityAddTo;
		int co = 0;
		while (co < ColorCnt && r > ColorConf[co].Probability) {
			r -= ColorConf[co].Probability;
			co++;
		}
		if (co == ColorCnt)
			co = ColorCnt - 1;

		Color rare = (Color)(co);
		GameObject prefabUI = ColorConf [co].Prefab;
		UnityEngine.Color color = ColorConf [co].color;
		BuffUtils.HeapAttrBuff buff;
		string name, description;

		randomAttrsAndNameAndDescription (rare, out buff, out name, out description);

		return new RingDefault (buff, prefabUI, rare, name, description, color);
	}


}
