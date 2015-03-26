namespace Wraithguard
{
	public static class Debug
	{
		public static void Assert(bool condition)
		{
			if(!condition)
			{
				UnityEngine.Debug.LogError("An assertion failed.");
			}
		}
		public static void Log(object message)
		{
			UnityEngine.Debug.Log(message);
		}
	}
}