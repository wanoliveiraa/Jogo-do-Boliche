using UnityEngine;
using System.Collections;

namespace BowlingKit
{
	//Will make sure the object never goes slower then this

	public class MinSpeed : MonoBehaviour {

		public float minSpeed = 8f;
		private Rigidbody m_rigidBody;

		private bool m_on = true;
		void Start()
		{
			m_rigidBody = gameObject.GetComponent<Rigidbody>();
		}
		public void OnEnable()
		{
			BaseGameManager.onBallHitBowlingPit += onBallHitBowlingPit;
			BaseGameManager.onResetPlayer	+= onResetPlayer;
		}
		public void OnDisable()
		{
			BaseGameManager.onBallHitBowlingPit -= onBallHitBowlingPit;
			BaseGameManager.onResetPlayer	-= onResetPlayer;

		}
		void onResetPlayer(int pi)
		{
			m_on=true;
		}
		void onBallHitBowlingPit(string id)
		{
			if(id.Contains("Slow"))
			{
				m_on=false;
			}
		}
		void FixedUpdate () 
		{
			if(m_on)
			{
				clampVelocity();
			}
		}
		
		//we want to clamp the balls velocity, so it never goes faster then the max speed, if it does we could clip through the bowling pins.
		void clampVelocity()
		{
			//we need a rigidbody.
			if(m_rigidBody==null || m_rigidBody.isKinematic)
			{
				return;
			}

			//lets zero out the gravity, we dont want to account for gravity when looking at this.
			Vector3 vel = m_rigidBody.velocity;
		//	vel.y=0;
			
			float d0 = vel.magnitude;
//				Debug.Log("Ball Velocity" + d0 + "vel " + m_rigidBody.velocity);
			//make sure ball doesnt go slower then the min speed or it wont be able to knock over the pins -- you will need to play around with this.
			if(d0 < minSpeed)
			{
				m_rigidBody.velocity = vel.normalized * minSpeed;
			}
			
		}
	}
}