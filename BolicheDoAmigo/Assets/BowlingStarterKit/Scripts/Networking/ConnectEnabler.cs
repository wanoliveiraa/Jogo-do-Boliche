using UnityEngine;
using System.Collections;
namespace BowlingKit
	{
	public class ConnectEnabler : MonoBehaviour {
		public static ConnectManager K_CONNECT_MANAGER;
		public ConnectManager connectManger;
		// Use this for initialization
		void Start () {
			if(K_CONNECT_MANAGER==null)
			{
				if(connectManger)
				{
					connectManger.gameObject.SetActive(true);
					K_CONNECT_MANAGER = connectManger;
				}
			}
		}
	}
}