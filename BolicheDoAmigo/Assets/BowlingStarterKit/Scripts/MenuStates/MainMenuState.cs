using UnityEngine;
using System.Collections;
namespace BowlingKit
	{
	public class MainMenuState : MonoBehaviour {

		public TouchButton2 graphicsQuality;
		public GameObject mainPanel;
		public GameObject optionsPanel;
		public GameObject lobbyPanel;
		public GameObject configPanel;
		public GameObject alleyPanel;

		public TouchButton2 aiDifficultyButton;
		public TouchButton2 enemyButton;

		public Texture[] aiLevels;
		public Texture[] enemyTextures;
		public Texture[] alleySelect;
		public Texture[] graphicsTextures;
		public int lvltoLoad = 1;
		public TouchButton2 selectedAlley;


		void Start()
		{
			updateGraphicsQuality();
			updateAI();
			updateEnemy();
			updateAlley();
		}
		public void OnEnable()
		{
			BaseGameManager.onButtonPress += onButtonClickCBF;
		}
		public void OnDisable()
		{
			BaseGameManager.onButtonPress -= onButtonClickCBF;
		}
		public void onButtonClickCBF(string buttonID)
		{
			switch (buttonID) 
			{
			case "SinglePlayer":
				configPanel.SetActive(true);
				mainPanel.SetActive(false);
				//BaseGameManager.connect(true,1,false);
				break;
				
				
			case "Player2Toggle":
				toggleEnemies();
				//BaseGameManager.connect(true,1,false);
				break;
			case "StartGame":
				handleStartGame();
				//BaseGameManager.connect(true,1,false);
				break;
			case "AlleySelect":
				alleyPanel.SetActive(true);
				mainPanel.SetActive(false);
				break;
			case "AlleyBack":
				alleyPanel.SetActive(false);
				mainPanel.SetActive(true);
				break;
			case "AlleyToggle":
				toggleAlley();
				break;
			case "AIToggle":
			{
				toggleAIDifficulty();
			}
			break;
			case "Options":
				optionsPanel.SetActive(true);
				mainPanel.SetActive(false);
				break;
			case "LobbyBack":
				leaveLobby();
				break;
				
			case "Multiplayer":
				handleMultiplayer();
				break;
			case "OptionsBack":
				Debug.Log ("optionsBack");
				optionsPanel.SetActive(false);
				mainPanel.SetActive(true);
				break;
				
			case "GraphicsToggle":
				toggleQuality();
				break;
			}
		}
		public int getLevelToLoad()
		{
			return PlayerPrefs.GetInt("Alley",0) + 1;
		}
		public void handleMultiplayer()
		{
			lobbyPanel.SetActive(true);
			mainPanel.SetActive(false);
			//lets connect to photon, 2 human players no AI
			BaseGameManager.connect(false,getLevelToLoad(),1,0);
		}
		public void leaveLobby()
		{
			lobbyPanel.SetActive(false);
			mainPanel.SetActive(true);
			//lets disconnect from photon.
			BaseGameManager.disconnect();
		}
		void handleStartGame()
		{
			int enemy = PlayerPrefs.GetInt("Enemy",0);
			int nomHumans = 1;
			int nomAI = 0;
			if(enemy==1)
			{
				nomAI = 1;
			}

			if(enemy == 2)
			{
				nomHumans=2;
			}
			BaseGameManager.connect(true,getLevelToLoad(),nomHumans,nomAI);
		}
//		public void OnClick(dfControl control, dfMouseEventArgs mouseEvent ){

	
		void toggleAlley()
		{
			int val = PlayerPrefs.GetInt("Alley",0);
			val++;
			if(val>=alleySelect.Length)
			{
				val=0;
			}
			PlayerPrefs.SetInt("Alley",val);
			updateAlley();

		}
		void updateAlley()
		{
			int alley = PlayerPrefs.GetInt("Alley",0);

			if(selectedAlley)
			{	
				selectedAlley.setTexture(alleySelect[alley]);
			}
		}


		void toggleEnemies()
		{
			int val = PlayerPrefs.GetInt("Enemy",0);
			val++;
			if(val>=enemyTextures.Length)
			{
				val=0;
			}
			PlayerPrefs.SetInt("Enemy",val);
			updateEnemy();
		}
		void updateEnemy()
		{
			int enemy = PlayerPrefs.GetInt("Enemy",0);
			if(enemyButton)
			{	
				if(enemy>-1 && enemy<enemyTextures.Length)
				{
					enemyButton.setTexture(enemyTextures[enemy]);
				}
			}
		}




		void toggleAIDifficulty()
		{
			int ai = PlayerPrefs.GetInt("AIDifficultyX",1);
			ai++;
			if(ai>=aiLevels.Length)
			{
				ai=0;
			}
			PlayerPrefs.SetInt("AIDifficultyX",ai);
			updateAI();
		}
		void updateAI()
		{
			int ai = PlayerPrefs.GetInt("AIDifficultyX",1);
//			Debug.Log ("AI" + ai);
			if(aiDifficultyButton)
			{	
				if(ai>-1 && ai<aiLevels.Length)
				{
					aiDifficultyButton.setTexture(aiLevels[ai]);
				}
			}
		}
		public void handleGameCenter()
		{
			//handle gamecenter scores
		}

		public void toggleQuality()
		{
			int currentQuality = QualitySettings.GetQualityLevel();

			if(currentQuality==0)
			{
				QualitySettings.SetQualityLevel(1);		
			}
			else if(currentQuality==1)
			{
				QualitySettings.SetQualityLevel(2);		
			}
			else if(currentQuality==2)
			{
				QualitySettings.SetQualityLevel(0);		
			}
			Debug.Log ("toggleQuality" + QualitySettings.GetQualityLevel() + " oldquality " + currentQuality);
			updateGraphicsQuality();
		}

		void updateGraphicsQuality()
		{
			if(graphicsQuality)
			{
				graphicsQuality.setTexture(graphicsTextures[QualitySettings.GetQualityLevel()]);// = "Graphics: "+ QualitySettings.names[QualitySettings.GetQualityLevel()];
			}
		}
	}

}