using UnityEngine;
using System.Collections;

public interface IThing  {
	System.Guid Guid { get; }
	string Name { get; }
	string Description { get; }
	GameObject gameObject { get; }

	void Use();
}
