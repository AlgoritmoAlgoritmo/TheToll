/*
* Author:	Iris Bermudez
* Date:		20/09/2024
*/



using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Misc.Command;
using Solitaire.Gameplay.Cards;
using Solitaire.Gameplay.CardContainers;



namespace Solitaire.GameModes.UndoableCommands {
    public class ColumnCompletitionCheckCommand : IUndoableCommand {
        #region Variables
        private List<CardFacade> columnOfCards;
        private AbstractCardContainer originalCardContainer;
        private AbstractCardContainer completedColumnContainer;
        #endregion


        #region Constructor methods
        public ColumnCompletitionCheckCommand( List<CardFacade> _columnOfCards,
                                            AbstractCardContainer _completedColumnContainer,
                                            AbstractCardContainer _originalCardContainer ) {
            columnOfCards = _columnOfCards;
            completedColumnContainer = _completedColumnContainer;
            originalCardContainer = _originalCardContainer;
        }
        #endregion


        #region Public methods
        public async Task Execute() {
            columnOfCards[0].SetParentCard( null );

            foreach( CardFacade auxCard in columnOfCards ) {
                auxCard.ActivatePhysics( false );
                auxCard.SetCanBeDragged( false );

                await Task.Yield();
            }

            originalCardContainer.RemoveCards( columnOfCards );
            completedColumnContainer.AddCards( columnOfCards );
        }

        public async Task Undo() {
            /*
             * Intentionally unimplemented
             */
            await Task.Yield();
        }
        #endregion
    }
}