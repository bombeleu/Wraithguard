using UnityEngine;

namespace Wraithguard
{
	public class EditMesh
	{
		public Vector3[] vertexPositions;
		public Vector3[] vertexNormals;
		public Vector2[] vertexUVs;
		public Vector4[] vertexTangents;
		public int[] vertexIndices;
		
		public int vertexCount
		{
			get
			{
				return vertexPositions.Length;
			}
		}
		public int triangleCount
		{
			get
			{
				return vertexIndices.Length / 3;
			}
		}
		
		public EditMesh()
		{
		}
		public EditMesh(Vector3[] vertexPositions, Vector3[] vertexNormals, Vector2[] vertexUVs, Vector4[] vertexTangents, int[] vertexIndices)
		{
			this.vertexPositions = vertexPositions;
			this.vertexNormals = vertexNormals;
			this.vertexUVs = vertexUVs;
			this.vertexTangents = vertexTangents;
			this.vertexIndices = vertexIndices;
		}
	}
}