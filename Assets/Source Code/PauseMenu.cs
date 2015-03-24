using System;
using UnityEngine;

namespace Wraithguard
{
	public class PauseMenu
	{
		public bool isVisible = false;
		
		public PauseMenu(Action onReturnToGame, Action onQuitToMainMenu)
		{
			this.onReturnToGame = onReturnToGame;
			this.onQuitToMainMenu = onQuitToMainMenu;
		}
		public void OnGUI()
		{
			if(isVisible)
			{
				if(GUI.Button(new Rect(10, 50, 200, 50), "Return To Game"))
				{
					if(onReturnToGame != null)
					{
						onReturnToGame();
					}
				}
				
				if(GUI.Button(new Rect(10, 110, 200, 50), "Quit To Main Menu"))
				{
					if(onQuitToMainMenu != null)
					{
						onQuitToMainMenu();
					}
				}
			}
		}
		
		private Action onReturnToGame;
		private Action onQuitToMainMenu;
	}
}