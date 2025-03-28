/*
* GitHub: https://github.com/AlgoritmoAlgoritmo
* Date: 27/03/2025 (DD/MM/YYYY)
*/



using System.Collections.Generic;
using UnityEngine;



namespace NSFWJam15.Player.Movement {
    [System.Serializable]
    public class LookAtTransformRotation {
        #region Variables
        [SerializeField]
        private float avatarRotationSpeed = 10f;
        [SerializeField]
        private Transform avatarParent;
        public Transform AvatarParent {
            get {
                return avatarParent;
            }
            set {
                avatarParent = value;
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

        private Vector3 movementDirectionV3 = Vector3.zero;
        private Quaternion targetAvatarRotation = new Quaternion();
        #endregion


        #region Public methods
        public void Initialize() {
            directionTransformsDictionary = new Dictionary<Vector2, Transform>();

            directionTransformsDictionary.Add( Vector2.up, forwardTransform );
            directionTransformsDictionary.Add( Vector2.down, backTransform );
            directionTransformsDictionary.Add( Vector2.left, leftTransform );
            directionTransformsDictionary.Add( Vector2.right, rightTransform );
            directionTransformsDictionary.Add( new Vector2( 1, 1 ), forwardRightTransform );
            directionTransformsDictionary.Add( new Vector2( -1, 1 ), forwardLeftTransform );
            directionTransformsDictionary.Add( new Vector2( -1, -1 ), backLeftTransform );
            directionTransformsDictionary.Add( new Vector2( 1, -1 ), backRightTransform );
        }

        public void Rotate( Vector2 _movementdirection ) {
            if( _movementdirection != Vector2.zero ) {
                movementDirectionV3 = avatarParent.position - directionTransformsDictionary
                                            [ParseDirectionValues( _movementdirection )].position;

                movementDirectionV3.y = 0f;

                targetAvatarRotation = Quaternion.LookRotation( movementDirectionV3 );

                avatarParent.rotation = Quaternion.Slerp( avatarParent.rotation,
                                                            targetAvatarRotation,
                                                            avatarRotationSpeed * Time.deltaTime );
            }
        }
        #endregion


        #region Private methods
        private Vector2 ParseDirectionValues( Vector2 _direction ) {
            if( _direction.x != 0f )
                _direction.x = Mathf.Sign( _direction.x );

            if( _direction.y != 0f )
                _direction.y = Mathf.Sign( _direction.y );

            return _direction;
        }
        #endregion
    }
}
