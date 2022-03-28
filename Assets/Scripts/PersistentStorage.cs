using System.IO;
using UnityEngine;

public class PersistentStorage : MonoBehaviour
{
	private string _path;
	private const string FileName = "savefile";

	private void Awake()
	{
		_path = Path.Combine(Application.persistentDataPath, FileName);
		Debug.Log($"Path for saved files {_path}");
	}

	public void Save(IPersistableObject persistableObject)
	{
		using var writer = new GameDataWriter(new BinaryWriter(File.Open(_path, FileMode.Create)));
		persistableObject.Save(writer);
	}

	public void Load(IPersistableObject persistableObject)
	{
		using var reader = new GameDataReader(new BinaryReader(File.Open(_path, FileMode.Open)));
		persistableObject.Load(reader);
	}
}