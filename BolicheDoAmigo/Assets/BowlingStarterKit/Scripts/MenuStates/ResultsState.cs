using UnityEngine;
using System.Collections;


namespace BowlingKit
{
	public class ResultsState : MonoBehaviour {

		//the game over panel
		public GameObject gameoverPanel;

		public GUIText player1Label;
		public GUIText player2Label;
		public GUIText winnerLabel;

		public GameObject pauseButton;


		void Start()
		{
			gameoverPanel.SetActive(false);
		}

		public void OnEnable()	
		{

			BaseGameManager.onGameOver += onGameOver;
			BaseGameManager.onButtonPress += onButtonClickCBF;


		}
		public void OnDisable()	
		{
			BaseGameManager.onGameOver -= onGameOver;
			BaseGameManager.onButtonPress -= onButtonClickCBF;

		}



		public void onGameOver(bool vic)
		{
			if(pauseButton)
			{
				pauseButton.SetActive(false);
			}

			GameScript gameScript = (GameScript)GameObject.FindObjectOfType(typeof(GameScript));
			Scoreboard scoreboard = (Scoreboard)GameObject.FindObjectOfType(typeof(Scoreboard));
			if(gameScript)
			{
				if(gameScript.nomPlayers==1)
				{
					player1Label.text = "Player 1 :  " + scoreboard.getPlayerScore(0).getScore();

					player2Label.gameObject.SetActive(false);
					winnerLabel.text = "GAMEOVER";

				}else{
					int p1Score = scoreboard.getPlayerScore(0).getScore();
					int p2Score = scoreboard.getPlayerScore(1).getScore();

					string winner= "No one wins!";
					if(p1Score>p2Score)
					{
						winner = "Player 1 Wins!";
					}else if(p2Score>p1Score){
						winner = "Player 2 Wins!";

					}
					player1Label.text = "Player 1 :  " + p1Score;
					player2Label.text = "Player 2 :  " + p2Score;
					winnerLabel.text = winner;
				}
				
			}
			if(gameoverPanel)
			{
				gameoverPanel.SetActive(true);
			}
		}
		public void onButtonClickCBF(string buttonID)
		{
			switch (buttonID) {
			case "Restart":
				Application.LoadLevel(Application.loadedLevel);
				break;
			case "Main Menu":
				Application.LoadLevel(0);
				break;
			}
		}
	
	
	}
}