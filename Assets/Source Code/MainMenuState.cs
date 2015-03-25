using UnityEngine;

namespace Wraithguard
{
	public class MainMenuState : GameState
	{
		public override void OnStart()
		{
			Global.instance.CreateCamera();
		}
		public override void OnGUI()
		{
			if(GUI.Button(new Rect(10, 10, 200, 50), "Play"))
			{
				Play();
			}
			
			if(GUI.Button(new Rect(10, 70, 200, 50), "Quit"))
			{
				Application.Quit();
			}
		}
		
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