using UnityEngine;
using System.Collections;
namespace BowlingKit
{
	//the base play for our bowling characters -- wether it be human or AI
	public class BasePlayer : MonoBehaviour 
	{
		//a scalar that effects how much our x-component will effect when firing the ball. The smaller it is the easier it should be to get a strike.
		public float xScalar = .25f;
		
		//the power we use to fire the ball
		public float power = 100f;
		
		


		//reference to the ball
		protected Ball m_ball;
		
		//have we already fired the ball.
		protected bool m_fired = false;

		//do we have a gameover yet
		protected bool m_gameOver=false;

		//the player index for the player
		public int playerIndex = 0;

		//is it my turn.
		protected bool m_myTurn = false;
		protected int m_frame = 0;

		//the chance to do an angle spread
		public float xMax = 1;
		public virtual void Start()
		{
			m_ball = gameObject.GetComponentInChildren<Ball>();
			m_myTurn = playerIndex==0;
			onPlayerTurn(0);
		}
		
		public virtual void OnEnable()
		{
			BaseGameManager.onGameOver 		+= onGameOver;
			BaseGameManager.onPlayerTurn 	+= onPlayerTurn;
			BaseGameManager.onResetPlayer 	+= onResetPlayer;
#if PHOTON
			ShotClock.onClockExpires		+= onShotExpires;
#endif
		}
		public virtual void OnDisable()
		{
			BaseGameManager.onGameOver 		-= onGameOver;
			BaseGameManager.onPlayerTurn 	-= onPlayerTurn;
			BaseGameManager.onResetPlayer 	-= onResetPlayer;
			#if PHOTON
			ShotClock.onClockExpires		-= onShotExpires;
			#endif
		}

		public virtual void onPlayerTurn(int pi)
		{
			if(pi==playerIndex)
			{
				m_myTurn = true;
				m_frame = 0;
				BaseGameManager.setActiveBall(m_ball.transform);
			}else{
				m_myTurn = false;
			}
			//hide the ball thats not active and vice versa.
			if(m_ball) 
			{	
				m_ball.gameObject.SetActive(m_myTurn);
			}
		}
		void onGameOver(bool vic)
		{
			m_gameOver=true;
		}
		public void onResetPlayer(int pi)
		{
			reset ();
		}

		public virtual void reset()
		{
			m_fired=false;
			if(m_ball)
			{
				m_ball.reset();
			}
		}
		void onShotExpires()
		{
			if(m_myTurn)
			{
				fireBall();
			}
		}
		void fireBall()
		{
			if ( m_fired==false)
			{
				m_fired = true;

				if(m_gameOver==false)
				{
					float vel = Random.Range(1000f,5000f);
					Quaternion q = Quaternion.AngleAxis(Random.Range(-xMax,xMax),Vector3.up);
					m_ball.fireBall( q * transform.forward * vel * power);
				}
			}
		}

	}		

}
