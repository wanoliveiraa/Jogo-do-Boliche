using UnityEngine;
using System.Collections;

//#define PHOTON

namespace BowlingKit
{
	public class NetworkSpawnPlayer : MonoBehaviour 
	{
		public Vector3 spawnLoc = new Vector3(0,0,-0.21f);

#if PHOTON
		private PhotonView m_view;
#endif
		void Start()
		{
			#if PHOTON
				m_view = gameObject.GetComponent<PhotonView>();
				spawnPlayer(PhotonNetwork.player.ID);
			#else
				int nomAIs = PlayerPrefs.GetInt("AisToSpawn");
				int nomHumans = PlayerPrefs.GetInt("HumansToSpawn");
				spawnPlayer(1);
				onSpawnPlayers(nomHumans,nomAIs);
			#endif
		}
		public void OnEnable()
		{
			BowlingKit.BaseGameManager.onSpawnPlayers += onSpawnPlayers;
		}
		
		public void OnDisable()
		{
			BowlingKit.BaseGameManager.onSpawnPlayers -= onSpawnPlayers;
		}

		void updateNomPlayers()
		{
			#if PHOTON

				if(m_view)
				{
					m_view.RPC("updateNomPlayersRPC",PhotonTargets.All);
				}
			#else
				_updateNomPlayersRPC();

			#endif
		}
		[RPC]
		void updateNomPlayersRPC() 
		{
			_updateNomPlayersRPC();
		}
		void _updateNomPlayersRPC() 
		{
			BowlingKit.BasePlayer[] players = (BowlingKit.BasePlayer[])GameObject.FindObjectsOfType(typeof(BowlingKit.BasePlayer));
			BowlingKit.GameScript bk = (BowlingKit.GameScript)GameObject.FindObjectOfType(typeof(BowlingKit.GameScript));
			if(bk)
			{
				bk.nomPlayers = players.Length;
			}
			BowlingKit.BaseGameManager.startGame();
		}

		void spawnPlayer(int playerID)
		{
			string objectToSpawn = "HumanPlayer" + playerID;
			//lets spawn our players here.

			spawnObject(objectToSpawn,spawnLoc);
			updateNomPlayers();
		}
		void spawnObject(string str,Vector3 pos)
		{
			#if PHOTON
				PhotonNetwork.Instantiate(str,pos,Quaternion.identity,0);
			#else
				Instantiate(Resources.Load(str) as GameObject,pos,Quaternion.identity);

			#endif
		}
		//spawn the objects owned by the master client.
		public void onSpawnPlayers(int localHumansToSpawn,
		                           int nomAI)
		{
#if PHOTON
			if(PhotonNetwork.isMasterClient)
			{
#endif
				//the humans will as well.
				for(int i=1; i<localHumansToSpawn; i++)
				{
					spawnPlayer(i+1);
				}

				//the ai will be owned by the master client
				for(int i=0; i<nomAI; i++)
				{
					spawnAI(i);
				}
				string str = "Pins"+Application.loadedLevel;
				spawnObject(str,Vector3.zero);

#if PHOTON
			}
#endif
		}

		void spawnAI (int i) 
		{
			//set our ai difficulty based on what level they got selected.
			string objectToSpawn = "AIPlayer" + PlayerPrefs.GetInt("AIDifficultyX",1);
			//lets spawn our players here.
			spawnObject(objectToSpawn,spawnLoc);

			updateNomPlayers();
		}




	}
}
