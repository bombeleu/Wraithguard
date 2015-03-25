using UnityEngine;

namespace Wraithguard
{
	public static class PhysicsUtilities
	{
		public static bool RaycastClosest(Ray ray, out RaycastHit raycastHit, float maxDistance = Mathf.Infinity)
		{
			RaycastHit[] raycastHits = Physics.RaycastAll(ray, maxDistance);
			
			if(raycastHits.Length > 0)
			{
				uint closestHitIndex = 0;
				float closestHitDistance = Mathf.Infinity;
				
				for(uint raycastHitIndex = 0; raycastHitIndex < raycastHits.Length; raycastHitIndex++)
				{
					RaycastHit hit = raycastHits[raycastHitIndex];
					
					if(hit.distance < closestHitDistance)
					{
						closestHitIndex = raycastHitIndex;
						closestHitDistance = hit.distance;
					}
				}
				
				raycastHit = raycastHits[closestHitIndex];
				
				return true;
			}
			else
			{
				raycastHit = new RaycastHit();
				
				return false;
			}
		}
	}
}