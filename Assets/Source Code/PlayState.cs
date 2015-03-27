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
			
			camera = Global.instance.CreateCamera();
			
			player.GetComponent<PlayerComponent>().camera = camera;
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
		}
		public override void OnGUI()
		{
			pauseMenu.OnGUI();
		}
				
		private GameObject player;
		private GameObject camera;
		
		private PauseMenu pauseMenu;
		
		private void CreateTerrain()
		{
			TerrainData terrainData = new TerrainData();
			
			Terrain.CreateTerrainGameObject(terrainData);
		}
		private void LoadScene()
		{
			Global.instance.CreateDirectionalLight().transform.eulerAngles = new Vector3(45, 45, 0);
			
			CreateTerrain();
			
			GameObject chest = GameObject.CreatePrimitive(PrimitiveType.Cube);
			chest.transform.position = new Vector3(5, 0.5f, 15);
			chest.AddComponent<InventoryComponent>();
		}
		private void CreatePlayer(float playerHeight = Measures.averageMaleHumanHeight)
		{
			player = new GameObject("player");
			
			player.transform.position = new Vector3(10, 10, 3);
			
			CapsuleCollider collider = player.AddComponent<CapsuleCollider>();
			collider.height = playerHeight;
			collider.radius = 1.5f * Measures.footInMeters;
			
			Rigidbody rigidbody = player.AddComponent<Rigidbody>();
			rigidbody.mass = 75;
			rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
			
			player.AddComponent<PlayerComponent>().height = playerHeight;
			
			player.AddComponent<InventoryComponent>().inventory = new Inventory();
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
		
		private void TogglePauseMenu()
		{
			if(!pauseMenu.isVisible)
			{
				PauseWorld();
				
				pauseMenu.isVisible = true;
				
				Cursor.visible = true;
				Cursor.lockState = CursorLockMode.None;
			}
			else
			{
				UnpauseWorld();
				
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