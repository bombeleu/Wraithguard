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
		}
		public override void OnStart()
		{
			Global.CreateCamera();
		}
		public override void OnGUI()
		{
			if(GUI.Button(buttonLayout.rectangles[0], "Play"))
			{
				Play();
			}
			
			if(GUI.Button(buttonLayout.rectangles[1], "Quit"))
			{
				Application.Quit();
			}
		}
		
		private VerticalRectangleLayout buttonLayout;
		private readonly Vector2 buttonSize = new Vector2(200, 50);
		
		private void Play()
		{
			Global.instance.ChangeGameStateImmediately(null);
			
			Global.instance.onNextSceneLoaded = delegate()
			{
				Global.instance.ChangeGameState(new PlayState());
			};
			
			Application.LoadLevel(0);
		}
	}
}