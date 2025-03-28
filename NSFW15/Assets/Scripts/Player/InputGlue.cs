/*
* GitHub: https://github.com/AlgoritmoAlgoritmo
* Date: 15/03/2025 (DD/MM/YYYY)
*/



using UnityEngine;
using UnityEngine.InputSystem;
using Jam15.Interactions;



namespace Jam15.Player {
	public class InputGlue : MonoBehaviour {
		#region Variables
		[SerializeField]
        private PlayerInput playerInput;
        [SerializeField]
        private PlayerFacade playerFacade;

        [SerializeField]
        private string movementActionName = "Move";
        [SerializeField]
        private string mouseRotationActionName = "CamLook";
		[SerializeField]
		private string interactActionName = "Interact";

        [SerializeField]
        private string defaultMapID = "DefaultMap";
        [SerializeField]
        private string solitaireMapID = "SolitaireMap";
        #endregion


        #region MonoBehaviour methods
        private void Awake() {
            if( !playerFacade )
                playerFacade = FindObjectOfType<PlayerFacade>();

            if( !playerInput )
                playerInput = FindObjectOfType<PlayerInput>();
        }

        private void Start() {
            playerInput.actions[interactActionName].performed += playerFacade.Interact;
        }

        private void Update() {
            playerFacade.MovementDirection = playerInput.actions[movementActionName].ReadValue<Vector2>();
            playerFacade.RotationDirection = playerInput.actions[mouseRotationActionName].ReadValue<Vector2>();
        }
        #endregion


        #region Public methods
        public void SwitchToSolitaireMap() {
            playerInput.SwitchCurrentActionMap( solitaireMapID );
        }

        public void SwitchToDefaultMap() {
            playerInput.SwitchCurrentActionMap( defaultMapID );
        }
        #endregion
    }
}