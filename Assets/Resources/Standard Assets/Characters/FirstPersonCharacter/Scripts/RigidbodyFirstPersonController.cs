using System;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Characters.FirstPerson
{
    [RequireComponent(typeof (Rigidbody))]
    [RequireComponent(typeof (CapsuleCollider))]
    public class RigidbodyFirstPersonController : MonoBehaviour
    {
        [Serializable]
        public class MovementSettings
        {
            public float ForwardSpeed = 8.0f;   // Speed when walking forward
            public float BackwardSpeed = 4.0f;  // Speed when walking backwards
            public float StrafeSpeed = 4.0f;    // Speed when walking sideways
            public float RunMultiplier = 2.0f;   // Speed when sprinting
	        public KeyCode RunKey = KeyCode.LeftShift;
            public float JumpForce = 30f;
            public AnimationCurve SlopeCurveModifier = new AnimationCurve(new Keyframe(-90.0f, 1.0f), new Keyframe(0.0f, 1.0f), new Keyframe(90.0f, 0.0f));
            [HideInInspector] public float CurrentTargetSpeed = 8f;


#if !MOBILE_INPUT
            private bool m_Running;
#endif

            public void UpdateDesiredTargetSpeed(Vector2 input)
            {
	            if (input == Vector2.zero) return;
				if (input.x > 0 || input.x < 0)
				{
					//strafe
					CurrentTargetSpeed = StrafeSpeed;
				}
				if (input.y < 0)
				{
					//backwards
					CurrentTargetSpeed = BackwardSpeed;
				}
				if (input.y > 0)
				{
					//forwards
					//handled last as if strafing and moving forward at the same time forwards speed should take precedence
					CurrentTargetSpeed = ForwardSpeed;
				}
#if !MOBILE_INPUT
	            if (Input.GetKey(RunKey))
	            {
		            CurrentTargetSpeed *= RunMultiplier;
		            m_Running = true;
	            }
	            else
	            {
		            m_Running = false;
	            }
#endif
            }

#if !MOBILE_INPUT
            public bool Running
            {
                get { return m_Running; }
            }
#endif
        }


        [Serializable]
        public class AdvancedSettings
        {
            public float groundCheckDistance = 0.01f; // distance for checking if the controller is grounded ( 0.01f seems to work best for this )
            public float stickToGroundHelperDistance = 0.5f; // stops the character
            public float slowDownRate = 20f; // rate at which the controller comes to a stop when there is no input
            public bool airControl; // can the user control the direction that is being moved in the air
            [Tooltip("set it to 0.1 or more if you get stuck in wall")]
            public float shellOffset; //reduce the radius by that ratio to avoid getting stuck in wall (a value of 0.1f is nice)
        }


        public Transform cam;
        public MovementSettings movementSettings = new MovementSettings();
        public MouseLook mouseLook = new MouseLook();
        public AdvancedSettings advancedSettings = new AdvancedSettings();
		public bool enableLogic = true;

		[Header("Steps")]
		public float maxStepHeight = 0.4f;
		public float stepSearchOvershoot = 0.01f;
		private List<ContactPoint> allCPs = new List<ContactPoint>();
		private Vector3 lastVelocity;

		[Header("Other")]
        private Rigidbody m_RigidBody;
        private CapsuleCollider m_Capsule;
        private float m_YRotation;
        private Vector3 m_GroundContactNormal;
        private bool m_Jump, m_PreviouslyGrounded, m_Jumping, m_IsGrounded;


        public Vector3 Velocity
        {
            get { return m_RigidBody.velocity; }
        }

        public bool Grounded
        {
            get { return m_IsGrounded; }
        }

        public bool Jumping
        {
            get { return m_Jumping; }
        }

        public bool Running
        {
            get
            {
 #if !MOBILE_INPUT
				return movementSettings.Running;
#else
	            return false;
#endif
            }
        }


        private void Start()
        {
            m_RigidBody = GetComponent<Rigidbody>();
            m_Capsule = GetComponent<CapsuleCollider>();
            mouseLook.Init (transform, cam.transform);
        }


        private void Update()
        {
            RotateView();

            if (CrossPlatformInputManager.GetButtonDown("Jump") && !m_Jump)
            {
                m_Jump = true;
            }
        }


        private void FixedUpdate()
        {
			if (!enableLogic) {
				return;
			}

			//Filter through the ContactPoints to see if we're grounded and to see if we can step up
			ContactPoint groundCP = default(ContactPoint);
			bool grounded = FindGround(out groundCP, allCPs);
			m_IsGrounded = grounded;

            GroundCheck();
            Vector2 input = GetInput();

            if ((Mathf.Abs(input.x) > float.Epsilon || Mathf.Abs(input.y) > float.Epsilon) && (advancedSettings.airControl || m_IsGrounded))
            {
                // always move along the camera forward as it is the direction that it being aimed at
                Vector3 desiredMove = cam.transform.forward * input.y + cam.transform.right * input.x;
                desiredMove = Vector3.ProjectOnPlane(desiredMove, m_GroundContactNormal).normalized;

                desiredMove.x = desiredMove.x * movementSettings.CurrentTargetSpeed;
                desiredMove.z = desiredMove.z * movementSettings.CurrentTargetSpeed;
                desiredMove.y = desiredMove.y * movementSettings.CurrentTargetSpeed;
                if (m_RigidBody.velocity.sqrMagnitude <
                    (movementSettings.CurrentTargetSpeed*movementSettings.CurrentTargetSpeed))
                {
                    m_RigidBody.AddForce(desiredMove*SlopeMultiplier(), ForceMode.Impulse);
                }
            }

            if (m_IsGrounded)
            {
                m_RigidBody.drag = 5f;

                if (m_Jump)
                {
                    m_RigidBody.drag = 0f;
                    m_RigidBody.velocity = new Vector3(m_RigidBody.velocity.x, 0f, m_RigidBody.velocity.z);
                    m_RigidBody.AddForce(new Vector3(0f, movementSettings.JumpForce, 0f), ForceMode.Impulse);
                    m_Jumping = true;
                }

                if (!m_Jumping && Mathf.Abs(input.x) < float.Epsilon && Mathf.Abs(input.y) < float.Epsilon && m_RigidBody.velocity.magnitude < 1f)
                {
                    //m_RigidBody.Sleep();
                }
            }
            else
            {
                m_RigidBody.drag = 0f;
                if (m_PreviouslyGrounded && !m_Jumping)
                {
                    //StickToGroundHelper();
                }
            }

            m_Jump = false;

			Debug.LogError ("[Player] Checking stairs");
			Vector3 velocity = m_RigidBody.velocity;

			Vector3 stepUpOffset = default(Vector3);
			bool stepUp = false;
			if(grounded) 
				stepUp = FindStep(out stepUpOffset, allCPs, groundCP, velocity);

			//Steps
			if(stepUp)
			{
				m_RigidBody.position += stepUpOffset;
				m_RigidBody.velocity = lastVelocity;
			}

			Debug.LogError ("[Player] Step up status: " + stepUp);
			//Debug.LogError ("[Player] Grounded (my and stolen): " + m_IsGrounded + " " + grounded);

			allCPs.Clear();
			lastVelocity = velocity;
        }

		void OnCollisionEnter(Collision col)
		{
			//Debug.LogError ("[Player] Colliding: " + col.gameObject.name);
			//Debug.LogError ("[Player] Contacts: " + col.contacts.GetLength(0));
			allCPs.AddRange(col.contacts);
		}

		void OnCollisionStay(Collision col)
		{
			//Debug.LogError ("[Player] Colliding: " + col.gameObject.name);
			//Debug.LogError ("[Player] Contacts: " + col.contacts.GetLength(0));
			allCPs.AddRange(col.contacts);
		}

		/// Finds the MOST grounded (flattest y component) ContactPoint
		/// \param allCPs List to search
		/// \param groundCP The contact point with the ground
		/// \return If grounded
		bool FindGround(out ContactPoint groundCP, List<ContactPoint> allCPs)
		{
			groundCP = default(ContactPoint);
			bool found = false;
			foreach(ContactPoint cp in allCPs)
			{   
				//Pointing with some up direction
				if(cp.normal.y > 0.0001f && (found == false || cp.normal.y > groundCP.normal.y))
				{
					groundCP = cp;
					found = true;
				}
			}

			return found;
		}

		/// Find the first step up point if we hit a step
		/// \param allCPs List to search
		/// \param stepUpOffset A Vector3 of the offset of the player to step up the step
		/// \return If we found a step
		bool FindStep(out Vector3 stepUpOffset, List<ContactPoint> allCPs, ContactPoint groundCP, Vector3 currVelocity)
		{
			stepUpOffset = default(Vector3);

			//No chance to step if the player is not moving
			Vector2 velocityXZ = new Vector2(currVelocity.x, currVelocity.z);
			if (velocityXZ.sqrMagnitude < 0.0001f) {
				//Debug.LogError ("[Player] No step-check because player dont moving");
				return false;
			}

			foreach(ContactPoint cp in allCPs)
			{
				bool test = ResolveStepUp(out stepUpOffset, cp, groundCP);
				if (test) {
					//Debug.LogError ("[Player] Resolve step up:" + test);
					return test;
				}
			}
			return false;
		}

		/// Takes a contact point that looks as though it's the side face of a step and sees if we can climb it
		/// \param stepTestCP ContactPoint to check.
		/// \param groundCP ContactPoint on the ground.
		/// \param stepUpOffset The offset from the stepTestCP.point to the stepUpPoint (to add to the player's position so they're now on the step)
		/// \return If the passed ContactPoint was a step
		bool ResolveStepUp(out Vector3 stepUpOffset, ContactPoint stepTestCP, ContactPoint groundCP)
		{
			stepUpOffset = default(Vector3);
			Collider stepCol = stepTestCP.otherCollider;

			//( 1 ) Check if the contact point normal matches that of a step (y close to 0)
			if(Mathf.Abs(stepTestCP.normal.y) >= 0.4f)
			{
				Debug.LogError ("[Player] Not step because normal not close to 0: " + stepTestCP.normal);
				return false;
			}

			//( 2 ) Make sure the contact point is low enough to be a step
			if( !(stepTestCP.point.y - groundCP.point.y < maxStepHeight) )
			{
				Debug.LogError ("[Player] Contact point is not low enough to be a step");
				return false;
			}

			//( 3 ) Check to see if there's actually a place to step in front of us
			//Fires one Raycast
			RaycastHit hitInfo;
			float stepHeight = groundCP.point.y + maxStepHeight + 0.0001f;
			Vector3 stepTestInvDir = new Vector3(-stepTestCP.normal.x, 0, -stepTestCP.normal.z).normalized;
			Vector3 origin = new Vector3(stepTestCP.point.x, stepHeight, stepTestCP.point.z) + (stepTestInvDir * stepSearchOvershoot);
			Vector3 direction = Vector3.down;
			if( !(stepCol.Raycast(new Ray(origin, direction), out hitInfo, maxStepHeight)) )
			{
				Debug.LogError ("[Player] There is no place to step in front of us");
				return false;
			}

			//We have enough info to calculate the points
			Vector3 stepUpPoint = new Vector3(stepTestCP.point.x, hitInfo.point.y+0.0001f, stepTestCP.point.z) + (stepTestInvDir * stepSearchOvershoot);
			Vector3 stepUpPointOffset = stepUpPoint - new Vector3(stepTestCP.point.x, groundCP.point.y, stepTestCP.point.z);

			//We passed all the checks! Calculate and return the point!
			stepUpOffset = stepUpPointOffset;
			return true;
		}

        private float SlopeMultiplier()
        {
            float angle = Vector3.Angle(m_GroundContactNormal, Vector3.up);
            return movementSettings.SlopeCurveModifier.Evaluate(angle);
        }


        private void StickToGroundHelper()
        {
            RaycastHit hitInfo;
            if (Physics.SphereCast(transform.position, m_Capsule.radius * (1.0f - advancedSettings.shellOffset), Vector3.down, out hitInfo,
                                   ((m_Capsule.height/2f) - m_Capsule.radius) +
                                   advancedSettings.stickToGroundHelperDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                if (Mathf.Abs(Vector3.Angle(hitInfo.normal, Vector3.up)) < 85f)
                {
                    m_RigidBody.velocity = Vector3.ProjectOnPlane(m_RigidBody.velocity, hitInfo.normal);
                }
            }
        }


        private Vector2 GetInput()
        {
            
            Vector2 input = new Vector2
                {
                    x = CrossPlatformInputManager.GetAxis("Horizontal"),
                    y = CrossPlatformInputManager.GetAxis("Vertical")
                };
			movementSettings.UpdateDesiredTargetSpeed(input);
            return input;
        }


        private void RotateView()
        {
            //avoids the mouse looking if the game is effectively paused
            if (Mathf.Abs(Time.timeScale) < float.Epsilon) return;

            // get the rotation before it's changed
            float oldYRotation = transform.eulerAngles.y;

            mouseLook.LookRotation (transform, cam.transform);

            if (m_IsGrounded || advancedSettings.airControl)
            {
                // Rotate the rigidbody velocity to match the new direction that the character is looking
                Quaternion velRotation = Quaternion.AngleAxis(transform.eulerAngles.y - oldYRotation, Vector3.up);
                m_RigidBody.velocity = velRotation*m_RigidBody.velocity;
            }
        }

        /// sphere cast down just beyond the bottom of the capsule to see if the capsule is colliding round the bottom
        private void GroundCheck()
        {
            m_PreviouslyGrounded = m_IsGrounded;
            RaycastHit hitInfo;
            if (Physics.SphereCast(transform.position, m_Capsule.radius * (1.0f - advancedSettings.shellOffset), Vector3.down, out hitInfo,
                                   ((m_Capsule.height/2f) - m_Capsule.radius) + advancedSettings.groundCheckDistance, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                m_IsGrounded = true;
                m_GroundContactNormal = hitInfo.normal;
            }
            else
            {
                m_IsGrounded = false;
                m_GroundContactNormal = Vector3.up;
            }
            if (!m_PreviouslyGrounded && m_IsGrounded && m_Jumping)
            {
                m_Jumping = false;
            }
        }
    }
}
