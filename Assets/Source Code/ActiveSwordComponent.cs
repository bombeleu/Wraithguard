using UnityEngine;

namespace Wraithguard
{
	public class ActiveSwordComponent : MonoBehaviour
	{
		public GameObject owner;
		
		private void OnTriggerEnter(Collider collider)
		{
			Global.DamageObject(collider.gameObject, 25, owner);
		}
	}
}