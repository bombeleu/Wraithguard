using UnityEngine;

namespace Wraithguard
{
	public class TestState : GameState
	{
		public override void OnStart()
		{
			Global.CreateCamera();
			
			EditMesh editMesh = EditMesh.CreateCylinderAlongCurve(PositionFunction, 0.02f, 128, 4, -1, 1);
			
			Bounds meshBounds = editMesh.ComputeBounds();
			
			Vector3[] bowStringPolyline = new Vector3[]
			{
				new Vector3(meshBounds.min.x, -meshBounds.extents.y, 0),
				new Vector3(meshBounds.min.x, 0, 0),
				new Vector3(meshBounds.min.x, meshBounds.extents.y, 0)
			};
			
			EditMesh bowString = EditMesh.CreateCylinderAlongPolyline(bowStringPolyline, 0.01f, 4);
			editMesh.Append(bowString);
			
			editMesh.Translate(-meshBounds.center);
			
			Mesh mesh = editMesh.ToMesh();
			
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
		
		public Vector3 PositionFunction(float t)
		{
			return Superellipse(2.5f, 1, 1, t);
		}
		public Vector3 Superellipse(float n, float a, float b, float t)
		{
			float twoOverN = 2 / n;
			float cosT = Mathf.Cos(t);
			float sinT = Mathf.Sin(t);
			
			float x = a * Mathf.Pow(Mathf.Abs(cosT), twoOverN) * Mathf.Sign(cosT);
			float y = b * Mathf.Pow(Mathf.Abs(sinT), twoOverN) * Mathf.Sign(sinT);
			
			return new Vector3(x, y, 0);
		}
	}
}