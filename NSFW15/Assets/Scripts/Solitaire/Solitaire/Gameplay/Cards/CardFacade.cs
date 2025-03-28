/*
* Author:	Iris Bermudez
* Date:		06/12/2023
*/



using System;
using UnityEngine;
using UnityEngine.Events;



namespace Solitaire.Gameplay.Cards {
    public class CardFacade : MonoBehaviour {
        #region Variables
        [SerializeField]
        private CardData cardData;
        [SerializeField]
        private CardView cardView;
        [SerializeField]
        private CardPhysics cardPhysics;


        private CardFacade childCard;
        public CardFacade ChildCard {
            get => childCard;
        }

        private CardFacade parentCard;
        public CardFacade ParentCard {
            get => parentCard;
        }


        public event Action<CardFacade> OnStartDrag;
        public event Action<CardFacade, GameObject> OnCardEvent;

        public UnityEvent OnValidDrag = new UnityEvent();
        public UnityEvent OnInvalidDrag = new UnityEvent();
        public UnityEvent OnValidDrop = new UnityEvent();
        public UnityEvent OnInvalidDrop = new UnityEvent();
        #endregion


        #region MonoBehaviour Methods
        private void Start() {
            if( cardPhysics ) {
                cardPhysics.OnStartDragging += InvokeOnStartDragEvent;
                cardPhysics.OnCardEvent += InvokeOnCardEvent;
                cardPhysics.OnDragging += MoveToPosition;
            }
        }
        #endregion


        #region Public methods
        public void ConfigureCard( CardData _cardData, Sprite _backSprite,
                                                        Sprite _frontSprite) {
            SetCardData( _cardData );
            SetBackSprite(  _backSprite );
            SetFrontSprite( _frontSprite );
            FlipCard( false );
        }

        public short GetCardNumber() {
            return cardData.number;
        }

        public string GetSuit() {
            return cardData.suit;
        }

        public string GetColor() {
            return cardData.color;
        }

        public string GetID() {
            return cardData.id;
        }

        public bool IsFacingUp() {
            return cardView.IsFacingUp;
        }

        public void SetCardData( CardData _cardData ) {
            cardData = _cardData;
        }

        public void SetFrontSprite( Sprite frontSprite ) {
            cardView?.SetFrontSprite( frontSprite );
        }

        public void SetBackSprite( Sprite backtSprite ) {
            cardView?.SetBackSprite( backtSprite );
        }

        public void FlipCard( bool _facingUp ) {
            cardView.FlipCard( _facingUp );
        }

        public void SubscribeToOnStartDragging( Action<CardFacade> action ) {
            OnStartDrag += action;
        }

        public void SubscribeToCardEvent( Action<CardFacade, GameObject> action ) {
            OnCardEvent += action;
        }

        public void InvokeOnStartDragEvent() {
            RenderOnTop();
            OnStartDrag( this );
        }

        public void InvokeOnCardEvent( GameObject _detectedGameObject ) {
            OnCardEvent( this, _detectedGameObject );
        }

        public void SetCanBeDragged( bool _canBeDragged ) {
            cardPhysics.SetCanBeDragged( _canBeDragged );
        }

        public void SetCanBeInteractable( bool _isInteractable ) {
            cardView.SetInteractable( _isInteractable );
        }

        public void ActivateParentDetection( bool _active ) {
            cardPhysics.ActivateParentDetection( _active );
        }

        public void ActivatePhysics( bool _activate ) {
            cardPhysics.ActivatePhysicsInteractions( _activate );
        }

        public void MoveToPosition( Vector3 _newPositionOffset ) {
            if( ChildCard ) {
                ChildCard.MoveToPosition( _newPositionOffset );
            }

            transform.position += _newPositionOffset;
        }

        public void RenderOnTop() {
            cardView.RenderOnTop(transform);

            if (ChildCard) {
                ChildCard.RenderOnTop();
            }
        }

        public void ActivateChildsPhysics( bool _activate ) {
            var auxChild = ChildCard;

            while( auxChild ) {
                auxChild.ActivatePhysics( _activate );
                auxChild = auxChild.ChildCard;
            }
        }
        #endregion


        #region Setters
        public void SetChildCard( CardFacade _newChild ) {
            if( childCard ) {
                childCard.parentCard = null;
            }

            childCard = _newChild;

            if( childCard ) {
                childCard.parentCard = this;
            }
        }


        public void SetParentCard( CardFacade _newParent ) {
            if( parentCard ) {
                parentCard.childCard = null;
            }

            parentCard = _newParent;

            if( parentCard ) {
                parentCard.childCard = this;
            }
        }
        #endregion
    }
}