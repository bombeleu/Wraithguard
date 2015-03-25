using UnityEngine;

namespace Wraithguard
{
	public class ContainerComponent : MonoBehaviour
	{
		private void Activate(GameObject activator)
		{
			UnityEngine.Debug.Log("Chest activated!");
		}
	}
}