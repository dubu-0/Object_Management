using UnityEngine;

namespace Extensions
{
	public static class RandomExtension
	{
		public static Vector3 InsideUnitCube()
		{
			Vector3 p;
			p.x = Random.Range(-0.5f, 0.5f);
			p.y = Random.Range(-0.5f, 0.5f);
			p.z = Random.Range(-0.5f, 0.5f);
			return p;
		}

		public static Vector3 OnUnitCube()
		{
			var p = InsideUnitCube();
			var axis = Random.Range(0, 3);
			p[axis] = p[axis] < 0f ? -0.5f : 0.5f;
			return p;
		}
	}
}