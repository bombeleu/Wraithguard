using System;
using UnityEngine;
using Wraithguard;

namespace Wraithguard
{
	public class Global : MonoBehaviour
	{
		private void OnStart()
		{
			ChangeGameState(new MainMenuState());
		}
		
		// Utilities
		// =========
		public GameObject CreateCamera()
		{
			GameObject camera = new GameObject("camera");
			camera.AddComponent<Camera>();
			
			return camera;
		}
		
		// Internal Stuff
		// ==============
		public static Global instance;
		
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
			if(instance == null)
			{
				instance = this;
				
				DontDestroyOnLoad(gameObject);
			}
			else
			{
				enabled = false;
				Destroy(gameObject);
				
				return;
			}
			
			OnStart();
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