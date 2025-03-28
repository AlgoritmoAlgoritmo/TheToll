/*
* GitHub: https://github.com/AlgoritmoAlgoritmo
* Date: 16/03/2025 (DD/MM/YYYY)
*/



using UnityEngine;
using UnityEngine.Events;

namespace Jam15.Interactions {
    public class InteractableNPC : MonoBehaviour, Interfaces.IInteractable {
        #region Variables
        [SerializeField]
        private GameObject solitaireGamePrefab;
        [SerializeField]
        private GameObject npcCamera;

        public GamObjectEvent OnSolitaireGameStart;
        #endregion


        #region Public methods
        public void Interact() {
            Debug.Log( "Interacting..." );
            OnSolitaireGameStart?.Invoke( this );
        }

        public string GetActionText() {
            return "Interact";
        }

        public GameObject GetGamePrefab() {
            return solitaireGamePrefab;
        }

        public GameObject GetCamera() {
            return npcCamera;        
        }
        #endregion
    }

    [System.Serializable]
    public class GamObjectEvent : UnityEvent<InteractableNPC> { }
}