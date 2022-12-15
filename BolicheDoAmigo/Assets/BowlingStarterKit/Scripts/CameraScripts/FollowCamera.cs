using UnityEngine;
using System.Collections;
namespace BowlingKit
{
	public class FollowCamera : MonoBehaviour {

		//a reference to the ball transform
		private Transform m_ballTransform;

		//the maximum z position 
		public float maxZ = 18f;

		//the lerp scalar for moving between the current position and the previous position.
		public float lerpTime = 5;


		public void OnEnable()
		{
			BaseGameManager.onSetActiveBall += onSetActiveBall;
		}
		public void OnDisable()
		{
			BaseGameManager.onSetActiveBall -= onSetActiveBall;
		}
		public void onSetActiveBall(Transform b)
		{
			m_ballTransform = b;
		}
		// Update is called once per frame
		void LateUpdate () {
			lerpTowardsBallTransform();

		}
		void lerpTowardsBallTransform()
		{
			//if we do not have a ball transform quit this function.
			if(m_ballTransform==null)	
			{
				return;
			}


			Vector3  newPosition = m_ballTransform.position;
			//zero out the y, and x component, so this transform will simply follow the cameras z axius up to the max z -- where it will stop.
			newPosition.y=0;
			newPosition.x=0;

			if(newPosition.z > maxZ)
			{
				newPosition.z = maxZ;
			}

			//lerp between the current position and the new position
			transform.position = Vector3.Lerp(transform.position,newPosition,Time.smoothDeltaTime * lerpTime);

		}
	}
}
