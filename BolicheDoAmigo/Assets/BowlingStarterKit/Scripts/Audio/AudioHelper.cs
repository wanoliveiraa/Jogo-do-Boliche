using UnityEngine;
using System.Collections;
namespace BowlingKit
{
	public class AudioHelper : MonoBehaviour {

		//called when the pin is knocked down
		public AudioClip[] onPinKnockDownAC;

		//called when therer is a gutter ball
		public AudioClip onGutterAC;

		//the audio clip to play when there is a strike
		public AudioClip onStrikeAC;

		//the audio clip to play when there is a spare.
		public AudioClip onSpareAC;

		//the audio clip to play when the ball is fired.
		public AudioClip onFireBallAC;

		//the audio clip to play when the pin is knocked down.
		public AudioClip onKnockDownAC;



		public void OnEnable()
		{
			BaseGameManager.onPinKnockDown 		+= onPinKnockDown;
			BaseGameManager.onGutterUI 			+= onGutter;
			BaseGameManager.onStrike 			+= onStrike;
			BaseGameManager.onSpare 			+= onSpare;
			BaseGameManager.onPinDown 			+= onPinDown;
			BaseGameManager.onFireBall 			+= onFireBall;

		}
		public void OnDisable()
		{
			BaseGameManager.onPinKnockDown  -= onPinKnockDown;
			BaseGameManager.onGutterUI 		-= onGutter;
			BaseGameManager.onStrike 		-= onStrike;
			BaseGameManager.onSpare 		-= onSpare;
			BaseGameManager.onPinDown 		-= onPinDown;
			BaseGameManager.onFireBall 		-= onFireBall;


		}
		public void onPinKnockDown(int onScore)
		{
			if(audio && onPinKnockDownAC!=null && 
			   onScore > -1 && onScore < onPinKnockDownAC.Length)
			{
				audio.PlayOneShot( onPinKnockDownAC[onScore]);
			}
		}
		public void onFireBall()
		{
			if(audio)
			{
				audio.PlayOneShot( onFireBallAC );
			}
			
		}
		public void onPinDown()
		{
			if(audio)
			{
				audio.PlayOneShot( onKnockDownAC );
			}
			
		}


		public void onStrike()
		{
			if(audio)
			{
				audio.PlayOneShot( onStrikeAC );
			}

		}
		public void onGutter()
		{
			if(audio)
			{
				audio.PlayOneShot( onGutterAC );
			}
			
		}

		public void onSpare()
		{
			if(audio)
			{
				audio.PlayOneShot( onSpareAC);
			}
		}
	}
}