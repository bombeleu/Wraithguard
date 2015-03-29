using UnityEngine;

namespace Wraithguard
{
	public class SpellComponent : MonoBehaviour
	{
		public GameObject owner;
		
		private const float explosionRadius = 15;
		private const float explosionImpulseMagnitude = 2000;
		
		private void OnCollisionEnter(Collision collision)
		{
			if(collision.gameObject != owner)
			{
				Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);
				
				foreach(Collider hitCollider in hitColliders)
				{
					GameObject hitGameObject = hitCollider.gameObject;
					Rigidbody hitRigidbody = hitCollider.attachedRigidbody;
					
					if(hitRigidbody != null && hitGameObject.name != "spell")
					{
						float distanceFromExplosion = Vector3.Distance(hitGameObject.transform.position, transform.position);
						
						Global.DamageObject(hitGameObject, 50 * (1 - (distanceFromExplosion / explosionRadius)), owner);
						
						hitRigidbody.AddExplosionForce(explosionImpulseMagnitude, transform.position, explosionRadius, 3, ForceMode.Impulse);
					}
				}
				
				Destroy(gameObject);
			}
		}
	}
}