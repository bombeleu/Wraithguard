using UnityEngine;

namespace Wraithguard
{
	public class TestState : GameState
	{
		public override void OnStart()
		{
			Global.CreateCamera();

			(new GameObject()).AddComponent<BowComponent>();
		}
		public override void OnUpdate()
		{
			if(Input.GetKeyDown(KeyCode.Escape))
			{
				Global.ClearSceneAndChangeGameState(new MainMenuState());
			}
		}
	}
}