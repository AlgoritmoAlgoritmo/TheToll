/*
* Author:	Iris Bermudez
* Date:		07/12/2023
*/



using System.Collections.Generic;
using UnityEngine;
using Solitaire.Gameplay.Cards;



namespace Solitaire.Gameplay.CardContainers {

    public abstract class AbstractCardContainer : MonoBehaviour {
        #region Variables
        [SerializeField]
        protected Vector2 cardsOffset = Vector2.zero;
        [SerializeField]
        protected short initialCardsAmount = 0;
        [SerializeField]
        protected bool canAddCards = true;
        public bool CanAddCards {
            get => canAddCards;
        }

        protected List<CardFacade> cards = new List<CardFacade>();
        #endregion


        #region Public Abstract methods
        public abstract List<CardFacade> Initialize(List<CardFacade> _cards);
        public abstract void AddCard(CardFacade _card);
        public abstract bool AddCards(List<CardFacade> _cards);
        public abstract void RemoveCard(CardFacade _card);
        public abstract void RemoveCards(List<CardFacade> _cards);
        #endregion


        #region Public methods
        public void SetDefaultAmountOfCards( short _defaultAmountOfCards ) {
            initialCardsAmount = _defaultAmountOfCards;
        }

        public virtual bool ContainsCard( CardFacade _card ) {
            return cards.Contains( _card );
        }
        
        public CardFacade GetTopCard() {
            if( cards.Count == 0 ) 
                return null;

            return cards[cards.Count - 1];
        }

        public int GetCardCount() {
            return cards.Count;
        }
        
        public virtual void Refresh() {
            if( cards.Count > 0 ) {
                int index = 0;

                foreach( CardFacade auxCard in cards ) {
                    auxCard.transform.position = GetCardPosition(index);
                    index++;
                }
            }
        }

        public virtual void FlipUpperCard( bool _faceUp) {
            FlipCard( GetTopCard(), _faceUp );
        }
        #endregion


        #region Protected methods
        protected abstract void SetUpStarterCards();

        ///<summary> Adds X amount of cards from the given CardFacade list and returns 
        ///the rest of them, where X is the initial card amount. </summary>
        protected List<CardFacade> AddInitializationCards( List<CardFacade> _cards ) {
            List<CardFacade> auxCardList = _cards;

            for( int i = 0; i < initialCardsAmount; i++ ) {
                cards.Add(auxCardList[0]);
                cards[i].transform.position = GetCardPosition( i );
                auxCardList.RemoveAt(0);
            }

            SetUpStarterCards();

            return auxCardList;
        }
        
        protected int GetCardIndex( string _cardID ) {
            short index = 0;

            foreach(var auxCard in cards ) {
                if( _cardID.Equals( cards[index].GetID() ) ) {
                    return index;
                }
            }

            return -1;
        }

        protected virtual Vector2 GetCardPosition( int _index ) {
            return (Vector2) transform.position + ( cardsOffset * _index );
        }

        protected virtual void FlipCard( CardFacade _card, bool _facingUp ) {
            if( _card ) {
                _card.FlipCard(_facingUp);
                _card.SetCanBeInteractable(_facingUp);
                _card.ActivatePhysics(_facingUp);
            }
        }
        #endregion
    }
}