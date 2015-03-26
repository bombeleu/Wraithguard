using UnityEngine;

namespace Wraithguard
{
	public class InventoryComponent : MonoBehaviour
	{
		public Inventory inventory;
		
		private void Activate(GameObject activator)
		{
			InventoryComponent activatorInventoryComponent = activator.GetComponent<InventoryComponent>();
			
			if(activatorInventoryComponent != null)
			{
				activatorInventoryComponent.inventory.AddItem(0);
				
				Debug.Log("Added an item to your inventory.");
			}
		}
	}
}