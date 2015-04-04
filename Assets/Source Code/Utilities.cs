namespace Wraithguard
{
	public static class Utilities
	{
		public static void Append<T>(ref T[] a, T[] b)
		{
			T[] newArray = new T[a.Length + b.Length];
			a.CopyTo(newArray, 0);
			b.CopyTo(newArray, a.Length);
			
			a = newArray;
		}
	}
}