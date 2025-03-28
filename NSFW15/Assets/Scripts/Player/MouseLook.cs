/*
* GitHub: https://github.com/AlgoritmoAlgoritmo
* Date: 16/03/2025 (DD/MM/YYYY)
*/



using UnityEngine;



namespace Jam15.Player {
	[System.Serializable]
    public class MouseLook {
        #region Variables
        public RotationAxes axes = RotationAxes.MouseY;
        public static bool allowRotation = true;

        [SerializeField]
        private Transform cameraTransform;
        [SerializeField]
        private float mouseSensitivity = 1.5f;

        [SerializeField]
        private float minimumX = -360f;
        [SerializeField]
        private float maximumX = 360f;

        [SerializeField]
        private float minimumY = -60f;
        [SerializeField]
        private float maximumY = 60f;

        private float currentSensitivityX = 1.5f;
        private float currentSensitivityY = 1.5f;

        private Quaternion originalRotation;
        private Vector2 rotation;
        #endregion


        #region Public methods
        public void Initialize() {
            originalRotation = cameraTransform.rotation;
        }

        public void HandleRotation( Vector2 _mouseDelta ) {
            if( currentSensitivityX != mouseSensitivity
                || currentSensitivityY != mouseSensitivity ) {
                currentSensitivityX = currentSensitivityY = mouseSensitivity;
            }

            mouseSensitivity = currentSensitivityX;
            mouseSensitivity = currentSensitivityY;

            if( axes == RotationAxes.MouseX ) {
                rotation.x += _mouseDelta.x * mouseSensitivity * Time.deltaTime;

                rotation.x = ClampAngle( rotation.x, minimumX, maximumX );
                Quaternion yQuaternion = Quaternion.AngleAxis( -rotation.x, Vector3.up );
                cameraTransform.localRotation = originalRotation * yQuaternion;

            } else if( axes == RotationAxes.MouseY ) {
                rotation.y += _mouseDelta.y * mouseSensitivity * Time.deltaTime;

                rotation.y = ClampAngle( rotation.y, minimumY, maximumY );
                Quaternion yQuaternion = Quaternion.AngleAxis( -rotation.y, Vector3.right );
                cameraTransform.localRotation = originalRotation * yQuaternion;
            }

            /*
            public void HandleRotation( Vector2 _mouseDelta ) {
                if( currentSensitivityX != mouseSensitivity
                    || currentSensitivityY != mouseSensitivity ) {
                    currentSensitivityX = currentSensitivityY = mouseSensitivity;
                }

                mouseSensitivity = currentSensitivityX;
                mouseSensitivity = currentSensitivityY;

                if( axes == RotationAxes.MouseX ) {
                    _mouseDelta.x *= mouseSensitivity;

                    _mouseDelta.x = ClampAngle( _mouseDelta.x, minimumX, maximumX );
                    Quaternion xQuaternion = Quaternion.AngleAxis( _mouseDelta.x, Vector3.up );
                    cameraTransform.localRotation = originalRotation * xQuaternion;

                } else if( axes == RotationAxes.MouseY ) {
                    _mouseDelta.y *= mouseSensitivity;

                    _mouseDelta.y = ClampAngle( _mouseDelta.y, minimumY, maximumY );
                    Quaternion yQuaternion = Quaternion.AngleAxis( -_mouseDelta.y, Vector3.right );
                    cameraTransform.localRotation = originalRotation * yQuaternion;
                }
                */
        }
        #endregion


        #region Private functions
        private float ClampAngle( float angle, float min, float max ) {
            if( angle < -360f ) {
                angle += 360f;
            } else if( angle > 360 ) {
                angle -= 360f;
            }

            return Mathf.Clamp( angle, min, max );
        }
        #endregion
    }

    public enum RotationAxes {
        MouseX, MouseY
    }
}

