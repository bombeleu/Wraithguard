using UnityEngine;

namespace Wraithguard
{
	public static class Math
	{
		public static Vector3 Reject(Vector3 a, Vector3 b)
		{
			return a - Vector3.Project(a, b);
		}
		
		public static Vector2 AngleToUnitVector(float angle)
		{
			return new Vector3(Mathf.Cos(angle), Mathf.Sin(angle));
		}
		
		public static Vector3 SphericalCoordinatesToUnitVector(float azimuth, float polarAngle)
		{
			float sinA = Mathf.Sin(azimuth);
			float cosA = Mathf.Cos(azimuth);
			float sinP = Mathf.Sin(polarAngle);
			float cosP = Mathf.Cos(polarAngle);
			
			return new Vector3(cosA * sinP, cosP, sinA * sinP);
		}
		public static Vector3 SphericalCoordinatesToVector(float radius, float azimuth, float polarAngle)
		{
			return radius * SphericalCoordinatesToUnitVector(azimuth, polarAngle);
		}
		
		public static Vector3 LatitudeLongitudeToUnitVector(float latitude, float longitude)
		{
			return SphericalCoordinatesToUnitVector(longitude, (Mathf.PI / 2) - latitude);
		}
		public static Vector3 LatitudeLongitudeElevationToVector(float latitude, float longitude, float elevation)
		{
			return elevation * LatitudeLongitudeToUnitVector(latitude, longitude);
		}
		
		public static Vector3 GetPointOnTorus(float majorRadius, float minorRadius, float majorAngle, float minorAngle)
		{
			float sinMaj = Mathf.Sin(majorAngle);
			float cosMaj = Mathf.Cos(majorAngle);
			float sinMin = Mathf.Sin(minorAngle);
			float cosMin = Mathf.Cos(minorAngle);
			float totalRadius = majorRadius + (minorRadius * cosMin);
			
			return new Vector3(totalRadius * cosMaj, minorRadius * sinMin, totalRadius * sinMaj);
		}
	}
}