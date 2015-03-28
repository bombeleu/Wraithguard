using System.Collections;
using System.Collections.Generic;

namespace Wraithguard
{
	public class Inventory : IEnumerable
	{
		public uint slotCount
		{
			get
			{
				return (uint)slots.Count;
			}
		}
		
		public Inventory()
		{
			slots = new List<InventorySlot>();
		}
		
		public uint GetItemCount(uint itemTypeID)
		{
			InventorySlot slot = FindSlot(itemTypeID);
			
			if(slot != null)
			{
				return slot.itemCount;
			}
			else
			{
				return 0;
			}
		}
		
		public void AddItem(uint itemTypeID)
		{
			AddItems(itemTypeID, 1);
		}
		public void AddItems(uint itemTypeID, uint itemCount)
		{
			Debug.Assert(itemCount > 0);
			
			InventorySlot slot = FindSlot(itemTypeID);
			
			if(slot == null)
			{
				// if not, add a slot
				slots.Add(new InventorySlot(itemTypeID, itemCount));
			}
			else
			{
				slot.itemCount += itemCount;
			}
		}
		public void RemoveItem(uint itemTypeID)
		{
			RemoveItems(itemTypeID, 1);
		}
		public void RemoveItems(uint itemTypeID)
		{
			uint slotIndex;
			InventorySlot slot = FindSlot(itemTypeID, out slotIndex);
			
			Debug.Assert(slot != null);
			
			slots.RemoveAt((int)slotIndex);
		}
		public void RemoveItems(uint itemTypeID, uint itemCount)
		{
			uint slotIndex;
			InventorySlot slot = FindSlot(itemTypeID, out slotIndex);
			
			Debug.Assert(slot != null);
			Debug.Assert(itemCount <= slot.itemCount);
			
			if(itemCount < slot.itemCount)
			{
				slot.itemCount -= itemCount;
			}
			else // itemCount == slot.itemCount
			{
				slots.RemoveAt((int)slotIndex);
			}
		}
		public void Clear()
		{
			slots.Clear();
		}
		
		public IEnumerator GetEnumerator()
		{
			return slots.GetEnumerator();
		}
		
		private List<InventorySlot> slots;
		
		private InventorySlot FindSlot(uint itemTypeID, out uint slotIndex)
		{
			for(int i = 0; i < slots.Count; i++)
			{
				InventorySlot slot = slots[i];
				
				if(slot.itemTypeID == itemTypeID)
				{
					slotIndex = (uint)i;
					return slot;
				}
			}
			
			slotIndex = 0;
			return null;
		}
		private InventorySlot FindSlot(uint itemTypeID)
		{
			uint slotIndex;
			
			return FindSlot(itemTypeID, out slotIndex);
		}
	}
}