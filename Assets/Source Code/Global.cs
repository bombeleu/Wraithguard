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
		
		public void ActivateObject(GameObject gameObject, GameObject activator)
		{
			gameObject.SendMessage("Activate", activator, SendMessageOptions.DontRequireReceiver);
		}
		public void DamageObject(GameObject gameObject, float damage)
		{
			Debug.Assert(damage >= 0);
			
			gameObject.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
		}
		
		// GameObject Creation Functions
		// =============================
		public GameObject CreateCamera()
		{
			GameObject camera = new GameObject("camera");
			camera.AddComponent<Camera>();
			
			return camera;
		}
		public GameObject CreateDirectionalLight()
		{
			GameObject directionalLight = new GameObject("directionalLight");
			directionalLight.AddComponent<Light>().type = LightType.Directional;
			
			return directionalLight;
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