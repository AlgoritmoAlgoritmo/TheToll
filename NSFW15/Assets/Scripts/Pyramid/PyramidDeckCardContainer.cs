/*
* Author: Iris Bermudez
* GitHub: https://github.com/AlgoritmoAlgoritmo
* Date: 10/03/2025 (DD/MM/YYYY)
*/



using Solitaire.Gameplay.CardContainers;
using Solitaire.Gameplay.Cards;
using System.Collections.Generic;
using UnityEngine;



namespace FNS.Gameplay.GameModes.Pyramid {
    public class PyramidDeckCardContainer : AbstractCardContainer {
        #region Variables
        [SerializeField]
        private RectTransform originRectTransform;
        [SerializeField]
        private RectTransform targetRectTransform;


        private List<CardFacade> displayedCards = new List<CardFacade>();
        private List<CardFacade> hiddenCards = new List<CardFacade>();
        #endregion


        #region Public methods
        public override List<CardFacade> Initialize( List<CardFacade> _cards ) {
            if( _cards == null || _cards.Count == 0 ) {
                throw new System.Exception( "Cards list is empty." );

            } else if( _cards.Contains( null ) ) {
                throw new System.NullReferenceException( "There's a null element in the list of cards"
                                                        + " passed for initialization." );

            } else if( _cards.Count < initialCardsAmount ) {
                throw new System.Exception( "There aren't enough cards to initialize CardContainer. "
                                            + $"It was expected to {initialCardsAmount} but received"
                                            + $" {_cards.Count} instead." );
            }

            return AddInitializationCards( _cards );
        }

        public override void AddCard( CardFacade _card ) {
            throw new System.NotImplementedException();
        }

        public override bool AddCards( List<CardFacade> _cards ) {
            throw new System.NotImplementedException();
        }        

        public override void RemoveCard( CardFacade _card ) {
            if( !_card ) {
                throw new System.NullReferenceException( 
                                    "The card intended to be removed is null." );
            }

            displayedCards.Remove( _card );
            cards.Remove( _card );
            Refresh();
        }

        public override void RemoveCards( List<CardFacade> _cards ) {
            foreach( var auxCard in _cards ) {
                RemoveCard( auxCard );
            }
        }

        public void PassCard() {
            if( hiddenCards.Count > 0 ) {
                DisplayCard( hiddenCards[0] );

            } else if( displayedCards.Count > 0 ) {
                ResetCards();
            }
        }

        public override void Refresh() {
            if( cards.Count > 0 ) {
                short index = 0;

                foreach( var auxCard in cards ) {
                    auxCard.transform.position = GetCardPosition( index );
                    auxCard.RenderOnTop();
                    index++;
                }

                if( displayedCards.Count > 0 ) {
                    ActivateCard( displayedCards[displayedCards.Count - 1], true );
                }
            }
        }

        public List<CardFacade> GetCards() {
            return cards;
        }
        #endregion


        #region Protected method
        protected override Vector2 GetCardPosition( int _index ) {
            if( displayedCards.Contains( cards[_index] ) ) {
                return targetRectTransform.position;

            } else {
                return originRectTransform.position;
            }
        }

        protected override void SetUpStarterCards() {
            foreach( var auxCard in cards ) {
                HideCard( auxCard );
            }
        }
        #endregion


        #region Private methods
        private void DisplayCard( CardFacade _card ) {
            if( displayedCards.Count > 0 )
                ActivateCard( displayedCards[displayedCards.Count - 1], false);

            displayedCards.Add( _card );
            hiddenCards.Remove( _card );
            _card.transform.GetComponent<RectTransform>().position = targetRectTransform.position;

            FlipCard( _card, true );
            ActivateCard( _card, true );
        }

        private void HideCard( CardFacade _card ) {
            displayedCards.Remove( _card );
            hiddenCards.Add( _card );

            _card.transform.GetComponent<RectTransform>().position = originRectTransform.position;

            FlipCard( _card, false );
            ActivateCard( _card, false );
        }

        private void ResetCards() {
            short displayedCardsAmount = (short)displayedCards.Count;

            for( int i = 0; i < displayedCardsAmount; i++ ) {
                HideCard( displayedCards[0] );
            }
        }

        private void ActivateCard( CardFacade _card, bool _isActive ) {
            _card.SetCanBeInteractable( _isActive );
            _card.ActivatePhysics( _isActive );
        }
        #endregion
    }
}