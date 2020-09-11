using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;
using System.Collections.Generic;

namespace UnityStandardAssets.Characters.FirstPerson
{
    [RequireComponent(typeof (CharacterController))]
    public class FirstPersonController : MonoBehaviour
    {
        public bool isWalking;
        public float walkSpeed;
		public float runSpeed;
        [SerializeField] [Range(0f, 1f)] private float m_RunstepLenghten;
		public float jumpSpeed;
        public float stickToGroundForce;
        public float gravityMultiplier;
		public MouseLook mouseLook;
        [SerializeField] private float m_StepInterval;
		[SerializeField] private Transform m_Camera;

        private bool m_Jump;
        private float m_YRotation;
        private Vector2 inputDirection;
        public Vector3 moveDir = Vector3.zero;
		public CharacterController characterController;
        private CollisionFlags m_CollisionFlags;
        private bool m_PreviouslyGrounded;
        private Vector3 m_OriginalCameraPosition;
        private float stepDistance;
        private float m_NextStep;
        private bool m_Jumping;

		public bool enableMouseLook = true;
		public float targetHeight = 2.0f;
		public float crouchHeight = 0.9f;
		public float crouchSpeed = 1.0f;
		public bool holdCrouch = true;
		public float inAirMoveMultiplier = 0.3f;
		public bool inertionInAir = true;
		public bool enableLogic = true;

		private bool isCrouching = false;
		private bool isEnteredCrouching = false;
		private float previousY = 0.0f;
		private float startHeight = 0.0f;

		public bool isRunning = false;

		public float stepSpeedMultiplier = 1.0f;
		public float stepDistanceToPlaySound = 1.0f;
		public float stepDistanceToPlaySoundOnRun = 2.0f;
		public float stepVolume = 1.0f;

		public float turningRotationCheckValue = 0.05f;
		public float turningRotationCheckValueMax = 0.3f;
		public float turningTimer = 0.0f;
		public float turningDelay = 1.0f;
		public List<string> turningSoundNames = new List<string>();

		public float physicsPushPower = 1.0f;

        // Use this for initialization
        private void Start()
        {
            characterController = GetComponent<CharacterController>();
			startHeight = characterController.height;
            //m_Camera = Camera.main;
            m_OriginalCameraPosition = m_Camera.transform.localPosition;
            //m_FovKick.Setup(m_Camera);
            //m_HeadBob.Setup(m_Camera, m_StepInterval);
            stepDistance = 0f;
            m_NextStep = stepDistance/2f;
            m_Jumping = false;
            //m_AudioSource = GetComponent<AudioSource>();
			mouseLook.Init(transform , m_Camera.transform);
			mouseLook.SetCursorLock (true);
        }

		private Quaternion previousRotation = Quaternion.identity;
		private void TurningSoundPlay() {
			if (turningTimer > turningDelay) {
				float turningValue = Mathf.Abs(previousRotation.y - transform.rotation.y);
				if (turningValue > turningRotationCheckValue && turningValue < turningRotationCheckValueMax) {
					int index = Random.Range (0, turningSoundNames.Count);
					SoundObjectData data = SoundManager.instance.GetBasicSoundObjectData (turningSoundNames[index]);
					data.spatialBlend = 0.0f;
					SoundManager.instance.CreateSound (data, Vector3.zero, transform);
					turningTimer = 0.0f;
				}
			} else {
				previousRotation = transform.rotation;
				return;
			}
		}


        // Update is called once per frame
        private void Update()
        {
			if (enableMouseLook) {
				RotateView ();
				TurningSoundPlay ();
			}
			turningTimer += Time.deltaTime;

            // the jump state needs to read here to make sure it is not missed
            if (!m_Jump)
            {
                m_Jump = InputManager.GetButtonDown("PlayerJump");
            }

			//

			previousY = characterController.transform.position.y - characterController.height / 2 - characterController.skinWidth;

			if (holdCrouch) {
				if (InputManager.GetButtonDown ("PlayerCrouch") == true) {
					if (isCrouching == false) {
						isCrouching = true;
						targetHeight = crouchHeight;
					} else {
						float radius = characterController.radius;
						float height = characterController.height;
						bool isHitToOpaque = Physics.CheckBox (
							m_Camera.position + new Vector3 (0.0f, height + radius, 0.0f), 
							new Vector3 (radius, radius * 2.0f, radius)
						);
						if (!isHitToOpaque) {
							targetHeight = startHeight;
						} else {
							isCrouching = true;
						}
					}
				}
			} else {
				//isCrouching = InputManager.GetButton ("PlayerCrouch");
				bool isPressedCrouching = InputManager.GetButton("PlayerCrouch");

				if (isPressedCrouching) {
					isEnteredCrouching = true;
				}

				if (isCrouching && !isPressedCrouching) {
					float radius = characterController.radius;
					float height = characterController.height;
					bool isHitToOpaque = Physics.CheckBox (
						m_Camera.position + new Vector3 (0.0f, height + radius, 0.0f), 
						new Vector3 (radius, radius * 2.0f, radius)
					);
					if (!isHitToOpaque) {
						targetHeight = startHeight;
						isCrouching = false;
					} else {
						isCrouching = true;
					}
				} else if(isPressedCrouching) {
					targetHeight = crouchHeight;
					isCrouching = true;
				} 
			}

			characterController.height = Mathf.Lerp (characterController.height, targetHeight, 5.0f * Time.deltaTime);

			m_Camera.transform.position = Vector3.Lerp (
				m_Camera.transform.position, 
				new Vector3 (
					m_Camera.transform.position.x, 
					characterController.transform.position.y + targetHeight / 2.0f - 0.1f,
					m_Camera.transform.position.z
				),
				5.0f * Time.deltaTime
			);

			characterController.transform.position = Vector3.Lerp (
				characterController.transform.position,
				new Vector3 (
					characterController.transform.position.x,
					previousY + targetHeight / 2.0f + characterController.skinWidth,
					characterController.transform.position.z
				),
				5.0f * Time.deltaTime
			);
        }


        private void PlayLandingSound()
        {
            //m_AudioSource.clip = m_LandSound;
            //m_AudioSource.Play();
            //m_NextStep = stepDistance + .5f;
			PlayFootStepAudio (true);
        }

		private float previousSpeed = 0.0f;

		private Vector2 moveDirection = new Vector2();

        private void FixedUpdate()
        {
			if (!enableLogic) {
				return;
			}

			//

			if (!m_PreviouslyGrounded && characterController.isGrounded)
			{
				PlayLandingSound();
				moveDir.y = 0f;
				m_Jumping = false;
				Player.instance.character.FallDamage (previousSpeed);
			}
			if (!characterController.isGrounded && !m_Jumping && m_PreviouslyGrounded)
			{
				moveDir.y = 0f;
			}

			m_PreviouslyGrounded = characterController.isGrounded;
			previousSpeed = Math.Abs(characterController.velocity.y);

			float speed;
			GetInput(out speed);
			// always move along the camera forward as it is the direction that it being aimed at
			Vector3 desiredMove = transform.forward * inputDirection.y + transform.right * inputDirection.x;

			// get a normal for the surface that is being touched to move along it
			RaycastHit hitInfo;
			Physics.SphereCast(transform.position, characterController.radius, Vector3.down, out hitInfo,
				characterController.height/2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
			desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

			moveDir.x = desiredMove.x * speed;
			//moveDir.y = desiredMove.y * speed;
			moveDir.z = desiredMove.z * speed;

			if (characterController.isGrounded && inertionInAir)
			{
				moveDir.y = -stickToGroundForce;

				if (m_Jump)
				{
					moveDir.y = jumpSpeed;
					PlayJumpSound();
					m_Jump = false;
					m_Jumping = true;
				}
			}
			else
			{
				moveDir += Physics.gravity * gravityMultiplier * Time.fixedDeltaTime;
				moveDir.x = characterController.velocity.x + (desiredMove.x * inAirMoveMultiplier);
				moveDir.z = characterController.velocity.z + (desiredMove.z * inAirMoveMultiplier);
				if (isCrouching) {
					moveDir.x -= (desiredMove.x * inAirMoveMultiplier) / 3.0f;
					moveDir.z -= (desiredMove.z * inAirMoveMultiplier) / 3.0f;
				}
			}

			if (moveDir.x + moveDir.z == 0.0f) {
				moveDirection = Vector2.Lerp (moveDirection, Vector2.zero, Time.fixedDeltaTime * 6.0f);
			} else {
				moveDirection = Vector2.Lerp (moveDirection, new Vector2 (moveDir.x, moveDir.z), Time.fixedDeltaTime * 10.0f);
			}
			m_CollisionFlags = characterController.Move (new Vector3 (moveDirection.x, moveDir.y, moveDirection.y) * Time.fixedDeltaTime);

			ProgressStepCycle(speed);

			mouseLook.UpdateCursorLock ();
        }


        private void PlayJumpSound()
        {
            //m_AudioSource.clip = m_JumpSound;
            //m_AudioSource.Play();
			PlayFootStepAudio(true);
        }


        private void ProgressStepCycle(float speed)
        {
			if (characterController.velocity.sqrMagnitude > 0 && (inputDirection.x != 0 || inputDirection.y != 0))
            {
				stepDistance += (speed * stepSpeedMultiplier) * Time.fixedDeltaTime;
            }

			if (stepDistance > stepDistanceToPlaySound && isWalking) {
				PlayFootStepAudio ();
				stepDistance = 0.0f;
			} else if(stepDistance > stepDistanceToPlaySoundOnRun && isRunning) {
				PlayFootStepAudio ();
				stepDistance = 0.0f;
			}
        }


		private void PlayFootStepAudio(bool landing = false)
        {
            if (!characterController.isGrounded)
            {
                return;
            }

			//if(m_FootstepSounds.GetLength(0) > 0) {
	            // pick & play a random footstep sound from the array,
	            // excluding sound at index 0
	        //    int n = Random.Range(1, m_FootstepSounds.Length);
	       //     m_AudioSource.clip = m_FootstepSounds[n];
	       //     m_AudioSource.PlayOneShot(m_AudioSource.clip);
	            // move picked sound to index 0 so it's not picked next time
	        //    m_FootstepSounds[n] = m_FootstepSounds[0];
	        //    m_FootstepSounds[0] = m_AudioSource.clip;
			//}

			RaycastHit[] hits = Physics.RaycastAll (transform.position + new Vector3(0.0f, 0.5f, 0.0f), Vector3.down, 2.4f);
			RaycastHit neededHit = new RaycastHit ();
			bool neededHitFound = false;
			foreach (RaycastHit hit in hits) {
				if (hit.transform.root.tag != "Player") {
					if (hit.transform.GetComponent<Renderer> () == null) {
						continue;
					}
					if (hit.triangleIndex == -1) {
						continue;
					}
					neededHit = hit;
					neededHitFound = true;
					break;
				}
			}
			if (neededHitFound) {
				Material material = MaterialManager.instance.GetMaterialFromRaycast (neededHit);
				if (material == null) {
					return;
				} 
				SoundMaterialType type = SoundManager.instance.GetSoundMaterialType (material);
				if (type != null) {
					List<string> clips;
					if (landing) {
						clips = type.landingClipNames;
					} else if (isRunning) {
						clips = type.runClipNames;
					} else {
						clips = type.walkClipNames;
					}
					string clip = clips [Random.Range (0, clips.Count)];
					SoundObjectData data = SoundManager.instance.GetBasicSoundObjectData (clip);
					data.volume = stepVolume;
					data.spatialBlend = 0.0f;
					SoundManager.instance.CreateSound (data, Vector3.zero, transform);
				}
			}
        }

        private void GetInput(out float speed)
        {
            // Read input
			float horizontal = 0.0f; 

			if (InputManager.GetButton ("PlayerLeft")) {
				horizontal -= 1.0f;
			}

			if (InputManager.GetButton ("PlayerRight")) {
				horizontal += 1.0f;
			}

			float vertical = 0.0f;

			if (InputManager.GetButton ("PlayerBackward")) {
				vertical -= 1.0f;
			}

			if (InputManager.GetButton ("PlayerForward")) {
				vertical += 1.0f;
			}

            bool waswalking = isWalking;

            isWalking = !InputManager.GetButton ("PlayerRun");

			isRunning = !isWalking;

            speed = isWalking ? walkSpeed : runSpeed;
			speed = isCrouching ? crouchSpeed : speed;
            inputDirection = new Vector2(horizontal, vertical);

            // normalize input if it exceeds 1 in combined length:
            if (inputDirection.sqrMagnitude > 1) {
                inputDirection.Normalize();
            }
        }


        private void RotateView()
        {
            mouseLook.LookRotation (transform, m_Camera.transform);
        }


        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
			if (!enableLogic) {
				return;
			}
            Rigidbody body = hit.collider.attachedRigidbody;
            //dont move the rigidbody if the character is on top of it
            if (m_CollisionFlags == CollisionFlags.Below)
            {
                return;
            }

            if (body == null || body.isKinematic)
            {
                return;
            }
			body.AddForceAtPosition(characterController.velocity * physicsPushPower, hit.point, ForceMode.Impulse);
        }
    }
}
