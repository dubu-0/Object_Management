using UnityEngine;

[DisallowMultipleComponent]
public class Cube : MonoBehaviour, IPersistableObject
{
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

	public void Destroy(float time = 0)
	{
		Object.Destroy(gameObject, time);
	}
}