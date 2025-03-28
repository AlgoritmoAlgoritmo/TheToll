/*
* GitHub: https://github.com/AlgoritmoAlgoritmo
* Date: 18/03/2025 (DD/MM/YYYY)
*/



using UnityEngine;
using UnityEngine.Events;
using Jam15.Interactions;
using Solitaire.Gameplay;
using Solitaire.Gameplay.GameMode;



namespace Jam15 {
	public class GameController : MonoBehaviour {
		#region Variables
		[SerializeField]
		private RectTransform solitaireGameParent;
        
        public UnityEvent OnGameOverEvent = new UnityEvent();
        public UnityEvent OnSolitaireModeStarts = new UnityEvent();
        public UnityEvent OnSolitaireModeEnds = new UnityEvent();

        private GameObject solitaireGameInstance;
        private AbstractGameMode gameMode;
        private DeckController deckController;
        private GameObject lastNPCCam;
        #endregion


        #region MonoBehaviour methods
        #endregion


        #region Public methods
        public void StartSolitaireGame( InteractableNPC _interactableNPC ) {
            solitaireGameInstance = Instantiate( _interactableNPC.GetGamePrefab(),
                                                solitaireGameParent );
            lastNPCCam = _interactableNPC.GetCamera();
            lastNPCCam.SetActive( true );

            gameMode = solitaireGameInstance.GetComponent<AbstractGameMode>();
            deckController = solitaireGameInstance.GetComponent<DeckController>();

            StartGame();
            OnSolitaireModeStarts.Invoke();
        }

        public void EndClearedGame( object _object, System.EventArgs _args ) {
            Debug.Log("Solitaire game cleared.");
            lastNPCCam.SetActive( false );
            Destroy( solitaireGameInstance );

            OnGameOverEvent?.Invoke();
            OnSolitaireModeEnds?.Invoke();
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