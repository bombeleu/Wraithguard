using UnityEngine;
using Debug = CUF.Debug;

namespace Wraithguard
{
	public class Object
	{
		public static GameObject CreateGameObject(uint objectTypeID)
		{
			return CreateGameObject(objectTypeID, Vector3.zero, Quaternion.identity);
		}
		public static GameObject CreateGameObject(uint objectTypeID, Vector3 position)
		{
			return CreateGameObject(objectTypeID, position, Quaternion.identity);
		}
		public static GameObject CreateGameObject(uint objectTypeID, Vector3 position, Quaternion orientation)
		{
			GameObject createdObject = null;
			
			if(objectTypeID == 0)
			{
				createdObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
				createdObject.AddComponent<Rigidbody>().mass = 5;
				createdObject.AddComponent<DamageBoxComponent>();
				createdObject.AddComponent<StatsComponent>().attributes.health.value = 100;
			}
			else
			{
				Debug.Assert(false);
			}
			
			Debug.Assert(createdObject != null);
			
			createdObject.transform.position = position;
			createdObject.transform.rotation = orientation;
			
			createdObject.AddComponent<ObjectComponent>().objectTypeID = objectTypeID;
			
			return createdObject;
		}
		
		public readonly uint typeID;
		
		public Object(uint typeID)
		{
			this.typeID = typeID;
		}
	}
}