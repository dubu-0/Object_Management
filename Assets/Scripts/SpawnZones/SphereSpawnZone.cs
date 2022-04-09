using UnityEngine;

namespace SpawnZones
{
	public class SphereSpawnZone : SpawnZone
	{
		[SerializeField] protected bool _surfaceOnly;
		
		public override Vector3 SpawnPoint => transform.TransformPoint(_surfaceOnly ? Random.onUnitSphere : Random.insideUnitSphere);

		private void OnDrawGizmos()
		{
			Gizmos.color = Color.cyan;
			Gizmos.matrix = transform.localToWorldMatrix;
			Gizmos.DrawWireSphere(Vector3.zero, 1f);
		}
	}
}