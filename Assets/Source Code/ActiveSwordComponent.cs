using UnityEngine;

namespace Wraithguard
{
	public class ActiveSwordComponent : MonoBehaviour
	{
		private void OnTriggerEnter(Collider collider)
		{
			Global.DamageObject(collider.gameObject, 25);
		}
	}
}