using UnityEngine;
using System.Collections;
namespace BowlingKit
{
	//the bowling ball has gone into the bowling pit triggerbox box.
	public class BowlingPitTrigger : MonoBehaviour 
	{
		public Color gizmoColor = new Color(1,0,0,0.5f);

		public string triggerID = "bowlingPit";
		void OnDrawGizmos() {
			// Draw a yellow sphere at the transform's position
			Gizmos.color = gizmoColor;
			Gizmos.DrawCube (transform.position, transform.localScale);
		}

		//our ball has landed in the water, lets call the ball bowling pit
		void OnTriggerEnter(Collider col)
		{

			if(col.name.Contains("Ball"))
			{	
//				Debug.Log ("BowlingPitTrigger" + triggerID + 
//				           " col "  + col.name);
				BaseGameManager.ballHitBowlingPit(triggerID);
			}
		}
	}
}