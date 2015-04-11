using UnityEngine;
using CUF;
using Debug = CUF.Debug;

namespace Wraithguard
{
	public class TestState : GameState
	{
		private const float armLength0 = Measures.averageMaleHumanHeight * Measures.averageHumanUpperArmLengthPercentageOfHeight;
		private const float armLength1 = Measures.averageMaleHumanHeight * Measures.averageHumanForearmLengthPercentageOfHeight;
		private GameObject[] armSegments;
		private GameObject bow;
		private GameObject bowStringBone;
		
		public override void OnStart()
		{
			Framework.CreateCamera().transform.position = new Vector3(0, 0, -3);
			
			armSegments = CreateArm(0.1f, armLength0, armLength1);
			armSegments[0].name = "arm";
			armSegments[0].transform.position = new Vector3(-(armLength0 + armLength1), 0, 0);
			
			bow = new GameObject("bow");
			bow.AddComponent<BowComponent>();
		}
		public override void OnUpdate()
		{
			if(Input.GetKeyDown(KeyCode.Escape))
			{
				Framework.ClearSceneAndChangeGameState(new MainMenuState());
			}
			
			Transform bowStringBoneTransform = bow.transform.Find("skeletonRoot/string");
			
			if(bowStringBoneTransform != null)
			{
				bowStringBone = bowStringBoneTransform.gameObject;
				
				Vector3 targetDisplacement = bowStringBone.transform.position - armSegments[0].transform.position;
				
				Quaternion armBone0Orientation, armBone1Orientation;
				Math.Solve2BoneIK(armLength0, armLength1, targetDisplacement, out armBone0Orientation, out armBone1Orientation, -(Math.pi / 2));
				
				armSegments[0].transform.localRotation = armBone0Orientation;
				armSegments[1].transform.localRotation = armBone1Orientation;
			}
		}
		
		private GameObject[] CreateArm(float armWidth, params float[] armLengths)
		{
			Debug.Assert(armWidth > 0);
			Debug.Assert(armLengths.Length > 0);
			
			GameObject[] armSegments = new GameObject[armLengths.Length];
			Transform parent = null;
			
			for(int armIndex = 0; armIndex < armLengths.Length; armIndex++)
			{
				float armLength = armLengths[armIndex];
				
				EditMesh editMesh = EditMesh.CreateCuboid(new Vector3(armLength, armWidth, armWidth));
				editMesh.Translate(new Vector3(armLength / 2, 0, 0));
				
				GameObject armSegment = new GameObject();
				armSegment.AddComponent<MeshFilter>().mesh = editMesh.ToMesh();
				armSegment.AddComponent<MeshRenderer>();
				
				if(armIndex > 0)
				{
					armSegment.transform.parent = parent;
					armSegment.transform.localPosition = new Vector3(armLengths[armIndex - 1], 0, 0);
				}
				
				armSegments[armIndex] = armSegment;
				
				parent = armSegment.transform;
			}
			
			return armSegments;
		}
	}
}