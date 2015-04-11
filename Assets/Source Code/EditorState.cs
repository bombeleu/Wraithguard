using UnityEngine;
using CUF;

namespace Wraithguard
{
	public class EditorState : GameState
	{
		public override void OnStart()
		{
			Framework.CreateCamera();
			
			Mesh mesh = EditMesh.CreateTorus(0.5f, 0.1f, 32, 16).ToMesh();
			
			GameObject meshObject = new GameObject("generatedMesh");
			meshObject.AddComponent<MeshFilter>().mesh = mesh;
			meshObject.AddComponent<MeshRenderer>();
			
			meshObject.transform.position = Vector3.forward * 5;
		}
		public override void OnUpdate()
		{
			if(Input.GetKeyDown(KeyCode.Escape))
			{
				Framework.ClearSceneAndChangeGameState(new MainMenuState());
			}
		}
	}
}