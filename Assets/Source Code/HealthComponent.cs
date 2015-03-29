using UnityEngine;

namespace Wraithguard
{
	public class HealthComponent : MonoBehaviour
	{
		public float health;
		
		private void OnStart()
		{
			Debug.Assert(health > 0);
		}
		
		private void TakeDamage(TakeDamageArgs arg)
		{
			health -= arg.damage;
			
			if(health <= 0)
			{
				Destroy(gameObject);
			}
		}
	}
}