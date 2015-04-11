using UnityEngine;
using CUF;

namespace Wraithguard
{
	public class MainMenuState : GameState
	{
		public MainMenuState()
		{
			buttonLayout = new VerticalRectangleLayout(new Vector2(20, 20), 10);
			buttonLayout.AddRectangle(buttonSize);
			buttonLayout.AddRectangle(buttonSize);
			buttonLayout.AddRectangle(buttonSize);
			buttonLayout.AddRectangle(buttonSize);
		}
		public override void OnStart()
		{
			Framework.CreateCamera();
		}
		public override void OnGUI()
		{
			if(GUI.Button(buttonLayout.rectangles[0], "Play"))
			{
				Framework.ClearSceneAndChangeGameState(new PlayState());
			}
			
			if(GUI.Button(buttonLayout.rectangles[1], "Editor"))
			{
				Framework.ClearSceneAndChangeGameState(new EditorState());
			}
			
			if(GUI.Button(buttonLayout.rectangles[2], "Test"))
			{
				Framework.ClearSceneAndChangeGameState(new TestState());
			}
			
			if(GUI.Button(buttonLayout.rectangles[3], "Quit"))
			{
				Application.Quit();
			}
		}
		
		private VerticalRectangleLayout buttonLayout;
		private readonly Vector2 buttonSize = new Vector2(200, 50);
	}
}