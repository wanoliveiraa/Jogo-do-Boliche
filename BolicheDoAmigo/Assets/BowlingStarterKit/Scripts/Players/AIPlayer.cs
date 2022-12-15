using UnityEngine;
using System.Collections;
namespace BowlingKit
{
	//a simple "AI" it will simply fire the ball at the closest pin.
	//You could make this ai easier or harder by increasing or decreasing the xMax 
	public class AIPlayer : BasePlayer 
	{
		//the minimum power we are going to use.
		public float minPower = 0.25f;
		//the max maxpower to have
		public float maxPower = 1000f;

		//how long should we wait before firing the ball
		public float aiWaitTime = 1f;

		//a ref to the pins
		private Pin[] m_pins;

		//our target x position
		private float m_targetX = 0;



		public void Awake()
		{
			m_pins = (Pin[])GameObject.FindObjectsOfType(typeof(Pin));
		}
		public void Update()
		{
			if(m_fired==false && m_myTurn && m_gameOver==false)
			{
				StartCoroutine(fireBallIE());
			}
		}


		IEnumerator fireBallIE()
		{
			if ( m_fired==false)
			{
				m_fired = true;

				yield return new WaitForSeconds(aiWaitTime);

				if(m_gameOver==false)
				{
					float vel = Random.Range(minPower,maxPower);

					//if its the frame find a new pin
					if(m_frame==1)
					{
						findClosestPin();
						iTween.MoveTo(gameObject,iTween.Hash("x",m_targetX,"time",.5f));
						yield return new WaitForSeconds(1.25f);

					}
					m_frame++;
					Quaternion q = Quaternion.AngleAxis(Random.Range(-xMax,xMax),Vector3.up);

					m_ball.fireBall( q * transform.forward * vel * power);
				}
			}
		}


		public void findClosestPin()
		{
			float d0 = 1000000f;
			for(int i=0; i<m_pins.Length; i++)
			{
				float d1 = (transform.position - m_pins[i].transform.position).magnitude;
				if(d1<d0){
					m_targetX = m_pins[i].transform.position.x;
					d0 = d1;
				}
			}
		}

	}
}
