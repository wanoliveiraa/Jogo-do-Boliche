using UnityEngine;
using System.Collections;

namespace BowlingKit
	{
	public class PausePanel : MonoBehaviour {

		public GameObject pausePanel;
		public float pauseXPos = 1.4f;
		public float unapuseXPos = 0.5f;
		void toggleAudio()
		{
			if(PlayerPrefs.GetFloat("AudioVolume",0)==0)
			{
				PlayerPrefs.SetFloat("AudioVolume",1f);
			}else{
				PlayerPrefs.SetFloat("AudioVolume",0f);
			}
			BaseGameManager.toggleAudio();
		}
		public void OnEnable()
		{
			BaseGameManager.onButtonPress += onButtonClickCBF;
		}
		public void OnDisable()
		{
			BaseGameManager.onButtonPress -= onButtonClickCBF;
		}

		void togglePause()
		{
			if(Time.timeScale==0)
			{
				Time.timeScale=1;
				iTween.MoveTo(pausePanel,iTween.Hash("x",unapuseXPos,"time",1,"ignoretimescale",true));
			}
			else if(Time.timeScale==1)
			{
				Time.timeScale=0;
				iTween.MoveTo(pausePanel,iTween.Hash("x",pauseXPos,"time",1,"ignoretimescale",true));
			}


		}
		public void onButtonClickCBF(string buttonID )
		{
			switch (buttonID)
			{
			case "AudioToggle":
				toggleAudio();
				break;
			case "Pause":
				togglePause();
				break;
			case "Restart":
				Time.timeScale=1;
				Application.LoadLevel(Application.loadedLevel);
				break;
			case "Main Menu":
				Time.timeScale=1;
				Application.LoadLevel(0);
				break;
			}
		}
	}
}