using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//#define PHOTON

namespace BowlingKit
{
	public class ConnectManager : MonoBehaviour 
	{
		//the maximum number of playes in the game
		public int MAX_PLAYERS  = 2;

#if PHOTON
		//a ref to the photon view
		PhotonView m_photonView;
#endif

		private int m_levelToLoad;

		private int m_playersInRoom;
		//our label to display whats going on 
		public GUIText playersInRoomLbl;

		//the number of ai to spawn.
		private int m_aiToSpawn;
		//the number of humans to spawn.
		private int m_nomHumans;

		//do we want to use the shot clock
		private bool m_useShotClock = false;

		private BowlingKit.ShotClock m_shotClock;
		void Awake()
		{
			DontDestroyOnLoad( gameObject );

#if PHOTON
			m_photonView = gameObject.GetComponent<PhotonView>();
#endif

			m_shotClock = gameObject.GetComponent<BowlingKit.ShotClock>();

			GameObject go = GameObject.Find("LobbyPlayerGT");
			if(go)
			{
				playersInRoomLbl = go.guiText;
			}
		}
		public void OnEnable()
		{
			BowlingKit.BaseGameManager.onConnect 	+=connect;
			BowlingKit.BaseGameManager.onDisconnect += disconnect;

		}
		public void OnDisable()
		{
			BowlingKit.BaseGameManager.onConnect 	-=connect;
			BowlingKit.BaseGameManager.onDisconnect -= disconnect;
		}

		//lets handle connect. Do we want to load offline mode, or connect to photon.
		void connect(bool offlineMode, 
		             int levelToLoad,
		             int nomHumans,
		             int nomAI)
		{
			m_aiToSpawn = nomAI;
			m_nomHumans = nomHumans;
			m_levelToLoad = levelToLoad;

#if PHOTON
			if(offlineMode)
			{
				handleOfflineMode();
			}else{
				handleConnect();
			}		
#else
			PlayerPrefs.SetInt("AisToSpawn",m_aiToSpawn);
			PlayerPrefs.SetInt("HumansToSpawn",m_nomHumans);

			Application.LoadLevel(levelToLoad);
#endif
		}


		void handleOfflineMode()
		{
			m_playersInRoom=1;

#if PHOTON
			PhotonNetwork.offlineMode=true;

			//if we were already in a room, lets leave the room
			if(PhotonNetwork.room!=null)
			{
				PhotonNetwork.LeaveRoom();
			}
			//we can simply join a random room...
			PhotonNetwork.JoinRandomRoom();
#endif
		}

		void handleConnect()
		{
#if PHOTON
			m_playersInRoom = MAX_PLAYERS;
			if(PhotonNetwork.connected==false)
			{
				if(playersInRoomLbl)
				{
					playersInRoomLbl.text = "Connecting to Photon";
				}
				PhotonNetwork.ConnectUsingSettings( "1.0" );
				Debug.Log ("Connect to photon");
			}else{
				if(playersInRoomLbl)
				{
					playersInRoomLbl.text = "Connected to Photon";
				}

			}
#endif
		}

		//we need to join a room, but we only care if we are in the offline mode.
		void OnJoinedRoom()
		{
			#if PHOTON

			if(PhotonNetwork.offlineMode)
			{
//				Debug.Log ("lets roll!");
				if(PhotonNetwork.isMasterClient)
				{
					m_photonView.RPC("loadScene",PhotonTargets.All,m_levelToLoad);
				}
			}
#endif
		}


		//lets disconnect from photon so we got a free spot.
		void disconnect(){
			#if PHOTON

			if(playersInRoomLbl)
			{
				playersInRoomLbl.text="";
			}
			PhotonNetwork.Disconnect();
#endif
		}

		//we joined the lobby, lets join a random room 
		void OnJoinedLobby()
		{
#if PHOTON
			if( Application.loadedLevel==0)
			{
				PhotonNetwork.JoinRandomRoom();
			}
#endif
		}

		//couldnt join the room, lets try making a room.
		void OnPhotonRandomJoinFailed()
		{
#if PHOTON

				PhotonNetwork.CreateRoom( null, true,true,m_playersInRoom);
				Debug.Log ("OnCreatedRoom");
#endif
		}
		public void Update()
		{
			#if PHOTON
			if(playersInRoomLbl && PhotonNetwork.room!=null)
			{
				if(Application.loadedLevel==0)
				{
					playersInRoomLbl.text = "Room: " + PhotonNetwork.room.playerCount +  " / " + PhotonNetwork.room.maxPlayers;
				}else{
					playersInRoomLbl.text = "";
				}
			}
			//we got max players in our room. Let's tell them to start the game using an RPC call.
			//make sure we are actually in the main menu.
			if(PhotonNetwork.room!=null && PhotonNetwork.room.playerCount==PhotonNetwork.room.maxPlayers)
			{
				if(Application.loadedLevel==0)
				{
					Debug.Log ("lets roll!");
					if(PhotonNetwork.isMasterClient)
					{
						Debug.Log ("load the scene");
						m_photonView.RPC("loadScene",PhotonTargets.All,m_levelToLoad);
					}
				}
			}
			#endif
		}

		#if PHOTON
		//lets load the game
		[RPC]
		void loadScene(int lvl)
		{
			PhotonNetwork.isMessageQueueRunning = false;
			Application.LoadLevel( lvl );
		}
		#endif

		#if PHOTON

		//we loaded the level, lets turn the queue back on.
		void OnLevelWasLoaded( int level )
		{
			PhotonNetwork.isMessageQueueRunning = true;

			//we went back to the main menu -- lets disconnect.
			if(level==0)
			{
				playersInRoomLbl.text="";

				if(m_shotClock)
				{
					m_shotClock.stop();
				}

				PhotonNetwork.Disconnect();

			}

			if(Application.loadedLevel>0)
			{
				playersInRoomLbl.text="";
				if(PhotonNetwork.connected && PhotonNetwork.room!=null)
				{
					if(PhotonNetwork.isMasterClient)
					{

						BowlingKit.BaseGameManager.spawnPlayers(m_nomHumans,m_aiToSpawn);
						//PhotonNetwork.Instantiate("Pins",Vector3.zero,Quaternion.identity,0);
						//PhotonNetwork.Instantiate("Balls",Vector3.zero,Quaternion.identity,0);

					}
				}
			}
		}
#endif

		#if PHOTON
		//someone else got discconnected from photon!
		public void OnPhotonPlayerDisconnected(PhotonPlayer player)
		{

			//if our opponent was disconnected lets simply kick us back to the main menu.
			if(Application.loadedLevel>0)
			{
				if(player.ID != PhotonNetwork.player.ID)
				{
					PhotonNetwork.LeaveRoom();

					Application.LoadLevel( 0 );
				}
			}
		}
		#endif

		//we got disconnected from photon. Let's leave the game.
		void OnDisconnectedFromPhoton()
		{
			if( Application.loadedLevel!=0 )
			{
				Application.LoadLevel( 0 );
			}
		}
	}
}