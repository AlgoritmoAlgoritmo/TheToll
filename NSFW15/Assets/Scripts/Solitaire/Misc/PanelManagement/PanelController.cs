/*
* Author:	Iris Bermudez
* Date:		19/03/2024
*/



using UnityEngine;
using UnityEngine.Events;



namespace Misc.PanelManagement {
    public class PanelController : MonoBehaviour {
        #region Variables
        [SerializeField]
        private UnityEvent onOpenPanel = new UnityEvent();
        [SerializeField]
        private UnityEvent onClosePanel = new UnityEvent();
        #endregion


        #region Public methods
        public void OpenPanel() {
            onOpenPanel.Invoke();
        }

        public void ClosePanel() {
            onClosePanel.Invoke();
        }
        #endregion
    }
}