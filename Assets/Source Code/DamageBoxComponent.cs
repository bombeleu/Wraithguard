using UnityEngine;

namespace Wraithguard
{
	public class DamageBoxComponent : MonoBehaviour
	{
		private readonly float damageCooldown = 1;
		
		private float timeUntilCooled;
		
		private MeshRenderer meshRenderer;
		private Color cooledColor;
		
		private void Start()
		{
			meshRenderer = GetComponent<MeshRenderer>();
			cooledColor = meshRenderer.material.color;
		}
		private void Update()
		{
			timeUntilCooled = Mathf.Clamp(timeUntilCooled - Time.deltaTime, 0, Mathf.Infinity);
			
			meshRenderer.material.color = Color.Lerp(cooledColor, Color.red, timeUntilCooled / damageCooldown);
		}
		
		private void TakeDamage(float damage)
		{
			timeUntilCooled = damageCooldown;
		}
	}
}