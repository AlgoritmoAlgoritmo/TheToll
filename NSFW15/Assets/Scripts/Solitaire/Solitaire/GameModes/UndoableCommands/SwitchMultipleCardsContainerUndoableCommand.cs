/*
* Author:	Iris Bermudez
* Date:		15/11/2024
*/



using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Misc.Command;
using Solitaire.Gameplay.CardContainers;
using Solitaire.Gameplay.Cards;



namespace Solitaire.GameModes.UndoableCommands {
    public class SwitchMultipleCardsContainerUndoableCommand : IUndoableCommand {
        #region Variables
        private List<CardFacade> cards;
        private AbstractCardContainer originalCardContainer;
        private AbstractCardContainer newCardContainer;
        #endregion


        #region Constructor methods
        public SwitchMultipleCardsContainerUndoableCommand( List<CardFacade> _cards,
                                                AbstractCardContainer _originalCardContainer,
                                                AbstractCardContainer _newCardContainer ) {
            cards = _cards;
            originalCardContainer = _originalCardContainer;
            newCardContainer = _newCardContainer;
        }
        #endregion


        #region Public methods
        public async Task Execute() {
            foreach( var auxCard in cards ) {
                originalCardContainer.RemoveCard( auxCard );
                newCardContainer.AddCard( auxCard );

                await Task.Yield();
            }
        }

        public async Task Undo() {
            originalCardContainer.FlipUpperCard( false );

            foreach( var auxCard in cards ) {
                newCardContainer.RemoveCard( auxCard );
                originalCardContainer.AddCard( auxCard );

                await Task.Yield();
            }

        }
        #endregion


        #region Private methods

        #endregion

    }
}