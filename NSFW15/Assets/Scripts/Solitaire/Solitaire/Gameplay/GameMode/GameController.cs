/*
* Author:	Iris Bermudez
* Date:		08/12/2023
*/


using UnityEngine;
using UnityEngine.Events;


namespace Solitaire.Gameplay.GameMode {
    public class GameController : MonoBehaviour {
        #region Variables
        [SerializeField]
        private AbstractGameMode gameMode;
        [SerializeField]
        private DeckController deckController;
        [SerializeField]
        private UnityEvent onGameOverEvent;
        #endregion


        #region MonoBehaviour methods
        private void Start() {
            StartGame();
        }
        #endregion


        #region Public methods
        public void EndClearedGame( object _object, System.EventArgs _args ) {
            onGameOverEvent.Invoke();
        }
        #endregion


        #region Private methods
        private void StartGame() {
            deckController.onCardsCleared += EndClearedGame;
            gameMode.OnCardsCleared.AddListener( deckController.RemoveCardsFromGame );
            gameMode.Initialize( deckController.InitializeCards( gameMode.Suits,
                                                                gameMode.AmountOfEachSuit ) );
        }
        #endregion
    }
}