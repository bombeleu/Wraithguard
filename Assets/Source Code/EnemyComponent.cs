using UnityEngine;

namespace Wraithguard
{
	public class EnemyComponent : MonoBehaviour
	{
		private Rigidbody rigidbody;
		private GameObject target;
		
		private const float wanderDirectionChangeCooldown = 1;
		private float timeUntilWanderDirectionChange;
		private Vector3 wanderDirection;
		
		private const float attackCooldown = 1;
		private float timeUntilAttackCooled = attackCooldown;
		
		private void Start()
		{
			rigidbody = GetComponent<Rigidbody>();
		}
		private void Update()
		{
			if(target == null)
			{
				timeUntilWanderDirectionChange = Mathf.Max(timeUntilWanderDirectionChange - Time.deltaTime, 0);
				
				if(timeUntilWanderDirectionChange <= 0)
				{
					wanderDirection = Quaternion.AngleAxis(Random.value * 360, Vector3.up) * Vector3.right;
					
					timeUntilWanderDirectionChange = wanderDirectionChangeCooldown;
				}
			}
			else
			{
				timeUntilAttackCooled = Mathf.Max(timeUntilAttackCooled - Time.deltaTime, 0);
			}
		}
		private void FixedUpdate()
		{
			const float movementForceMagnitude = 750;
			
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
					Vector3 horizontalTargetDisplacement = Vector3.ProjectOnPlane(targetDisplacement, Vector3.up);
					Vector3 horizontalDirectionToTarget = horizontalTargetDisplacement.normalized;
					
					rigidbody.AddForce(horizontalDirectionToTarget * movementForceMagnitude);
				}
			}
			else
			{
				rigidbody.AddForce(wanderDirection * movementForceMagnitude);
			}
		}
		
		private void TakeDamage(TakeDamageArgs arg)
		{
			target = arg.attacker;
		}
	}
}