using UnityEngine;

namespace Wraithguard
{
	public class EditorState : GameState
	{
		public override void OnStart()
		{
			Global.CreateCamera();
			
			Mesh mesh = MeshUtilities.CreateTorusMesh(0.5f, 0.1f, 32, 16);
			
			GameObject meshObject = new GameObject("generatedMesh");
			meshObject.AddComponent<MeshFilter>().mesh = mesh;
			meshObject.AddComponent<MeshRenderer>();
			
			meshObject.transform.position = Vector3.forward * 5;
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