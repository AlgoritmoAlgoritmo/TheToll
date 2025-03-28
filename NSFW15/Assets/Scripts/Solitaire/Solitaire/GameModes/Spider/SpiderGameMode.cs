/*
* Author:	Iris Bermudez
* Date:		08/12/2023
*/



using System;
using System.Collections.Generic;
using UnityEngine;
using Solitaire.Gameplay.Cards;
using Solitaire.Gameplay.Common;
using Solitaire.Gameplay.CardContainers;
using Solitaire.Gameplay;



namespace Solitaire.GameModes.Spider {
    public class SpiderGameMode : AbstractGameMode {
        #region Variables
        [SerializeField]
        protected List<AbstractCardContainer> completedColumnContainers;
        #endregion


        #region Public methods
        public override void ValidateCardDragging( CardFacade _card ) {
            bool canBeDragged = CanCardBeDragged( _card );
            _card.SetCanBeDragged( canBeDragged );

            if( canBeDragged ) {
                // Deactivating childs physics to avoid the parent to detect them
                // during dragging
                _card.ActivateChildsPhysics( false );
                _card.ActivatePhysics( true );
                _card.OnValidDrag?.Invoke();

            } else {
                _card.OnInvalidDrag?.Invoke();
            }
        }

        public virtual void DistributeCardsBetweenCardContainers( SpiderCardContainerForCardDistribution _container ) {
            if( _container.GetCardsForDistribution().Contains( null ) ) {
                throw new NullReferenceException( "There is a null element in the list of cards "
                                                    + "passed for distribution." );
            }

            short cardContainerIndex = 0;
            bool hasToStopLoop = false;
            List<CardFacade> cardsToDistribute = _container.GetCardsForDistribution();

            for( int i = cardsToDistribute.Count - 1; i >= 0; i-- ) {
                while( !( cardContainers[cardContainerIndex] is SpiderCardContainer ) ) {
                    cardContainerIndex++;
                    if( cardContainerIndex == cardContainers.Count ) {
                        hasToStopLoop = true;
                        break;
                    }
                }

                if( !hasToStopLoop ) {
                    cardsToDistribute[i].SetCanBeInteractable( true );
                    cardContainers[cardContainerIndex].AddCard( cardsToDistribute[i] );
                    cardContainerIndex++;
                } else {
                    break;
                }
            }

            cardContainers.Remove( _container );
            Destroy( _container.gameObject );
        }
        #endregion


        #region Protected methods
        protected override void ManageCardEvent(CardFacade _placedCard,
                                                        GameObject _detectedGameObject) {
            //  CASE: no object colliding
            if (_detectedGameObject is null) {
                GetCardContainer( _placedCard ).Refresh();
                _placedCard.OnInvalidDrag?.Invoke();



            //  CASE: colliding object is a Card
            } else if (_detectedGameObject.layer == LayerMask.NameToLayer(Constants.CARDS_LAYER_NAME)) {
                CardFacade detectedCardFacade = _detectedGameObject.GetComponent<CardFacade>();

                if( !detectedCardFacade )
                    throw new Exception( $"The object {_detectedGameObject.name} doesn't"
                                                + $" have a CardFacade component.");

                // Logic to move card from one container to another
                // CASE: Card CANNOT be child of potential parent                
                if( !CanBeChildOf(_placedCard, detectedCardFacade)
                                    || _placedCard.ParentCard == detectedCardFacade) {
                    GetCardContainer(_placedCard).Refresh();
                    _placedCard.OnInvalidDrop?.Invoke();

                // CASE: Card CAN be child of potential parent
                } else {
                    MoveCardToNewContainer( _placedCard, GetCardContainer(detectedCardFacade) );

                    _placedCard.OnValidDrop?.Invoke();
                }



            //  CASE:  detected object is a CardContainer
            } else if ( _detectedGameObject.layer == LayerMask.NameToLayer(
                                                                Constants.CARD_CONTAINERS_LAYER_NAME)) {
                   var detectedCardContainer = _detectedGameObject
                                                    .GetComponent<AbstractCardContainer>();
                   if( !detectedCardContainer )
                        throw new Exception($"The object {_detectedGameObject.name} "
                                    + $"doesn't have an AbstractCardContainer component.");

                   MoveCardToNewContainer(_placedCard, detectedCardContainer);
                    _placedCard.OnValidDrop?.Invoke();



            //  CASE: detected object isn't a Card nor a CardContainer
            } else {
                throw new Exception($"The object {_detectedGameObject.name}'s layer ("
                                    + LayerMask.LayerToName(_detectedGameObject.layer)
                                    + ") is not valid.");
            }


            _placedCard.ActivateChildsPhysics( true );
            CheckIfColumnWasCompleted( _placedCard );
        }

        protected override bool CanBeChildOf( CardFacade _card, CardFacade _potentialParent ) {
            return _potentialParent.GetCardNumber() == _card.GetCardNumber() + 1;
        }

        protected override bool CanCardBeDragged( CardFacade _card ) {
            if(  !_card.IsFacingUp() ) {
                return false;
            }

            if ( !_card.ChildCard ) {
                return true;
            }

            // Check every single child card
            CardFacade auxCard = _card;

            while( auxCard.ChildCard ) {
                // Check if card number and suit are incorrect
                if( !CanBeChildOf(auxCard.ChildCard, auxCard)
                            || !auxCard.GetSuit().Equals(auxCard.ChildCard.GetSuit()) ) {
                    return false;
                }

                auxCard = auxCard.ChildCard;
            }

            // Passed child checking so return true
            return true;
        }
        #endregion


        #region Protected methods
        protected virtual void MoveCardToNewContainer( CardFacade _card, AbstractCardContainer _cardContainer ) {
            // Recursively check childs
            var auxCardFacade = _card;

            while( auxCardFacade != null ) {
                // 1- Remove card from its card container
                GetCardContainer( auxCardFacade ).RemoveCard( auxCardFacade );

                // 2- Add card to new card container
                _cardContainer.AddCard( auxCardFacade );

                // 3- Set ChildCard as card to check on next loop
                auxCardFacade = auxCardFacade.ChildCard;
            }
        }

        protected virtual void CheckIfColumnWasCompleted( CardFacade _placedCard ) {
            List<CardFacade> columnOfCards = GetCardColumn( _placedCard );

            if( IsColumnCompleted( columnOfCards ) ) {
                MoveColumnToCompletedColumnContainer( columnOfCards );
                OnCardsCleared.Invoke( columnOfCards );
            }
        }

        protected List<CardFacade> GetCardColumn( CardFacade _card ) {
            List<CardFacade> columnOfCards = new List<CardFacade>();
            CardFacade auxCard = _card;

            // Checking if parent numbers are consecutive
            // And if there's a king at the end of them
            while( auxCard ) {
                if( auxCard.ParentCard
                            && auxCard.GetCardNumber() != 13
                            && auxCard.GetCardNumber() + 1 == auxCard.ParentCard.
                                                                GetCardNumber() ) {
                    auxCard = auxCard.ParentCard;

                } else {
                    break;
                }
            }

            // Checking if child numbers are consecutive
            // And if there's an as at the end of them
            while( auxCard ) {
                if( auxCard.GetCardNumber() == 1 ) {
                    columnOfCards.Add( auxCard );


                }
                if( auxCard.GetCardNumber() != 1
                                          && auxCard.ChildCard != null
                                          && auxCard.GetCardNumber() ==
                                              auxCard.ChildCard.GetCardNumber() + 1 ) {
                    columnOfCards.Add( auxCard );
                    auxCard = auxCard.ChildCard;

                } else {
                    break;
                }
            }

            return columnOfCards;
        }

        protected bool IsColumnCompleted( List<CardFacade> _columnOfCards ) {
            return _columnOfCards.Count == 13
                    && _columnOfCards[0].GetCardNumber() == 13
                    && _columnOfCards[12].GetCardNumber() == 1;
        }
        #endregion


        #region Private methods
        protected virtual void MoveColumnToCompletedColumnContainer( List<CardFacade> _cards ) {
            AbstractCardContainer auxCardContainer = GetCardContainer( _cards[0] );
            _cards[0].SetParentCard( null );

            foreach( CardFacade auxCard in _cards ) {
                auxCard.ActivatePhysics( false );
                auxCard.SetCanBeDragged( false );
            }

            auxCardContainer.RemoveCards( _cards );

            completedColumnContainers[completedColumnContainers.Count - 1].AddCards( _cards );
            completedColumnContainers.RemoveAt( completedColumnContainers.Count - 1 );
        }
        #endregion
    }
}