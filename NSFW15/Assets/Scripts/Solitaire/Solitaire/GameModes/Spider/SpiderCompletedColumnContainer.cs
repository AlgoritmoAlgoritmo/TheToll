/*
* Author:	Iris Bermudez
* Date:		29/03/2024
*/



using System.Collections.Generic;
using UnityEngine;
using Misc.Command;
using Solitaire.Gameplay.CardContainers;
using Solitaire.Gameplay.Cards;
using Solitaire.Feedbacks;



namespace Solitaire.GameModes.Spider {
    public class SpiderCompletedColumnContainer : BasicCardContainer {
        #region Variables
        [SerializeField]
        [Range(.0f, 1f)]
        private float cardAnimationSpeed;
        #endregion


        #region Public methods
        public override bool AddCards(List<CardFacade> _cards) {
            if (_cards.Contains(null))
                throw new System.NullReferenceException( "The list of cards that is intended to be added "
                                                        + "contains at least one null element.");

            foreach (CardFacade auxCard in _cards) {
                cards.Add(auxCard);
                auxCard.SetCanBeInteractable( false );
                auxCard.ActivateChildsPhysics( false );
                SingletonCommandQueue.Instance.AddCommand( new CardMovementAnimationICommand(
                                                                        auxCard.GetComponent<RectTransform>(),
                                                                        GetComponent<RectTransform>(),
                                                                        cardAnimationSpeed ));
            }

            return true;
        }

        #endregion
    }
}