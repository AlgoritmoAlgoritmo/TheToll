/*
* Author:	Iris Bermudez
* Date:		11/06/2024
*/



using System;
using System.Collections.Generic;
using UnityEngine;
using Solitaire.Gameplay;
using Solitaire.Gameplay.Cards;
using Solitaire.Gameplay.CardContainers;
using Solitaire.Gameplay.Common;



namespace Solitaire.GameModes.Klondike {
    public class KlondikeGameMode : AbstractGameMode {
        #region Public methods
        public override void ValidateCardDragging( CardFacade _card ) {
            bool canBeDragged = CanCardBeDragged(_card);
            _card.SetCanBeDragged(canBeDragged);

            if (canBeDragged) {
                // Deactivating childs physics to avoid the parent to detect them
                // during dragging
                _card.ActivateChildsPhysics(false);
                _card.ActivatePhysics(true);
                _card.OnValidDrag?.Invoke();

            } else {
                _card.OnInvalidDrag?.Invoke();
            }
        }
        #endregion


        #region Protected methods
        protected override bool CanBeChildOf(CardFacade _card, CardFacade _potentialParent) {
            return _potentialParent.GetCardNumber() == _card.GetCardNumber() + 1
                        &&  !_potentialParent.GetColor().Equals( _card.GetColor() );
        }
    

        protected override bool CanCardBeDragged(CardFacade _card) {
            if (!_card.IsFacingUp()) {
                return false;
            }

            if (!_card.ChildCard) {
                return true;
            }

            // Check every single child card
            CardFacade auxCard = _card;

            while (auxCard.ChildCard) {
                // Check if card number and suit are incorrect
                if ( !CanBeChildOf(auxCard.ChildCard, auxCard)
                            || auxCard.GetColor().Equals(auxCard.ChildCard.GetColor())) {

                    return false;
                }

                auxCard = auxCard.ChildCard;
            }

            // Passed child checking so return true
            return true;
        }


        protected override void ManageCardEvent( CardFacade _placedCard, GameObject _detectedGameObject ) {
            if (_detectedGameObject is null) {
                GetCardContainer(_placedCard).Refresh();
                _placedCard.OnInvalidDrag?.Invoke();

            //  CASE: colliding object is a Card
            } else if (_detectedGameObject.layer == LayerMask.NameToLayer(Constants.CARDS_LAYER_NAME)) {
                DropCardOnAnotherCard( _placedCard, _detectedGameObject );

            //  CASE: colliding object is a KlondikeCardContainer
            } else if (_detectedGameObject.layer == LayerMask.NameToLayer(Constants.CARD_CONTAINERS_LAYER_NAME)) {
                DropCardOnKlondikeCardContainer( _placedCard, _detectedGameObject );

            //  CASE: colliding object is a KlondikeCompletedColumnContainer
            } else if (_detectedGameObject.layer == LayerMask.NameToLayer(
                                                    Constants.COMPLETED_CARD_COLUMN_CONTAINER_LAYER_NAME)) {
                DropCardOnKlondikeCompletedColumnContainer( _placedCard, _detectedGameObject );
            }
        }
        #endregion


        #region Private methods
        private void DropCardOnAnotherCard( CardFacade _placedCard, GameObject _detectedGameObject ) {
            CardFacade detectedCardFacade = _detectedGameObject.GetComponent<CardFacade>();

            if( !detectedCardFacade )
                throw new Exception( $"The object {_detectedGameObject.name} doesn't"
                                            + $" have a CardFacade component." );

            // Logic to move card from one container to another
            // Case: Card CANNOT be child of potential parent
            if( !CanBeChildOf( _placedCard, detectedCardFacade )
                                || _placedCard.ParentCard == detectedCardFacade ) {
                GetCardContainer( _placedCard ).Refresh();
                _placedCard.OnInvalidDrop?.Invoke();

                // Case: Card CAN be child of potential parent
            } else {
                MoveCardToNewContainer( _placedCard, GetCardContainer( detectedCardFacade ) );

                _placedCard.OnValidDrop?.Invoke();
            }
        }


        private void DropCardOnKlondikeCardContainer( CardFacade _placedCard, GameObject _detectedGameObject ) {
            // IF PLACED CARD IS A KING
            if( _placedCard.GetCardNumber() == 13 ) {
                MoveCardToNewContainer( _placedCard,
                                        _detectedGameObject.GetComponent<AbstractCardContainer>() );
                _placedCard.OnValidDrop?.Invoke();

                // ELSE RESET CARD POSITION
            } else {
                GetCardContainer( _placedCard ).Refresh();
                _placedCard.OnInvalidDrop?.Invoke();
            }
        }


        private void DropCardOnKlondikeCompletedColumnContainer( CardFacade _placedCard, GameObject _detectedGameObject ) {
            AbstractCardContainer detectedCardContainer
                                    = _detectedGameObject.GetComponent<AbstractCardContainer>();
            CardFacade detectedContainerTopCard = detectedCardContainer.GetTopCard();

            // IF CONTAINER IS EMPTY
            if( detectedCardContainer.GetTopCard() is null ) {
                if( _placedCard.GetCardNumber() == 1 ) {
                    MoveCardToNewContainer( _placedCard, detectedCardContainer );
                    _placedCard.OnValidDrop?.Invoke();
                    OnCardsCleared.Invoke( new List<CardFacade>() { _placedCard } );

                    // RESET CARD POSITION
                } else {
                    GetCardContainer( _placedCard ).Refresh();
                    _placedCard.OnInvalidDrop?.Invoke();
                }

                // ELSE IF CONTAINER IS NOT EMPTY, PLACED CARD IS THE NEXT IN THE SEQUENCE
                // AND HAS NO CHILD CARDS
            } else if( _placedCard.GetCardNumber() == detectedContainerTopCard.GetCardNumber() + 1
                            && _placedCard.GetSuit().Equals( detectedContainerTopCard.GetSuit() )
                            && _placedCard.ChildCard is null ) {
                MoveCardToNewContainer( _placedCard, _detectedGameObject.GetComponent<AbstractCardContainer>() );
                _placedCard.OnValidDrop?.Invoke();
                OnCardsCleared.Invoke( new List<CardFacade>() { _placedCard } );


                // ELSE, IF IT'S NOT THE NEXT IN THE SEQUENCE, RESET CARD POSITION
            } else {
                GetCardContainer( _placedCard ).Refresh();
                _placedCard.OnInvalidDrop?.Invoke();
            }
        }


        private void MoveCardToNewContainer( CardFacade _card, AbstractCardContainer _cardContainer ) {
            // Recursively check childs
            var auxCardFacade = _card;

            while (auxCardFacade != null) {
                // 1- Remove card from its card container
                GetCardContainer(auxCardFacade).RemoveCard(auxCardFacade);

                // 2- Add card to new card container
                _cardContainer.AddCard(auxCardFacade);

                // 3- Set ChildCard as card to check on next loop
                auxCardFacade = auxCardFacade.ChildCard;
            }
        }
        #endregion
    }
}