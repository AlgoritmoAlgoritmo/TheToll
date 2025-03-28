/*
* Author: Iris Bermudez
* GitHub: https://github.com/AlgoritmoAlgoritmo
* Date: 08/03/2025 (DD/MM/YYYY)
*/



using System.Collections.Generic;
using UnityEngine;
using Solitaire.Gameplay;
using Solitaire.Gameplay.Cards;
using System;
using Solitaire.Gameplay.CardContainers;
using Solitaire.Gameplay.Common;



namespace FNS.Gameplay.GameModes.Pyramid {
    public class PyramidGameMode : AbstractGameMode {
        #region Variables
        private PyramidClearedCardContainer pyramidClearedCardContainer;
        #endregion


        #region Public methods
        public override void Initialize( List<CardFacade> _cards ) {
            if( _cards.Contains( null ) ) {
                throw new NullReferenceException( "The list of cards passed for "
                                        + "initialization contains a null element." );

            } else if( _cards.Count < 1 ) {
                throw new IndexOutOfRangeException( "The list of cards passed for "
                                        + "initialization is empty." );
            }

            if( cardContainers is null || cardContainers.Count == 0 ) {
                cardContainers = new List<AbstractCardContainer>( FindObjectsOfType<AbstractCardContainer>() );
                SortCardContainerListByHierarchy();
            }

            List<CardFacade> auxCards = new List<CardFacade>();

            foreach( CardFacade auxCard in _cards ) {
                auxCards.Add( auxCard );
                auxCard.SubscribeToOnStartDragging( ValidateCardDragging );
                auxCard.SubscribeToCardEvent( ManageCardEvent );
            }

            foreach( AbstractCardContainer auxCardContainer in cardContainers ) {
                auxCards = auxCardContainer.Initialize( auxCards );

                if( auxCardContainer is  PyramidClearedCardContainer ) {
                    pyramidClearedCardContainer = auxCardContainer as PyramidClearedCardContainer;
                }
            }

            if( pyramidClearedCardContainer is null ) {
                pyramidClearedCardContainer = FindObjectOfType<PyramidClearedCardContainer>();

                if( pyramidClearedCardContainer is null )
                    throw new NullReferenceException( "There's no PyramidClearedCardContainer"
                                                            + " instance in the current scene." );
            }
        }

        public override void ValidateCardDragging( CardFacade _card ) {
            _card.SetCanBeDragged( CanCardBeDragged( _card ) );
        }
        #endregion


        #region Protected methods
        protected override bool CanBeChildOf( CardFacade _card, CardFacade _potentialParent ) {
            return _card.GetCardNumber() + _potentialParent.GetCardNumber() == 13;
        }

        protected override bool CanCardBeDragged( CardFacade _card ) {
            if( GetCardContainer( _card ) is PyramidCardContainer ) {
                if( ( GetCardContainer( _card ) as PyramidCardContainer ).IsBlocked() ) {
                    Debug.Log( "The card belongs to a blocked container." );
                    
                    return false;
                }
            }

            if( _card.GetCardNumber() != 13 ) {
                if( GetCardContainer( _card ) is PyramidCardContainer )
                    return !( GetCardContainer( _card ) as PyramidCardContainer ).IsBlocked();

                return true;
            }

            ClearCard( _card );

            return false;
        }

        protected override void ManageCardEvent( CardFacade _placedCard, GameObject _detectedGameObject ) {
            Debug.Log("Card dropped.");

            if( _detectedGameObject is null ) {
                GetCardContainer( _placedCard ).Refresh();
                _placedCard.OnInvalidDrag?.Invoke();

            //  CASE: colliding object is a Card
            } else if( _detectedGameObject.layer == LayerMask.NameToLayer( Constants.CARDS_LAYER_NAME ) ) {
                DropCardOnAnotherCard( _placedCard, _detectedGameObject );
            }
        }
        #endregion


        #region Private methods
        private void DropCardOnAnotherCard( CardFacade _cardFacade, GameObject _detectedGameObject ) {
            var potentialParent = _detectedGameObject.GetComponent<CardFacade>();

            if( !( potentialParent is null ) ) {
                if( GetCardContainer( potentialParent ) is PyramidCardContainer ) {
                    if( ( GetCardContainer( potentialParent ) as PyramidCardContainer ).IsBlocked() ) {
                        Debug.Log( "The potential parent card belongs to a blocked container." );
                        GetCardContainer( _cardFacade ).Refresh();
                        _cardFacade.OnInvalidDrag?.Invoke();

                        return;
                    }
                }

                if( CanBeChildOf( _cardFacade, potentialParent ) ) {
                    Debug.Log( "Both cards add 13" );
                    ClearCard( _cardFacade );
                    ClearCard( potentialParent );
                    CheckIfGameIsOver();

                } else {
                    Debug.Log( $"Both cards ({_cardFacade.GetCardNumber()}, "
                                +$"{potentialParent.GetCardNumber()}) don't add 13" );
                    GetCardContainer( _cardFacade ).Refresh();
                    _cardFacade.OnInvalidDrag?.Invoke();
                }
            }
        }

        private void ClearCard( CardFacade _cardFacade ) {
            GetCardContainer( _cardFacade ).RemoveCard( _cardFacade );
            pyramidClearedCardContainer.AddCard( _cardFacade );
            OnCardsCleared.Invoke( new List<CardFacade> { _cardFacade } );
        }

        private void CheckIfGameIsOver() {
            Debug.Log( "Checking if game is over..." );

            if( CheckIfAllPyramidCardContainersAreEmpty() ) {
                Debug.Log( "All PyramidCardContainers are empty!" );
                List<CardFacade> cardsToClear = new List<CardFacade>();
                
                foreach( var auxContainer in cardContainers ) {
                    if( auxContainer is PyramidDeckCardContainer ) {
                        cardsToClear = (auxContainer as PyramidDeckCardContainer ).GetCards();
                    }
                }

                for( int index = cardsToClear.Count -1; index >= 0; index-- ) {
                    ClearCard( cardsToClear[index] );
                }
            }
        }

        private bool CheckIfAllPyramidCardContainersAreEmpty() {
            foreach( var auxContainer in cardContainers ) {
                if( auxContainer is PyramidCardContainer
                        && auxContainer.GetCardCount() > 0 ) {
                    return false;
                }
            }

            return true;
        }
        #endregion
    }
}