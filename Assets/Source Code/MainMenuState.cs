using UnityEngine;

namespace Wraithguard
{
	public class MainMenuState : GameState
	{
		public override void OnGUI()
		{
			if(GUI.Button(new Rect(10, 10, 200, 50), "Play"))
			{
				Global.instance.ChangeGameState(new PlayState());
			}
			
			if(GUI.Button(new Rect(10, 70, 200, 50), "Quit"))
			{
				Application.Quit();
			}
		}
	}
}