using UnityEngine;
using System.Collections;

namespace BowlingKit
{
	//this is our flick script that allows to detect a flick.
	public class FlickScript : MonoBehaviour {
		
		#region varaibles
		//the comfort zone - if its not at least this much disregauard it.
		public float comfortZone = 0.1f;
		
		//the start touch
	    private Vector3 m_touchStart; 
		//the end touch
	    private Vector3 m_touchEnd;
		
		//the startTime
		private float m_startTime;
		
		//could it be a swipe.
		private bool m_couldbeswipe=false;
	#endregion

		void Update () {
			if(Application.platform == RuntimePlatform.Android || 
				Application.platform == RuntimePlatform.IPhonePlayer)
			{
				handleMobileInput();
			}else{
				handleNonMobileInput();
			}
			
		}
		void handleMobileInput()
		{
	        if (Input.touchCount > 0)
	        {
	            Touch t = Input.touches[0];
				Vector2 touch = t.position;
	            if (t.phase == TouchPhase.Began)
	            {
		            m_touchStart = Input.mousePosition;
					m_couldbeswipe=true;
					m_startTime = Time.timeSinceLevelLoad;
	            }
				if(t.phase == TouchPhase.Moved)
				{
	;
				    if (Mathf.Abs(touch.y - m_touchStart.y) < comfortZone)
					{
						m_couldbeswipe = false;
					}else{
						m_couldbeswipe = true;
					}
				}
				if(t.phase == TouchPhase.Stationary)
				{
					m_couldbeswipe=false;
				}
				
	            if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled)
	            {
					if(m_couldbeswipe)
					{
			            m_touchEnd.y = touch.y;
						
			            m_touchEnd.x = touch.x;
		    	        handleSwipe();
					}
	            }
	        }
		}
		void handleNonMobileInput()
		{
	        if (Input.GetButtonDown("Fire1"))
	        {
	            m_touchStart = Input.mousePosition;
				m_couldbeswipe=true;
				m_startTime = Time.timeSinceLevelLoad;
	        }
			if( Input.GetButton("Fire1"))
			{
				Vector3 touch = Input.mousePosition;
				Vector3 touchDelta = touch - m_touchStart;
				if(touchDelta != Vector3.zero)
				{
				    if (Mathf.Abs(touch.y - m_touchStart.y) < comfortZone)
					{
						m_couldbeswipe = false;
					}else{
						m_couldbeswipe = true;
					}
				}else{
				    if (Mathf.Abs(touch.y - m_touchStart.y) < comfortZone)
					{
						m_couldbeswipe = false;
					}
				}
			}

	        if (Input.GetButtonUp("Fire1"))
	        {
				if(m_couldbeswipe)
				{
		            m_touchEnd = Input.mousePosition;
	    	        handleSwipe();
				}
	        }
	    }


	    void handleSwipe()
	    {
			float touchDelta = Mathf.Abs(m_touchEnd.y - m_touchStart.y);
			if(touchDelta > comfortZone)
			{
				float time = Time.timeSinceLevelLoad -m_startTime;
				BaseGameManager.flick( m_touchStart, m_touchEnd, time ); 
			}

	    }

	}
}