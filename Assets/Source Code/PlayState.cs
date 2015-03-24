using UnityEngine;

namespace Wraithguard
{
	public class PlayState : GameState
	{
		public PlayState()
		{
			pauseMenu = new PauseMenu(delegate(){TogglePauseMenu();}, delegate(){ReturnToMainMenu();});
		}
		public override void OnStart()
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
			
			Global.instance.CreateCamera().AddComponent<FlyingCameraComponent>();
			
			GameObject.CreatePrimitive(PrimitiveType.Cube);
		}
		public override void OnStop()
		{
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}
		public override void OnUpdate()
		{
			if(Input.GetKeyDown(KeyCode.Escape))
			{
				TogglePauseMenu();
			}
		}
		public override void OnGUI()
		{
			GUI.Label(new Rect(10, 10, 300, 25), "Press Escape to return to the main menu.");
			
			pauseMenu.OnGUI();
		}
		
		private PauseMenu pauseMenu;
		
		private void TogglePauseMenu()
		{
			if(!pauseMenu.isVisible)
			{
				pauseMenu.isVisible = true;
				
				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.None;
			}
			else
			{
				pauseMenu.isVisible = false;
				
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
			}
		}
		private void ReturnToMainMenu()
		{
			Global.instance.ChangeGameStateImmediately(null);
			
			Global.instance.onNextSceneLoaded = delegate()
			{
				Global.instance.ChangeGameState(new MainMenuState());
			};
			
			Application.LoadLevel(0);
		}
	}
}