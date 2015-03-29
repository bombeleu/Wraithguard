using UnityEngine;

namespace Wraithguard
{
	public class ArrowComponent : MonoBehaviour
	{
		public GameObject owner;
		
		private void OnCollisionEnter(Collision collision)
		{
			Global.DamageObject(collision.gameObject, 10, owner);
			
			Destroy(gameObject);
		}
	}
}