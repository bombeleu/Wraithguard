using UnityEngine;

namespace Wraithguard
{
	public struct TakeDamageArgs
	{
		public float damage;
		public GameObject attacker;
		
		public TakeDamageArgs(float damage, GameObject attacker)
		{
			this.damage = damage;
			this.attacker = attacker;
		}
	}
}