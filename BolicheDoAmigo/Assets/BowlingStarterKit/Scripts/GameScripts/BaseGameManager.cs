using UnityEngine;
using System.Collections;

namespace BowlingKit
{
	public class BaseGameManager
	{
		//called when the game starts
		public delegate void OnStartStar(float time);
		public static event OnStartStar onStartStar;
		public static void startStar(float time)
		{
			if(onStartStar!=null)
			{
				onStartStar(time);	
			}
		}

		//we want to spawn the scene.
		public delegate void OnSpawnPlayers(int nomHumans, int nomAI);
		public static event OnSpawnPlayers onSpawnPlayers;
		public static void spawnPlayers(int nomHumans, int nomAI)
		{
			if(onSpawnPlayers!=null)
			{
				onSpawnPlayers(nomHumans, nomAI);
			}
		}

		//when we want to connect to the network using photon.
		public delegate void OnConnect(bool offlineMode, int levelToLoad, int nomPlayers,int nomAI);
		public static event OnConnect onConnect;
		public static void connect(bool offlineMode, int lvl, int nomPlayers,int nomAI)
		{
			if(onConnect!=null)
			{
				onConnect(offlineMode,lvl, nomPlayers,nomAI);
			}
		}

		public delegate void OnSetPins(bool pinsUp);
		public static event OnSetPins onSetPins;
		public static void setPins(bool pinsUp)
		{
			if(onSetPins!=null)
			{
				onSetPins(pinsUp);
			}
		}
		//we want to spawn the scene.
		public delegate void OnSweepPins();
		public static event OnSweepPins onSweepPins;
		public static void sweepPins()
		{
			if(onSweepPins!=null)
			{
				onSweepPins();
			}
		}

		//when we want to connect to the network using photon.
		public delegate void OnDisconnect();
		public static event OnDisconnect onDisconnect;
		public static void disconnect()
		{
			if(onDisconnect!=null)
			{
				onDisconnect();
			}
		}


		//called when the score is set
		public delegate void OnSetScore(int frameIndex, int throwIndex, int score,bool gutterBall, int playerIndex);
		public static event OnSetScore onSetScore;
		public static void setScore(int frameIndex,int throwIndex, int score,bool gutterBall, int playerIndex)
		{
			if(onSetScore!=null)
			{
				onSetScore(frameIndex,throwIndex,score,gutterBall,playerIndex);
			}
		}

		//a button has been pressed
		public delegate void OnButtonPress(string buttonID);
		public static event OnButtonPress onButtonPress;
		public static void buttonPress(string buttonID)
		{
			if(onButtonPress!=null)
			{
				onButtonPress(buttonID);
			}
		}


		//called when a ball becomes active
		public delegate void OnSetActiveBall(Transform t);
		public static event OnSetActiveBall onSetActiveBall;
		public static void setActiveBall(Transform t)
		{
			if(onSetActiveBall!=null)
			{
				onSetActiveBall(t);	
			}
		}

		//called when the player rests
		public delegate void OnResetPlayer(int playerIndex);
		public static event OnResetPlayer onResetPlayer;
		public static void resetPlayer(int playerIndex)
		{
			if(onResetPlayer!=null)
			{
				onResetPlayer(playerIndex);	
			}
		}

		//called when the turn changes.
		public delegate void OnPlayerTurn(int playerIndex);
		public static event OnPlayerTurn onPlayerTurn;
		public static void playersTurn(int playerIndex)
		{
			if(onPlayerTurn!=null)
			{
				onPlayerTurn(playerIndex);	
			}
		}

		//called when we want to fade out
		public delegate void OnFadeOut();
		public static event OnFadeOut onFadeOut;
		public static void fadeOut()
		{
			if(onFadeOut!=null)
			{
				onFadeOut();	
			}
		}
		//called when we want fade in
		public delegate void OnFadeIn();
		public static event OnFadeIn onFadeIn;
		public static void fadeIn()
		{
			if(onFadeIn!=null)
			{
				onFadeIn();	
			}
		}

		//called when the ball is fired.
		public delegate void OnFireBall();
		public static event OnFireBall onFireBall;
		public static void fireBall()
		{
			if(onFireBall!=null)
			{
				onFireBall();	
			}
		}

		//called when the game starts
		public delegate void OnGameStart();
		public static event OnGameStart onGameStart;
		public static void startGame()
		{
			if(onGameStart!=null)
			{
				onGameStart();	
			}
		}
		
		//called when the game starts
		public delegate void OnShowTitleCard(string title);
		public static event OnShowTitleCard onShowTitleCard;
		public static void showTitleCard(string title)
		{
			if(onShowTitleCard!=null)
			{
				onShowTitleCard(title);	
			}
		}


		//called when the game is paused
		public delegate void OnGamePause(bool pause);
		public static event OnGamePause onGamePause;
		public static void pauseGame(bool pause)
		{
			if(onGamePause!=null)
			{
				onGamePause(pause);	
			}
		}

		//called when the game is over
		public delegate void OnGameOver(bool victory);
		public static event OnGameOver onGameOver;
		public static void gameover(bool victory)
		{
			if(onGameOver!=null)
			{
				onGameOver(victory);	
			}
		}

		//called when we reset a pin
		public delegate void OnResetPin();
		public static event OnResetPin onResetPin;
		public static void resetPins()
		{
			if(onResetPin!=null)
			{
				onResetPin();	
			}
		}

		/// <summary>
		/// Occurs when we remove a pin.
		/// </summary>
		public delegate void OnRemovePin();
		public static event OnRemovePin onRemovePin;
		public static void removePins()
		{
			if(onRemovePin!=null)
			{
				onRemovePin();	
			}
		}

		/// <summary>
		/// Occurs when a pin down is knocked down.
		/// </summary>
		public delegate void OnPinDown();
		public static event OnPinDown onPinDown;
		public static void pinDown()
		{
			if(onPinDown!=null)
			{
				onPinDown();	
			}
		}
		

		/// <summary>
		/// Occurs when on ball hits the bowling pit.
		/// </summary>
		public delegate void OnBallHitBowlingPit(string id);
		public static event OnBallHitBowlingPit onBallHitBowlingPit;
		public static void ballHitBowlingPit(string id)
		{
			if(onBallHitBowlingPit!=null)
			{
				onBallHitBowlingPit(id);	
			}
		}

		//called when the ball is flicked.
		public delegate void OnPinKnockDown(int score);
		public static event OnPinKnockDown onPinKnockDown;
		public static void pinKnockDown(int score)
		{
			if(onPinKnockDown!=null)
			{
				onPinKnockDown(score);	
			}
		}

		//called whehe ball goes in the gutter
		public delegate void OnGutterBall();
		public static event OnGutterBall onGutterBall;
		public static void gutterBall()
		{
			if(onGutterBall!=null)
			{
				onGutterBall();	
			}
		}
		//called whehe ball is flicked.
		public delegate void OnSpareUI();
		public static event OnSpareUI onSpare;
		public static void spareUI()
		{
			if(onSpare!=null)
			{
				onSpare();	
			}
		}


		//called whehe ball is flicked.
		public delegate void OnGutterUI();
		public static event OnGutterUI onGutterUI;
		public static void gutterBallUI()
		{
			if(onGutterUI!=null)
			{
				onGutterUI();	
			}
		}

		//called whehe ball is flicked.
		public delegate void OnStrikeUI();
		public static event OnStrikeUI onStrike;
		public static void strikeUI()
		{
			if(onStrike!=null)
			{
				onStrike();	
			}
		}
		
		//called whehe ball is flicked.
		public delegate void OnToggleAudio();
		public static event OnToggleAudio onToggleAudio;
		public static void toggleAudio()
		{
			if(onToggleAudio!=null)
			{
				onToggleAudio();	
			}
		}

		//called when the ball is flicked.
		public delegate void OnFlick(Vector3 start, Vector3 end,float pow);
		public static event OnFlick onFlick;
		public static void flick(Vector3 start, Vector3 end,float pow)
		{
			if(onFlick!=null)
			{
				onFlick(start,end,pow);	
			}
		}
	}
}
