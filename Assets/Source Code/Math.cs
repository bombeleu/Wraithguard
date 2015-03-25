using UnityEngine;

namespace Wraithguard
{
	public static class Math
	{
		public static Vector3 Reject(Vector3 a, Vector3 b)
		{
			return a - Vector3.Project(a, b);
		}
	}
}