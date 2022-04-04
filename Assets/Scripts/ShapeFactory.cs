using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "Create SO/ShapeFactory", fileName = "ShapeFactory", order = 0)]
public class ShapeFactory : ScriptableObject
{
	[SerializeField] private bool _recycle;
	[SerializeField] private Shape[] _shapes;
	[SerializeField] private Material[] _materials;

	private List<Shape>[] _pools;

	public Shape Create(Color color, int shapeID = 0, int materialID = 0)
	{
		Shape instance = null;
		Setup(ref instance, shapeID, materialID, color);
		return instance;
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

		Shape instance = null;
		Setup(ref instance, randomID, randomMaterialID, randomColor);
		return instance;
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

	private void Setup([NotNull] ref Shape instance, int shapeID, int materialID, Color color)
	{
		if (_recycle)
		{
			if (_pools == null)
				InitPools();

			var pool = _pools[shapeID];
			var last = pool.Count - 1;

			if (last >= 0)
			{
				instance = pool[last];
				instance.gameObject.SetActive(true);
				pool.RemoveAt(last);
			}
		}
		
		if (instance == null)
			instance = Instantiate(_shapes[shapeID]);
		
		instance.transform.localPosition = Random.insideUnitSphere * 5f;
		instance.transform.localRotation = Random.rotation;
		instance.transform.localScale = Vector3.one * Random.Range(0.1f, 1f);

		if (instance.ShapeIDNotSet)
			instance.InitID(shapeID);
		if (instance.MaterialIDNotSet)
			instance.InitMaterialID(materialID);
		
		instance.SetMaterial(_materials[materialID]);
		instance.SetColor(color);
	}

	private void InitPools()
	{
		if (_pools == null)
		{
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