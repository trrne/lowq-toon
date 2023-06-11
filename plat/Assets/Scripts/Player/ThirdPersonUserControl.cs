using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using Toon;
using Toon.Extend;

namespace UnityStandardAssets.Characters.ThirdPerson
{
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class ThirdPersonUserControl : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("mine")]
        GameObject cam;

        [SerializeField]
        BossCamera bossCam;

        [SerializeField]
        Manager manager;

        ThirdPersonCharacter tpc;
        PlayerInput pi;
        Player player;

        Transform m_Cam;
        Vector3 m_CamForward;
        Vector3 m_Move;
        public Vector3 M_Move => m_Move;

        bool m_Jump;

        void Start()
        {
            m_Cam = cam.transform;

            tpc = GetComponent<ThirdPersonCharacter>();
            cam = GameObject.FindGameObjectWithTag(constant.Camera);
            pi = GameObject.FindGameObjectWithTag(constant.Manager).GetComponent<PlayerInput>();
            player = GameObject.FindGameObjectWithTag(constant.Player).GetComponent<Player>();
        }

        void Update()
        {
            if (!m_Jump)
            {
                // m_Jump = CrossPlatformInputManager.GetButtonDown(Keys.Jump);
            }

            if (bossCam.IsMoving)
            {
                m_Move = Vector3.zero;
            }

            m_Move.show();
        }

        void FixedUpdate()
        {
            // 生きてる間のみ制御可能
            if (!manager.Controllable && bossCam.IsMoving)
            {
                return;
            }

            float h = CrossPlatformInputManager.GetAxis(constant.Horizontal),
                v = CrossPlatformInputManager.GetAxis(constant.Vertical);
            bool crouchKey = input.pressed(constant.Crouch);

            m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
            m_Move = h * m_Cam.right + v * m_CamForward;

            if (Input.GetKey(KeyCode.LeftShift) || tpc.WalkWhileShooting)
            {
                m_Move *= 0.5f;
            }

            tpc.Move(m_Move, crouchKey, m_Jump);
            m_Jump = false;
        }
    }
}