using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BowlingKit
{
	public class Scoreboard : MonoBehaviour 
	{

		//the maximum number of players
		private static  int MAX_PLAYERS = 2;

		//the previous score
		private int m_prevScore = 0;

		//the height for the scoreboard
		public float yHeight = 1f;

		//a ref to the player label
		public GUIText playerLabel;


		//a dictionary used for the frames
		private Dictionary<string,GUIText> 	m_frameLabels = new Dictionary<string,GUIText>();

		//do we have a gameover yet
		private bool m_gameOver=false;

		//a dictionary to hold the players score
		private Dictionary<int,PlayerScore> m_playerScore = new Dictionary<int,PlayerScore>();

		//the current play index
		private int m_playerIndex = 0;

		public Vector2 startPos = new Vector2(-261,150);
		public Vector2 scorePos = new Vector3(-276,121);

		private Dictionary<string,string> m_dictionary = new Dictionary<string,string>();


		public void Awake()
		{
			//lets programmatically create the GUI text
			createGUITexts();

			//lets set the frame labels
			for(int i=0; i<10; i++)
			{
				int j = i+1;

				m_frameLabels["f" + i + "t1"] = getLabel(j,1);
				m_frameLabels["f" + i + "t2"] = getLabel(j,2);
				m_frameLabels["f" + i + "s"] = getScoreLabel(j);

			}
			//lets set the final frame
			m_frameLabels["f9t3"] = getLabel(10,3);

			//look for the player label -- called PLAYER
			GameObject go = GameObject.Find ("PLAYER");
			if(go)
			{
				playerLabel = go.GetComponent<GUIText>();
			}

			//start the scoreboard out of view
			transform.position = new Vector3(0,2,0);
			moveUp();

			//lets add the player score
			for(int i=0; i<MAX_PLAYERS; i++)
			{
				if(m_playerScore.ContainsKey(i)==false)
				{
					m_playerScore[i] = gameObject.AddComponent<PlayerScore>();
				}
			}


		}

		//we are going to create the GUI Texts
		void createGUITexts()
		{
			Vector2 vec2 = scorePos;
			for(int frameIndex=0; frameIndex<10;frameIndex++)
			{
				GameObject newObj = new GameObject();
				GUIText gt = newObj.AddComponent<GUIText>();
				gt.anchor = TextAnchor.MiddleCenter;
				gt.alignment = TextAlignment.Center;
				gt.font = Resources.Load ("HFF Light Petals 32") as Font;
				gt.material = gt.font.material;
				gt.pixelOffset = vec2;
				vec2.x += 58;
				gt.text  = "8";
				newObj.name = "f" + (frameIndex+1) + "s";
				newObj.transform.parent = transform;
				newObj.transform.position = new Vector3(0.5f,0.5f,1f);
				
			}
			vec2 = startPos;
			for(int frameIndex=0; frameIndex<10;frameIndex++)
			{
				int n = 2;
				if(frameIndex==9)
				{
					n=3;
				}
				for(int throwIndex=0; throwIndex<n; throwIndex++)
				{
					
					GameObject newObj = new GameObject();
					GUIText gt = newObj.AddComponent<GUIText>();
					gt.anchor = TextAnchor.MiddleCenter;
					gt.alignment = TextAlignment.Center;
					gt.font = Resources.Load ("HFF Light Petals 20") as Font;
					gt.material = gt.font.material;
					gt.pixelOffset = vec2;
					vec2.x += 29;
					gt.text  = "X";
					newObj.name = "f" + (frameIndex+1) + "t" +(throwIndex+1).ToString();
					newObj.transform.parent = transform;
					newObj.transform.position = new Vector3(0.5f,0.5f,1f);
				}
				
			}
		}
		//figure out who won
		public int getWinner()
		{
			int winnerIndex = 0;
			int score = 0;
			foreach(KeyValuePair<int, PlayerScore> item in m_playerScore)
			{
				int pscore = item.Value.getScore();
				if(pscore > score)
				{
					winnerIndex = item.Key;
					score = pscore;
				}
			}
			return (winnerIndex+1);
		}

		//lets clear the board
		void clearBoard()
		{
			for(int i=0; i<10; i++)
			{
				
				m_frameLabels["f" + i + "t1"].text = "";
				m_frameLabels["f" + i + "t2"].text = "";
				m_frameLabels["f" + i + "s"].text = "";
				
			}
		}

		public void OnEnable()
		{
			BaseGameManager.onSetScore += onSetScore;
			BaseGameManager.onGameOver += onGameOver;
		}
		public void OnDisable()
		{
			BaseGameManager.onSetScore -= onSetScore;
			BaseGameManager.onGameOver -= onGameOver;
		}
		public PlayerScore getPlayerScore(int index)
		{
			PlayerScore ps = null;
			if(m_playerScore.ContainsKey(index))
			{
				ps = m_playerScore[index];
			}
			return ps;
		}

		void updateFrameScores(int frameIndex)
		{
			for(int i=0; i<(frameIndex+1); i++)
			{
				string str2 =  "f" + i + "s";
				if(m_frameLabels.ContainsKey(str2))
				{
					int score = m_playerScore[m_playerIndex].getScoreForFrame(i);
					m_frameLabels[str2].text = score.ToString();
				}
			}
		}

		public void onSetScore(int frameIndex, 
		                       int throwIndex, 
		                       int score, 
		                       bool gutterBall,
		                       int playerIndex)
		{
			m_playerIndex = playerIndex;
			string str = "f" + frameIndex + "t" + throwIndex;
			string scoreSTR = score.ToString();
			clearBoard();

			if(playerLabel)
			{
				playerLabel.text = "Player " + (playerIndex+1).ToString();
			}

			if(throwIndex==1)
			{
				m_prevScore = score;
			}

			moveDown();
			m_playerScore[playerIndex].addScore(score,frameIndex);

			if(score==0 && gutterBall)
			{
				BaseGameManager.showTitleCard("Gutter Ball!");
				BaseGameManager.gutterBallUI();
			}
			else if(score==10 && throwIndex==1 ||
			        (score==10 && throwIndex==2 && frameIndex==GameScript.MAX_NOM_FRAMES) || 
			        (score==10 && throwIndex==3 && frameIndex==GameScript.MAX_NOM_FRAMES))
			{
				BaseGameManager.showTitleCard("Strike!");
				scoreSTR = "X";
				BaseGameManager.strikeUI();
				
			}
			else if(score+m_prevScore==10 && throwIndex>=2)
			{
				BaseGameManager.showTitleCard("Spare!");
				scoreSTR = "/";
				BaseGameManager.spareUI();
			}else 
			{
				BaseGameManager.showTitleCard(score + " Pins!");
				BaseGameManager.pinKnockDown(score);
			}
			
			
			
			updateFrameScores(frameIndex);
			string str2 = str + "player" + playerIndex;

			m_dictionary[str2] = scoreSTR;
			for(int i=0; i<frameIndex+1; i++)
			{
				for(int j=0; j<3; j++)
				{
					string id = "f" + i + "t" + j;
					string id2 = id + "player" + playerIndex;
					if(m_frameLabels.ContainsKey(id) && m_dictionary.ContainsKey(id2))
					{
						m_frameLabels[id].text = m_dictionary[ id2];
					}
				}
			}
		}
		public void onGameOver(bool vic)
		{
			m_gameOver=true;
			//moveDown();
		}
		public void moveUp()
		{
			if(m_gameOver==false)
			{
				iTween.MoveTo(gameObject,  iTween.Hash("delay",1,
											   "time",1,"y",2));
			}
		}
		public void moveDown()
		{
			iTween.MoveTo(gameObject, iTween.Hash("time",1,"y",yHeight,
			                                   "oncomplete","moveUp",
			                                   "oncompletetarget",gameObject));
		}




		GUIText getScoreLabel(int i)
		{
			GUIText rc = null;
			
			GameObject go = GameObject.Find ("f" + i +"s");
			if(go)
			{
				rc = go.GetComponent<GUIText>();
				rc.text="";
			}
			return rc;
		}

		GUIText getLabel(int i, int throwIndex)
		{
			GUIText rc = null;

			GameObject go = GameObject.Find ("f" + i + "t" + throwIndex);
			if(go)
			{
				rc = go.GetComponent<GUIText>();
				rc.text="";
			}
			return rc;
		}
	}
}