using UnityEngine;
using System.Collections;
namespace BowlingKit
{
	/*
	 * The dummy scene is just a simple scene we use for the main menu without any photon networking, scoring or any fancy effects.
	 */
	public class DummyScene : MonoBehaviour
	{
		private int m_playerTurn = 1;
		private int m_score = 0;
		private int m_throw=0;
		private bool m_isClear = true;

		void OnEnable()
		{
			BaseGameManager.onBallHitBowlingPit += onBallHitBowlingPit;
			BaseGameManager.onPinDown 		+= onPinDown;

		}
		void OnDisable()
		{
			BaseGameManager.onBallHitBowlingPit -= onBallHitBowlingPit;
			BaseGameManager.onPinDown 	   -= onPinDown;

		}
		public void onPinDown()
		{
			m_score++;
		}
		//the ball hit the bowling pit, lets reset the ball.
		public void onBallHitBowlingPit(string id)
		{
			if(id.Contains("Pit"))
			{
				if(m_isClear)
				{
					m_isClear=false;
					StartCoroutine(resetPins());
				}
			}
		}
	

		IEnumerator resetPins()
		{
			yield return new WaitForSeconds(2f);
			BaseGameManager.setPins(true);
			yield return new WaitForSeconds(2.5f);
			BaseGameManager.sweepPins();
			yield return new WaitForSeconds(3.5f);

//			Debug.Log ("score : " + m_score);
			if(m_score==10 || m_throw==1)
			{
				m_throw=-1;
				BaseGameManager.resetPins();
			}else{
				BaseGameManager.removePins();
			}
			m_throw++;
			

			m_score=0;
			m_playerTurn^=1;
			BaseGameManager.resetPlayer(0);
			m_isClear=true;
		}
	}
}
