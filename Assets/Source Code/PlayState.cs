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
			
			LoadScene();
			CreatePlayer();
			
			camera = Global.CreateCamera();
			
			player.GetComponent<PlayerComponent>().camera = camera;
			
			inventoryWindow = new InventoryWindow(player.GetComponent<InventoryComponent>().inventory);
		}
		public override void OnStop()
		{
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
			
			UnpauseWorld();
		}
		public override void OnUpdate()
		{
			if(Input.GetKeyDown(KeyCode.Escape))
			{
				TogglePauseMenu();
			}
			
			if(!pauseMenu.isVisible && Input.GetKeyDown(KeyCode.B))
			{
				ToggleInventoryWindow();
			}
		}
		public override void OnGUI()
		{
			if(player != null)
			{
				GUI.Label(new Rect(10, 10, 200, 50), player.GetComponent<HealthComponent>().health.ToString());
			}
			
			pauseMenu.OnGUI();
			inventoryWindow.OnGUI();
		}
				
		private GameObject player;
		private GameObject camera;
		
		private const float terrainWidth = 1024;
		private const float maxTerrainHeight = terrainWidth;
		private const float terrainLength = terrainWidth;
		
		private const float terrainArea = terrainWidth * terrainLength;
		private const float enemyCount = terrainArea / 2048;
		
		private void CreateTerrain()
		{
			TerrainData terrainData = new TerrainData();
			
			terrainData.size = new Vector3(terrainWidth, maxTerrainHeight, terrainLength);
			
			SplatPrototype grassSplat = new SplatPrototype();
			grassSplat.texture = Global.instance.grassAlbedoTexture;
			
			terrainData.splatPrototypes = new SplatPrototype[]{grassSplat};
			
			Terrain.CreateTerrainGameObject(terrainData);
		}
		private void LoadScene()
		{
			Global.CreateDirectionalLight().transform.eulerAngles = new Vector3(45, 45, 0);
			
			CreateTerrain();
			
			GameObject chest = GameObject.CreatePrimitive(PrimitiveType.Cube);
			chest.transform.position = new Vector3(5, 0.5f, 15);
			chest.AddComponent<InventoryComponent>();
			
			for(uint enemyIndex = 0; enemyIndex < enemyCount; enemyIndex++)
			{
				CreateEnemy(new Vector3(Random.value * terrainWidth, 1, Random.value * terrainLength));
			}
			
		}
		private void CreatePlayer(float playerHeight = Measures.averageMaleHumanHeight)
		{
			player = new GameObject("player");
			
			player.transform.position = new Vector3(10, 10, 3);
			
			CapsuleCollider collider = player.AddComponent<CapsuleCollider>();
			collider.height = playerHeight;
			collider.radius = 1.5f * Measures.footInMeters;
			
			Rigidbody rigidbody = player.AddComponent<Rigidbody>();
			rigidbody.mass = Measures.averageHumanMass;
			rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
			
			player.AddComponent<PlayerComponent>().height = playerHeight;
			player.AddComponent<HealthComponent>().health = 100;
			player.AddComponent<InventoryComponent>().inventory = new Inventory();
		}
		private GameObject CreateEnemy(Vector3 position)
		{
			GameObject enemy = GameObject.CreatePrimitive(PrimitiveType.Capsule);
			
			Rigidbody rigidbody = enemy.AddComponent<Rigidbody>();
			rigidbody.mass = Measures.averageHumanMass;
			rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
			
			enemy.AddComponent<HealthComponent>().health = 100;
			enemy.AddComponent<EnemyComponent>();
			enemy.AddComponent<DamageBoxComponent>();
			
			enemy.transform.position = position;
			
			return enemy;
		}
		
		private void PauseWorld()
		{
			Time.timeScale = 0;
			
			if(player != null)
			{
				player.GetComponent<PlayerComponent>().enabled = false;
			}
		}
		private void UnpauseWorld()
		{
			Time.timeScale = 1;
			
			if(player != null)
			{
				player.GetComponent<PlayerComponent>().enabled = true;
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
		
		#region GUI
		private PauseMenu pauseMenu;
		private InventoryWindow inventoryWindow;
		
		private void OnEnterGUIScreen()
		{
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}
		private void OnExitGUIScreen()
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
		
		private void TogglePauseMenu()
		{
			if(!pauseMenu.isVisible)
			{
				PauseWorld();
				
				pauseMenu.isVisible = true;
				
				OnEnterGUIScreen();
			}
			else
			{
				UnpauseWorld();
				
				pauseMenu.isVisible = false;
				
				OnExitGUIScreen();
			}
		}
		private void ToggleInventoryWindow()
		{
			if(!inventoryWindow.isVisible)
			{
				PauseWorld();
				
				inventoryWindow.isVisible = true;
				
				OnEnterGUIScreen();
			}
			else
			{
				UnpauseWorld();
				
				inventoryWindow.isVisible = false;
				
				OnExitGUIScreen();
			}
		}
		#endregion
	}
}