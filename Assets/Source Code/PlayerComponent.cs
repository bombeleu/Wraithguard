using UnityEngine;

namespace Wraithguard
{
	public class PlayerComponent : MonoBehaviour
	{
		GameObject sword;
		
		#region Camera Stuff
		public GameObject camera
		{
			get
			{
				return _camera;
			}
			set
			{
				_camera = value;
				cameraXAngle = 0;
				
				_camera.transform.parent = transform;
				
				float cameraY = height * (Measures.averageHumanEyeHeightPercentage - 0.5f);
				_camera.transform.localPosition = new Vector3(0, cameraY, 0);
				
				_camera.transform.localRotation = Quaternion.identity;
			}
		}
		private GameObject _camera;
		
		public float maxVerticalAngle = 90;
		public float maxActivationDistance = 2;
		
		private float cameraXAngle;
		private Ray cameraRay
		{
			get
			{
				return new Ray(camera.transform.position, camera.transform.forward);
			}
		}
		#endregion
		
		#region Jumping Stuff
		private const float groundedMargin = 0.1f;
		private bool isGrounded
		{
			get
			{
				return Physics.Raycast(groundedTestRay, groundedTestRayLength);
			}
		}
		private Ray groundedTestRay
		{
			get
			{
				return new Ray(transform.position, Vector3.down);
			}
		}
		private float groundedTestRayLength
		{
			get
			{
				return (height / 2) + groundedMargin;
			}
		}
		
		private const float jumpImpulseMagnitude = 400;
		#endregion
		
		private Rigidbody rigidbody;
		private float yAngle;
		
		public float height;
		
		private Vector3 xzForward
		{
			get
			{
				return Math.Reject(transform.forward, Vector3.up);
			}
		}
		private Vector3 xzRight
		{
			get
			{
				return Math.Reject(transform.right, Vector3.up);
			}
		}
		
		public Inventory inventory
		{
			get
			{
				return GetComponent<InventoryComponent>().inventory;
			}
		}
		
		private void Start()
		{
			rigidbody = GetComponent<Rigidbody>();
			yAngle = transform.eulerAngles.y;
		}
		private void Update()
		{
			Rotate();
			
			if(Input.GetKeyDown(KeyCode.E))
			{
				RaycastHit raycastHit;
				
				if(PhysicsUtilities.RaycastClosest(cameraRay, out raycastHit, maxActivationDistance))
				{
					Global.ActivateObject(raycastHit.transform.gameObject, gameObject);
				}
			}
			
			if(Input.GetKeyDown(KeyCode.T))
			{
				const uint itemTypeID = 0;
				
				if(inventory.GetItemCount(itemTypeID) > 0)
				{
					inventory.RemoveItem(itemTypeID);
					Object.CreateGameObject(itemTypeID, cameraRay.GetPoint(3));
				}
			}
			
			if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
			{
				rigidbody.AddForce(Vector3.up * jumpImpulseMagnitude, ForceMode.Impulse);
			}
			
			if(Input.GetMouseButtonDown(0))
			{
				if(sword == null)
				{
					SlashSword();
				}
			}
			
			if(sword != null)
			{
				UpdateSword();
			}
			
			if(Input.GetMouseButtonDown(1))
			{
				FireArrow();
			}
		}
		private void FixedUpdate()
		{
			if(isGrounded)
			{
				Vector3 force = GetMovementDirectionFromInput() * 750;
				
				rigidbody.AddForce(force);
			}
		}
		
		private void ApplyLateralMovementForce()
		{
			Vector3 force = GetMovementDirectionFromInput() * 750;
			
			rigidbody.AddForce(force);
		}
		
		private Vector3 GetMovementDirectionFromInput()
		{
			Vector3 movementDirection = Vector3.zero;
			
			if(Input.GetKey(KeyCode.W))
			{
				movementDirection += xzForward;
			}
			
			if(Input.GetKey(KeyCode.A))
			{
				movementDirection -= xzRight;
			}
			
			if(Input.GetKey(KeyCode.S))
			{
				movementDirection -= xzForward;
			}
			
			if(Input.GetKey(KeyCode.D))
			{
				movementDirection += xzRight;
			}
			
			return movementDirection.normalized;
		}
		private void Rotate()
		{
			Vector2 deltaMouse = Mouse.deltaMouse;
			yAngle = Mathf.Repeat(yAngle + Mouse.deltaMouse.x, 360);
			
			transform.eulerAngles = new Vector3(0, yAngle, 0);
			
			cameraXAngle = Mathf.Clamp(cameraXAngle - deltaMouse.y, -maxVerticalAngle, maxVerticalAngle);
			
			camera.transform.localEulerAngles = new Vector3(cameraXAngle, 0, 0);
		}
		
		private void SlashSword()
		{
			Debug.Assert(sword == null);
			
			const float swordLength = 1;
			
			GameObject actualSword = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
			actualSword.AddComponent<ActiveSwordComponent>();
			
			actualSword.transform.localScale = new Vector3(0.1f, swordLength / 2, 0.1f);
			actualSword.GetComponent<CapsuleCollider>().isTrigger = true;
			
			actualSword.transform.position = new Vector3(0, swordLength / 2, 0);
			
			sword = new GameObject();
			
			actualSword.transform.parent = sword.transform;
			
			sword.transform.position = cameraRay.GetPoint(1);
			sword.transform.eulerAngles = new Vector3(cameraXAngle, yAngle, 0);
			sword.transform.parent = camera.transform;
		}
		private void UpdateSword()
		{
			Debug.Assert(sword != null);
			
			sword.transform.Rotate(Vector3.right * Time.deltaTime * 720);
			
			if(sword.transform.localEulerAngles.x >= 180)
			{
				Destroy(sword);
				sword = null;
			}
		}
		
		private void FireArrow()
		{
			GameObject arrow = GameObject.CreatePrimitive(PrimitiveType.Capsule);
			arrow.AddComponent<ArrowComponent>();
			
			Rigidbody rigidbody = arrow.AddComponent<Rigidbody>();
			rigidbody.mass = 1;
			
			arrow.transform.position = cameraRay.GetPoint(1);
			arrow.transform.eulerAngles = new Vector3(cameraXAngle + 90, yAngle, 0);
			arrow.transform.localScale = new Vector3(0.1f, 0.3f, 0.1f);
			
			rigidbody.velocity = cameraRay.direction * 40;
		}
		
		// Debug Stuff
		// ===========
		private void OnDrawGizmos()
		{
			DrawXZAxesGizmo();
			DrawCameraRayGizmo();
			DrawGroundedTestGizmo();
		}
		private void DrawXZAxesGizmo()
		{
			const float axisLength = 2;
			
			Gizmos.color = Color.red;
			Gizmos.DrawRay(transform.position, xzRight * axisLength);
			
			Gizmos.color = Color.blue;
			Gizmos.DrawRay(transform.position, xzForward * axisLength);
		}
		private void DrawCameraRayGizmo()
		{
			Gizmos.color = Color.white;
			Gizmos.DrawRay(cameraRay);
		}
		private void DrawGroundedTestGizmo()
		{
			Ray ray = groundedTestRay;
			ray.direction *= groundedTestRayLength;
			
			Gizmos.color = isGrounded ? Color.white : Color.black;
			Gizmos.DrawRay(ray);
		}
	}
}