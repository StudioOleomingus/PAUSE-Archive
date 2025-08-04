using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Characters.FirstPerson
{
    [Serializable]
    public class MouseLook
    {
        public float XSensitivity = 2f;
        public float YSensitivity = 2f;
        public bool clampVerticalRotation = true;
        public float MinimumX = -90F;
        public float MaximumX = 90F;
        public bool smooth;
        public float smoothTime = 5f;
        public bool lockCursor = true;
        public bool canLook = true;

        private Quaternion m_CharacterTargetRot;
        private Quaternion m_CameraTargetRot;
        private bool m_cursorIsLocked = true;

        public GameObject kilnDisplayManager;

        public bool isInvertedPlayer;

        public MotionDetection motionDetection;

        public UnityEngine.UI.Slider slider_rotation_sensitivity;

        public void Init(Transform character, Transform camera)
        {
            GameObject g_manager = GameObject.Find("GameManager");

            if (g_manager != null)
                g_manager.SendMessage("GetRotationSensitivity");

            m_CharacterTargetRot = character.localRotation;
            m_CameraTargetRot = camera.localRotation;
        }

        float rotation_sensitivity = 1;
        public void LookRotation(Transform character, Transform camera)
        {
            
            if(slider_rotation_sensitivity != null)
            {
                rotation_sensitivity = slider_rotation_sensitivity.value;
            }
            float yRot = 0f;// CrossPlatformInputManager.GetAxis("Mouse X") * XSensitivity;
            float xRot = 0f;// CrossPlatformInputManager.GetAxis("Mouse Y") * YSensitivity;

            if(OleoInputManager.instance.iNPUT_TYPE == OleoInputManager.INPUT_TYPE.Joystick)
            {
                yRot = Input.GetAxis("JoystickMouse X") * XSensitivity * rotation_sensitivity;
                xRot = -Input.GetAxis("JoystickMouse Y") * YSensitivity * rotation_sensitivity;
            }
            else if (OleoInputManager.instance.iNPUT_TYPE == OleoInputManager.INPUT_TYPE.Keyboard)
            {
                yRot = CrossPlatformInputManager.GetAxis("Mouse X") * XSensitivity * slider_rotation_sensitivity.value;
                xRot = CrossPlatformInputManager.GetAxis("Mouse Y") * YSensitivity * slider_rotation_sensitivity.value;
            }
            else if (OleoInputManager.instance.iNPUT_TYPE == OleoInputManager.INPUT_TYPE.Leapmotion)
            {
                if (motionDetection != null && motionDetection.gameObject.activeSelf)
                {
                    yRot = motionDetection.GetMouseAxis("Horizontal");
                    xRot = motionDetection.GetMouseAxis("Vertical");
                }
            }

            if (!canLook)
            {
                yRot = 0;
                xRot = 0;
            }

            m_CharacterTargetRot *= Quaternion.Euler (0f, yRot, 0f);
            m_CameraTargetRot *= Quaternion.Euler (-xRot, 0f, 0f);

            if(clampVerticalRotation)
                m_CameraTargetRot = ClampRotationAroundXAxis (m_CameraTargetRot);

            if(smooth)
            {
                character.localRotation = Quaternion.Slerp (character.localRotation, m_CharacterTargetRot,
                    smoothTime * Time.deltaTime);
                camera.localRotation = Quaternion.Slerp (camera.localRotation, m_CameraTargetRot,
                    smoothTime * Time.deltaTime);
            }
            else
            {
                character.localRotation = m_CharacterTargetRot;
                camera.localRotation = m_CameraTargetRot;
            }

            UpdateCursorLock();
        }

        public void SetCursorLock(bool value)
        {
            lockCursor = value;
            if(!lockCursor)
            {//we force unlock the cursor if the user disable the cursor locking helper
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        public void UpdateCursorLock()
        {
            //if the user set "lockCursor" we check & properly lock the cursos
            if (kilnDisplayManager != null)
            {
                if (lockCursor && !kilnDisplayManager.activeSelf)
                    InternalLockUpdate();
            }
            else
            {
                if (lockCursor)
                    InternalLockUpdate();
            }
        }

        private void InternalLockUpdate()
        {
            if(Input.GetKeyUp(KeyCode.Escape))
            {
                m_cursorIsLocked = false;
            }
            else if(Input.GetMouseButtonUp(0))
            {
                m_cursorIsLocked = true;
            }

            if (m_cursorIsLocked)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else if (!m_cursorIsLocked)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }

        Quaternion ClampRotationAroundXAxis(Quaternion q)
        {
            if(q.w != 0)
                q.x /= q.w;
            if (q.w != 0)
                q.y /= q.w;
            if (q.w != 0)
                q.z /= q.w;
            q.w = 1.0f;

            float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan (q.x);

            angleX = Mathf.Clamp (angleX, MinimumX, MaximumX);

            q.x = Mathf.Tan (0.5f * Mathf.Deg2Rad * angleX);

            return q;
        }

    }
}
