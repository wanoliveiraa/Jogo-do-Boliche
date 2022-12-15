using UnityEngine;
using System.Collections;
using System.Collections.Generic;
namespace BowlingKit
{
	//this is our player score class
	public class PlayerScore : MonoBehaviour {
		//the current roll
		private int currentRoll = 0;
		private int score = 0;
		private Dictionary<int,int> 		m_rollFrameCount = new Dictionary<int,int>();
		private readonly IList<int> 		m_rolls = new List<int>();

		//does the player have a perfect game.
		private bool m_perfectGame = false;

		//is the player done.
		private bool m_playerDone = false;

		//the score for the player
		private int m_score = 0;

		public int getScore()
		{
			return m_score;
		}
		public void setPlayerDone(bool done)
		{
			m_playerDone = done;
		}

		public void addScore(int score,int frameIndex)
		{
			m_rolls.Add(score);
			m_rollFrameCount[frameIndex] = m_rolls.Count;
		}
		public int calculateScore(int frameIndex)
		{
			score = 0;
			currentRoll = 0;
			scoreAllRolls(frameIndex);
			return score;
		}
		private void scoreAllRolls(int frameIndex)
		{
			for (var frame = 0; frame < frameIndex; frame++)
			{
				if(frameIndex > currentRoll && m_rolls.Count > currentRoll)
				{
					if (m_rolls[currentRoll] == 10)
						scoreStrike(frameIndex);
					else if (sumUpRollsFromCurrent(2,frameIndex) == 10)
						scoreSpare(frameIndex);
					else
						scoreNormal(frameIndex);
				}
			}
		}
		private void scoreNormal(int frameIndex)
		{
			score += sumUpRollsFromCurrent(2,frameIndex);
			currentRoll += 2;
		}

		private void scoreSpare(int frameIndex)
		{
			score += sumUpRollsFromCurrent(3,frameIndex);
			currentRoll += 2;
		}
		private void scoreStrike(int frameIndex)
		{
			score += sumUpRollsFromCurrent(3,frameIndex);
			currentRoll++;
		}
		
		private int sumUpRollsFromCurrent(int numberOfRolls, int frameIndex)
		{
			var sum = 0;
			for (var i = 0; i < numberOfRolls; i++)
			{
				int n = currentRoll + i;
				if(m_rolls.Count > n)
				{
					sum += m_rolls[currentRoll + i];
				}
			}
			return sum;
			
		}

		public int getScoreForFrame(int index)
		{
			int score = calculateScore(m_rollFrameCount[index]);
			if(score > m_score)
			{
				m_score = score;
			}
			//we got 300 a perfect game or something went wrong and we got more then 300.
			if(score>=300)
			{
				m_perfectGame = true;
			}
			return score;
		}

		public bool hasPerfectGame()
		{
			return m_perfectGame;
		}
		public bool isPlayerDone()
		{
			return m_playerDone;
		}

	}
}
