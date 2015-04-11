using UnityEngine;

namespace CUF
{
	public static class Mouse
	{
		public static float mouseSensitivity = 1;
		
		public static Vector2 deltaMouse
		{
			get
			{
				return new Vector2(Input.GetAxis("Mouse X") * mouseSensitivity, Input.GetAxis("Mouse Y") * mouseSensitivity);
			}
		}
	}
}