using UnityEngine;
using System.Collections;
//#define PHOTON

namespace BowlingKit
{
	public class ShotClock : MonoBehaviour
	{
		#if PHOTON

		private PhotonView m_view;
		//the current clock time
		private float m_clockTime = 10;
		
		//the time we have before we give the player -- 0 points
		public float clockTime = 10;
		private GUIText m_shotClockGT;
		public int fontSize=  32;
		public Color fontColor = Color.red;
		private bool m_clockOn=false;

		public Vector3 clockPosition = new Vector3(.5f,0.5f,0);
		public void Start()
		{
			m_view = gameObject.GetComponent<PhotonView>();
			GameObject newgameObject = new GameObject();
			newgameObject.transform.position = clockPosition;
			GUIText gt = newgameObject.AddComponent<GUIText>();
			if(gt)
			{
				gt.font = Resources.Load ("Huggable") as Font;
				gt.fontSize = fontSize;
				gt.font.material.color = fontColor;
				gt.text = "";
				m_shotClockGT=gt;
			}
			newgameObject.transform.parent = transform;
		}

		public void OnEnable()
		{
			BaseGameManager.onResetPlayer 	 	+= handleStartClock;
			BaseGameManager.onFireBall		+= handleStopClock;
			BaseGameManager.onGameStart		+= onGameStart;


		}
		public void OnDisable()
		{
			BaseGameManager.onResetPlayer  	-= handleStartClock;
			BaseGameManager.onFireBall		-= handleStopClock;
			BaseGameManager.onGameStart		-= onGameStart;

		}

		public void onGameStart()
		{
//			Debug.Log ("onGameStart");
			handleStartClock(0);
		}

		public void handleStopClock()
		{
			if(PhotonNetwork.isMasterClient)
			{
				if(m_view)
				{
					m_view.RPC ("stopClockRPC",PhotonTargets.All);
				}
			}
		}

		[RPC]
		void stopClockRPC()
		{
			stop ();
		}


		public void stop()
		{
			m_clockOn=false;
			if(m_shotClockGT)
			{
				m_shotClockGT.text = "";
			}
		}

		public void handleStartClock(int playerIndex)
		{
			if(PhotonNetwork.isMasterClient && PhotonNetwork.offlineMode==false && Application.loadedLevel>0)
			{
				if(m_view)
				{
					m_view.RPC ("startClockRPC",PhotonTargets.All,clockTime);
				}
			}
		}

		[RPC]
		void startClockRPC(float ct)
		{
			m_clockOn=true;
			m_clockTime = ct;
			StartCoroutine(onTickIE());
			if(m_shotClockGT)
			{
				int time = (int)m_clockTime;
				m_shotClockGT.text = time.ToString();
			}
		}
		[RPC]
		void updateTimeRPC(float ct)
		{
			if(m_shotClockGT && m_clockOn)
			{
				int time = (int)ct;
				m_shotClockGT.text = time.ToString();
			}
		}
		[RPC]
		void timeExpires()
		{
			clockExpires();
		}

		IEnumerator onTickIE()
		{
			if(PhotonNetwork.isMasterClient)
			{
				yield return new WaitForSeconds(1);
				m_clockTime--;

				if(m_view)
				{
					m_view.RPC ("updateTimeRPC",PhotonTargets.All,m_clockTime);
				}

				if(m_clockTime<1 && m_clockOn)
				{
					if(m_view)
					{
						m_view.RPC ("timeExpires",PhotonTargets.All);
					}

				}
				else{
					if(m_clockOn)
					{
						StartCoroutine(onTickIE());
					}
				}
			}
		}
		
		//we want to spawn the scene.
		public delegate void OnClockExpires();
		public static event OnClockExpires onClockExpires;
		public static void clockExpires()
		{
			if(onClockExpires!=null)
			{
				onClockExpires();
			}
		}
#endif

	}
}