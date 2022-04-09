using Extensions;
using UnityEngine;

namespace SpawnZones
{
	public class CubeSpawnZone : SpawnZone
	{
		[SerializeField] protected bool _surfaceOnly;
		
		public override Vector3 SpawnPoint => transform.TransformPoint(_surfaceOnly ? RandomExtension.OnUnitCube() : RandomExtension.InsideUnitCube());
	
		private void OnDrawGizmos()
		{
			Gizmos.color = Color.cyan;
			Gizmos.matrix = transform.localToWorldMatrix;
			Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
		}
	}
}
