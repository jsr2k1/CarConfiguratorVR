/******************************************************************************
 * Copyright (C) Leap Motion, Inc. 2011-2017.                                 *
 * Leap Motion proprietary and  confidential.                                 *
 *                                                                            *
 * Use subject to the terms of the Leap Motion SDK Agreement available at     *
 * https://developer.leapmotion.com/sdk_agreement, or another agreement       *
 * between Leap Motion and you, your company or other organization.           *
 ******************************************************************************/

using UnityEngine;
using UnityEngine.Events;
using Leap.Unity.Attributes;
using System.Collections.Generic;

namespace Leap.Unity.Interaction
{

	///<summary>
	/// A physics-enabled slider. Sliding is triggered by physically pushing the slider to its compressed position. 
	///</summary>
	public class InteractionCircularSlider : InteractionButton
	{

		[Space, Space]
		[Tooltip("The minimum and maximum values that the slider reports on the axis.")]
		public Vector2 valueRange = new Vector2(0f, 1f);

		[Space]
		[Tooltip("Radius of the circle.")]
		[MinValue(0)]
		public float radius = 0;

		[Space]
		[Tooltip("The minimum and maximum extents that the slider can slide to in world space.")]
		[MinMax(0f, 1f)]
		public Vector2 sliderLimits = new Vector2(0f, 1f);

		[Tooltip("The number of discrete quantized notches that this slider can occupy on the circular axis.")]
		[MinValue(0)]
		public int steps = 0;

		[Tooltip("Has the slider to face the center of the circle?")]
		public bool faceCenter = true;

		[System.Serializable]
		public class FloatEvent : UnityEvent<float> { }
		///<summary> Triggered while this slider is depressed. </summary>
		public FloatEvent slideEvent = new FloatEvent();

		public float SliderPercent
		{
			get
			{
				return _sliderPercent;
			}
			set
			{
				if (!_started) Start();

				_sliderPercent = value;

				Vector2 pos = new Vector2(Mathf.Cos(2 * Mathf.PI * _sliderPercent), Mathf.Sin(2 * Mathf.PI * _sliderPercent));

				MathExtension.FindLineCircleIntersections(initialLocalPosition, radius, initialLocalPosition, pos, out intersection1, out intersection2);

				localPhysicsPosition.x = intersection1.x;
				localPhysicsPosition.y = intersection1.y;
				physicsPosition = transform.parent.TransformPoint(localPhysicsPosition);
				rigidbody.position = physicsPosition;
			}
		}

		///<summary> This slider's value, mapped between the values in the valueRange. </summary>
		public float SliderValue
		{
			get
			{
				return Mathf.Lerp(valueRange.x, valueRange.y, _sliderPercent);
			}
			set
			{
				SliderPercent = Mathf.InverseLerp(valueRange.x, valueRange.y, value);
			}
		}

		//Internal Slider Values
		protected float _sliderPercent;
		protected RectTransform parent;
		protected Vector2 intersection1, intersection2;
		protected Vector3 newLocalEulerAngles = new Vector3(0,0,0);

		private bool _started = false;
		private bool _firstCorrection = true;

		protected override void Start()
		{
			if (_started) return;

			_started = true;

			if (transform.parent != null)
			{
				parent = transform.parent.GetComponent<RectTransform>();
				if (parent != null)
				{
					if (parent.rect.width < 0f || parent.rect.height < 0f)
					{
						Debug.LogError("Parent Rectangle dimensions negative; can't set slider boundaries!", parent.gameObject);
						enabled = false;
					}
					else
					{
						//sliderLimits = new Vector2(parent.rect.xMin - transform.localPosition.x, parent.rect.xMax - transform.localPosition.x);
						radius = parent.rect.xMin - transform.localPosition.x;
					}
				}
			}

			base.Start();
		}

		protected override void Update()
		{
			base.Update();

			if (isDepressed || isGrasped)
			{
				calculateSliderValues();
			}

			if (faceCenter)
			{
				transform.localEulerAngles = newLocalEulerAngles;
			}
			else
			{
				transform.localEulerAngles = new Vector3(0, 0, 0);
			}


			if (_firstCorrection)// && !isPrimaryHovered && !isGrasped)
			{
				//MoveToInitialPosition();
				_firstCorrection = false;
				SliderValue = 0;
			}
			/*
			else
			{
				_firstCorrection = false;
			}
			*/
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			OnContactStay += calculateSliderValues;
		}

		protected override void OnDisable()
		{
			OnContactStay -= calculateSliderValues;
			base.OnDisable();
		}

		public void ForceCalculateSliderValues()
		{
			calculateSliderValues();
		}

		private void calculateSliderValues()
		{
			//Calculate the Renormalized Slider Values
			if (sliderLimits.x != sliderLimits.y)
			{
				float angle = Mathf.Atan2(intersection1.y, intersection1.x);

				if (angle < 0)
				{
					angle += 2 * Mathf.PI;
				}
				_sliderPercent = Mathf.InverseLerp(sliderLimits.x, sliderLimits.y, angle / (2 * Mathf.PI));
				slideEvent.Invoke(SliderValue);
			}
		}

		protected override Vector3 getDepressedConstrainedLocalPosition(Vector3 desiredOffset)
		{
			Vector2 desiredLocalPosition = new Vector2((localPhysicsPosition.x + desiredOffset.x), (localPhysicsPosition.y + desiredOffset.y));

			//Avoid Line Circle Intersection errors
			if(desiredLocalPosition == new Vector2(0,0))
			{
				desiredLocalPosition = new Vector2(1,0);
			}

			MathExtension.FindLineCircleIntersections(initialLocalPosition, radius, initialLocalPosition, desiredLocalPosition, out intersection1, out intersection2);

			float angle = Mathf.Atan2(intersection1.y, intersection1.x);

			if(angle < 0)
			{
				angle += 2 * Mathf.PI;
			}

			float sliderPercent = Mathf.InverseLerp(0, 1, angle/(2*Mathf.PI));
			
			if (steps > 0)
			{
				sliderPercent = Mathf.Round(sliderPercent * (steps)) / (steps);
			}

			Vector2 pos = new Vector2(Mathf.Cos(2 * Mathf.PI * sliderPercent), Mathf.Sin(2 * Mathf.PI * sliderPercent));

			MathExtension.FindLineCircleIntersections(initialLocalPosition, radius, initialLocalPosition, pos, out intersection1, out intersection2);

			if (angle > sliderLimits.y * 2 * Mathf.PI)
			{
				pos = new Vector2(Mathf.Cos(2 * Mathf.PI * sliderLimits.y), Mathf.Sin(2 * Mathf.PI * sliderLimits.y));
				MathExtension.FindLineCircleIntersections(initialLocalPosition, radius, initialLocalPosition, pos, out intersection1, out intersection2);
			}
			else if(angle < sliderLimits.x * 2 * Mathf.PI)
			{
				pos = new Vector2(Mathf.Cos(2 * Mathf.PI * sliderLimits.x), Mathf.Sin(2 * Mathf.PI * sliderLimits.x));
				MathExtension.FindLineCircleIntersections(initialLocalPosition, radius, initialLocalPosition, pos, out intersection1, out intersection2);
			}

			if (faceCenter)
			{
				angle = Mathf.Atan2(intersection1.y, intersection1.x);

				if (angle < 0)
				{
					angle += 2 * Mathf.PI;
				}

				newLocalEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, angle * Mathf.Rad2Deg);
			}

			return new Vector3(intersection1.x, intersection1.y, localPhysicsPosition.z + desiredOffset.z);
		}

		protected override void OnDrawGizmosSelected()
		{
			base.OnDrawGizmosSelected();
			if (transform.parent != null)
			{
				Vector3 originPosition = Application.isPlaying ? initialLocalPosition : transform.localPosition;

				parent = transform.parent.GetComponent<RectTransform>();
				if (parent != null)
				{
					//sliderLimits = new Vector2(parent.rect.xMin - originPosition.x, parent.rect.xMax - originPosition.x);
					radius = parent.rect.xMin - transform.localPosition.x;
				}

				Gizmos.color = Color.blue;
				Gizmos.DrawWireSphere(originPosition, radius);
				Gizmos.DrawLine(originPosition, new Vector3(intersection1.x, intersection1.y, transform.localPosition.z));
				Gizmos.DrawWireSphere(new Vector3(intersection1.x, intersection1.y, originPosition.z), 0.01f);
				Gizmos.color = Color.red;
				Gizmos.DrawWireSphere(transform.localPosition, 0.01f);
			}
		}

		void MoveToInitialPosition()
		{
			transform.localPosition += new Vector3(radius, 0, 0);
			Debug.Log("aaaaaaaaaa");
		}

	}
}
