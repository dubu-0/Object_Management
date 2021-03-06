using System.IO;
using SaveSystem.Services;
using UnityEngine;

namespace SaveSystem
{
	public class PersistentStorage : MonoBehaviour
	{
		private const string FileName = "savefile";
		private string _path;

		private void Awake()
		{
			_path = Path.Combine(Application.persistentDataPath, FileName);
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
}
