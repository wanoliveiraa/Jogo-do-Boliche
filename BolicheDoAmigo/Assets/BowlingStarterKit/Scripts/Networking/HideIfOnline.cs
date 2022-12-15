using UnityEngine;
using System.Collections;

namespace BowlingKit
	{
	public class HideIfOnline : MonoBehaviour {
		//deactive this if the player is not in offline mode.
		void Start () {
#if PHOTON
			if(PhotonNetwork.offlineMode==false)
			{
#endif
				gameObject.SetActive(false);
#if PHOTON
			}
#endif
		}
		

	}
}