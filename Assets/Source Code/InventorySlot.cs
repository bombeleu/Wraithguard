namespace Wraithguard
{
	public class InventorySlot
	{
		public uint itemTypeID;
		public uint itemCount;
		
		public InventorySlot()
		{
		}
		public InventorySlot(uint itemTypeID, uint itemCount)
		{
			this.itemTypeID = itemTypeID;
			this.itemCount = itemCount;
		}
	}
}