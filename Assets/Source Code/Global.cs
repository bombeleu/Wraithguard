using System;
using UnityEngine;
using CUF;
using Debug = CUF.Debug;

namespace Wraithguard
{
	public class Global : CUF.GlobalComponent<Global>
	{
		// Textures
		// ========
		public Texture2D grassAlbedoTexture;
		
		public static void ActivateObject(GameObject gameObject, GameObject activator)
		{
			gameObject.SendMessage("Activate", activator, SendMessageOptions.DontRequireReceiver);
		}
		public static void DamageObject(GameObject gameObject, float damage, GameObject attacker = null)
		{
			Debug.Assert(damage >= 0);
			
			gameObject.SendMessage("TakeDamage", new TakeDamageArgs(damage, attacker), SendMessageOptions.DontRequireReceiver);
		}
		
		// Internal Stuff
		// ==============
		private void Start()
		{
			base.OnStart();
			
			gameObject.AddComponent<Framework>().ChangeGameState(new MainMenuState());
		}
	}
}