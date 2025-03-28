/*
* Author:	Iris Bermudez
* Date:		15/12/2023
*/



using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



namespace Solitaire.Gameplay.Cards {
    public class CardPhysics : MonoBehaviour, IBeginDragHandler, IDragHandler,
                                                                    IEndDragHandler {
        #region Variables
        [SerializeField]
        private bool canBeDragged;
        public bool CanBeDragged {
            get => canBeDragged;
        }

        private Collider2D attachedCollider2D;
        private List<Collider2D> detectedColliders = new List<Collider2D>();

        public event Action OnStartDragging;
        public event Action<Vector3> OnDragging;
        public event Action<GameObject> OnCardEvent;
        #endregion



        #region MonoBehaviour/Dragging methods
        private void Awake() {
            attachedCollider2D = GetComponent<Collider2D>();
        }

        public void OnBeginDrag( PointerEventData eventData ) {
            ActivateParentDetection( true );
            OnStartDragging();
        }


        public void OnDrag( PointerEventData eventData ) {
            if ( canBeDragged ) {
                OnDragging( (Vector3)eventData.delta );
            }
        }


        public void OnEndDrag( PointerEventData eventData ) {
            if ( canBeDragged ) {
                InvokeOnCardPlacedAction();
            }
        }


        private void OnTriggerEnter2D ( Collider2D collision ) {
            if( canBeDragged )
                detectedColliders.Add( collision );
        }


        private void OnTriggerExit2D( Collider2D collision ) {
            if( canBeDragged )
                detectedColliders.Remove( collision );
        }
        #endregion



        #region Public Methods
        public void SetCanBeDragged( bool _canBeDragged ) {
            canBeDragged = _canBeDragged;
        }


        public void ActivateParentDetection( bool _activate ) {
            attachedCollider2D.isTrigger = _activate;
        }


        public void ActivatePhysicsInteractions( bool _activate ) {
            attachedCollider2D.enabled = _activate;
        }
        #endregion



        #region Private methods
        private void InvokeOnCardPlacedAction() {            
            // Pass closest collider object through Event
            OnCardEvent( GetClosestCollidingGameObject() );            
            detectedColliders.Clear();
        }


        private GameObject GetClosestCollidingGameObject() {
            //  If there's no object detected, return null
            if( detectedColliders.Count == 0 ) {
                return null;

            //  If there's only 1 detected object, return it
            } else if( detectedColliders.Count == 1 ) {
                return detectedColliders[0].gameObject;


            //  If there are multiple detected objects, return the closest one
            } else {
                return GetClosestDetectedObject();
            }
        }


        private GameObject GetClosestDetectedObject(  ) {
            GameObject closestObject = detectedColliders[0].gameObject;
            float closestDistance = Vector3.Distance(transform.position,
                                        closestObject.transform.position);

            for( int i = 1; i < detectedColliders.Count; i++ ) {
                if (Vector3.Distance(transform.position,
                        detectedColliders[i].transform.position) < closestDistance) {

                    closestObject = detectedColliders[i].gameObject;
                    closestDistance = Vector3.Distance(transform.position,
                                                closestObject.transform.position);
                }
            }

            return closestObject;
        }
        #endregion
    }
}