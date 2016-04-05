using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RingUtils {
	//the number below is roll by

	private class confForRandomAttr
	{
		public string FieldName;
		public float Least;	//under mutiply of one
		public float Most;	//under mutiply of one;
		public string Format;
		public string Prefix;
		public confForRandomAttr(string fieldName, float least, float most, string format, string prefix) {
			this.FieldName = fieldName;
			this.Least = least;
			this.Most = most;
			this.Format = format;
			this.Prefix = prefix;
		}
	}

	const int ColorCnt = 6;
	private static int[] ProbabilityOfColor = new int[ColorCnt] {5, 60, 30, 10, 5, 1};
	private static readonly int ProbabilityAddTo = 111;
	private static readonly string SpriteLocation = "ring/sprites/";
	private static UISprite[] Sprites;

	private static readonly confForRandomAttr[] AttrConfig = new confForRandomAttr[] {
		new confForRandomAttr("strength", 3, 6, "力量+{0}", "强壮的"),
		new confForRandomAttr("agility", 3, 6, "敏捷+{0}", "矫健的"),
		new confForRandomAttr("intelligence", 3, 6, "智力+{0}", "聪慧的"),
		new confForRandomAttr("speed_addition", 1, 3, "移动速度+{0}", "迅捷的"),
		new confForRandomAttr("exp_mutiply", 40, 50, "经验获取+{0}%", "灵光的"),
		new confForRandomAttr("max_hp_addtion", 100, 150, "生命值+{0}", "红色的"),
		new confForRandomAttr("max_mp_addition", 100, 150, "魔法值+{0}", "蓝色的"),
		new confForRandomAttr("mp_cost_minus", 3, 4, "魔法消耗-{0}", "节能的"),
		new confForRandomAttr("mp_cost_percentage_minus", 10, 30, "魔法消耗-{0}%", "减排的"),
	};
	private static readonly float[] AttrMutiplyOfColor = new float[ColorCnt] {0, 1, 1.5f, 2f, 3f, 5f};
	private static readonly int[] LineCntOfColor = new int[ColorCnt] {0, 1, 2, 3, 3, 4};


	public static readonly int[] MoneyWhenSaleOfColor = new int[ColorCnt] {1000, 10, 50, 100, 500, 1000};
	public static Dictionary<RingUtils.Color, UnityEngine.Color> RGBToColor = 
		new Dictionary<RingUtils.Color, UnityEngine.Color>() {
		{RingUtils.Color.Expensive, UnityEngine.Color.yellow},
		{RingUtils.Color.Common, UnityEngine.Color.white},
		{RingUtils.Color.Rare, UnityEngine.Color.green},
		{RingUtils.Color.Epic, UnityEngine.Color.blue},
		{RingUtils.Color.Legend, UnityEngine.Color.cyan},
		{RingUtils.Color.Unique, UnityEngine.Color.red}
	};

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

	public static void load() {
		Sprites = Resources.LoadAll<UISprite> (SpriteLocation);
	}

	private static void randomAttrsAndNameAndDescription(Color co, out BuffUtils.HeapAttrBuff buff, 
		out string name, out string description) {

		var lineCnt = LineCntOfColor [(int)(co)];

		var choose = new bool[AttrConfig.Length];

		for (int i = 0; i < lineCnt; ) {
			int n = Random.Range (0, AttrConfig.Length);
			if (!choose [n]) {
				choose [n] = true;
				i++;
			}
		}

		string prefix = "";
		string desc = "";
		BuffUtils.HeapAttrBuff b = new BuffUtils.HeapAttrBuff ();

		for (int i = 0; i < AttrConfig.Length; i++) {
			if (choose [i]) {
				prefix += AttrConfig [i].Prefix;

				var fieldInfo = typeof(attributes_manager.t_mid_attributes).GetField (AttrConfig [i].FieldName);
				if (fieldInfo.FieldType == typeof(int)) {
					var val = (int)(Random.Range(AttrConfig[i].Least, AttrConfig[i].Most) * AttrMutiplyOfColor[(int)(co)]);
					desc += string.Format (AttrConfig [i].Format, val) + "\n";
					b.Add (new BuffUtils.OneAttrBuff_int (AttrConfig[i].FieldName, val));
				}

				if (fieldInfo.FieldType == typeof(float)) {
					var val = Random.Range (AttrConfig [i].Least, AttrConfig [i].Most) * AttrMutiplyOfColor [(int)(co)];
					desc += string.Format (AttrConfig [i].Format, val) + "\n";
					b.Add (new BuffUtils.OneAttrBuff_float (AttrConfig [i].FieldName, val));
				}

				//TODO: etc
			}
		}

		prefix += "指环";
		buff = b;
		name = prefix;
		description = desc;
	}

	public static RingDefault RandomOneRing() {
		float r = Random.value * ProbabilityAddTo;
		int co = 0;
		while (co < ColorCnt && r > ProbabilityOfColor [co]) {
			r -= ProbabilityOfColor [co];
			co++;
		}
		if (co == ColorCnt)
			co = ColorCnt - 1;

		Color rare = (Color) (co);
		UISprite sprite = Sprites[Random.Range (0, Sprites.Length)];
		BuffUtils.HeapAttrBuff buff;
		string name, description;

		randomAttrsAndNameAndDescription (rare, out buff, out name, out description);
		return new RingDefault (buff, sprite, rare, name, description);
	}


}
