/*
* Author:	Iris Bermudez
* Date:		07/02/2024
*/



using Solitaire.Gameplay.Cards;
using System.Collections.Generic;
using UnityEngine;



namespace Solitaire.Gameplay.CardContainers {
    public class BasicCardContainer : AbstractCardContainer {
        #region Public methods
        public override void AddCard( CardFacade _card ) {
            if( !_card )
                throw new System.NullReferenceException("The card intended to be added is null.");

            cards.Add( _card );
            _card.transform.position = new Vector3( 
                                            transform.position.x + (cardsOffset.x * (cards.Count - 1)),
                                            transform.position.y + (cardsOffset.y * (cards.Count - 1)),
                                            transform.position.z
                                    );
            Refresh();
        }

        public override bool AddCards( List<CardFacade> _cards ) {
            if (_cards.Contains(null))
                throw new System.NullReferenceException("The list of cards that is intended to be added "
                                                        + "contains at least one null element.");

            for( int i = _cards.Count - 1; i >= 0; i--  ) {
                cards.Add( _cards[i] );

                _cards[i].transform.position = new Vector3(
                                        transform.position.x + cardsOffset.x,
                                        transform.position.y + cardsOffset.y,
                                        transform.position.z
                                    );
            }

            Refresh();

            return true;
        }

        public override List<CardFacade> Initialize( List<CardFacade> _cards ) {
            if( _cards.Contains( null ) ) {
                throw new System.NullReferenceException("There's a null element in the list of cards"
                                                        + " passed for initialization.");
            }

            return AddInitializationCards(_cards);
        }

        public override void RemoveCard( CardFacade _card ) {
            if( !_card ) {
                throw new System.NullReferenceException("The card intended to be removed is null.");
            }

            cards.Remove( _card );
        }

        public override void RemoveCards( List<CardFacade> _cards ) {
            if( _cards.Contains( null ) ) {
                throw new System.NullReferenceException("At least one of the card intended to be "
                                                        + "removed is null.");
            }

            foreach( var auxCard in _cards ) {
                cards.Remove(auxCard);
            }
        }
        #endregion


        #region Protected
        protected override void SetUpStarterCards() {
            // Does nothing to let other classes determine how to set them up
        }
        #endregion
    }
}