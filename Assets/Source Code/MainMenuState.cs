using UnityEngine;

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
			Global.CreateCamera();
		}
		public override void OnGUI()
		{
			if(GUI.Button(buttonLayout.rectangles[0], "Play"))
			{
				Global.ClearSceneAndChangeGameState(new PlayState());
			}
			
			if(GUI.Button(buttonLayout.rectangles[1], "Editor"))
			{
				Global.ClearSceneAndChangeGameState(new EditorState());
			}
			
			if(GUI.Button(buttonLayout.rectangles[2], "Test"))
			{
				Global.ClearSceneAndChangeGameState(new TestState());
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