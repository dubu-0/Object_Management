using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour, IPersistableObject
{
	[SerializeField] private PersistentStorage _storage;
	[SerializeField] private Cube _prefab;
	[SerializeField] private KeyCode _createPrefabKeyCode = KeyCode.C;
	[SerializeField] private KeyCode _beginNewGameKeyCode = KeyCode.N;
	[SerializeField] private KeyCode _saveKeyCode = KeyCode.S;
	[SerializeField] private KeyCode _loadKeyCode = KeyCode.L;

	private readonly List<IPersistableObject> _instances = new List<IPersistableObject>();

	private void Update()
	{
		if (Input.GetKeyDown(_createPrefabKeyCode))
			CreatePrefabInstance();
		else if (Input.GetKeyDown(_beginNewGameKeyCode))
			BeginNewGame();
		else if (Input.GetKeyDown(_saveKeyCode))
			_storage.Save(this);
		else if (Input.GetKeyDown(_loadKeyCode)) 
			_storage.Load(this);
	}

	public void Save(GameDataWriter writer)
	{
		writer.Write(_instances.Count);
		foreach (var instance in _instances) 
			instance.Save(writer);
	}

	public void Load(GameDataReader reader)
	{
		BeginNewGame();

		var count = reader.ReadInt();
		
		for (var i = 0; i < count; i++)
		{
			var instance = Instantiate(_prefab);
			instance.Load(reader);
			_instances.Add(instance);
		}
	}

	public void Destroy(float time = 0)
	{
		Object.Destroy(gameObject, time);
	}

	private void CreatePrefabInstance()
	{
		var instance = Instantiate(_prefab);
		instance.transform.localPosition = Random.insideUnitSphere * 5f;
		instance.transform.localRotation = Random.rotation;
		instance.transform.localScale = Vector3.one * Random.Range(0.1f, 1f);
		_instances.Add(instance);
	}

	private void BeginNewGame()
	{
		foreach (var instance in _instances) 
			instance.Destroy();
		
		_instances.Clear();
	}
}