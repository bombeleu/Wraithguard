using UnityEngine;
using Wraithguard;

namespace Wraithguard
{
	public class Global : MonoBehaviour
	{
		private void OnStart()
		{
			CreateCamera().AddComponent<FlyingCameraComponent>();
			
			GameObject.CreatePrimitive(PrimitiveType.Cube);
		}
		
		private GameObject CreateCamera()
		{
			GameObject camera = new GameObject("camera");
			camera.AddComponent<Camera>();
			
			return camera;
		}
		
		// Internal stuff.
		public static Global instance;
		
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
	}
}