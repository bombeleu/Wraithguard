using UnityEngine;

namespace Wraithguard
{
	public class ArrowComponent : MonoBehaviour
	{
		private void OnCollisionEnter(Collision collision)
		{
			Global.DamageObject(collision.gameObject, 10);
			
			Destroy(gameObject);
		}
	}
}