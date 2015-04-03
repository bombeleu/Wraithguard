using UnityEngine;

namespace Wraithguard
{
	public class StatsComponent : MonoBehaviour
	{
		public Attributes attributes;
		
		public StatsComponent()
		{
			attributes = new Attributes();
		}
		
		private void TakeDamage(TakeDamageArgs arg)
		{
			attributes.health -= arg.damage;
			
			if(attributes.health <= 0)
			{
				Destroy(gameObject);
			}
		}
	}
}