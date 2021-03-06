﻿using UnityEngine;

namespace CUF
{
	public class FlyingCameraComponent : MonoBehaviour
	{
		public float speed = 3;
		public float maxVerticalAngle = 90;
		
		private Vector3 eulerAngles;
		
		private void Start()
		{
			eulerAngles = transform.eulerAngles;
		}
		private void Update()
		{
			Rotate();
			Translate();
		}
		
		private void Rotate()
		{
			Vector2 deltaMouse = Mouse.deltaMouse;
			
			eulerAngles.x = Mathf.Clamp(eulerAngles.x - deltaMouse.y, -maxVerticalAngle, maxVerticalAngle);
			eulerAngles.y = Mathf.Repeat(eulerAngles.y + deltaMouse.x, 360);
			
			transform.eulerAngles = eulerAngles;
		}
		private void Translate()
		{
			Vector3 velocity = GetMovementDirectionFromInput() * speed;
			Vector3 translation = velocity * Time.deltaTime;
			
			transform.Translate(translation);
		}
		private Vector3 GetMovementDirectionFromInput()
		{
			Vector3 movementDirection = Vector3.zero;
			
			if(Input.GetKey(KeyCode.W))
			{
				movementDirection += Vector3.forward;
			}
			
			if(Input.GetKey(KeyCode.A))
			{
				movementDirection -= Vector3.right;
			}
			
			if(Input.GetKey(KeyCode.S))
			{
				movementDirection -= Vector3.forward;
			}
			
			if(Input.GetKey(KeyCode.D))
			{
				movementDirection += Vector3.right;
			}
			
			if(Input.GetKey(KeyCode.Q))
			{
				movementDirection -= Vector3.up;
			}
			
			if(Input.GetKey(KeyCode.E))
			{
				movementDirection += Vector3.up;
			}
			
			return movementDirection.normalized;
		}
	}
}