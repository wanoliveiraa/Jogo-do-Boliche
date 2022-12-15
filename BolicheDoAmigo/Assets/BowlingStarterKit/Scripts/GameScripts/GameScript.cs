using UnityEngine;
using System.Collections;

//#define PHOTON
namespace BowlingKit
{
	public class GameScript : MonoBehaviour
	{
		//the time to wait before resetting the pins, or moving to the next frame.
		public float timeToWaitBeforeResettingPins = 2;

		//which throw are we on. 
		private int m_throw = 0;
		//which frame are we on
		private int m_frame = 0;

		//the number of pins we knocked down 
		private int m_score = 0;

		//did we get a gutter ball
		private bool m_gutterBall = false;

		//should we show the frame.
		private bool m_gameover = false;

		//the maximum number of frames
		public static int MAX_NOM_FRAMES = 9;

		//the previous score.
		private int m_prevScore = 0;

		//do we want to test a perfect game
		public bool testPerfectGame = false;

		//can we reset the score.
		private bool m_canReset = true;

		//the current turn
		private int m_currentTurn = 0;

		//a ref to the scoreboard
		private Scoreboard m_scoreboard;

		public int nomFrames = 9;

		//the current number of playersr in the game
		public int nomPlayers = 1;

#if PHOTON
		//a ref to the photon view
		private PhotonView m_photonView;
#endif

		void Start()
		{
			MAX_NOM_FRAMES = nomFrames;
			m_scoreboard = (Scoreboard)GameObject.FindObjectOfType(typeof(Scoreboard));

#if PHOTON
			m_photonView = (PhotonView)gameObject.GetComponent<PhotonView>();
#endif
			//WE need the connect manager without it lets go back to the main menu.
			ConnectManager cm = (ConnectManager)GameObject.FindObjectOfType(typeof(ConnectManager));
			if(cm==null)
			{
				Application.LoadLevel(0);
			}

		}

		void OnEnable()
		{
			BaseGameManager.onBallHitBowlingPit += onBallHitBowlingPit;
			BaseGameManager.onPinDown 		+= onPinDown;
			BaseGameManager.onGutterBall	+= onGutterBall;
			BaseGameManager.onGameOver 		+= onGameOver;
		}
		void OnDisable()
		{
			BaseGameManager.onBallHitBowlingPit -= onBallHitBowlingPit;
			BaseGameManager.onPinDown 	   -= onPinDown;
			BaseGameManager.onGutterBall	-= onGutterBall;
			BaseGameManager.onGameOver 		-= onGameOver;

		}
		public void onGameOver(bool vic)
		{

			m_gameover=true;
		}
		public void onGutterBall()
		{
			m_gutterBall = true;
		}
		public void onPinDown()
		{
			m_score++;
		}
		//the ball hit the bowling pit, lets reset the ball.
		public void onBallHitBowlingPit(string id)
		{
//			Debug.Log ("onBallHitBowlingPit:" + id);
			if(id.Contains("Pit"))
			{
				bool master = true;
				#if PHOTON
					master = PhotonNetwork.isMasterClient;
				#endif
				if(m_canReset && m_gameover==false && master)
				{

					m_canReset=false;

					StartCoroutine(resetPins());

				}
			}
		}
		[RPC]
		public void resetPinsRPC(int score)
		{
			_resetPins(score);
		}
		void _resetPins(int score){
			m_throw++;
			m_score = score;
			StartCoroutine(resetPins2());	
		}
		public void nextTurn()
		{
			m_currentTurn++;


			if(nomPlayers>1 && m_currentTurn!=nomPlayers)
			{

				m_throw=0;
				m_score=0;
				m_frame--;
				m_prevScore=0;
			}

			if(m_currentTurn>=nomPlayers)
			{
				if(m_frame>MAX_NOM_FRAMES)
				{
					BaseGameManager.gameover(true);
				}
				m_currentTurn=0;
			}
			BaseGameManager.playersTurn(m_currentTurn);
		}

		IEnumerator resetPins()
		{
			//lets wait a few seconds before we start looking at the score, give the pins a chance to get knocked down! 
			yield return new WaitForSeconds(timeToWaitBeforeResettingPins+1f);

//			Debug.Log ("resetPins");
			handleDebugScore();


#if PHOTON
			if(PhotonNetwork.isMasterClient)
			{
				//Debug.Log ("resetPinsMaster");

				if(m_photonView)
				{
					m_photonView.RPC("resetPinsRPC",PhotonTargets.All,m_score);
				}
			}
#else
			_resetPins(m_score);
#endif

		}
		IEnumerator resetPins2()
		{
			//set the pins
			BaseGameManager.setPins(true);
			if(m_throw==1)
			{
				m_prevScore = m_score;
			}
			if(m_gameover==false)
			{
				BaseGameManager.setScore(m_frame,m_throw,m_score,m_gutterBall,m_currentTurn);
			}

			yield return new WaitForSeconds(2f);


			//sweep the pins
			BaseGameManager.sweepPins();
			yield return new WaitForSeconds(2f);
			BaseGameManager.fadeOut();
			yield return new WaitForSeconds(2.5f);
			BaseGameManager.fadeIn();


			handleDebugScore();
			m_gutterBall=false;

			if(m_gameover==false)
			{
				if(m_frame!=MAX_NOM_FRAMES)
				{
					if(m_throw==1)
					{
						handleThrow1();
					}else if(m_throw==2)
					{
						handleThrow2();
						nextTurn();
					}
				}else{
					handleFinalThrow();
					if(m_scoreboard)
					{
						PlayerScore ps = m_scoreboard.getPlayerScore(m_currentTurn);
						if(ps && ps.hasPerfectGame())
						{
							doGameOver();
							//nextTurn();
						}
					}
				}

				BaseGameManager.setPins(false);
				//yield return new WaitForSeconds(2f);

				BaseGameManager.resetPlayer(m_currentTurn);
				showFrame();
			}

			m_canReset=true;
		}
		public void doGameOver()
		{
			Debug.Log ("doGameOver" + m_currentTurn);
			//end if its only single player or the current turn is the last player.
			if(nomPlayers==1 || m_currentTurn==nomPlayers)
			{
				BaseGameManager.gameover(true);
			}else{
				m_frame++;
				BaseGameManager.resetPins();
				nextTurn();
			}

		}
		public void handleFinalThrow()
		{
			if(m_throw==1)
			{
				if(m_score!=10)
				{
					BaseGameManager.removePins();
				}else{
					BaseGameManager.resetPins();
				}
			}
			if(m_throw==2)
			{
				int comboScore = m_prevScore+m_score;
				Debug.Log ("throw " + m_throw + " m_score " + m_score +
				           " + " +
				           "comboSccore" + comboScore + 
				           " m_prevScore " + m_prevScore);
				if(m_score==10 || m_prevScore==10 ||  comboScore==10)
				{
					BaseGameManager.resetPins();
				}else
				{
					doGameOver();
				}
			}
			if(m_throw==3)
			{
				doGameOver();

			}
			m_score = 0;
		}
		public void handleThrow2()
		{
			m_frame++;
			m_throw=0;
			BaseGameManager.resetPins();
			m_score = 0;
		}
		public void handleThrow1()
		{
			//we didnt get a strike, lets remove the pins
			if(m_score!=10)
			{
				BaseGameManager.removePins();
			}
			//we got a strike, lets reset the throws, move to the next frame
			else
			{
				//move to the next frame unless its the final frame.
				m_frame++;
				m_throw=0;

				nextTurn();
				BaseGameManager.resetPins();
			}
			m_score=0;
		}

		
		void showFrame()
		{
			if(m_gameover==false)
				BaseGameManager.showTitleCard("Frame " + (m_frame+1) + " throw " + (m_throw+1));
			
		}
		void handleDebugScore()
		{
			if(testPerfectGame)
			{
				m_score=10;
			}
		}

	}
}
