/*
* GitHub: https://github.com/AlgoritmoAlgoritmo
* Date: 25/03/2025 (DD/MM/YYYY)
*/



using System.Collections.Generic;
using UnityEngine;



namespace NSFWJam15.Player.Movement {
    [System.Serializable]
    public class LocalTransformReferenceMovement {
        #region Variables
        [SerializeField]
        private float speed = 1.5f;

        [SerializeField]
        private Rigidbody rigidBody;
        public Rigidbody RigidBody {
            get {
                return rigidBody;
            }
            set {
                rigidBody = value;
            }
        }

        [SerializeField]
        private Transform forwardTransform;
        public Transform ForwardTransform {
            get {
                return forwardTransform;
            }
            set {
                forwardTransform = value;
            }
        }

        [SerializeField]
        private Transform forwardRightTransform;
        public Transform ForwardRightTransform {
            get {
                return forwardRightTransform;
            }
            set {
                forwardRightTransform = value;
            }
        }

        [SerializeField]
        private Transform forwardLeftTransform;
        public Transform ForwardLeftTransform {
            get {
                return forwardLeftTransform;
            }
            set {
                forwardLeftTransform = value;
            }
        }

        [SerializeField]
        private Transform backTransform;
        public Transform BackTransform {
            get {
                return backTransform;
            }
            set {
                backTransform = value;
            }
        }

        [SerializeField]
        private Transform backRightTransform;
        public Transform BackRightTransform {
            get {
                return backRightTransform;
            }
            set {
                backRightTransform = value;
            }
        }

        [SerializeField]
        private Transform backLeftTransform;
        public Transform BackLeftTransform {
            get {
                return backLeftTransform;
            }
            set {
                backLeftTransform = value;
            }
        }

        [SerializeField]
        private Transform leftTransform;
        public Transform LeftTransform {
            get {
                return leftTransform;
            }
            set {
                leftTransform = value;
            }
        }

        [SerializeField]
        private Transform rightTransform;
        public Transform RightTransform {
            get {
                return rightTransform;
            }
            set {
                rightTransform = value;
            }
        }


        private Dictionary<Vector2, Transform> directionTransformsDictionary;
        private Vector2 direction = Vector2.zero;
        private Vector3 directionV3 = Vector3.zero;
        #endregion


        #region Public methods
        public void Initialize() {
            directionTransformsDictionary = new Dictionary<Vector2, Transform>();

            directionTransformsDictionary.Add( Vector2.zero, rigidBody.gameObject.transform );
            directionTransformsDictionary.Add( Vector2.up, forwardTransform );
            directionTransformsDictionary.Add( Vector2.down, backTransform );
            directionTransformsDictionary.Add( Vector2.left, leftTransform );
            directionTransformsDictionary.Add( Vector2.right, rightTransform );
            directionTransformsDictionary.Add( new Vector2( 1, 1 ), forwardRightTransform );
            directionTransformsDictionary.Add( new Vector2( -1, 1 ), forwardLeftTransform );
            directionTransformsDictionary.Add( new Vector2( -1, -1 ), backLeftTransform );
            directionTransformsDictionary.Add( new Vector2( 1, -1 ), backRightTransform );
        }


        public void Move( Vector2 _direction ) {
            direction = _direction;
            ParseDirectionValues();
            directionV3 = directionTransformsDictionary[direction].position;
            directionV3.y = rigidBody.transform.position.y;
            directionV3 = ( directionV3 - rigidBody.transform.position );
            directionV3.Normalize();

            rigidBody.MovePosition( rigidBody.transform.position
                                    + ( directionV3 * speed * Time.deltaTime ) );
        }
        #endregion


        #region Private methods
        private void ParseDirectionValues() {
            if( direction.x != 0f )
                direction.x = Mathf.Sign( direction.x );

            if( direction.y != 0f )
                direction.y = Mathf.Sign( direction.y );
        }
        #endregion
    }
}
