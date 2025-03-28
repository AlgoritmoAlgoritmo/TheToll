/*
* Author:	Iris Bermudez
* Date:		12/06/2024
*/



using UnityEngine;
using System.Collections.Generic;
using Solitaire.Gameplay.CardContainers;
using Solitaire.Gameplay.Cards;



namespace Solitaire.GameModes.Klondike {
    public class KlondikeCardContainer : AbstractCardContainer {
        #region Variables
        private new Collider2D collider;
        #endregion


        #region MonoBehaviour methods
        private void Awake() {
            collider = GetComponent<Collider2D>();
            collider.enabled = false;
        }
        #endregion


        #region Public methods
        public override List<CardFacade> Initialize(List<CardFacade> _cards) {
            if (_cards == null || _cards.Count == 0) {
                throw new System.Exception("Cards list is empty.");

            } else if (_cards.Contains(null)) {
                throw new System.NullReferenceException("There's a null element in the list of cards"
                                                        + " passed for initialization.");

            } else if (_cards.Count < initialCardsAmount) {
                throw new System.Exception("There aren't enough cards to initialize CardContainer. "
                                            + $"It was expected to {initialCardsAmount} but received"
                                            + $" {_cards.Count} instead. ");
            }

            return AddInitializationCards(_cards);
        }

        public override void AddCard(CardFacade _card) {
            if (!_card) {
                throw new System.NullReferenceException( "The card intended to be added "
                                                        + "is null.");
            }

            _card.RenderOnTop();
            _card.SetParentCard(GetTopCard());
            GetTopCard()?.SetChildCard(_card);
            cards.Add(_card);

            Refresh();
            CheckAndFlipUpperCard();
        }

        public override void RemoveCard(CardFacade _card) {
            if (!_card) {
                throw new System.NullReferenceException( "The card intended to be removed is null." );
            }

            cards.Remove(_card);
            Refresh();
            CheckAndFlipUpperCard();
        }

        public override void RemoveCards(List<CardFacade> _cards) {
            throw new System.NotImplementedException();
        }
                
        public override bool AddCards(List<CardFacade> _cards) {
            throw new System.NotImplementedException();
        }
        #endregion


        #region Protected methods
        protected override void SetUpStarterCards() {
            short counter = 0;

            foreach( var auxCard in cards ) {
                counter++;
                FlipCard( auxCard, !(counter < cards.Count));
            }
        }

        public override void Refresh() {
            base.Refresh();

            //  If there aren't any cards left, activate collider
            //  so it can be detected by cards
            collider.enabled = cards.Count < 1;
        }
        #endregion


        #region Private methods
        private void CheckAndFlipUpperCard() {
            if( GetTopCard() ) {
                FlipCard( GetTopCard(), true );
            }
        }
        #endregion
    }
}