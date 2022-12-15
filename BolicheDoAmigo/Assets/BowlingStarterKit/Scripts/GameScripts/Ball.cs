using UnityEngine;
using System.Collections;

//#define PHOTON
namespace BowlingKit
{
	//this is our simlpy ball class
	//ideally we would want to run the simulation on its own client, so whenever the player fired the ball we would simply run it on the master.
	public class Ball : MonoBehaviour
	{
		//a reference to the balls rigidbody
		private Rigidbody m_rigidBody;

		//a reference to the balls inital position.
		private Vector3 m_initalPos;

		public Vector3 angVel = new Vector3(0,5,0);

		//the minimum and maximum gutter positions.
		public Vector2 gutterPositions = new Vector2(-0.65f,0.65f);

		//has the ball been fired.
		private bool m_fired=false;

		//the inital rotation
		private Quaternion m_initalRot;

		//the pointer gameobject, lets hide it when the ball is fired and show it when we reset the ball
		public GameObject pointerGO;

		//the force mode we want to use when "firing the ball".
		public ForceMode forceMode;

		Vector3 screenPoint;
		Vector3 offset;
		public float dragThreshold = 20;
		private bool m_mouseDown = false;
		private Vector3 m_prevLoc;
#if PHOTON
		private PhotonView m_view;
#endif
		void Start () 
		{
			m_rigidBody = gameObject.GetComponent<Rigidbody>();
			m_initalPos = transform.position;
			m_initalRot = transform.rotation;

#if PHOTON
			m_view = gameObject.GetComponent<PhotonView>();
#endif

			if(pointerGO)
				pointerGO.SetActive(true);
		}
		public void OnDisable()
		{
			BaseGameManager.onBallHitBowlingPit -= onBallHitBowlingPit;
		}
		
		public void OnEnable()
		{
			BaseGameManager.onBallHitBowlingPit += onBallHitBowlingPit;
		}

		//we have landed in the gutter
		void handleGutter()
		{
			//lets zero out the angular velocity
			m_rigidBody.angularVelocity=Vector3.zero;
			
			//we only care about the z-component of the velocity, zero everything else out -- so it moves along the z-axis.
			Vector3 vel = m_rigidBody.velocity;
			vel.y=0;
			vel.x = 0;
			m_rigidBody.velocity=vel;


		}
		private float m_gutterTime = 0;
		private bool m_moveToGutterCenter = false;
		public float gutterTimeToCenter = 0.3f;
		private float m_startGutterXPos;

		public Vector3 curveForce = new Vector3(0,0,3.8f);
		void Update () {

			if(m_moveToGutterCenter)
			{
				m_gutterTime+=Time.deltaTime;
				float val = m_gutterTime / gutterTimeToCenter;

				if(val<=1)
				{
					Vector3 vec = transform.position;
					vec.x = Mathf.Lerp(m_startGutterXPos,m_gutterPos,m_gutterTime);
					transform.position =vec;
				}else{
					m_moveToGutterCenter=false;
				}
			}

		}
		public void onBallHitBowlingPit(string id)
		{
			if(id.Contains("Pit")){
				if(audio)
				{
					audio.Stop();
				}
			}
		}

		//reset the balls position.
		public void reset()
		{
			if(m_fired)
			{
				m_fired=false;
				if(pointerGO)
					pointerGO.SetActive(true);

				transform.rotation = m_initalRot;
				if(m_rigidBody){
					m_rigidBody.isKinematic=false;

					m_rigidBody.angularVelocity=Vector3.zero;
					m_rigidBody.velocity=Vector3.zero;
					m_rigidBody.isKinematic=true;
				}
				transform.position = m_initalPos;
			}
		}

		//fire the ball
		public void fireBall(Vector3 vec)
		{
#if PHOTON
			if(m_view)
			{
				m_view.RPC ("fireBallRPC",PhotonTargets.All,vec);
			}
		}else{
			_fireBall(vec);
		}

#else
		_fireBall(vec);
#endif
		}
		[RPC]
		public void fireBallRPC(Vector3 vec)
		{
//			Debug.Log ("fireBall3a");
			_fireBall(vec);
		}
		void _fireBall(Vector3 vec)
		{
			//make sure we have a rigidbody and that the time is not paused
			if(m_rigidBody && Time.timeScale!=0)
			{

				//play the ball audio clip 
				if(audio)
				{
					audio.Play();
				}

				//call the fire ball function
				BaseGameManager.fireBall();

				//hide the pointer
				if(pointerGO)
					pointerGO.SetActive(false);

				//we have fired the ball
				m_fired=true;
				m_rigidBody.isKinematic=false;
				//add a force
				m_rigidBody.AddForce(transform.rotation * vec,forceMode);
				m_rigidBody.AddTorque (angVel);

			}
		}


		//the mouse is down lets get the screen point and offset
		void OnMouseDown()
		{
			if(m_fired==false)
			{
				m_mouseDown=true;
				m_prevLoc = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);

				screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);

				//m_prevLoc = screenPoint;
				offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
			}
			
		}
		void OnMouseUp()
		{
			m_mouseDown=false;
		}

		//your dragging the ball around
		void OnMouseDrag()
		{
			if(m_fired==false && m_mouseDown)
			{
				Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
				float d1 = Mathf.Abs(m_prevLoc.y - curScreenPoint.y);

				//look dead ahead
				//transform.LookAt( new Vector3(transform.position.x,transform.position.y,18));

				//if our input is less then the drag threshold then we will move the balls position to that world position along the x-axis
				//this should sync up with the value "comfortZone" in the flickscript. If its less then this value its a drag, if its bigger its a flick.
				if(d1 < dragThreshold)
				{
					Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
					curPosition.z = transform.position.z;
					curPosition.y = transform.position.y;
					transform.position = curPosition;
				}
			}
		}


		//we landed in the gutter, its looking to ensure that the gutter actually has a tag called "Gutter".
		public void OnCollisionStay(Collision col)
		{
			if(col.gameObject.tag.Equals("Gutter"))
			{
				handleGutter();				
			}
		}
		private float m_gutterPos = 0;
		//we entered into a collision with the gutter
		public void OnCollisionEnter(Collision col)
		{
			if(col.gameObject.tag.Equals("Gutter"))
			{
				//lets call the gutter ball function
				BaseGameManager.gutterBall ();

				//rigidbody.collider.enabled=false;
				//lets zero out the angular velocity, and the velocity along the x and y axises
				handleGutter();

				//if the ball is greater then 1 move the ball "centre" of the left gutter lane
				Vector3 pos = transform.position;
				if(pos.x>0)
				{
					pos.x = gutterPositions.y;
				
				}
				//if the ball is less then 1 move the ball "centre" of the right gutter lane
				if(pos.x<0)
				{
					pos.x = gutterPositions.x;
				}
				m_startGutterXPos = transform.position.x;
				m_gutterPos = pos.x;
				m_gutterTime=0;
				//m_moveToGutterCenter = true;
				//warp the ball to this position
				transform.position =pos;
			}

		}



		
	}
}
