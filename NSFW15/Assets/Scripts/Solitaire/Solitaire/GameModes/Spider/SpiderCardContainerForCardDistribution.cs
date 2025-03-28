/*
* Author:	Iris Bermudez
* Date:		14/12/2023
*/



using System.Collections.Generic;
using Solitaire.Gameplay.Cards;
using Solitaire.Gameplay.CardContainers;



namespace Solitaire.GameModes.Spider {
    public class SpiderCardContainerForCardDistribution : AbstractCardContainer {
        #region Public methods
        public override List<CardFacade> Initialize( List<CardFacade> _cards ) {
            if( _cards.Contains(null) ) {
                throw new System.NullReferenceException("There's a null element in the list of cards"
                                                                + " passed for initialization.");
            }


            return AddInitializationCards( _cards );
        }


        public override void AddCard( CardFacade _card ) {
            throw new System.NotImplementedException();
        }


        public override bool AddCards( List<CardFacade> _cards ) {
            throw new System.NotImplementedException();
        }


        public override void RemoveCard(CardFacade _card) {
            if (!_card) {
                throw new System.NullReferenceException("The card intended to be removed is null.");
            }

            cards.Remove( _card );
        }


        public override void RemoveCards( List<CardFacade> _cards ) {
            throw new System.NotImplementedException();
        }

        public List<CardFacade> GetCardsForDistribution() {
            return cards;
        }
        #endregion


        #region Protected methods
        protected override void SetUpStarterCards() {
            for (int i = 0; i <= cards.Count - 1; i++) {
                cards[i].FlipCard( false );
                cards[i].SetCanBeDragged( false );
                cards[i].ActivateParentDetection( false );
                cards[i].SetCanBeInteractable( false );
            }
        }
        #endregion
    }
}