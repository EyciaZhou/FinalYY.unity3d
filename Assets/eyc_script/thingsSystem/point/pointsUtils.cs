using UnityEngine;
using System.Collections;

public class pointsUtils {
	public enum TypeOfPoint
	{
		HP = 0,
		MP = 1,
		EXP = 2,
	};

	private class confForEachPoint {
		public string Name;
		public TypeOfPoint pointType;
		public int point;
		public UnityEngine.Color color;
		public string PrefabName;
		public GameObject Prefab;
		public string Description;
		public int Probability;

		public confForEachPoint(string Name, TypeOfPoint pointType, int point, 
			UnityEngine.Color color, string PrefabName, GameObject Prefab, 
			string Description, int Probability) {
			this.Name = Name;
			this.pointType = pointType;
			this.point = point;
			this.color = color;
			this.PrefabName = PrefabName;
			this.Prefab = Prefab;
			this.Description = Description;
			this.Probability = Probability;
		}
	}

	private static readonly string prefabUILocation = "thing/points/";

	private static confForEachPoint[] PointConf = new confForEachPoint[] {
		new confForEachPoint ("小瓶的魔法药水", TypeOfPoint.MP, 50, Color.blue, "mp", null, "魔法恢复50", 500),
		new confForEachPoint ("小瓶的生命药水", TypeOfPoint.HP, 50, Color.red, "hp", null, "生命值恢复50", 500),
		new confForEachPoint ("小瓶的经验药水", TypeOfPoint.EXP, 50, Color.yellow, "exp", null, "经验增加50", 0),
	};
	private static int ProbabilityAddTo = 1000;

	public static void load() {
		for (int i = 0; i < PointConf.Length; i++) {
			PointConf [i].Prefab = Resources.Load<GameObject> (prefabUILocation + PointConf [i].PrefabName);
		}
	}

	public static ThingPoint RandomOneRing() {
		float r = Random.value * ProbabilityAddTo;
		int item = 0;
		while (item < PointConf.Length && r > PointConf [item].Probability) {
			r -= PointConf [item].Probability;
			item++;
		}
		if (item == PointConf.Length)
			item = PointConf.Length - 1;

		Debug.Log (PointConf [item].Prefab);

		return new ThingPoint(
			PointConf[item].pointType, 
			PointConf[item].point, 
			PointConf[item].Name,
			PointConf[item].Description,
			PointConf[item].Prefab,
			PointConf[item].color
		);
	}
}
