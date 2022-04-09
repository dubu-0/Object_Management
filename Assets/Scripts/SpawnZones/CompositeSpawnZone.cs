using UnityEngine;

namespace SpawnZones
{
	public class CompositeSpawnZone : SpawnZone
	{
		[SerializeField] private SpawnZone[] _spawnZones;

		public override Vector3 SpawnPoint => _spawnZones[Random.Range(0, _spawnZones.Length)].SpawnPoint;
	}
}
