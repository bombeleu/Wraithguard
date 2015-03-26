using UnityEngine;

namespace Wraithguard
{
	public class ObjectComponent : MonoBehaviour
	{
		public uint objectTypeID;
		
		private void Activate(GameObject activator)
		{
			InventoryComponent activatorInventoryComponent = activator.GetComponent<InventoryComponent>();
			
			if(activatorInventoryComponent != null)
			{
				activatorInventoryComponent.inventory.AddItem(objectTypeID);
				
				Destroy(gameObject);
			}
		}
	}
}