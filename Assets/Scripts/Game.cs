using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour, IPersistableObject
{
	[SerializeField] private PersistentStorage _storage;
	[SerializeField] private ShapeFactory _shapeFactory;
	[SerializeField] private KeyCode _beginNewGameKeyCode = KeyCode.N;
	[SerializeField] private KeyCode _saveKeyCode = KeyCode.S;
	[SerializeField] private KeyCode _loadKeyCode = KeyCode.L;
	[SerializeField] private int _levelCount;

	private float _creationProgress;
	private float _destructionProgress;
	private List<Shape> _shapes;
	private int _loadedLevelBuildIndex;

	public float CreationSpeed { get; private set; }
	public float DestructionSpeed { get; private set; }
	private bool HasShapes => _shapes.Count > 0;

	private void Start()
	{
		_shapes = new List<Shape>(10000 * _shapeFactory.ShapeTypeCount);
		LoadLevel(1);
	}

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
		else
		{
			for (var i = 1; i <= _levelCount; i++)
			{
				if (Input.GetKeyDown(KeyCode.Alpha0 + i))
				{
					BeginNewGame();
					LoadLevel(i);
					return;
				}
			}
		}
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

	private void LoadLevel(int buildIndex)
	{
		if (Application.isEditor)
		{
			for (var i = 0; i < SceneManager.sceneCount; i++)
			{
				var level = SceneManager.GetSceneAt(i);
				if (level.name.Contains("Level "))
					SceneManager.SetActiveScene(level);
			}
		}
		
		StartCoroutine(LevelLoading(buildIndex));
	}
	
	private IEnumerator LevelLoading(int buildIndex)
	{
		enabled = false;

		var anyLevel = _loadedLevelBuildIndex > 0;
		if (anyLevel)
			yield return SceneManager.UnloadSceneAsync(_loadedLevelBuildIndex);

		yield return SceneManager.LoadSceneAsync(buildIndex, LoadSceneMode.Additive);
		SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(buildIndex));
		_loadedLevelBuildIndex = buildIndex;
		enabled = true;
	}

	private void CreateShape()
	{
		_shapes.Add(_shapeFactory.CreateRandom());
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
			if (!HasShapes)
				return;

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
