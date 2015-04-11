using System;
using UnityEngine;

namespace CUF
{
	public class Framework : CUF.GlobalComponent<Framework>
	{
		public static void ClearSceneAndChangeGameState(GameState gameState)
		{
			Framework.instance.ChangeGameStateImmediately(null);
			
			Framework.instance.onNextSceneLoaded = delegate()
			{
				Framework.instance.ChangeGameState(gameState);
			};
			
			Application.LoadLevel(0);
		}
		
		// GameObject Creation Functions
		// =============================
		public static GameObject CreateCamera()
		{
			GameObject camera = new GameObject("camera");
			camera.AddComponent<Camera>();
			
			return camera;
		}
		public static GameObject CreateDirectionalLight()
		{
			GameObject directionalLight = new GameObject("directionalLight");
			directionalLight.AddComponent<Light>().type = LightType.Directional;
			
			return directionalLight;
		}
		
		// Internal Stuff
		// ==============
		public Action onNextSceneLoaded;
		
		public void ChangeGameState(GameState gameState)
		{
			nextGameState = gameState;
			shouldChangeGameState = true;
		}
		public void ChangeGameStateImmediately(GameState gameState)
		{
			if(currentGameState != null)
			{
				currentGameState.OnStop();
			}
			
			currentGameState = gameState;
			
			if(currentGameState != null)
			{
				currentGameState.OnStart();
			}
			
			nextGameState = null;
			shouldChangeGameState = false;
		}
		
		private GameState currentGameState;
		private GameState nextGameState;
		private bool shouldChangeGameState;
		
		private void Start()
		{
			base.OnStart();
		}
		private void OnDestroy()
		{
			ChangeGameStateImmediately(null); // Stop the current game state.
		}
		private void Update()
		{
			if(shouldChangeGameState)
			{
				ChangeGameStateImmediately(nextGameState);
			}
			
			if(currentGameState != null)
			{
				currentGameState.OnUpdate();
			}
		}
		private void OnGUI()
		{
			if(currentGameState != null)
			{
				currentGameState.OnGUI();
			}
		}
		private void OnLevelWasLoaded(int sceneIndex)
		{
			if(onNextSceneLoaded != null)
			{
				onNextSceneLoaded();
				
				onNextSceneLoaded = null;
			}
		}
	}
}