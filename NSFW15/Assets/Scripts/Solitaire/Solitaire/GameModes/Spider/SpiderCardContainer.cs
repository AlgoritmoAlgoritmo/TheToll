/*
* Author:	Iris Bermudez
* Date:		11/12/2023
*/



using System.Collections.Generic;
using Solitaire.Gameplay.Cards;
using Solitaire.Gameplay.CardContainers;



namespace Solitaire.GameModes.Spider {
    public class SpiderCardContainer : AbstractCardContainer {
        #region Variables
        private new UnityEngine.Collider2D collider;
        #endregion


        #region MonoBehaviour methods
        private void Awake() {
            collider = GetComponent<UnityEngine.Collider2D>();
            collider.enabled = false;
        }
        #endregion


        #region Public methods
        public override List<CardFacade> Initialize( List<CardFacade> _cards ) {
            if (_cards == null || _cards.Count == 0) {
                throw new System.Exception("Cards list is empty.");

            } else if (_cards.Contains( null ) ) {
                throw new System.NullReferenceException("There's a null element in the list of cards"
                                                        + " passed for initialization.");
            
            } else if (_cards.Count < initialCardsAmount) {
                throw new System.Exception("There aren't enough cards to initialize CardContainer. "
                                            + $"It was expected to {initialCardsAmount} but received"
                                            + $" {_cards.Count} instead. ");
            }

            return AddInitializationCards( _cards );
        }


        public override void AddCard( CardFacade _card ) {
            if( !_card ) {
                throw new System.NullReferenceException("The card intended to be added "
                                                        + "is null.");
            }

            _card.RenderOnTop();
            _card.SetParentCard( GetTopCard() );
            GetTopCard()?.SetChildCard( _card );
            cards.Add( _card );
            Refresh();
        }
        

        public override bool AddCards( List<CardFacade> _cards ) {
            if( _cards.Contains(null) ) {
                throw new System.NullReferenceException("At least one of the "
                            + "elements from the list of cards passed is null.");
            }

            foreach( var auxCard in _cards ) {
                auxCard.RenderOnTop();
                auxCard.SetParentCard(GetTopCard());
                GetTopCard()?.SetChildCard(auxCard);
                cards.Add(auxCard);
            }
            
            Refresh();

            return true;
        }


        public override void RemoveCard( CardFacade _card ) {
            if( !_card ) {
                throw new System.NullReferenceException("The card intended to be removed is "
                                                            + "null.");
            }

            cards.Remove(_card);
            Refresh();
        }


        public override void RemoveCards( List<CardFacade> _cards ) {
            if (_cards.Contains(null)) {
                throw new System.NullReferenceException("At least one of the "
                                            + "elements in the list of cards passed "
                                            + "for multiple card removal is null.");
            }

            for( int i = _cards.Count - 1; i >= 0; i-- ) {
                cards.Remove( _cards[i] );
            }

            Refresh();
        }
        #endregion


        #region Protected methods
        protected override void SetUpStarterCards() {
            for( int i = 0; i <= cards.Count - 1; i++ ) {
                if( i != cards.Count - 1) {
                    FlipCard(cards[i], false);

                    cards[i].SetChildCard( cards[i + 1] );
                    cards[i + 1].SetParentCard( cards[i] );

                } else {
                    FlipCard(cards[i], true);
                }
            }
        }
        #endregion


        #region Private methods
        public override void Refresh() {
            base.Refresh();
            
            // Deactivate physics from cards
            foreach( CardFacade auxCard in cards ) {
                FlipCard( GetTopCard(), false );
            }

            // Flip up top card 
            FlipUpperCardUp();

            //  If there aren't any cards left, activate collider
            //  so it can be detected by cards
            collider.enabled = cards.Count < 1;
        }


        private void FlipUpperCardUp() {
            if( GetTopCard() ) {
                FlipCard( GetTopCard(), true );
            }
        }
        #endregion
    }
}