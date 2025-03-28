/*
* Author: Iris Bermudez
* GitHub: https://github.com/AlgoritmoAlgoritmo
* Date: 07/03/2025 (DD/MM/YYYY)
*/



using System.Collections.Generic;
using UnityEngine;
using Solitaire.Gameplay.CardContainers;
using Solitaire.Gameplay.Cards;



namespace FNS.Gameplay.GameModes.Pyramid {
    public class PyramidCardContainer : AbstractCardContainer {
        #region Variables
        [SerializeField]
        private List<PyramidCardContainer> upperBlockingCardContainers;
        #endregion


        #region Public AbstractCardContainer methods
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
            cards.Remove( _card );
        }

        public override void RemoveCards( List<CardFacade> _cards ) {
            throw new System.NotImplementedException();
        }

        public bool IsBlocked() {
            foreach( var auxContainer in upperBlockingCardContainers ) {
                if( auxContainer.GetCardCount() != 0 )
                    return true;
            }

            return false;
        }
        #endregion


        #region Protected AbstractCardContainer methods
        protected override void SetUpStarterCards() {
            short counter = 0;

            foreach( var auxCard in cards ) {
                counter++;
                FlipCard( auxCard, true );
                auxCard.GetComponent<RectTransform>().position = GetComponent<RectTransform>().position;
            }
        }
        #endregion
    }
}