using System;
using System.IO;
using UnityEngine;

namespace SaveSystem.Services
{
	public class GameDataWriter : IDisposable
	{
		private readonly BinaryWriter _binaryWriter;

		public GameDataWriter(BinaryWriter binaryWriter)
		{
			_binaryWriter = binaryWriter;
		}

		public void Dispose()
		{
			_binaryWriter?.Dispose();
		}

		public void Write(int value)
		{
			_binaryWriter.Write(value);
		}

		public void Write(float value)
		{
			_binaryWriter.Write(value);
		}

		public void Write(Vector3 value)
		{
			_binaryWriter.Write(value.x);
			_binaryWriter.Write(value.y);
			_binaryWriter.Write(value.z);
		}

		public void Write(Quaternion value)
		{
			_binaryWriter.Write(value.x);
			_binaryWriter.Write(value.y);
			_binaryWriter.Write(value.z);
			_binaryWriter.Write(value.w);
		}

		public void Write(Color color)
		{
			_binaryWriter.Write(color.r);
			_binaryWriter.Write(color.g);
			_binaryWriter.Write(color.b);
			_binaryWriter.Write(color.a);
		}
	}
}
