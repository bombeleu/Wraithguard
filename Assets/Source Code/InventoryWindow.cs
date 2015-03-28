using UnityEngine;

namespace Wraithguard
{
	public class InventoryWindow
	{
		public bool isVisible = false;
		public readonly Inventory inventory;
		
		public InventoryWindow(Inventory inventory)
		{
			this.inventory = inventory;
		}
		public void OnGUI()
		{
			if(isVisible)
			{
				rectangle = GUI.Window(0, rectangle, DoWindow, "Inventory");
			}
		}
		
		private Rect rectangle = new Rect(10, 10, 500, 300);
		private readonly Vector2 slotGUISize = new Vector2(64, 64);
		
		private void DoWindow(int windowID)
		{
			GUI.DragWindow();
			
			RectOffset windowBorder = GUI.skin.window.border;
			Vector2 position = new Vector2(windowBorder.left, windowBorder.top);
			
			foreach(InventorySlot slot in inventory)
			{
				GUI.Box(new Rect(position.x, position.y, slotGUISize.x, slotGUISize.y), slot.itemCount.ToString());
				
				position.x += slotGUISize.x + 10;
			}
		}
	}
}