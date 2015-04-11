using UnityEngine;
using CUF;

namespace Wraithguard
{
	public class BowComponent : MonoBehaviour
	{
		private const float bowRadius = 0.02f;
		private const float stringRadius = 0.01f;
		private const float arrowThickness = 0.02f;
		
		private Vector3 bowSize;
		
		private float bowAngle;
		private float stringXOffset;
		
		private Transform topHalfRootBone;
		private Transform bottomHalfRootBone;
		private Transform bowStringBone;
		
		private GameObject loadedArrow;
		
		private void Start()
		{
			CreateStaticBow();
			AddBowAnimations();
		}
		private void Update()
		{
			BendBow();
		}
		
		private void CreateStaticBow()
		{
			// Create unstringed bow.
			float minT = -1;
			float maxT = 1;
			float meanT = Math.Mean(minT, maxT);
			
			EditMesh editMesh = EditMesh.CreateCylinderAlongCurve(PositionFunction, bowRadius, 8, 4, minT, maxT);
			
			// Center the mesh.
			Bounds meshBounds = editMesh.ComputeBounds();
			Vector3 bowMeshTranslation = new Vector3(meshBounds.extents.x, 0, 0) - meshBounds.center;
			editMesh.Translate(bowMeshTranslation);
			
			// Create bow string.
			bowSize = meshBounds.size;
			
			Vector3[] bowStringPolyline = new Vector3[]
			{
				new Vector3(0, -(bowSize.y / 2), 0),
				new Vector3(0, 0, 0),
				new Vector3(0, bowSize.y / 2, 0)
			};
			
			EditMesh bowString = EditMesh.CreateCylinderAlongPolyline(bowStringPolyline, stringRadius, 4);
			editMesh.Append(bowString);

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
			
			foreach(int vertexIndex in editMesh.GetVerticesInSphere(Vector3.zero, stringRadius * 2))
			{
				editMesh.vertexBoneWeights[vertexIndex].boneIndex0 = 1;
				editMesh.vertexBoneWeights[vertexIndex].weight0 = 1;
			}
			
			Mesh mesh = editMesh.ToMesh();
			
			// Create the bones.
			int halfBowDeformBoneCount = 4;
			
			Transform[] bones = new Transform[2 + (halfBowDeformBoneCount * 2)];
			
			bones[0] = new GameObject("skeletonRoot").transform;
			bones[0].parent = transform;
			bones[0].localRotation = Quaternion.identity;
			bones[0].localPosition = new Vector3(bowSize.x / 2, 0, 0);
			
			bowStringBone = new GameObject("string").transform;
			
			bones[1] = bowStringBone;
			bones[1].parent = bones[0];
			bones[1].localRotation = Quaternion.identity;
			bones[1].localPosition = new Vector3(-(bowSize.x / 2), 0, 0);
			
			// Generate top half of bow deform bones.
			Transform deformBoneParent = bones[0];
			int baseBowDeformBoneIndex = 2;
			int lastBowDeformBoneIndex = halfBowDeformBoneCount - 1;
			
			for(int bowDeformBoneIndex = 0; bowDeformBoneIndex <= lastBowDeformBoneIndex; bowDeformBoneIndex++)
			{
				Transform boneTransform = (new GameObject()).transform;
				
				float t = (float)bowDeformBoneIndex / halfBowDeformBoneCount;
				boneTransform.position = PositionFunction(Math.LinearInterpolate(meanT, maxT, t)) + bowMeshTranslation;
				
				boneTransform.parent = deformBoneParent;
				boneTransform.localRotation = Quaternion.identity;
				
				bones[baseBowDeformBoneIndex + bowDeformBoneIndex] = boneTransform;
				
				deformBoneParent = boneTransform;
			}
			
			// Mirror top half of bow deform bones.
			deformBoneParent = bones[0];
			baseBowDeformBoneIndex += halfBowDeformBoneCount;
			
			for(int bowDeformBoneIndex = 0; bowDeformBoneIndex <= lastBowDeformBoneIndex; bowDeformBoneIndex++)
			{
				Transform boneTransform = (new GameObject()).transform;
				
				int mirroredBoneIndex = (baseBowDeformBoneIndex - halfBowDeformBoneCount) + bowDeformBoneIndex;
				Transform mirroredBone = bones[mirroredBoneIndex];
				Vector3 mirroredBonePosition = mirroredBone.position;
				
				boneTransform.position = new Vector3(mirroredBonePosition.x, -mirroredBonePosition.y, mirroredBonePosition.z);
				boneTransform.localRotation = Quaternion.Inverse(mirroredBone.rotation);
				
				boneTransform.parent = deformBoneParent;
				
				bones[baseBowDeformBoneIndex + bowDeformBoneIndex] = boneTransform;
				
				deformBoneParent = boneTransform;
			}
			
			topHalfRootBone = bones[2];
			bottomHalfRootBone = bones[2 + halfBowDeformBoneCount];
			
			// Compute the bind poses.
			Matrix4x4[] bindPoses = new Matrix4x4[bones.Length];
			
			for(int bindPoseIndex = 0; bindPoseIndex < bindPoses.Length; bindPoseIndex++)
			{
				bindPoses[bindPoseIndex] = Math.GetBindPoseMatrix(bones[bindPoseIndex], transform);
			}
			
			// Add everything to the GameObject.
			mesh.bindposes = bindPoses;
			
			SkinnedMeshRenderer renderer = gameObject.AddComponent<SkinnedMeshRenderer>();
			renderer.sharedMesh = mesh;
			renderer.bones = bones;
			
			// Misc
			Vector3 topVector = new Vector3(0, (bowSize.y / 2), 0) - topHalfRootBone.position;
			float topAngle = Mathf.Atan2(topVector.y, topVector.x);
			bowAngle = Math.twoPi - (2 * topAngle);
		}
		private void AddBowAnimations()
		{
			Animation animation = gameObject.AddComponent<Animation>();
			
			stringXOffset = bowStringBone.localPosition.x;
			AnimationCurve curve = new AnimationCurve(new Keyframe(0, stringXOffset), new Keyframe(1, stringXOffset - 0.5f), new Keyframe(1.05f, stringXOffset), new Keyframe(2, stringXOffset));
			
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
			float scale = 0.5f;
			
			return Math.Superellipse(2.5f, scale, scale, t);
		}
		
		private void BendBow()
		{
			float halfStringLength = bowSize.y / 2;
			
			float stringPullDistance = stringXOffset - bowStringBone.transform.localPosition.x;
			float halfPullDistance = stringPullDistance / 2;
			
			float topHalfTipTargetY = halfStringLength * Mathf.Sin(Mathf.Acos(halfPullDistance / halfStringLength));
			float topHalfTipTargetX = -halfPullDistance;
			Vector3 topHalfTipTarget = new Vector3(topHalfTipTargetX, topHalfTipTargetY, 0);
			
			float angleToTopHalfTip = (Math.twoPi - bowAngle) / 2;
			
			Vector3 desiredTopVector = topHalfTipTarget - topHalfRootBone.position;
			float desiredAngleToTopHalfTip = Mathf.Atan2(desiredTopVector.y, desiredTopVector.x);
			
			float deformAngle = desiredAngleToTopHalfTip - angleToTopHalfTip;
			
			topHalfRootBone.localRotation = Quaternion.AngleAxis(deformAngle * Mathf.Rad2Deg, Vector3.forward);
			bottomHalfRootBone.localRotation = Quaternion.AngleAxis(-deformAngle * Mathf.Rad2Deg, Vector3.forward);
		}
		
		private void LoadArrow()
		{
			Vector3 arrowSize = new Vector3(bowSize.x * 3.5f, arrowThickness, arrowThickness);
			
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