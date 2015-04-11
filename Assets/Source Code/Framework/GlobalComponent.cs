using UnityEngine;

namespace CUF
{
	public class GlobalComponent<ComponentType> : MonoBehaviour where ComponentType : class, new()
	{
		public static ComponentType instance
		{
			get
			{
				return _instance;
			}
		}
		private static ComponentType _instance;
		
		protected void OnStart()
		{
			if(_instance == null)
			{
				_instance = (ComponentType)((object)this);
				
				DontDestroyOnLoad(gameObject);
			}
			else
			{
				enabled = false;
				Destroy(gameObject);
				
				return;
			}
		}
	}
}