using System;
using UnityEngine;

namespace Wraithguard
{
	public class Console
	{
		public bool isVisible = false;
		
		public Console(Action<string> onCommandEntered)
		{
			this.onCommandEntered = onCommandEntered;
		}
		public void OnGUI()
		{
			if(isVisible)
			{
				if(Event.current.isKey && Event.current.keyCode == KeyCode.Return)
				{
					if(onCommandEntered != null)
					{
						onCommandEntered(commandString);
					}
					
					commandString = "";
				}
				
				commandString = GUI.TextField(new Rect(50, 50, 400, 30), commandString);
			}
		}
		
		private string commandString = "";
		
		private Action<string> onCommandEntered;
	}
}