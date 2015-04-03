namespace Wraithguard
{
	public struct Attribute
	{
		public static Attribute operator + (Attribute attribute, float delta)
		{
			return new Attribute(attribute.name, attribute.value + delta, attribute.maxValue);
		}
		public static Attribute operator - (Attribute attribute, float delta)
		{
			return attribute + (-delta);
		}
		public static implicit operator float(Attribute attribute)
		{
			return attribute.value;
		}
		
		public string name;
		public float value;
		public float maxValue;
		
		public Attribute(string name, float value, float maxValue)
		{
			this.name = name;
			this.value = value;
			this.maxValue = maxValue;
		}
	}
	
	public class Attributes
	{
		public Attribute health;
	}
}