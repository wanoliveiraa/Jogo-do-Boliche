using UnityEngine;
using System.Collections;
namespace BowlingKit
{
	//our simple script that will fade the screen out over time
	public class FadeOutTexture : MonoBehaviour {


			
			public void OnEnable()
			{
				BaseGameManager.onFadeOut += onFadeOut;
				BaseGameManager.onFadeIn += onFadeIn;

			}
			public void OnDisable()
			{
				BaseGameManager.onFadeIn -= onFadeIn;
				BaseGameManager.onFadeOut -= onFadeOut;
			}


			public void onFadeIn()
			{
				iTween.ColorTo(gameObject, iTween.Hash("time",.5f,"a",0f));
			}
			public void onFadeOut()
			{
				iTween.ColorTo(gameObject, iTween.Hash("time",.5f,"a",1f,"delay",1f));
			}
			

	}
}