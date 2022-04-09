using System;
using SaveSystem;
using SaveSystem.Services;
using UnityEngine;

namespace ShapeSystem
{
	[DisallowMultipleComponent]
	[RequireComponent(typeof(MeshRenderer))]
	public class Shape : MonoBehaviour, IPersistableObject
	{
		private static readonly int ColorID = Shader.PropertyToID("_Color");
		private static MaterialPropertyBlock _block;
		private MeshRenderer _meshRenderer;

		public int ID { get; private set; } = -1;
		public int MaterialID { get; private set; } = -1;
		public Color Color { get; private set; }

		public bool ShapeIDNotSet => ID < 0;
		public bool MaterialIDNotSet => MaterialID < 0;

		private void Awake()
		{
			_meshRenderer = GetComponent<MeshRenderer>();
		}

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

		public void SetMaterial(Material material)
		{
			_meshRenderer.material = material;
		}

		public void SetColor(Color color)
		{
			_block ??= new MaterialPropertyBlock();
			_block.SetColor(ColorID, color);
			Color = color;
			_meshRenderer.SetPropertyBlock(_block);
		}

		public void InitID(int id)
		{
			if (ID < 0 && id >= 0)
				ID = id;
			else if (ID >= 0)
				throw new Exception("Already inited");
			else
				throw new ArgumentOutOfRangeException(nameof(id), "id < 0");
		}

		public void InitMaterialID(int materialID)
		{
			if (MaterialID < 0 && materialID >= 0)
				MaterialID = materialID;
			else if (MaterialID >= 0)
				throw new Exception("Already inited");
			else
				throw new ArgumentOutOfRangeException(nameof(materialID), "id < 0");
		}
	}
}
