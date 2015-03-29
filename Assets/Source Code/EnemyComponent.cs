using UnityEngine;

namespace Wraithguard
{
	public class EnemyComponent : MonoBehaviour
	{
		private Rigidbody rigidbody;
		private GameObject target;
		
		private const float attackCooldown = 1;
		private float timeUntilAttackCooled;
		
		private void Start()
		{
			rigidbody = GetComponent<Rigidbody>();
		}
		private void Update()
		{
			timeUntilAttackCooled = Mathf.Max(timeUntilAttackCooled - Time.deltaTime, 0);
		}
		private void FixedUpdate()
		{
			if(target != null)
			{
				Vector3 targetDisplacement = target.transform.position - transform.position;
				float distanceToTarget = targetDisplacement.magnitude;
				
				if(distanceToTarget <= 2)
				{
					if(timeUntilAttackCooled <= 0)
					{
						Global.DamageObject(target, 10, gameObject);
						
						timeUntilAttackCooled = attackCooldown;
					}
				}
				else
				{
					Vector3 horizontalTargetDisplacement = Math.Reject(targetDisplacement, Vector3.up);
					Vector3 horizontalDirectionToTarget = horizontalTargetDisplacement.normalized;
					
					rigidbody.AddForce(horizontalDirectionToTarget * 750);
				}
			}
		}
		
		private void TakeDamage(TakeDamageArgs arg)
		{
			target = arg.attacker;
		}
	}
}