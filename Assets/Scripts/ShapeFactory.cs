using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "Create SO/ShapeFactory", fileName = "ShapeFactory", order = 0)]
public class ShapeFactory : ScriptableObject
{
	[SerializeField] private bool _recycle;
	[SerializeField] private Shape[] _shapes;
	[SerializeField] private Material[] _materials;

	private List<Shape>[] _pools;
	private Shape _instance;
	private Scene _poolScene;

	public Shape Create(Color color, int shapeID = 0, int materialID = 0)
	{
		SetupShape(shapeID, materialID, color);
		return _instance;
	}

	public Shape CreateRandom()
	{
		var randomID = Random.Range(0, _shapes.Length);
		var randomMaterialID = Random.Range(0, _materials.Length);
		var randomColor = Random.ColorHSV(
			0f, 1f, 
			0.5f, 1f,
			0.25f, 1f, 
			1f, 1f);

		SetupShape(randomID, randomMaterialID, randomColor);
		return _instance;
	}

	public void Reclaim(Shape shape)
	{
		if (_recycle)
		{
			shape.gameObject.SetActive(false);
			_pools[shape.ID].Add(shape);
		}
		else
		{
			Destroy(shape.gameObject);
		}
	}

	private void SetupShape(int shapeID, int materialID, Color color)
	{
		if (_recycle)
		{
			if (_pools == null)
				InitPools();

			var pool = _pools[shapeID];
			var last = pool.Count - 1;

			if (last >= 0)
			{
				_instance = pool[last];
				_instance.gameObject.SetActive(true);
				pool.RemoveAt(last);
			}
			else
			{
				_instance = Instantiate(_shapes[shapeID]);
			}
			
			SceneManager.MoveGameObjectToScene(_instance.gameObject, _poolScene);
		}
		
		_instance.transform.localPosition = Random.insideUnitSphere * 5f;
		_instance.transform.localRotation = Random.rotation;
		_instance.transform.localScale = Vector3.one * Random.Range(0.1f, 1f);

		if (_instance.ShapeIDNotSet)
			_instance.InitID(shapeID);
		if (_instance.MaterialIDNotSet)
			_instance.InitMaterialID(materialID);
		
		_instance.SetMaterial(_materials[materialID]);
		_instance.SetColor(color);
	}

	private void InitPools()
	{
		if (_pools == null)
		{
			_poolScene = SceneManager.CreateScene(name);
			_pools = new List<Shape>[_shapes.Length];
			for (var i = 0; i < _pools.Length; i++)
				_pools[i] = new List<Shape>(500);
		}
		else
		{
			throw new Exception("Already inited");
		}
	}
}