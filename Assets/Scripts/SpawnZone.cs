using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnZone : MonoBehaviour
{
	[SerializeField] private bool _surfaceOnly;
	
	// Spawn point relative to this transform
	public Vector3 SpawnPoint => transform.TransformPoint(_surfaceOnly ? Random.onUnitSphere : Random.insideUnitSphere);
}