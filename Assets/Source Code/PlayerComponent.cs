using UnityEngine;

namespace Wraithguard
{
	public class PlayerComponent : MonoBehaviour
	{
		private Rigidbody rigidbody;
		private float yAngle;
		
		// Camera Stuff
		// ============
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
				_camera.transform.localPosition = Vector3.zero;
				_camera.transform.localRotation = Quaternion.identity;
			}
		}
		public float maxVerticalAngle = 90;
		public float maxActivationDistance = 2;
		
		private GameObject _camera;
		private float cameraXAngle;
		private Ray cameraRay
		{
			get
			{
				return new Ray(camera.transform.position, camera.transform.forward);
			}
		}
		
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
					Global.instance.ActivateObject(raycastHit.transform.gameObject, gameObject);
				}
			}
		}
		private void FixedUpdate()
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
		
		// Debug Stuff
		// ===========
		private void OnDrawGizmos()
		{
			DrawXZAxesGizmo();
			
			Gizmos.color = Color.white;
			Gizmos.DrawRay(transform.position, GetMovementDirectionFromInput() * 2);
		}
		private void DrawXZAxesGizmo()
		{
			const float axisLength = 2;
			
			Gizmos.color = Color.red;
			Gizmos.DrawRay(transform.position, xzRight * axisLength);
			
			Gizmos.color = Color.blue;
			Gizmos.DrawRay(transform.position, xzForward * axisLength);
		}
	}
}