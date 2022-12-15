using UnityEngine;
using System.Collections;
namespace BowlingKit
{
	//Will make sure the object never goes faster then this
	public class MaxSpeed : MonoBehaviour
	{
		public float maxSpeed = 11f;
		private Rigidbody m_rigidBody;
		void Start()
		{
			m_rigidBody = gameObject.GetComponent<Rigidbody>();
		}

		void FixedUpdate () 
		{
			clampVelocity();
		}

		//we want to clamp the balls velocity, so it never goes faster then the max speed, if it does we could clip through the bowling pins.
		void clampVelocity()
		{
			//we need a rigidbody.
			if(m_rigidBody==null  || m_rigidBody.isKinematic)
			{
				return;
			}

			//lets zero out gravity
			Vector3 vel = m_rigidBody.velocity;
		//	vel.y=0;

			float d0 = vel.magnitude;
			//make sure ball doesnt go faster then the max speed or it wont hit the bowling pins.
			if(d0 > maxSpeed)
			{
				m_rigidBody.velocity = vel.normalized * maxSpeed;
			}
			
		}

	}
}
