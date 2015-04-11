using System;
using UnityEngine;
using CUF;

namespace Wraithguard
{
	public class PauseMenu
	{
		public bool isVisible = false;
		
		public PauseMenu(Action onReturnToGame, Action onQuitToMainMenu)
		{
			this.onReturnToGame = onReturnToGame;
			this.onQuitToMainMenu = onQuitToMainMenu;
			
			buttonLayout = new VerticalRectangleLayout(new Vector2(10, 10), 10);
			buttonLayout.AddRectangle(buttonSize);
			buttonLayout.AddRectangle(buttonSize);
		}
		public void OnGUI()
		{
			if(isVisible)
			{
				if(GUI.Button(buttonLayout.rectangles[0], "Return To Game"))
				{
					if(onReturnToGame != null)
					{
						onReturnToGame();
					}
				}
				
				if(GUI.Button(buttonLayout.rectangles[1], "Quit To Main Menu"))
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
		
		private VerticalRectangleLayout buttonLayout;
		
		private readonly Vector2 buttonSize = new Vector2(200, 50);
	}
}