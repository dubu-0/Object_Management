using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour, IPersistableObject
{
	[SerializeField] private PersistentStorage _storage;
	[SerializeField] private ShapeFactory _shapeFactory;
	[SerializeField] private KeyCode _beginNewGameKeyCode = KeyCode.N;
	[SerializeField] private KeyCode _saveKeyCode = KeyCode.S;
	[SerializeField] private KeyCode _loadKeyCode = KeyCode.L;

	private readonly List<Shape> _shapes = new List<Shape>();
	private float _creationProgress;
	private float _destructionProgress;

	public float CreationSpeed { get; set; }
	public float DestructionSpeed { set; get; }
	private bool HasShapes => _shapes.Count > 0;

	private void Update()
	{
		HandleCreation();
		HandleDestruction();

		if (Input.GetKeyDown(_beginNewGameKeyCode))
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
			writer.Write(shape.MaterialID);
			writer.Write(shape.Color);
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
			var materialID = reader.ReadInt();
			var color = reader.ReadColor();
			var shape = _shapeFactory.Create(color, id, materialID);
			shape.Load(reader);
			_shapes.Add(shape);
		}
	}

	private void CreateShape()
	{
		_shapes.Add(_shapeFactory.CreateRandom());
	}

	private void DestroyLastShape()
	{
		var last = _shapes.Count - 1;
		_shapeFactory.Reclaim(_shapes[last]);
		_shapes.RemoveAt(last);
	}

	private void DestroyShapeAt(int index)
	{
		_shapeFactory.Reclaim(_shapes[index]);

		var last = _shapes.Count - 1;
		_shapes[index] = _shapes[last];
		_shapes.RemoveAt(last);
	}

	private void DestroyRandomShape()
	{
		var randomIndex = Random.Range(0, _shapes.Count - 1);
		DestroyShapeAt(randomIndex);
	}

	private void HandleDestruction()
	{
		if (!HasShapes)
			return;

		_destructionProgress += DestructionSpeed * Time.deltaTime;

		while (_destructionProgress >= 1f)
		{
			DestroyRandomShape();
			_destructionProgress -= 1f;
		}
	}

	private void BeginNewGame()
	{
		foreach (var shape in _shapes)
			_shapeFactory.Reclaim(shape);

		_shapes.Clear();
	}


	private void HandleCreation()
	{
		_creationProgress += CreationSpeed * Time.deltaTime;

		while (_creationProgress >= 1f)
		{
			CreateShape();
			_creationProgress -= 1f;
		}
	}
}
