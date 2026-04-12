/*
* GitHub: https://github.com/AlgoritmoAlgoritmo
* Date: 10/04/2026 (DD/MM/YYYY)
*/



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



namespace Jam15.Player {
    public class TriggerEvent : MonoBehaviour {
        #region Properties
        [SerializeField]
        private UnityEvent OnTriggerEnterEvent = new UnityEvent();
        [SerializeField]
        private UnityEvent OnTriggerExitEvent = new UnityEvent();
        #endregion

        #region Functions
        private void OnTriggerEnter( Collider other ) {
            OnTriggerEnterEvent.Invoke();
        }


        private void OnTriggerExit( Collider other ) {
            OnTriggerExitEvent.Invoke();
        }
        #endregion

    }
}