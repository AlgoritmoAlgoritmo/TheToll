/*
* GitHub: https://github.com/AlgoritmoAlgoritmo
* Date: 15/03/2025 (DD/MM/YYYY)
*/



using UnityEngine;



namespace Jam15.Player {
    [System.Serializable]
    public class RigidBodyMovement {
        #region Variables
        [SerializeField]
        private float speed = 1f;
        [SerializeField]
        private Rigidbody rigidbody;

        private Vector3 movementDirection = new Vector3();
        #endregion


        #region Constructors
        #endregion


        #region Public methods
        public void Move( Vector2 _direction ) {
            movementDirection.x = _direction.x;
            movementDirection.z = _direction.y;

            rigidbody.MovePosition( rigidbody.rotation * movementDirection * speed );
        }
        #endregion
    }
}