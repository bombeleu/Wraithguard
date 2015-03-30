using System;
using UnityEngine;
using Wraithguard;

namespace Wraithguard
{
	public class Global : MonoBehaviour
	{
		// Textures
		// ========
		public Texture2D grassAlbedoTexture;
		
		private void OnStart()
		{
			ChangeGameState(new MainMenuState());
		}
		
		public static void ActivateObject(GameObject gameObject, GameObject activator)
		{
			gameObject.SendMessage("Activate", activator, SendMessageOptions.DontRequireReceiver);
		}
		public static void DamageObject(GameObject gameObject, float damage, GameObject attacker = null)
		{
			Debug.Assert(damage >= 0);
			
			gameObject.SendMessage("TakeDamage", new TakeDamageArgs(damage, attacker), SendMessageOptions.DontRequireReceiver);
		}
		
		public static void ClearSceneAndChangeGameState(GameState gameState)
		{
			Global.instance.ChangeGameStateImmediately(null);
			
			Global.instance.onNextSceneLoaded = delegate()
			{
				Global.instance.ChangeGameState(gameState);
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
		public static Global instance
		{
			get
			{
				return _instance;
			}
		}
		private static Global _instance;
		
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
			if(_instance == null)
			{
				_instance = this;
				
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