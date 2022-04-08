using UnityEngine;

public class SpawnZone : MonoBehaviour
{
	public Vector3 SpawnPoint => Random.insideUnitSphere * 5f;
}