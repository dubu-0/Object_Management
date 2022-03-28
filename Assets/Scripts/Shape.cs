using System;
using UnityEngine;

[DisallowMultipleComponent]
public class Shape : MonoBehaviour, IPersistableObject
{
	public int ID { get; private set; } = -1;
	
	public void Save(GameDataWriter writer)
	{
		writer.Write(transform.localPosition);
		writer.Write(transform.localRotation);
		writer.Write(transform.localScale);
	}

	public void Load(GameDataReader reader)
	{
		transform.localPosition = reader.ReadVector3();
		transform.localRotation = reader.ReadQuaternion();
		transform.localScale = reader.ReadVector3();
	}
	
	public void InitID(int id)
	{
		if (ID < 0 && id >= 0)
			ID = id;
		else if (ID >= 0)
			throw new Exception("Already inited");
		else
			throw new ArgumentOutOfRangeException(nameof(id), "id < 0");
	}
}