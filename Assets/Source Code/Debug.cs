﻿namespace Wraithguard
{
	public static class Debug
	{
		public static void Assert(bool condition)
		{
			if(!condition)
			{
				LogError("An assertion failed.");
			}
		}
		public static void Log(object message)
		{
			UnityEngine.Debug.Log(message);
		}
		public static void LogError(object message)
		{
			UnityEngine.Debug.LogError(message);
		}
	}
}