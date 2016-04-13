using UnityEngine;
using System.Collections;

public interface IThing  {
	System.Guid Guid { get; }
	string Name { get; }
	string Description { get; }
	GameObject gameObject { get; }
	UnityEngine.Color color { get; }

	void Use();
}
