using UnityEngine;

namespace Wraithguard
{
	public class BowComponent : MonoBehaviour
	{
		private GameObject bowStringBone;
		private GameObject loadedArrow;
		
		private void Start()
		{
			// Create unstringed bow.
			EditMesh editMesh = EditMesh.CreateCylinderAlongCurve(PositionFunction, 0.02f, 8, 4, -1, 1);

			// Create bow string.
			Bounds meshBounds = editMesh.ComputeBounds();
			
			Vector3[] bowStringPolyline = new Vector3[]
			{
				new Vector3(meshBounds.min.x, -meshBounds.extents.y, 0),
				new Vector3(meshBounds.min.x, 0, 0),
				new Vector3(meshBounds.min.x, meshBounds.extents.y, 0)
			};
			
			EditMesh bowString = EditMesh.CreateCylinderAlongPolyline(bowStringPolyline, 0.01f, 4);
			editMesh.Append(bowString);

			// Center the mesh.
			editMesh.Translate(-meshBounds.center);

			// Add bone weights.
			editMesh.vertexBoneWeights = new BoneWeight[editMesh.vertexCount];
			
			for(int vertexIndex = 0; vertexIndex < editMesh.vertexCount; vertexIndex++)
			{
				BoneWeight boneWeight = new BoneWeight();

				boneWeight.boneIndex0 = 0;
				boneWeight.weight0 = 1;

				boneWeight.boneIndex1 = 0;
				boneWeight.weight1 = 0;

				boneWeight.boneIndex2 = 0;
				boneWeight.weight2 = 0;

				boneWeight.boneIndex3 = 0;
				boneWeight.weight3 = 0;

				editMesh.vertexBoneWeights[vertexIndex] = boneWeight;
			}
			
			foreach(int vertexIndex in editMesh.GetVerticesInSphere(Vector3.zero, 0.05f))
			{
				editMesh.vertexBoneWeights[vertexIndex].boneIndex0 = 1;
				editMesh.vertexBoneWeights[vertexIndex].weight0 = 1;
			}
			
			Mesh mesh = editMesh.ToMesh();
			
			Transform[] bones = new Transform[2];
			Matrix4x4[] bindPoses = new Matrix4x4[2];
			
			bones[0] = new GameObject("skeletonRoot").transform;
			bones[0].parent = transform;
			bones[0].localRotation = Quaternion.identity;
			bones[0].localPosition = Vector3.zero;
			
			bindPoses[0] = Math.GetBindPoseMatrix(bones[0], transform);
			
			bowStringBone = new GameObject("string");
			
			bones[1] = bowStringBone.transform;
			bones[1].parent = bones[0];
			bones[1].localRotation = Quaternion.identity;
			bones[1].localPosition = Vector3.zero;
			
			bindPoses[1] = Math.GetBindPoseMatrix(bones[1], transform);
			
			mesh.bindposes = bindPoses;
			
			SkinnedMeshRenderer renderer = gameObject.AddComponent<SkinnedMeshRenderer>();
			renderer.sharedMesh = mesh;
			renderer.bones = bones;
			
			Animation animation = gameObject.AddComponent<Animation>();
			
			AnimationCurve curve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, -0.5f), new Keyframe(1.05f, 0), new Keyframe(2, 0));
			
			AnimationClip clip = new AnimationClip();
			clip.SetCurve("skeletonRoot/string", typeof(Transform), "m_LocalPosition.x", curve);
			clip.legacy = true;
			
			AnimationEvent load = new AnimationEvent();
			load.functionName = "LoadArrow";
			load.time = 0;
			
			AnimationEvent fire = new AnimationEvent();
			fire.functionName = "FireLoadedArrow";
			fire.time = 1.05f;
			
			clip.AddEvent(load);
			clip.AddEvent(fire);
			
			animation.wrapMode = WrapMode.Loop;
			animation.AddClip(clip, "test");
			animation.Play("test");
		}
		private Vector3 PositionFunction(float t)
		{
			return Math.Superellipse(2.5f, 1, 1, t);
		}
		private void LoadArrow()
		{
			Vector3 arrowSize = new Vector3(1, 0.02f, 0.02f);
			
			EditMesh arrowEditMesh = EditMesh.CreateCuboid(arrowSize);
			arrowEditMesh.Translate(new Vector3(arrowSize.x / 2, 0, 0));
			
			Mesh mesh = arrowEditMesh.ToMesh();
			
			loadedArrow = new GameObject("arrow");
			loadedArrow.AddComponent<MeshFilter>().mesh = mesh;
			loadedArrow.AddComponent<MeshRenderer>();
			
			loadedArrow.transform.parent = bowStringBone.transform;
			loadedArrow.transform.localPosition = Vector3.zero;
		}
		private void FireLoadedArrow()
		{
			loadedArrow.transform.parent = null;
			loadedArrow.AddComponent<Rigidbody>().velocity = Vector3.right * 20;
		}
	}
}