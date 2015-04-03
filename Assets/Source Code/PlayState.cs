using UnityEngine;

namespace Wraithguard
{
	public class PlayState : GameState
	{
		public PlayState()
		{
			pauseMenu = new PauseMenu(
			delegate()
			{
				TogglePauseMenu();
			},
			
			delegate()
			{
				Global.ClearSceneAndChangeGameState(new MainMenuState());
			});
			
			console = new Console(OnCommandEntered);
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
			
			if(Input.GetKeyDown(KeyCode.BackQuote))
			{
				ToggleConsole();
			}
			
			if(!pauseMenu.isVisible && !console.isVisible && Input.GetKeyDown(KeyCode.B))
			{
				ToggleInventoryWindow();
			}
		}
		public override void OnGUI()
		{
			if(player != null)
			{
				GUI.Label(new Rect(10, 10, 200, 50), player.GetComponent<StatsComponent>().attributes.health.value.ToString());
				
				DrawCrosshair();
				
				console.OnGUI();
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
		private const float enemyCount = terrainArea / 4096;
		
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
			player.AddComponent<StatsComponent>().attributes.health.value = 100;
			player.AddComponent<InventoryComponent>().inventory = new Inventory();
		}
		private GameObject CreateEnemy(Vector3 position)
		{
			GameObject enemy = GameObject.CreatePrimitive(PrimitiveType.Capsule);
			
			Rigidbody rigidbody = enemy.AddComponent<Rigidbody>();
			rigidbody.mass = Measures.averageHumanMass;
			rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
			
			enemy.AddComponent<StatsComponent>().attributes.health.value = 100;
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
		
		#region GUI
		private PauseMenu pauseMenu;
		private InventoryWindow inventoryWindow;
		private Console console;
		
		private Vector2 crosshairSize = new Vector2(25, 25);
		private const float crosshairThickness = 2;
		private readonly Color crosshairColor = Color.red;
		private Texture2D whiteTexture;
		
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
		private void ToggleConsole()
		{
			if(!console.isVisible)
			{
				PauseWorld();
				
				console.isVisible = true;
				
				OnEnterGUIScreen();
			}
			else
			{
				UnpauseWorld();
				
				console.isVisible = false;
				
				OnExitGUIScreen();
			}
		}
		private void OnCommandEntered(string command)
		{
			if(command == "quit")
			{
				Global.ClearSceneAndChangeGameState(new MainMenuState());
			}
		}
		private void DrawCrosshair()
		{
			if(whiteTexture == null)
			{
				whiteTexture = new Texture2D(1, 1);
				whiteTexture.SetPixel(1, 1, Color.white);
				whiteTexture.Apply();
			}
			
			Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
			Vector2 halfCrosshairSize = crosshairSize / 2;
			const float halfCrosshairThickness = crosshairThickness / 2;
			
			Color savedGUITint = GUI.color;
			
			GUI.color = crosshairColor;
			
			// Draw the horizontal line.
			GUI.DrawTexture(new Rect(screenCenter.x - halfCrosshairSize.x, screenCenter.y - halfCrosshairThickness, crosshairSize.x, crosshairThickness), whiteTexture);
			
			// Draw the vertical line.
			GUI.DrawTexture(new Rect(screenCenter.x - halfCrosshairThickness, screenCenter.y - halfCrosshairSize.y, crosshairThickness, crosshairSize.y), whiteTexture);
			
			GUI.color = savedGUITint;
		}
		#endregion
	}
}