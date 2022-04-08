using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnZone : MonoBehaviour
{
	[SerializeField] private bool _surfaceOnly;
	
	public Vector3 SpawnPoint => transform.TransformPoint(_surfaceOnly ? Random.onUnitSphere : Random.insideUnitSphere);

	private void OnDrawGizmos()
	{
		Gizmos.color = Color.white;
		Gizmos.matrix = transform.localToWorldMatrix;
		Gizmos.DrawWireSphere(Vector3.zero, 1f);
	}
}