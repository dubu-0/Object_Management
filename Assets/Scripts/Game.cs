using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour, IPersistableObject
{
	[SerializeField] private PersistentStorage _storage;
	[SerializeField] private ShapeFactory _shapeFactory;
	[SerializeField] private KeyCode _createPrefabKeyCode = KeyCode.C;
	[SerializeField] private KeyCode _beginNewGameKeyCode = KeyCode.N;
	[SerializeField] private KeyCode _saveKeyCode = KeyCode.S;
	[SerializeField] private KeyCode _loadKeyCode = KeyCode.L;

	private readonly List<Shape> _shapes = new List<Shape>();

	private void Update()
	{
		if (Input.GetKeyDown(_createPrefabKeyCode))
			CreateShape();
		else if (Input.GetKeyDown(_beginNewGameKeyCode))
			BeginNewGame();
		else if (Input.GetKeyDown(_saveKeyCode))
			_storage.Save(this);
		else if (Input.GetKeyDown(_loadKeyCode)) 
			_storage.Load(this);
	}

	public void Save(GameDataWriter writer)
	{
		writer.Write(_shapes.Count);
		foreach (var shape in _shapes)
		{
			writer.Write(shape.ID);
			shape.Save(writer);
		}
	}

	public void Load(GameDataReader reader)
	{
		BeginNewGame();

		var count = reader.ReadInt();
		
		for (var i = 0; i < count; i++)
		{
			var id = reader.ReadInt();
			var shape = _shapeFactory.Create(id);
			shape.Load(reader);
			_shapes.Add(shape);
		}
	}

	private void CreateShape()
	{
		_shapes.Add(_shapeFactory.CreateRandom());
	}

	private void BeginNewGame()
	{
		foreach (var shape in _shapes) 
			Destroy(shape.gameObject);
		
		_shapes.Clear();
	}
}