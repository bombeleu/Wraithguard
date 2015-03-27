using System.Collections.Generic;
using UnityEngine;

namespace Wraithguard
{
	public class VerticalRectangleLayout
	{
		public List<Rect> rectangles;
		public Vector2 position
		{
			get
			{
				return _position;
			}
			set
			{
				_position = value;
				
				Relayout();
			}
		}
		public float margin
		{
			get
			{
				return _margin;
			}
			set
			{
				_margin = value;
				
				Relayout();
			}
		}
		
		public VerticalRectangleLayout(Vector2 position, float margin)
		{
			rectangles = new List<Rect>();
			_position = position;
			_margin = margin;
		}
		public void AddRectangle(Vector2 rectangle)
		{
			rectangles.Add(new Rect(0, 0, rectangle.x, rectangle.y));
			
			Relayout();
		}
		
		private Vector2 _position;
		private float _margin;
		
		private void Relayout()
		{
			Vector2 currentPosition = position;
			
			for(int rectangleIndex = 0; rectangleIndex < rectangles.Count; rectangleIndex++)
			{
				Rect rectangle = rectangles[rectangleIndex];
				rectangle.x = currentPosition.x;
				rectangle.y = currentPosition.y;
				
				rectangles[rectangleIndex] = rectangle;
				
				currentPosition.y += rectangle.height + margin;
			}
		}
	}
}