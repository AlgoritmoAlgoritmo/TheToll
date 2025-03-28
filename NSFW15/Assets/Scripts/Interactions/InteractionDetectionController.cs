/*
* GitHub: https://github.com/AlgoritmoAlgoritmo
* Date: 16/03/2025 (DD/MM/YYYY)
*/



using UnityEngine;
using Jam15.Interactions.Interfaces;



namespace Jam15.Interactions {
    public class InteractionDetectionController : MonoBehaviour {
        #region Variables
        [SerializeField]
        private string interactionKey = "E";
        [SerializeField]
        private float MAX_DISTANCE = 2f;
        [SerializeField]
        private Transform raycastOrigin;
        [SerializeField]
        private TMPro.TMP_Text interactionText;

        private RaycastHit hit;
        private IInteractable interactableComponent;
        private string actionText;
        private bool canInteract = true;
        #endregion


        #region MonoBehaviour methods
        private void Start() {
            SetActionText();
        }

        private void FixedUpdate() {
            if( canInteract
                    && Physics.Raycast( raycastOrigin.position,
                                        raycastOrigin.forward,
                                        out hit, MAX_DISTANCE )
                    && hit.transform.gameObject.GetComponent<IInteractable>() != null ) {
                interactionText.gameObject.SetActive( true );
                interactionText.text = actionText + hit.transform.gameObject
                                                    .GetComponent<IInteractable>()?.GetActionText();
                interactableComponent = hit.transform.gameObject.GetComponent<IInteractable>();

            } else {
                interactionText.gameObject.SetActive(false);
                interactableComponent = null;
            }
        }

        #endregion


        #region Public methods
        public void CanNotInteract( bool canInteract ) {
            this.canInteract = !canInteract;
        }

        public void Interact() {
            if( interactableComponent != null ) {
                interactionText.text = actionText + hit.transform.gameObject
                                                    .GetComponent<IInteractable>()?.GetActionText();
                hit.transform.gameObject.GetComponent<IInteractable>()?.Interact();
            }
        }
        #endregion


        #region Private methods
        private void SetActionText() {
            actionText = "PRESS " + interactionKey + " TO\n";
        }
        #endregion

    }
}