/*
* Author:	Iris Bermudez
* Date:		12/06/2024
*/



using System.Collections.Generic;
using Solitaire.Gameplay.Cards;
using Solitaire.Gameplay.CardContainers;



namespace Solitaire.GameModes.Klondike {
    public class KlondikeCompletedColumnContainer : AbstractCardContainer {
        #region Public methods
        public override List<CardFacade> Initialize( List<CardFacade> _cards ) {
            // INTENTIONALLY DOES NOTHING SINCE IT MUST START WITH 0 CARDS
            return _cards;
        }

        public override void AddCard(CardFacade _card) {
            if (!_card) {
                throw new System.NullReferenceException("The card intended to be added "
                                                        + "is null.");
            }

            _card.RenderOnTop();
            _card.SetParentCard(GetTopCard());
            GetTopCard()?.SetChildCard(_card);
            cards.Add(_card);
            _card.ActivatePhysics(false);            

            Refresh();
        }

        public override bool AddCards(List<CardFacade> _cards) {
            throw new System.NotImplementedException();
        }        

        public override void RemoveCard(CardFacade _card) {
            if( !_card ) {
                throw new System.NullReferenceException( "The card intended to be "
                                                            + "removed is null." );
            }

            cards.Remove( _card );
            Refresh();
        }

        public override void RemoveCards(List<CardFacade> _cards) {
            throw new System.NotImplementedException();
        }
        #endregion


        #region Protected methods
        protected override void SetUpStarterCards() {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}