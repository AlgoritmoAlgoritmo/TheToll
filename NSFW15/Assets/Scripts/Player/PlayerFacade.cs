/*
* GitHub: https://github.com/AlgoritmoAlgoritmo
* Date: 15/03/2025 (DD/MM/YYYY)
*/



using UnityEngine;
using UnityEngine.InputSystem;
using UnityEditor.Animations;
using NSFWJam15.Player.Movement;
using Jam15.Interactions;



namespace Jam15.Player {
	public class PlayerFacade : MonoBehaviour {
		#region Variables
		[SerializeField]
		private LocalTransformReferenceMovement rigidBodyMovement;
		[SerializeField]
		private RigidBodyRotation rigidBodyRotation;
		[SerializeField]
		private LookAtTransformRotation avatarRotation;
		[SerializeField]
		private MouseLook mouseLook;
		[SerializeField]
		private InteractionDetectionController interactionController;

		public Vector2 MovementDirection = Vector2.zero;
		public Vector2 RotationDirection = Vector2.zero;


		[SerializeField]
		private Animator animatorController;
		[SerializeField]
		private string walkBoolName = "IsWalking";
        #endregion


        #region MonoBehaviour methods
        private void Start() {
			mouseLook.Initialize();
			rigidBodyMovement.Initialize();
			avatarRotation.Initialize();
		}

        private void FixedUpdate() {
			Move( MovementDirection );
			Rotate( RotationDirection );
			GetComponent<Rigidbody>().AddForce( Vector3.down * 2f, ForceMode.Impulse );
			avatarRotation.Rotate( MovementDirection );
		}
		#endregion
		

		#region Public methods
		public void Move( Vector2 _direction ) {
			rigidBodyMovement.Move( _direction );
			animatorController.SetBool( walkBoolName, _direction != Vector2.zero );
		}

		public void Rotate( Vector2 _delta ) {
			rigidBodyRotation.Rotate( _delta );
			mouseLook.HandleRotation( _delta );
		}

		public void Interact( InputAction.CallbackContext _context ) {
			Debug.Log( "Interact key pressed..." );
			interactionController.Interact();
		}
		#endregion
	}
}