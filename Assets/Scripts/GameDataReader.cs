using System;
using System.IO;
using UnityEngine;

public class GameDataReader : IDisposable
{
	private readonly BinaryReader _binaryReader;

	public GameDataReader(BinaryReader binaryReader)
	{
		_binaryReader = binaryReader;
	}

	public void Dispose()
	{
		_binaryReader?.Dispose();
	}

	public int ReadInt()
	{
		return _binaryReader.ReadInt32();
	}

	public float ReadFloat()
	{
		return _binaryReader.ReadSingle();
	}

	public Vector3 ReadVector3()
	{
		Vector3 vector3;
		vector3.x = _binaryReader.ReadSingle();
		vector3.y = _binaryReader.ReadSingle();
		vector3.z = _binaryReader.ReadSingle();
		return vector3;
	}

	public Quaternion ReadQuaternion()
	{
		Quaternion quaternion;
		quaternion.x = _binaryReader.ReadSingle();
		quaternion.y = _binaryReader.ReadSingle();
		quaternion.z = _binaryReader.ReadSingle();
		quaternion.w = _binaryReader.ReadSingle();
		return quaternion;
	}
}