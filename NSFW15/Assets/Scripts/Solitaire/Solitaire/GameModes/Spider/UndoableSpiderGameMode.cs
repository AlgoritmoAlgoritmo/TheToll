/*
* Author:	Iris Bermudez
* Date:		19/09/2024
*/



using System.Collections.Generic;
using Solitaire.GameModes.UndoableCommands;
using Solitaire.Gameplay.CardContainers;
using Solitaire.Gameplay.Cards;
using Solitaire.Gameplay.Common;



namespace Solitaire.GameModes.Spider {
	public class UndoableSpiderGameMode : SpiderGameMode {
		#region Variables
		private UndoLastPlayController undoController;
        #endregion


        #region MonoBehaviour methods
        private void Start() {
			undoController = new UndoLastPlayController();
        }
        #endregion


        #region Public methods
        public void MakePlay() {
			undoController.MakePlay();
		}

		public void Undo() {
			undoController.UndoPlay();
		}

		public override void DistributeCardsBetweenCardContainers(
								SpiderCardContainerForCardDistribution _container ) {
			undoController.NewPlay();
			base.DistributeCardsBetweenCardContainers( _container );
		}
		#endregion


		#region Private methods
		protected override void MoveCardToNewContainer( CardFacade _card,
														AbstractCardContainer _cardContainer ) {
			if( _card.ChildCard ) {
				MoveMultipleCardsToNewContainer( _card, _cardContainer );

			} else {
				MoveACardToNewContainer( _card, _cardContainer );
			}
		}

		protected virtual void MoveACardToNewContainer( CardFacade _card,
														AbstractCardContainer _newCardContainer ) {
			undoController.NewPlay();
			SwitchCardContainerUndoableCommand switchContainerCommand
							= new SwitchCardContainerUndoableCommand( _card,
																	GetCardContainer( _card ),
																	_newCardContainer );

			undoController.AddCommand( switchContainerCommand );
			undoController.MakePlay();
		}

		protected virtual void MoveMultipleCardsToNewContainer( CardFacade _card,
														AbstractCardContainer _newCardContainer ) {
			undoController.NewPlay();

			List<CardFacade> auxCardList = new List<CardFacade>();
			var auxCardFacade = _card;

			while( auxCardFacade != null ) {
				auxCardList.Add( auxCardFacade );
				auxCardFacade = auxCardFacade.ChildCard;
			}

			undoController.AddCommand( new SwitchMultipleCardsContainerUndoableCommand(
																auxCardList,
																GetCardContainer( _card ),
																_newCardContainer ) );
			undoController.MakePlay();
		}


		protected override void CheckIfColumnWasCompleted( CardFacade _placedCard ) {
			List<CardFacade> columnOfCards = GetCardColumn( _placedCard );

			if( IsColumnCompleted( columnOfCards ) ) {
				ColumnCompletitionCheckCommand columnCompletedCommand = new ColumnCompletitionCheckCommand(
											columnOfCards,
											completedColumnContainers[completedColumnContainers.Count - 1],
											GetCardContainer( columnOfCards[0] ) );

				undoController.AddCommand( columnCompletedCommand );
				completedColumnContainers.RemoveAt( completedColumnContainers.Count - 1 );
				OnCardsCleared.Invoke( columnOfCards );
			}
		}
		#endregion
	}
}
