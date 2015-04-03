using UnityEngine;

namespace Wraithguard
{
	public static class MeshUtilities
	{
		public static EditMesh CreateRectangleMesh(Vector2 size)
		{
			Debug.Assert(size.x > 0 && size.y > 0);
			
			const uint vertexCount = 4;
			Vector2 halfSize = size / 2;
			
			Vector3[] vertexPositions = new Vector3[vertexCount];
			vertexPositions[0] = new Vector3(-halfSize.x, halfSize.y, 0);
			vertexPositions[1] = new Vector3(halfSize.x, halfSize.y, 0);
			vertexPositions[2] = new Vector3(-halfSize.x, -halfSize.y, 0);
			vertexPositions[3] = new Vector3(halfSize.x, -halfSize.y, 0);
			
			Vector3 normal = -Vector3.forward;
			
			Vector3[] vertexNormals = new Vector3[vertexCount];
			vertexNormals[0] = normal;
			vertexNormals[1] = normal;
			vertexNormals[2] = normal;
			vertexNormals[3] = normal;
			
			Vector2[] vertexUVs = new Vector2[vertexCount];
			vertexUVs[0] = new Vector2(0, 1);
			vertexUVs[1] = new Vector2(1, 1);
			vertexUVs[2] = new Vector2(0, 0);
			vertexUVs[3] = new Vector2(1, 0);
			
			Vector4 tangent = new Vector4(1, 0, 0, -1);
			
			Vector4[] vertexTangents = new Vector4[vertexCount];
			vertexTangents[0] = tangent;
			vertexTangents[1] = tangent;
			vertexTangents[2] = tangent;
			vertexTangents[3] = tangent;
			
			const uint triangleCount = 2;
			const uint indexCount = triangleCount * 3;
			
			int[] vertexIndices = new int[indexCount];
			vertexIndices[0] = 0;
			vertexIndices[1] = 1;
			vertexIndices[2] = 2;
			
			vertexIndices[3] = 1;
			vertexIndices[4] = 3;
			vertexIndices[5] = 2;
			
			return new EditMesh(vertexPositions, vertexNormals, vertexUVs, vertexTangents, vertexIndices);
		}
		public static EditMesh CreateCircleMesh(float radius, uint vertexCount)
		{
			Debug.Assert(radius > 0);
			Debug.Assert(vertexCount >= 3);
			
			Vector3[] vertexPositions = new Vector3[vertexCount];
			Vector3[] vertexNormals = new Vector3[vertexCount];
			Vector2[] vertexUVs = new Vector2[vertexCount];
			Vector4[] vertexTangents = new Vector4[vertexCount];
			
			float angleIncrement = (360 / vertexCount) * Mathf.Deg2Rad;
			
			for(int vertexIndex = 0; vertexIndex < vertexCount; vertexIndex++)
			{
				float angle = angleIncrement * vertexIndex;
				
				Vector2 uv = Math.AngleToUnitVector(angle);
				
				vertexPositions[vertexIndex] = uv * radius;
				vertexNormals[vertexIndex] = -Vector3.forward;
				vertexUVs[vertexIndex] = uv;
				vertexTangents[vertexIndex] = new Vector4(1, 0, 0, -1);
			}
			
			int triangleCount = (int)vertexCount - 2;
			int[] vertexIndices = new int[triangleCount * 3];
			
			for(int vertexIndex = 0; vertexIndex < triangleCount; vertexIndex++)
			{
				int baseIndexIndex = vertexIndex * 3;
				
				vertexIndices[baseIndexIndex] = 0;
				vertexIndices[baseIndexIndex + 1] = vertexIndex + 2;
				vertexIndices[baseIndexIndex + 2] = vertexIndex + 1;
			}
			
			return new EditMesh(vertexPositions, vertexNormals, vertexUVs, vertexTangents, vertexIndices);
		}
		public static EditMesh CreateCuboidMesh(Vector3 size)
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
			Vector3[] vertexPositions = new Vector3[vertexCount]
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
			
			Vector3[] vertexNormals = new Vector3[vertexCount]
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
			
			Vector2[] vertexUVs = new Vector2[vertexCount]
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
			
			Vector4[] vertexTangents = new Vector4[vertexCount]
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
			
			int[] vertexIndices = new int[indexCount]
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
			
			return new EditMesh(vertexPositions, vertexNormals, vertexUVs, vertexTangents, vertexIndices);
		}
		public static EditMesh CreateUVSphereMesh(float radius, int latitudeRingCount, int longitudeRingCount)
		{
			Debug.Assert(radius > 0);
			Debug.Assert(latitudeRingCount >= 1);
			Debug.Assert(longitudeRingCount >= 2);
			
			// Vertices
			int vertexCountPerLatitudeRing = 2 * longitudeRingCount;
			int latitudeRingCountIncludingPoles = latitudeRingCount + 2;
			
			int vertexCount = latitudeRingCountIncludingPoles * vertexCountPerLatitudeRing;
			
			Vector3[] vertexPositions = new Vector3[vertexCount];
			Vector3[] vertexNormals = new Vector3[vertexCount];
			Vector2[] vertexUVs = null;
			Vector4[] vertexTangents = null;
			
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
					
					Vector3 vertexNormal = Math.LatitudeLongitudeToUnitVector(latitude, longitude);
					Vector3 vertexPosition = vertexNormal * radius;
					
					vertexPositions[vertexIndex] = vertexPosition;
					vertexNormals[vertexIndex] = vertexNormal;
				}
			}
			
			// Indices
			int quadRingCount = latitudeRingCount - 1;
			int quadCountPerQuadRing = vertexCountPerLatitudeRing;
			int triangleCountPerQuadRing = quadCountPerQuadRing * 2;
			
			int triangleCountPerCap = vertexCountPerLatitudeRing;
			
			int triangleCount = (triangleCountPerQuadRing * quadRingCount) + (triangleCountPerCap * 2);
			int indexCount = triangleCount * 3;
			
			int[] vertexIndices = new int[indexCount];
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
						vertexIndices[indexIndex] = BLVI;
						vertexIndices[indexIndex + 1] = TLVI;
						vertexIndices[indexIndex + 2] = TRVI;
						
						indexIndex += 3;
					}
					else if(latitudeRingIndex == latitudeRingCountIncludingPoles - 2) // top cap
					{
						vertexIndices[indexIndex] = BLVI;
						vertexIndices[indexIndex + 1] = TLVI;
						vertexIndices[indexIndex + 2] = BRVI;
						
						indexIndex += 3;
					}
					else // quad ring
					{
						vertexIndices[indexIndex] = BLVI;
						vertexIndices[indexIndex + 1] = TLVI;
						vertexIndices[indexIndex + 2] = TRVI;
						
						vertexIndices[indexIndex + 3] = TRVI;
						vertexIndices[indexIndex + 4] = BRVI;
						vertexIndices[indexIndex + 5] = BLVI;
						
						indexIndex += 6;
					}
				}
			}
			
			return new EditMesh(vertexPositions, vertexNormals, vertexUVs, vertexTangents, vertexIndices);
		}
		public static EditMesh CreateTorusMesh(float majorRadius, float minorRadius, int majorRingCount, int minorRingCount)
		{
			Debug.Assert(majorRadius > 0);
			Debug.Assert(minorRadius > 0);
			Debug.Assert(majorRingCount >= 3);
			Debug.Assert(minorRingCount >= 3);
			
			int vertexCount = majorRingCount * minorRingCount;
			
			Vector3[] vertexPositions = new Vector3[vertexCount];
			Vector3[] vertexNormals = null;
			Vector2[] vertexUVs = null;
			Vector4[] vertexTangents = null;
			
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
					
					vertexPositions[vertexIndex] = vertexPosition;
				}
			}
			
			int quadCount = majorRingCount * minorRingCount;
			int triangleCount = quadCount * 2;
			int indexCount = triangleCount * 3;
			
			int[] vertexIndices = new int[indexCount];
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
					
					vertexIndices[indexIndex] = BLVI;
					vertexIndices[indexIndex + 1] = TLVI;
					vertexIndices[indexIndex + 2] = TRVI;
					
					vertexIndices[indexIndex + 3] = TRVI;
					vertexIndices[indexIndex + 4] = BRVI;
					vertexIndices[indexIndex + 5] = BLVI;
					
					indexIndex += 6;
				}
			}
			
			return new EditMesh(vertexPositions, vertexNormals, vertexUVs, vertexTangents, vertexIndices);
		}
		
		public static Mesh CreateMesh(EditMesh editMesh)
		{
			Mesh mesh = new Mesh();
			mesh.vertices = editMesh.vertexPositions;
			mesh.normals = editMesh.vertexNormals;
			mesh.uv = editMesh.vertexUVs;
			mesh.tangents = editMesh.vertexTangents;
			mesh.triangles = editMesh.vertexIndices;
			
			return mesh;
		}
	}
}