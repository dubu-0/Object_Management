using UnityEngine;

public class SpawnZone : MonoBehaviour
{
	// Spawn point relative to this transform
	public Vector3 SpawnPoint => transform.TransformPoint(Random.insideUnitSphere);
}