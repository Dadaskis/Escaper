using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using Random = UnityEngine.Random;

namespace UnityStandardAssets.Characters.FirstPerson
{
    [RequireComponent(typeof (CharacterController))]
    [RequireComponent(typeof (AudioSource))]
    public class FirstPersonController : MonoBehaviour
    {
        [SerializeField] private bool m_IsWalking;
        [SerializeField] private float m_WalkSpeed;
        [SerializeField] private float m_RunSpeed;
        [SerializeField] [Range(0f, 1f)] private float m_RunstepLenghten;
        [SerializeField] private float m_JumpSpeed;
        [SerializeField] private float m_StickToGroundForce;
        [SerializeField] public float gravityMultiplier;
		public MouseLook mouseLook;
        [SerializeField] private bool m_UseFovKick;
        [SerializeField] private FOVKick m_FovKick = new FOVKick();
        [SerializeField] private bool m_UseHeadBob;
        [SerializeField] private CurveControlledBob m_HeadBob = new CurveControlledBob();
        [SerializeField] private LerpControlledBob m_JumpBob = new LerpControlledBob();
        [SerializeField] private float m_StepInterval;
        [SerializeField] private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
        [SerializeField] private AudioClip m_JumpSound;           // the sound played when character leaves the ground.
        [SerializeField] private AudioClip m_LandSound;           // the sound played when character touches back on ground.
		[SerializeField] private Transform m_Camera;

        private bool m_Jump;
        private float m_YRotation;
        private Vector2 m_Input;
        public Vector3 moveDir = Vector3.zero;
		public CharacterController characterController;
        private CollisionFlags m_CollisionFlags;
        private bool m_PreviouslyGrounded;
        private Vector3 m_OriginalCameraPosition;
        private float m_StepCycle;
        private float m_NextStep;
        private bool m_Jumping;
        private AudioSource m_AudioSource;

		public bool enableMouseLook = true;
		public float targetHeight = 2.0f;
		public float crouchHeight = 0.9f;
		public float crouchSpeed = 1.0f;
		public bool holdCrouch = true;
		public float inAirMoveMultiplier = 0.3f;
		public bool inertionInAir = true;
		public bool enableLogic = true;

		private bool isCrouching = false;
		private float previousY = 0.0f;
		private float startHeight = 0.0f;

        // Use this for initialization
        private void Start()
        {
            characterController = GetComponent<CharacterController>();
			startHeight = characterController.height;
            //m_Camera = Camera.main;
            m_OriginalCameraPosition = m_Camera.transform.localPosition;
            //m_FovKick.Setup(m_Camera);
            //m_HeadBob.Setup(m_Camera, m_StepInterval);
            m_StepCycle = 0f;
            m_NextStep = m_StepCycle/2f;
            m_Jumping = false;
            m_AudioSource = GetComponent<AudioSource>();
			mouseLook.Init(transform , m_Camera.transform);
			mouseLook.SetCursorLock (true);
        }


        // Update is called once per frame
        private void Update()
        {
			if (enableMouseLook) {
				RotateView ();
			}

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
						RaycastHit hit;
						bool isHitToOpaque = Physics.Raycast (m_Camera.position, Vector3.up, out hit);
						if (!isHitToOpaque || hit.distance > 1.0f) {
							isCrouching = false;
							targetHeight = startHeight;
						}
					}
				}
			} else {
				isCrouching = InputManager.GetButton ("PlayerCrouch");

				if (isCrouching == true) {
					targetHeight = crouchHeight;
				} else {
					//RaycastHit hit;
					float radius = characterController.radius;
					bool isHitToOpaque = Physics.CheckBox (m_Camera.position + new Vector3 (0.0f, radius * 2.0f, 0.0f), new Vector3 (radius, radius, radius));
					//bool isHitToOpaque = Physics.Raycast (m_Camera.position + new Vector3(0.0f, 0.2f, 0.0f), Vector3.up, out hit);
					if (!isHitToOpaque) {
						targetHeight = startHeight;
					} else {
						//Debug.Log (hit.distance);
						isCrouching = true;
					}
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

			if (!enableLogic) {
				return;
			}

			//

            if (!m_PreviouslyGrounded && characterController.isGrounded)
            {
                StartCoroutine(m_JumpBob.DoBobCycle());
                //PlayLandingSound();
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
        }


        private void PlayLandingSound()
        {
            m_AudioSource.clip = m_LandSound;
            m_AudioSource.Play();
            m_NextStep = m_StepCycle + .5f;
        }

		private float previousSpeed = 0.0f;

		private Vector2 moveDirection = new Vector2();

        private void FixedUpdate()
        {
			if (!enableLogic) {
				return;
			}

            float speed;
            GetInput(out speed);
            // always move along the camera forward as it is the direction that it being aimed at
            Vector3 desiredMove = transform.forward * m_Input.y + transform.right * m_Input.x;

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
                moveDir.y = -m_StickToGroundForce;

                if (m_Jump)
                {
                    moveDir.y = m_JumpSpeed;
                    //PlayJumpSound();
                    m_Jump = false;
                    m_Jumping = true;
                }
            }
            else
            {
				if (inertionInAir) {
					moveDir += Physics.gravity * gravityMultiplier * Time.fixedDeltaTime;
					moveDir.x = characterController.velocity.x + (desiredMove.x * inAirMoveMultiplier);
					moveDir.z = characterController.velocity.z + (desiredMove.z * inAirMoveMultiplier);
					if (isCrouching) {
						moveDir.x -= (desiredMove.x * inAirMoveMultiplier) / 3.0f;
						moveDir.z -= (desiredMove.z * inAirMoveMultiplier) / 3.0f;
					}
				} else {
					moveDir.y = m_Camera.transform.forward.y * speed;
				}
            }

			if (inertionInAir) {
				if (moveDir.x + moveDir.z == 0.0f) {
					moveDirection = Vector2.Lerp (moveDirection, Vector2.zero, Time.fixedDeltaTime * 6.0f);
				} else {
					moveDirection = Vector2.Lerp (moveDirection, new Vector2 (moveDir.x, moveDir.z), Time.fixedDeltaTime * 10.0f);
				}
				m_CollisionFlags = characterController.Move (new Vector3 (moveDirection.x, moveDir.y, moveDirection.y) * Time.fixedDeltaTime);
			} else {
				m_CollisionFlags = characterController.Move (moveDir * Time.fixedDeltaTime);
			}

            ProgressStepCycle(speed);
            UpdateCameraPosition(speed);

			mouseLook.UpdateCursorLock ();
        }


        private void PlayJumpSound()
        {
            m_AudioSource.clip = m_JumpSound;
            m_AudioSource.Play();
        }


        private void ProgressStepCycle(float speed)
        {
            if (characterController.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0))
            {
                m_StepCycle += (characterController.velocity.magnitude + (speed*(m_IsWalking ? 1f : m_RunstepLenghten)))*
                             Time.fixedDeltaTime;
            }

            if (!(m_StepCycle > m_NextStep))
            {
                return;
            }

            m_NextStep = m_StepCycle + m_StepInterval;

            PlayFootStepAudio();
        }


        private void PlayFootStepAudio()
        {
            if (!characterController.isGrounded)
            {
                return;
            }

			if(m_FootstepSounds.GetLength(0) > 0) {
	            // pick & play a random footstep sound from the array,
	            // excluding sound at index 0
	            int n = Random.Range(1, m_FootstepSounds.Length);
	            m_AudioSource.clip = m_FootstepSounds[n];
	            m_AudioSource.PlayOneShot(m_AudioSource.clip);
	            // move picked sound to index 0 so it's not picked next time
	            m_FootstepSounds[n] = m_FootstepSounds[0];
	            m_FootstepSounds[0] = m_AudioSource.clip;
			}
        }


        private void UpdateCameraPosition(float speed)
        {
            Vector3 newCameraPosition;
            if (!m_UseHeadBob)
            {
                return;
            }
            if (characterController.velocity.magnitude > 0 && characterController.isGrounded)
            {
                m_Camera.transform.localPosition =
                    m_HeadBob.DoHeadBob(characterController.velocity.magnitude +
                                      (speed*(m_IsWalking ? 1f : m_RunstepLenghten)));
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = m_Camera.transform.localPosition.y - m_JumpBob.Offset();
            }
            else
            {
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = m_OriginalCameraPosition.y - m_JumpBob.Offset();
            }
            m_Camera.transform.localPosition = newCameraPosition;
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

            bool waswalking = m_IsWalking;

#if !MOBILE_INPUT
            // On standalone builds, walk/run speed is modified by a key press.
            // keep track of whether or not the character is walking or running
            m_IsWalking = !InputManager.GetButton ("PlayerRun");
#endif
            // set the desired speed to be walking or running
            speed = m_IsWalking ? m_WalkSpeed : m_RunSpeed;
			speed = isCrouching ? crouchSpeed : speed;
            m_Input = new Vector2(horizontal, vertical);

            // normalize input if it exceeds 1 in combined length:
            if (m_Input.sqrMagnitude > 1)
            {
                m_Input.Normalize();
            }

            // handle speed change to give an fov kick
            // only if the player is going to a run, is running and the fovkick is to be used
            if (m_IsWalking != waswalking && m_UseFovKick && characterController.velocity.sqrMagnitude > 0)
            {
                StopAllCoroutines();
                StartCoroutine(!m_IsWalking ? m_FovKick.FOVKickUp() : m_FovKick.FOVKickDown());
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
            body.AddForceAtPosition(characterController.velocity*0.1f, hit.point, ForceMode.Impulse);
        }
    }
}
