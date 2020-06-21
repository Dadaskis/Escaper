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
        private Vector3 m_MoveDir = Vector3.zero;
        private CharacterController m_CharacterController;
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

		private bool isCrouching = false;
		private float previousY = 0.0f;
		private float startHeight = 0.0f;

        // Use this for initialization
        private void Start()
        {
            m_CharacterController = GetComponent<CharacterController>();
			startHeight = m_CharacterController.height;
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

			previousY = m_CharacterController.transform.position.y - m_CharacterController.height / 2 - m_CharacterController.skinWidth;

			if (holdCrouch) {
				if (InputManager.GetButtonDown ("PlayerCrouch") == true) {
					if (isCrouching == false) {
						isCrouching = true;
						targetHeight = crouchHeight;
					} else {
						RaycastHit hit;
						bool isHitToOpaque = Physics.Raycast (m_CharacterController.transform.position, m_CharacterController.transform.up, out hit);
						if (!isHitToOpaque || hit.distance > startHeight / 1.5f) {
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
					RaycastHit hit;
					bool isHitToOpaque = Physics.Raycast (m_CharacterController.transform.position, m_CharacterController.transform.up, out hit);
					if (!isHitToOpaque || hit.distance > startHeight / 1.5f) {
						targetHeight = startHeight;
					} else {
						isCrouching = true;
					}
				}
			}

			m_CharacterController.height = Mathf.Lerp (m_CharacterController.height, targetHeight, 5.0f * Time.deltaTime);

			m_Camera.transform.position = Vector3.Lerp (
				m_Camera.transform.position, 
				new Vector3 (
					m_Camera.transform.position.x, 
					m_CharacterController.transform.position.y + targetHeight / 2.0f - 0.1f,
					m_Camera.transform.position.z
				),
				5.0f * Time.deltaTime
			);

			m_CharacterController.transform.position = Vector3.Lerp (
				m_CharacterController.transform.position,
				new Vector3 (
					m_CharacterController.transform.position.x,
					previousY + targetHeight / 2.0f + m_CharacterController.skinWidth,
					m_CharacterController.transform.position.z
				),
				5.0f * Time.deltaTime
			);

			//

            if (!m_PreviouslyGrounded && m_CharacterController.isGrounded)
            {
                StartCoroutine(m_JumpBob.DoBobCycle());
                //PlayLandingSound();
                m_MoveDir.y = 0f;
                m_Jumping = false;
				Player.instance.character.FallDamage (previousSpeed);
            }
            if (!m_CharacterController.isGrounded && !m_Jumping && m_PreviouslyGrounded)
            {
                m_MoveDir.y = 0f;
            }

            m_PreviouslyGrounded = m_CharacterController.isGrounded;
			previousSpeed = Math.Abs(m_CharacterController.velocity.y);
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
            float speed;
            GetInput(out speed);
            // always move along the camera forward as it is the direction that it being aimed at
            Vector3 desiredMove = transform.forward * m_Input.y + transform.right * m_Input.x;

            // get a normal for the surface that is being touched to move along it
            RaycastHit hitInfo;
            Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
                               m_CharacterController.height/2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

            m_MoveDir.x = desiredMove.x*speed;
            m_MoveDir.z = desiredMove.z*speed;


            if (m_CharacterController.isGrounded)
            {
                m_MoveDir.y = -m_StickToGroundForce;

                if (m_Jump)
                {
                    m_MoveDir.y = m_JumpSpeed;
                    //PlayJumpSound();
                    m_Jump = false;
                    m_Jumping = true;
                }
            }
            else
            {
                m_MoveDir += Physics.gravity*gravityMultiplier*Time.fixedDeltaTime;
				m_MoveDir.x = m_CharacterController.velocity.x + (desiredMove.x * inAirMoveMultiplier);
				m_MoveDir.z = m_CharacterController.velocity.z + (desiredMove.z * inAirMoveMultiplier);
				if (isCrouching) {
					m_MoveDir.x -= (desiredMove.x * inAirMoveMultiplier) / 3.0f;
					m_MoveDir.z -= (desiredMove.z * inAirMoveMultiplier) / 3.0f;
				}
            }

			if (m_MoveDir.x + m_MoveDir.z == 0.0f) {
				moveDirection = Vector2.Lerp (moveDirection, Vector2.zero, Time.fixedDeltaTime * 8.0f);
			} else {
				moveDirection = Vector2.Lerp (moveDirection, new Vector2(m_MoveDir.x, m_MoveDir.z), Time.fixedDeltaTime * 8.0f);
			}

			m_CollisionFlags = m_CharacterController.Move(new Vector3(moveDirection.x, m_MoveDir.y, moveDirection.y) * Time.fixedDeltaTime);

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
            if (m_CharacterController.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0))
            {
                m_StepCycle += (m_CharacterController.velocity.magnitude + (speed*(m_IsWalking ? 1f : m_RunstepLenghten)))*
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
            if (!m_CharacterController.isGrounded)
            {
                return;
            }
            // pick & play a random footstep sound from the array,
            // excluding sound at index 0
            int n = Random.Range(1, m_FootstepSounds.Length);
            m_AudioSource.clip = m_FootstepSounds[n];
            m_AudioSource.PlayOneShot(m_AudioSource.clip);
            // move picked sound to index 0 so it's not picked next time
            m_FootstepSounds[n] = m_FootstepSounds[0];
            m_FootstepSounds[0] = m_AudioSource.clip;
        }


        private void UpdateCameraPosition(float speed)
        {
            Vector3 newCameraPosition;
            if (!m_UseHeadBob)
            {
                return;
            }
            if (m_CharacterController.velocity.magnitude > 0 && m_CharacterController.isGrounded)
            {
                m_Camera.transform.localPosition =
                    m_HeadBob.DoHeadBob(m_CharacterController.velocity.magnitude +
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
            if (m_IsWalking != waswalking && m_UseFovKick && m_CharacterController.velocity.sqrMagnitude > 0)
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
            body.AddForceAtPosition(m_CharacterController.velocity*0.1f, hit.point, ForceMode.Impulse);
        }
    }
}
