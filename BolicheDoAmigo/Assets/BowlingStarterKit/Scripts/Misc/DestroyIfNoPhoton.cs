using UnityEngine;
using System.Collections;
//#define PHOTON

public class DestroyIfNoPhoton : MonoBehaviour {

	// Use this for initialization
	void Start () {
#if PHOTON
#else
		Destroy(gameObject);
#endif
	}
}
