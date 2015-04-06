using UnityEngine;

namespace Wraithguard
{
	public static class Math
	{
		public delegate Vector2 Curve2DFunction(float t);
		public delegate Vector3 Curve3DFunction(float t);
		
		public const float pi = 3.14159265359f;
		public const float twoPi = 6.28318530718f;
		public const float piOverTwo = 1.57079632679f;
		public const float e = 2.718281828459045f;
		
		public static Vector3 Reject(Vector3 a, Vector3 b)
		{
			return a - Vector3.Project(a, b);
		}
		
		public static bool AreParallel(Vector3 a, Vector3 b)
		{
			return Vector3.Cross(a, b) == Vector3.zero;
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
		
		public static Vector3 CentralFiniteDifference(Vector3 previousVector, Vector3 nextVector, float h)
		{
			return (nextVector - previousVector) / (2 * h);
		}
		public static Vector3 CentralFiniteDifference2ndOrder(Vector3 previousVector, Vector3 vector, Vector3 nextVector, float h)
		{
			return (nextVector - (2 * vector) + previousVector) / (h * h);
		}
		
		public static void GetPolylinePlaneNormalsAndPerpendicularVectors(Vector3[] polylineVertices, out Vector3[] planeNormals, out Vector3[] perpendicularVectors)
		{
			
			planeNormals = new Vector3[polylineVertices.Length];
			perpendicularVectors = new Vector3[polylineVertices.Length];
			
			int lastVertexIndex = polylineVertices.Length - 1;
			
			for(int vertexIndex = 0; vertexIndex <= lastVertexIndex; vertexIndex++)
			{
				// Find the bisecting plane normal.
				Vector3 planeNormal;
				
				if(vertexIndex == 0)
				{
					planeNormal = polylineVertices[1] - polylineVertices[0];
				}
				else if(vertexIndex == lastVertexIndex)
				{
					planeNormal = polylineVertices[lastVertexIndex] - polylineVertices[lastVertexIndex - 1];
				}
				else
				{
					planeNormal = polylineVertices[vertexIndex + 1] - polylineVertices[vertexIndex - 1];
				}
				
				planeNormal.Normalize();
				planeNormals[vertexIndex] = planeNormal;
				
				// Find the perpendicular vector (always along the XZ plane).
				if(vertexIndex == 0)
				{
					if(!AreParallel(planeNormal, Vector3.up))
					{
						perpendicularVectors[vertexIndex] = Vector3.Cross(Vector3.up, planeNormal).normalized;
					}
					else
					{
						perpendicularVectors[vertexIndex] = Vector3.right;
					}
				}
				else
				{
					Vector3 previousPlaneNormal = planeNormals[vertexIndex - 1];
					float planeNormalsAngle = Vector3.Angle(planeNormal, previousPlaneNormal);
					
					if(Mathf.Approximately(planeNormalsAngle, 0) || Mathf.Approximately(planeNormalsAngle, 180))
					{
						perpendicularVectors[vertexIndex] = perpendicularVectors[vertexIndex - 1];
					}
					else
					{
						Vector3 planeNormalsRotationAxis = Vector3.Cross(previousPlaneNormal, planeNormal).normalized;
						Vector3 unprojectedPerpendicularVector = Quaternion.AngleAxis(planeNormalsAngle, planeNormalsRotationAxis) * perpendicularVectors[vertexIndex - 1];
						
						if(!AreParallel(unprojectedPerpendicularVector, Vector3.up))
						{
							perpendicularVectors[vertexIndex] = Vector3.ProjectOnPlane(unprojectedPerpendicularVector, Vector3.up).normalized;
						}
						else
						{
							perpendicularVectors[vertexIndex] = perpendicularVectors[vertexIndex - 1];
						}
					}
				}
			}
		}
		
		public static float NearestNeighborInterpolate(float a, float b, float t)
		{
			return (t < 0.5f) ? a : b;
		}
		public static float LinearInterpolate(float a, float b, float t)
		{
			return a + (t * (b - a));
		}
		
		public static float MassSpringDamper(float m, float k, float c, float x0, float dx0, float t)
		{
			float w0 = Mathf.Sqrt(k / m);
			float zeta = c / (2 * Mathf.Sqrt(m * k));
			
			Debug.Assert(zeta >= 0);
			
			if(zeta < 1) // under-damped
			{
				float wd = w0 * Mathf.Sqrt(1 - (zeta * zeta));
				float A = x0;
				float B = (1 / wd) * ((zeta * w0 * x0) + dx0);
				
				return Mathf.Pow(e, -(zeta * w0 * t)) * ((A * Mathf.Cos(wd * t)) + (B * Mathf.Sin(wd * t)));
			}
			else if(zeta == 1) // critically-damped
			{
				float A = x0;
				float B = dx0 + (w0 * x0);
				
				return (A + (B * t)) * Mathf.Pow(e, -(w0 * t));
			}
			else // over-damped
			{
				Debug.Assert(false); // hanven't implemented this yet
				return 0;
			}
		}
		
		public static Vector3 Superellipse(float n, float a, float b, float t)
		{
			float twoOverN = 2 / n;
			float cosT = Mathf.Cos(t);
			float sinT = Mathf.Sin(t);
			
			float x = a * Mathf.Pow(Mathf.Abs(cosT), twoOverN) * Mathf.Sign(cosT);
			float y = b * Mathf.Pow(Mathf.Abs(sinT), twoOverN) * Mathf.Sign(sinT);
			
			return new Vector3(x, y, 0);
		}
		
		public static Matrix4x4 GetLocalTransformationMatrix(Transform transform)
		{
			Matrix4x4 localTransformationMatrix = new Matrix4x4();
			localTransformationMatrix.SetTRS(transform.localPosition, transform.localRotation, transform.localScale);
			
			return localTransformationMatrix;
		}
		
		public static Matrix4x4 GetBindPoseMatrix(Transform boneTransform, Transform meshTransform)
		{
			return boneTransform.worldToLocalMatrix * meshTransform.localToWorldMatrix;
		}
		
		// Calculates angle A in a triangle with lengths a, b, and c (lengths are opposite of their respective angles).
		public static float GetAngleALawOfCosines(float a, float b, float c)
		{
			float a2 = a * a;
			float b2 = b * b;
			float c2 = c * c;
			
			return Mathf.Acos((-a2 + b2 + c2) / (2 * b * c));
		}
		
		public static bool Solve2BoneIK(float parentBoneLength, float childBoneLength, float distanceToTarget, out float parentAngle, out float relativeChildAngle)
		{
			Debug.Assert(parentBoneLength > 0);
			Debug.Assert(childBoneLength > 0);
			Debug.Assert(distanceToTarget >= 0);
			
			float minArmReach = Mathf.Abs(parentBoneLength - childBoneLength);
			float maxArmReach = parentBoneLength + childBoneLength;
			
			if(Mathf.Approximately(distanceToTarget, 0) || distanceToTarget < minArmReach)
			{
				parentAngle = 0;
				relativeChildAngle = pi;
				
				return false;
			}
			else if(distanceToTarget > maxArmReach)
			{
				parentAngle = 0;
				relativeChildAngle = 0;
				
				return false;
			}
			else
			{
				float PL2 = parentBoneLength * parentBoneLength;
				float CL2 = childBoneLength * childBoneLength;
				float D2 = distanceToTarget * distanceToTarget;
				
				parentAngle = GetAngleALawOfCosines(childBoneLength, parentBoneLength, distanceToTarget);
				relativeChildAngle = GetAngleALawOfCosines(distanceToTarget, childBoneLength, parentBoneLength) - pi;
				
				return true;
			}
		}
		public static bool Solve2BoneIK(float parentBoneLength, float childBoneLength, Vector3 targetDisplacement, out Quaternion parentOrientation, out Quaternion relativeChildOrientation, float rollAngle = 0)
		{
			Vector3 targetDisplacementXZ = Vector3.ProjectOnPlane(targetDisplacement, Vector3.up);
			Vector2 targetDisplacement2D = new Vector2(targetDisplacementXZ.magnitude, targetDisplacement.y);
			float baseAngle = Mathf.Atan2(targetDisplacement2D.y, targetDisplacement2D.x);
			float yAngle = Mathf.Atan2(targetDisplacementXZ.z, targetDisplacementXZ.x);
			
			float angle0, angle1;
			bool reachedTarget = Math.Solve2BoneIK(parentBoneLength, childBoneLength, targetDisplacement.magnitude, out angle0, out angle1);
			
			angle0 += baseAngle;
			
			parentOrientation = Quaternion.AngleAxis(rollAngle * Mathf.Rad2Deg, targetDisplacement) * Quaternion.AngleAxis(-yAngle * Mathf.Rad2Deg, Vector3.up) * Quaternion.AngleAxis(angle0 * Mathf.Rad2Deg, Vector3.forward);
			relativeChildOrientation = Quaternion.AngleAxis(angle1 * Mathf.Rad2Deg, Vector3.forward);
			
			return reachedTarget;
		}
	}
}