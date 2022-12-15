using UnityEngine;
using System.Collections;
//#define PHOTON


namespace BowlingKit
{
	//our human player script
	public class HumanPlayer : BasePlayer 
	{
		//the rotation speed 
		public float rotationSpeed = 5;

		//the minium angle that we will accept for a flick.
		public float minAngle = 80f;

		//the maximum angle that we will accept for a flick
		public float maxAngle = 100f;

		//we will decrease the power by this ammount for each rank
		public float powerScalarPerLevel = 0.25f;

		//our currnet power 
		private float m_power;

#if PHOTON
		//the ref to the photon view.
		private PhotonView m_view;
#endif
		//the current x-scalar.
		private float m_xScalar;


		public override void Start()
		{
			base.Start();
#if PHOTON
			m_view = gameObject.GetComponent<PhotonView>();
#endif
			//we get the current rank and give them power based on how much power they have...
			int currentRank = PlayerPrefs.GetInt("Rank",0);
			m_power = power - (currentRank * powerScalarPerLevel);
			m_xScalar = xScalar * currentRank;
		}
		public override void OnEnable()
		{
			base.OnEnable();
			BaseGameManager.onFlick 		+= onFlick;

		}
		public override void OnDisable()
		{
			base.OnDisable();
			BaseGameManager.onFlick 		-= onFlick;
		}
		void Update()
		{
			if(m_myTurn==false)
			{
				return;
			}

			rotateBall();
		}

		void rotateBall()
		{
			//lets rotate the ball.
			RaycastHit rch;
			float mx = Input.GetAxis("Mouse X");
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(Input.GetMouseButton(0) && Physics.Raycast(ray,out rch,1000f))
			{
				if(rch.collider.gameObject.name.Contains("Ball")==false)
				{
					m_ball.transform.Rotate(new Vector3(0,mx * rotationSpeed * Time.deltaTime,0));
				}
			}
		}	
		
		//we have a flick event
		void onFlick(Vector3 startPos,Vector3 endPos,float swipeTime)
		{
			//Debug.Log ("onFlick");
			//make sure we dont have a gameover and its our turn other quit this function.
			if(m_gameOver || m_myTurn==false)
			{
				return;
			}
			#if PHOTON
			(m_view && m_view.isMine==false)
			{
				return;
			}
			#endif

			Vector3 vec =  (endPos - startPos);
			//we have a flick with more y then x
			//we have a swipe time that is less than 1/2 second, and we arent paused.
			if(Mathf.Abs(vec.y) > Mathf.Abs(vec.x) && swipeTime < 0.5f && Time.timeScale!=0)
			{

				//okay lets get the velocity of it.
				float vel = vec.magnitude / swipeTime;



				//lets multply the x componet by a scalar value.
				vec.x = vec.x * m_xScalar;
				vec.z = vec.y;
				vec.y = 0;

				//lets get the angle in degrees
				float angle = Mathf.Atan2(vec.z,vec.x) * Mathf.Rad2Deg;

				//lets make sure the angle is between min angle and max angle
				if(angle>minAngle && angle<maxAngle)
				{
					if ( m_fired==false)
					{
						m_ball.fireBall(vec.normalized * vel * m_power);
						m_fired = true;
					}
				}
			}
		}
	}
}
