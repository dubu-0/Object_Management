using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnZone : MonoBehaviour
{
	[SerializeField] private bool _surfaceOnly;
	
	// Spawn point relative to this transform
	public Vector3 SpawnPoint => transform.TransformPoint(_surfaceOnly ? Random.onUnitSphere : Random.insideUnitSphere);

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.white;
		Gizmos.matrix = transform.localToWorldMatrix; // Gizmo also will be relative
		Gizmos.DrawWireSphere(Vector3.zero, 1f);
	}
}