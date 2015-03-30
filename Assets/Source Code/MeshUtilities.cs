using UnityEngine;

namespace Wraithguard
{
	public static class MeshUtilities
	{
		public static Mesh CreateRectangleMesh(Vector2 size)
		{
			Debug.Assert(size.x > 0 && size.y > 0);
			
			const uint vertexCount = 4;
			const uint triangleCount = 2;
			const uint indexCount = triangleCount * 3;
			
			Vector2 halfSize = size / 2;
			
			Vector3[] vertices = new Vector3[vertexCount];
			vertices[0] = new Vector3(-halfSize.x, halfSize.y, 0);
			vertices[1] = new Vector3(halfSize.x, halfSize.y, 0);
			vertices[2] = new Vector3(-halfSize.x, -halfSize.y, 0);
			vertices[3] = new Vector3(halfSize.x, -halfSize.y, 0);
			
			Vector3 normal = -Vector3.forward;
			
			Vector3[] normals = new Vector3[vertexCount];
			normals[0] = normal;
			normals[1] = normal;
			normals[2] = normal;
			normals[3] = normal;
			
			Vector2[] uvs = new Vector2[vertexCount];
			uvs[0] = new Vector2(0, 1);
			uvs[1] = new Vector2(1, 1);
			uvs[2] = new Vector2(0, 0);
			uvs[3] = new Vector2(1, 0);
			
			Vector4 tangent = new Vector4(1, 0, 0, -1);
			
			Vector4[] tangents = new Vector4[vertexCount];
			tangents[0] = tangent;
			tangents[1] = tangent;
			tangents[2] = tangent;
			tangents[3] = tangent;
			
			int[] indices = new int[indexCount];
			indices[0] = 0;
			indices[1] = 1;
			indices[2] = 2;
			
			indices[3] = 1;
			indices[4] = 3;
			indices[5] = 2;
			
			Mesh mesh = new Mesh();
			mesh.vertices = vertices;
			mesh.normals = normals;
			mesh.uv = uvs;
			mesh.tangents = tangents;
			mesh.triangles = indices;
			
			return mesh;
		}
		public static Mesh CreateCircleMesh(float radius, uint vertexCount)
		{
			Debug.Assert(radius > 0);
			Debug.Assert(vertexCount >= 3);
			
			Vector3[] vertices = new Vector3[vertexCount];
			Vector3[] normals = new Vector3[vertexCount];
			Vector2[] uvs = new Vector2[vertexCount];
			Vector4[] tangents = new Vector4[vertexCount];
			
			float angleIncrement = (360 / vertexCount) * Mathf.Deg2Rad;
			
			for(int vertexIndex = 0; vertexIndex < vertexCount; vertexIndex++)
			{
				float angle = angleIncrement * vertexIndex;
				
				Vector2 uv = Math.AngleToUnitVector(angle);
				
				vertices[vertexIndex] = uv * radius;
				normals[vertexIndex] = -Vector3.forward;
				uvs[vertexIndex] = uv;
				tangents[vertexIndex] = new Vector4(1, 0, 0, -1);
			}
			
			int triangleCount = (int)vertexCount - 2;
			int[] indices = new int[triangleCount * 3];
			
			for(int vertexIndex = 0; vertexIndex < triangleCount; vertexIndex++)
			{
				int baseIndexIndex = vertexIndex * 3;
				
				indices[baseIndexIndex] = 0;
				indices[baseIndexIndex + 1] = vertexIndex + 2;
				indices[baseIndexIndex + 2] = vertexIndex + 1;
			}
			
			Mesh mesh = new Mesh();
			mesh.vertices = vertices;
			mesh.normals = normals;
			mesh.uv = uvs;
			mesh.tangents = tangents;
			mesh.triangles = indices;
			
			return mesh;
		}
		public static Mesh CreateCuboidMesh(Vector3 size)
		{
			Debug.Assert(size.x > 0 && size.y > 0 && size.z > 0);
			
			const int vertexCount = 24;
			const int triangleCount = 12;
			const int indexCount = triangleCount * 3;
			
			Vector3 halfSize = size / 2;
			
			// common vertex attributes
			Vector3 LTFVP = new Vector3(-halfSize.x, halfSize.y, -halfSize.z);
			Vector3 RTFVP = new Vector3(halfSize.x, halfSize.y, -halfSize.z);
			Vector3 LBFVP = new Vector3(-halfSize.x, -halfSize.y, -halfSize.z);
			Vector3 RBFVP = new Vector3(halfSize.x, -halfSize.y, -halfSize.z);
			Vector3 LTBVP = new Vector3(-halfSize.x, halfSize.y, halfSize.z);
			Vector3 RTBVP = new Vector3(halfSize.x, halfSize.y, halfSize.z);
			Vector3 LBBVP = new Vector3(-halfSize.x, -halfSize.y, halfSize.z);
			Vector3 RBBVP = new Vector3(halfSize.x, -halfSize.y, halfSize.z);
			
			Vector3 leftFN = -Vector3.right;
			Vector3 rightFN = Vector3.right;
			Vector3 bottomFN = -Vector3.up;
			Vector3 topFN = Vector3.up;
			Vector3 frontFN = -Vector3.forward;
			Vector3 backFN = Vector3.forward;
			
			Vector2 TLUV = new Vector2(0, 1);
			Vector2 TRUV = new Vector2(1, 1);
			Vector2 BLUV = new Vector2(0, 0);
			Vector2 BRUV = new Vector2(1, 0);
			
			Vector4 leftFT = new Vector4(0, 0, -1, -1);
			Vector4 rightFT = new Vector4(0, 0, 1, -1);
			Vector4 bottomFT = new Vector4(1, 0, 0, -1);
			Vector4 topFT = new Vector4(1, 0, 0, -1);
			Vector4 frontFT = new Vector4(1, 0, 0, -1);
			Vector4 backFT = new Vector4(-1, 0, 0, -1);
			
			// vertex attribute arrays
			Vector3[] vertices = new Vector3[vertexCount]
			{
				// left
				LTBVP, LTFVP, LBBVP, LBFVP,
				
				// right
				RTFVP, RTBVP, RBFVP, RBBVP,
				
				// bottom
				LBFVP, RBFVP, LBBVP, RBBVP,
				
				// top
				LTBVP, RTBVP, LTFVP, RTFVP,
				
				// front
				LTFVP, RTFVP, LBFVP, RBFVP,
				
				// back
				RTBVP, LTBVP, RBBVP, LBBVP
			};
			
			Vector3[] normals = new Vector3[vertexCount]
			{
				// left
				leftFN, leftFN, leftFN, leftFN,
				
				// right
				rightFN, rightFN, rightFN, rightFN,
				
				// bottom
				bottomFN, bottomFN, bottomFN, bottomFN,
				
				// top
				topFN, topFN, topFN, topFN,
				
				// front
				frontFN, frontFN, frontFN, frontFN,
				
				// back
				backFN, backFN, backFN, backFN
			};
			
			Vector2[] uvs = new Vector2[vertexCount]
			{
				// left
				TLUV, TRUV, BLUV, BRUV,
				
				// right
				TLUV, TRUV, BLUV, BRUV,
				
				// bottom
				TLUV, TRUV, BLUV, BRUV,
				
				// top
				TLUV, TRUV, BLUV, BRUV,
				
				// front
				TLUV, TRUV, BLUV, BRUV,
				
				// back
				TLUV, TRUV, BLUV, BRUV
			};
			
			Vector4[] tangents = new Vector4[vertexCount]
			{
				// left
				leftFT, leftFT, leftFT, leftFT,
				
				// right
				rightFT, rightFT, rightFT, rightFT,
				
				// bottom
				bottomFT, bottomFT, bottomFT, bottomFT,
				
				// top
				topFT, topFT, topFT, topFT,
				
				// front
				frontFT, frontFT, frontFT, frontFT,
				
				// back
				backFT, backFT, backFT, backFT
			};
			
			int[] indices = new int[indexCount]
			{
				// left
				0, 1, 2, 1, 3, 2,
				
				// right
				4, 5, 6, 5, 7, 6,
				
				// bottom
				8, 9, 10, 9, 11, 10,
				
				// top
				12, 13, 14, 13, 15, 14,
				
				// front
				16, 17, 18, 17, 19, 18,
				
				// back
				20, 21, 22, 21, 23, 22,
			};
			
			Mesh mesh = new Mesh();
			mesh.vertices = vertices;
			mesh.normals = normals;
			mesh.uv = uvs;
			mesh.tangents = tangents;
			mesh.triangles = indices;
			
			return mesh;
		}
		public static Mesh CreateUVSphereMesh(float radius, int latitudeRingCount, int longitudeRingCount)
		{
			Debug.Assert(radius > 0);
			Debug.Assert(latitudeRingCount >= 1);
			Debug.Assert(longitudeRingCount >= 2);
			
			// Vertices
			int vertexCountPerLatitudeRing = 2 * longitudeRingCount;
			int latitudeRingCountIncludingPoles = latitudeRingCount + 2;
			
			int vertexCount = latitudeRingCountIncludingPoles * vertexCountPerLatitudeRing;
			
			Vector3[] vertices = new Vector3[vertexCount];
			Vector3[] normals = new Vector3[vertexCount];
			//Vector2[] uvs = new Vector2[vertexCount];
			//Vector4[] tangents = new Vector4[vertexCount];
			
			float latitudeIncrement = Mathf.PI / (latitudeRingCountIncludingPoles - 1);
			float longitudeIncrement = (2 * Mathf.PI) / vertexCountPerLatitudeRing;
			
			for(int latitudeRingIndex = 0; latitudeRingIndex < latitudeRingCountIncludingPoles; latitudeRingIndex++)
			{
				int baseVertexIndex = latitudeRingIndex * vertexCountPerLatitudeRing;
				float latitude = (latitudeRingIndex * latitudeIncrement) - (Mathf.PI / 2);
				
				for(int ringVertexIndex = 0; ringVertexIndex < vertexCountPerLatitudeRing; ringVertexIndex++)
				{
					int vertexIndex = baseVertexIndex + ringVertexIndex;
					float longitude = (ringVertexIndex * longitudeIncrement);
					
					Vector3 normal = Math.LatitudeLongitudeToUnitVector(latitude, longitude);
					Vector3 vertexPosition = normal * radius;
					
					vertices[vertexIndex] = vertexPosition;
					normals[vertexIndex] = normal;
				}
			}
			
			// Indices
			int quadRingCount = latitudeRingCount - 1;
			int quadCountPerQuadRing = vertexCountPerLatitudeRing;
			int triangleCountPerQuadRing = quadCountPerQuadRing * 2;
			
			int triangleCountPerCap = vertexCountPerLatitudeRing;
			
			int triangleCount = (triangleCountPerQuadRing * quadRingCount) + (triangleCountPerCap * 2);
			int indexCount = triangleCount * 3;
			
			int[] indices = new int[indexCount];
			int indexIndex = 0;
			
			for(int latitudeRingIndex = 0; latitudeRingIndex < (latitudeRingCountIncludingPoles - 1); latitudeRingIndex++)
			{
				int ringBaseVertexIndex = latitudeRingIndex * vertexCountPerLatitudeRing;
				
				for(int ringVertexIndex = 0; ringVertexIndex < vertexCountPerLatitudeRing; ringVertexIndex++)
				{
					int nextRingVertexIndex = (ringVertexIndex + 1) % vertexCountPerLatitudeRing;
					
					int BLVI = ringBaseVertexIndex + ringVertexIndex;
					int BRVI = ringBaseVertexIndex + nextRingVertexIndex;
					int TLVI = BLVI + vertexCountPerLatitudeRing;
					int TRVI = BRVI + vertexCountPerLatitudeRing;
					
					if(latitudeRingIndex == 0) // bottom cap
					{
						indices[indexIndex] = BLVI;
						indices[indexIndex + 1] = TLVI;
						indices[indexIndex + 2] = TRVI;
						
						indexIndex += 3;
					}
					else if(latitudeRingIndex == latitudeRingCountIncludingPoles - 2) // top cap
					{
						indices[indexIndex] = BLVI;
						indices[indexIndex + 1] = TLVI;
						indices[indexIndex + 2] = BRVI;
						
						indexIndex += 3;
					}
					else // quad ring
					{
						indices[indexIndex] = BLVI;
						indices[indexIndex + 1] = TLVI;
						indices[indexIndex + 2] = TRVI;
						
						indices[indexIndex + 3] = TRVI;
						indices[indexIndex + 4] = BRVI;
						indices[indexIndex + 5] = BLVI;
						
						indexIndex += 6;
					}
				}
			}
			
			Mesh mesh = new Mesh();
			mesh.vertices = vertices;
			mesh.normals = normals;
			//mesh.uv = uvs;
			//mesh.tangents = tangents;
			mesh.triangles = indices;
			
			return mesh;
		}
		public static Mesh CreateTorusMesh(float majorRadius, float minorRadius, int majorRingCount, int minorRingCount)
		{
			Debug.Assert(majorRadius > 0);
			Debug.Assert(minorRadius > 0);
			Debug.Assert(majorRingCount >= 3);
			Debug.Assert(minorRingCount >= 3);
			
			int vertexCount = majorRingCount * minorRingCount;
			
			Vector3[] vertices = new Vector3[vertexCount];
			//Vector3[] normals = new Vector3[vertexCount];
			//Vector2[] uvs = new Vector2[vertexCount];
			//Vector4[] tangents = new Vector4[vertexCount];
			
			float majorAngleIncrement = (2 * Mathf.PI) / majorRingCount;
			float minorAngleIncrement = (2 * Mathf.PI) / minorRingCount;
			
			for(int majorRingIndex = 0; majorRingIndex < majorRingCount; majorRingIndex++)
			{
				int baseVertexIndex = majorRingIndex * minorRingCount;
				float majorAngle = majorRingIndex * majorAngleIncrement;
				
				for(int minorRingIndex = 0; minorRingIndex < minorRingCount; minorRingIndex++)
				{
					int vertexIndex = baseVertexIndex + minorRingIndex;
					float minorAngle = minorRingIndex * minorAngleIncrement;
					
					Vector3 vertexPosition = Math.GetPointOnTorus(majorRadius, minorRadius, majorAngle, minorAngle);
					
					vertices[vertexIndex] = vertexPosition;
				}
			}
			
			int quadCount = majorRingCount * minorRingCount;
			int triangleCount = quadCount * 2;
			int indexCount = triangleCount * 3;
			
			int[] indices = new int[indexCount];
			int indexIndex = 0;
			
			for(int majorRingIndex = 0; majorRingIndex < majorRingCount; majorRingIndex++)
			{
				int baseVertexIndex = majorRingIndex * minorRingCount;
				
				for(int minorRingIndex = 0; minorRingIndex < minorRingCount; minorRingIndex++)
				{
					int nextMinorRingIndex = (minorRingIndex + 1) % minorRingCount;
					
					int BLVI = baseVertexIndex + nextMinorRingIndex;
					int BRVI = baseVertexIndex + minorRingIndex;
					int TLVI = (BLVI + minorRingCount) % vertexCount;
					int TRVI = (BRVI + minorRingCount) % vertexCount;
					
					indices[indexIndex] = BLVI;
					indices[indexIndex + 1] = TLVI;
					indices[indexIndex + 2] = TRVI;
					
					indices[indexIndex + 3] = TRVI;
					indices[indexIndex + 4] = BRVI;
					indices[indexIndex + 5] = BLVI;
					
					indexIndex += 6;
				}
			}
			
			Mesh mesh = new Mesh();
			mesh.vertices = vertices;
			//mesh.normals = normals;
			//mesh.uv = uvs;
			//mesh.tangents = tangents;
			mesh.triangles = indices;
			
			return mesh;
		}
	}
}