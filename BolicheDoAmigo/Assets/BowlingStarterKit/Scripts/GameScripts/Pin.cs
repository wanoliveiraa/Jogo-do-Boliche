using UnityEngine;
using System.Collections;

//#define PHOTON

namespace BowlingKit
{
	public class Pin : MonoBehaviour {
		//the inital position of the pin
		private Vector3 m_initalPos;

		//the inital rotation of the pin
		private Quaternion m_initalRot;

		//a reference to the pins rigidbody.
		private Rigidbody m_rigidBody;

		//is the pin down.
		private bool m_isDown = false;

		//when we are looking to see if a pin got knocked over we are going to see the difference between the current position and the inital position, if the change is greater then this value
		//we will consider the pin to be knocked over.
		public float deltaChange = .25f;

		//the wobble counter
		private int m_pinWobbleCounter = 0;

		//the times we want to wobble before considered knocked down
		public int pinWobbleFrames = 3;

		//the zrotation angle to look for
		public float pinWobbleRotZ = 0.26f;

		//the time before the pin is raised
		public float delayTime = 0.5f;

#if PHOTON
		//a ref to the photon view.
		private PhotonView m_view;
#endif

		public void Awake()
		{
			m_initalPos = transform.position;
			m_initalRot = transform.rotation;
			m_rigidBody = gameObject.GetComponent<Rigidbody>();

#if PHOTON
			m_view = gameObject.GetComponent<PhotonView>();
#endif
		}
		public void OnEnable()
		{
			BaseGameManager.onResetPin += onResetPin;
			BaseGameManager.onSetPins += onClearPins;
			BaseGameManager.onRemovePin += onRemovePin;
		}
		public void OnDisable()
		{
			BaseGameManager.onResetPin -= onResetPin;
			BaseGameManager.onSetPins -= onClearPins;
			BaseGameManager.onRemovePin -= onRemovePin;
		}

		//only clear the pins if we are the master client...
		void onClearPins(bool pinsUp)
		{
			if(m_isDown==false)
			{
#if PHOTON
				if(m_view)
				{
					if(PhotonNetwork.isMasterClient)
					{
						m_view.RPC ("liftPinsRPC",PhotonTargets.All,pinsUp);
					}
				}else{
					handlePins(pinsUp);

				}
#else
				handlePins(pinsUp);

#endif
			}
		}
		[RPC]
		void liftPinsRPC(bool pinsUp){
			handlePins(pinsUp);
		}
		void handlePins(bool pinsUp)
		{
			if(pinsUp)
			{
				liftPins();
			}else{
				//lowerPins();
			}
		}
		void lowerPins()
		{
			transform.rotation = m_initalRot;
			m_rigidBody.isKinematic=true;
			Vector3 pos = m_initalPos;
			pos.y = transform.position.y;
			transform.position = pos;
			iTween.MoveTo(gameObject,iTween.Hash("y",m_initalPos.y,"time",2f,
			                                     "oncompletetarget",gameObject,
			                                     "oncomplete","finishedLowering"));
		}
		void finishedLowering()
		{
			if(m_rigidBody)
			{
				m_rigidBody.isKinematic=false;
			}
			transform.position = m_initalPos;

		}
		void liftPins()
		{
			rigidbody.isKinematic=true;
			iTween.MoveTo(gameObject,iTween.Hash("y",m_initalPos.y+1f,"time",2f,"delay",delayTime*2f));
		}


		//every frame look to see if the pins has rotated too much or moved too much to be considered "knocked down"
		public void Update()
		{
			//if we are either the master client or we are in the menu scene -- scene0.

#if PHOTON
			if(PhotonNetwork.isMasterClient || Application.loadedLevel==0)
			{
				//we arent kinematic pin state.
				if(m_rigidBody.isKinematic==false)
				{
					updatePin();
				}
			}
#else
			if(m_rigidBody.isKinematic==false)
			{
				updatePin();
			}
#endif
		}

		void updatePin()
		{
			float d0 = (transform.position - m_initalPos).magnitude;

			bool tilted = false;
			//we rotated too much.
			if(transform.rotation.z > pinWobbleRotZ && m_isDown ==false )
			{

				if(m_pinWobbleCounter>pinWobbleFrames)
				{
					BaseGameManager.pinDown();

					//audio.PlayOneShot(bawlSound);
					m_isDown = true;
				}
				m_pinWobbleCounter++;

			}

			//if the pin has moved more then the delta change we will consider that pin to be knocked over.
			if((d0 > deltaChange || tilted) && m_isDown==false)
			{
				BaseGameManager.pinDown();
				m_isDown = true;
			}
		}

		//we want to remove the pin
		public void onRemovePin()
		{
			if(m_isDown)
			{
				//lets move the pin really far away so that it wont get in the way.
				Vector3 pos = transform.position;
				pos.z = 5000f;
				transform.position = pos;
				m_isDown = true;
				m_pinWobbleCounter=0;

			}else{
				//this pin wasnt knocked down lets simply reset it
				transform.position = m_initalPos;
				transform.rotation = m_initalRot;
				m_pinWobbleCounter=0;
				if(m_rigidBody)
				{
					m_rigidBody.isKinematic=false;
					m_rigidBody.velocity = Vector3.zero;
					m_rigidBody.angularVelocity = Vector3.zero;
				}
			}
		}

		//lets reset the pin, setting its position and rotation to the inital positions and rotation, set the angular and velcoity to zero, and tell it not to be considered knocked over.
		public void onResetPin()
		{
			transform.position = m_initalPos;
			transform.rotation = m_initalRot;
			m_pinWobbleCounter=0;

			m_isDown=false;
			if(m_rigidBody)
			{
				m_rigidBody.isKinematic=false;
				m_rigidBody.velocity = Vector3.zero;
				m_rigidBody.angularVelocity = Vector3.zero;
			}
		}
	}
}