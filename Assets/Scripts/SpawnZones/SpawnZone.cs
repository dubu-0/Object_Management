using UnityEngine;

namespace SpawnZones
{
	[DisallowMultipleComponent]
	public abstract class SpawnZone : MonoBehaviour
	{
		public abstract Vector3 SpawnPoint { get; }
	}
}