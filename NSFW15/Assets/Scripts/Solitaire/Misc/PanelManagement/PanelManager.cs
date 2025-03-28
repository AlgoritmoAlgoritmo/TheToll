/*
* Author:	Iris Bermudez
* Date:		19/03/2024
*/



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



namespace Misc.PanelManagement {
    public class PanelManager : MonoBehaviour {
        #region Variables
        [SerializeField]
        private PanelController starterPanel;
        [SerializeField]
        private UnityEvent onSwitchScreen = new UnityEvent();

        private PanelController previousPanel;
        public PanelController PreviousPanel { get => previousPanel; }

        private PanelController currentPanel;
        public PanelController CurrentPanel { get => currentPanel; }
        #endregion


        #region Inherited methods
        private void Start() {
            if( starterPanel ) {
                SwitchPanel( starterPanel );
            }
        }
        #endregion


        #region Public methods
        public void SwitchPanel( PanelController _newPanel ) {
            if( _newPanel ) {
                if( _newPanel == currentPanel ) {
                    _newPanel.ClosePanel();
                    currentPanel = null;
                    return;
                }

                if (currentPanel) {
                    currentPanel.ClosePanel();
                    previousPanel = currentPanel;
                    previousPanel.gameObject.SetActive(false);
                }

                currentPanel = _newPanel;
                currentPanel.gameObject.SetActive(true);
                currentPanel.OpenPanel();

                if (onSwitchScreen != null) {
                    onSwitchScreen.Invoke();
                }
            }
        }


        public void OpenUIObject( PanelController _newPanel ) {
            currentPanel?.ClosePanel();

            currentPanel = _newPanel;
            currentPanel.OpenPanel();
        }


        public void CloseUIObject( PanelController _newPanel) {
            _newPanel.ClosePanel();
        }


        public void OpenPreviousUIObject(PanelController _newPanel) {
            currentPanel?.ClosePanel();
            previousPanel?.OpenPanel();
        }
        #endregion
    }
}