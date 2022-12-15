using UnityEngine;
using System.Collections;

namespace BowlingKit
{
	public class PinSetter : MonoBehaviour 
	{
		//a ref to the assembly 
		public Animation assembly;

		//a ref to the sweeper
		public Animation sweeper;

		public void OnEnable()
		{
			BowlingKit.BaseGameManager.onSetPins 	+= onSetPins;
			BowlingKit.BaseGameManager.onSweepPins += onSweepPins;

		}
		public void OnDisable()
		{
			BowlingKit.BaseGameManager.onSetPins 	-= onSetPins;
			BowlingKit.BaseGameManager.onSweepPins -= onSweepPins;

		}
		void onSetPins(bool pinsUp)
		{
			if(assembly)
			{
				assembly.gameObject.SetActive(true);
				assembly.Play();
			}
		}

		void onSweepPins()
		{
			if(sweeper)
			{
				sweeper.gameObject.SetActive(true);
				sweeper.Play();
			}
		}
	}
}
