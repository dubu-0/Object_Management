using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace ShapeSystem
{
	[CreateAssetMenu(menuName = "Create SO/ShapeFactory", fileName = "ShapeFactory", order = 0)]
	public class ShapeFactory : ScriptableObject
	{
		[SerializeField] private bool _recycle;
		[SerializeField] private Shape[] _shapes;
		[SerializeField] private Material[] _materials;
		private Shape _instance;

		private List<Shape>[] _pools;
		private Scene _poolScene;

		public int ShapeTypeCount => _shapes.Length;

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
				if (_pools == null)
					InitPools();

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
			else
			{
				_instance = Instantiate(_shapes[shapeID]);
			}

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
				_pools = new List<Shape>[_shapes.Length];

				for (var i = 0; i < _pools.Length; i++)
					_pools[i] = new List<Shape>(10000);

				if (!_poolScene.isLoaded)
					_poolScene = SceneManager.CreateScene(name);

				SurviveHotReload();
			}
			else
			{
				throw new Exception("Already inited");
			}
		}

		private void SurviveHotReload()
		{
			if (Application.isEditor)
			{
				_poolScene = SceneManager.GetSceneByName(name);

				if (_poolScene.isLoaded)
				{
					var rootObjects = _poolScene.GetRootGameObjects();

					foreach (var rootObject in rootObjects)
					{
						var shape = rootObject.GetComponent<Shape>();

						if (!shape.gameObject.activeSelf)
							_pools[shape.ID].Add(shape);
					}
				}
			}
		}
	}
}
