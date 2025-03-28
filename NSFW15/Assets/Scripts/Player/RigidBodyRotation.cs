/*
* GitHub: https://github.com/AlgoritmoAlgoritmo
* Date: 16/03/2025 (DD/MM/YYYY)
*/



using UnityEngine;



namespace Jam15.Player {
    [System.Serializable]
    public class RigidBodyRotation {
        #region Variables
        [SerializeField]
        private float rotationSpeed = 1.5f;

        [SerializeField]
        private Rigidbody rigidbody;


        private Vector3 rotation = Vector3.zero;
        #endregion


        #region Public methods
        public void Rotate( Vector2 _rotationAxis ) {
            rotation.y = _rotationAxis.x * rotationSpeed * Time.deltaTime;
            rigidbody.rotation *= Quaternion.Euler( rotation * rotationSpeed );
        }
        #endregion
    }
}