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
		
		private const float jumpImpulseMagnitude = 350;
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
		
		private uint weaponID = 1;
		
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
			
			uint pressedDigit;
			
			if(GetPressedDigit(out pressedDigit))
			{
				weaponID = pressedDigit;
			}
			
			if(Input.GetMouseButtonDown(0))
			{
				if(weaponID == 1)
				{
					if(sword == null)
					{
						SlashSword();
					}
				}
				else if(weaponID == 2)
				{
					FireArrow();
				}
				else if(weaponID == 3)
				{
					CastRangedExplosion();
				}
			}
			
			if(sword != null)
			{
				UpdateSword();
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
			actualSword.AddComponent<ActiveSwordComponent>().owner = gameObject;
			
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
			arrow.transform.localScale = new Vector3(0.1f, 0.3f, 0.1f);
			
			arrow.AddComponent<ArrowComponent>().owner = gameObject;
			
			Rigidbody arrowRigidbody = arrow.AddComponent<Rigidbody>();
			arrowRigidbody.mass = 1;
			
			arrow.transform.position = cameraRay.GetPoint(1);
			arrow.transform.eulerAngles = new Vector3(cameraXAngle + 90, yAngle, 0);
			
			arrowRigidbody.velocity = rigidbody.velocity + (cameraRay.direction * 40);
		}
		
		private void CastRangedExplosion()
		{
			GameObject spell = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			spell.name = "spell";
			spell.transform.localScale = Vector3.one * 0.25f;
			spell.AddComponent<SpellComponent>().owner = gameObject;
			
			Rigidbody spellRigidbody = spell.AddComponent<Rigidbody>();
			spellRigidbody.useGravity = false;
			
			spell.transform.position = cameraRay.GetPoint(1);
			
			spellRigidbody.velocity = rigidbody.velocity + (cameraRay.direction * 40);
		}
		
		private bool GetPressedDigit(out uint pressedDigit)
		{
			if(Input.GetKeyDown(KeyCode.Alpha0))
			{
				pressedDigit = 0;
				return true;
			}
			else if(Input.GetKeyDown(KeyCode.Alpha1))
			{
				pressedDigit = 1;
				return true;
			}
			else if(Input.GetKeyDown(KeyCode.Alpha2))
			{
				pressedDigit = 2;
				return true;
			}
			else if(Input.GetKeyDown(KeyCode.Alpha3))
			{
				pressedDigit = 3;
				return true;
			}
			else if(Input.GetKeyDown(KeyCode.Alpha4))
			{
				pressedDigit = 4;
				return true;
			}
			else if(Input.GetKeyDown(KeyCode.Alpha5))
			{
				pressedDigit = 5;
				return true;
			}
			else if(Input.GetKeyDown(KeyCode.Alpha6))
			{
				pressedDigit = 6;
				return true;
			}
			else if(Input.GetKeyDown(KeyCode.Alpha7))
			{
				pressedDigit = 7;
				return true;
			}
			else if(Input.GetKeyDown(KeyCode.Alpha8))
			{
				pressedDigit = 8;
				return true;
			}
			else if(Input.GetKeyDown(KeyCode.Alpha9))
			{
				pressedDigit = 9;
				return true;
			}
			else
			{
				pressedDigit = 0;
				return false;
			}
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