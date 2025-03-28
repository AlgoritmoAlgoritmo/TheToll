/*
* Author: Iris Bermudez
* GitHub: https://github.com/AlgoritmoAlgoritmo
* Date: 12/03/2024 (DD/MM/YYYY)
*/



using System.Collections.Generic;
using Solitaire.Gameplay.CardContainers;
using Solitaire.Gameplay.Cards;



namespace FNS.Gameplay.GameModes.Pyramid {
    public class PyramidClearedCardContainer : AbstractCardContainer {
        #region Variables

        #endregion

        #region Public methods
        public override List<CardFacade> Initialize( List<CardFacade> _cards ) {
            // INTENTIONALLY DOES NOTHING SINCE IT MUST START WITH 0 CARDS
            return _cards;
        }

        public override void AddCard( CardFacade _card ) {
            if( !_card ) {
                throw new System.NullReferenceException( "The card intended to be added "
                                                        + "is null." );
            }

            _card.RenderOnTop();
            _card.SetParentCard( GetTopCard() );
            GetTopCard()?.SetChildCard( _card );
            cards.Add( _card );
            _card.ActivatePhysics( false );

            Refresh();
        }

        public override bool AddCards( List<CardFacade> _cards ) {
            throw new System.NotImplementedException();
        }

        public override void RemoveCard( CardFacade _card ) {
            throw new System.NotImplementedException();
        }

        public override void RemoveCards( List<CardFacade> _cards ) {
            throw new System.NotImplementedException();
        }
        #endregion


        #region Protected methods 
        protected override void SetUpStarterCards() {
            // INTENTIONALLY UNIMPLEMENTED SINCE IT WON'T BE USED
            throw new System.NotImplementedException();
        }
        #endregion
    }
}