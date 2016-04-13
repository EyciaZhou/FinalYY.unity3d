using UnityEngine;
using System.Collections;

public class ThingPoint : IThing {
	public pointsUtils.TypeOfPoint typ { get; private set; }
	public int point { get; private set; }

	#region IThing implementation


	public UnityEngine.Color color { get; private set; }
	public void Use ()
	{
		com.things.UsePoint (this);
	}

	public System.Guid Guid { get; private set; }
	public string Name { get; private set; }
	public string Description { get; private set; }
	public GameObject gameObject { get; private set; }
	#endregion

	public ThingPoint(pointsUtils.TypeOfPoint typ, int point, 
		string name, string description, GameObject go, Color color) {

		this.typ = typ;
		this.point = point;
		this.Guid = System.Guid.NewGuid ();
		this.Name = name;
		this.Description = description;
		this.gameObject = go;
		this.color = color;
	}
}
