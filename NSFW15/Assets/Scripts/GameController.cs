/*
* GitHub: https://github.com/AlgoritmoAlgoritmo
* Date: 18/03/2025 (DD/MM/YYYY)
*/



using UnityEngine;
using UnityEngine.Events;
using Jam15.Interactions;
using Solitaire.Gameplay;
using Solitaire.Gameplay.GameMode;
using System.Collections.Generic;

namespace Jam15 {
	public class GameController : MonoBehaviour {
		#region Variables
		[SerializeField]
		private RectTransform solitaireGameParent;
        [SerializeField]
        private InteractableNPC interactableNPCForTesting;


        public UnityEvent OnGameOverEvent = new UnityEvent();
        public UnityEvent OnSolitaireModeStarts = new UnityEvent();
        public UnityEvent OnSolitaireModeEnds = new UnityEvent();

        private GameObject solitaireGameInstance;
        private AbstractGameMode gameMode;
        private DeckController deckController;
        private GameObject lastNPCCam;

        private List<GameObject> solitaireStagesPrefabs;
        private short currentStage = 0;
        #endregion


        #region MonoBehaviour methods
        private void Update() {
            if( Input.GetKeyUp( KeyCode.P ) ) {
                StartSolitaireGame( interactableNPCForTesting );
            }
        }
        #endregion


        #region Public methods
        public void StartSolitaireGame( InteractableNPC _interactableNPC ) {
            solitaireStagesPrefabs = _interactableNPC.GetGamePrefabs();
            currentStage = 0;
            lastNPCCam = _interactableNPC.GetCamera();
            lastNPCCam.SetActive( true );

            StartNextStage();
            OnSolitaireModeStarts.Invoke();


            /*
             * Old Code
             */
            /*
            solitaireGameInstance = Instantiate( _interactableNPC.GetGamePrefabs(),
                                                solitaireGameParent );
            lastNPCCam = _interactableNPC.GetCamera();
            lastNPCCam.SetActive( true );

            gameMode = solitaireGameInstance.GetComponent<AbstractGameMode>();
            deckController = solitaireGameInstance.GetComponent<DeckController>();

            StartGame();
            OnSolitaireModeStarts.Invoke();
            */
        }

        public void EndClearedGame( object _object, System.EventArgs _args ) {
            if( currentStage < solitaireStagesPrefabs.Count ) {
                StartNextStage();

            } else {
                Debug.Log("Solitaire game cleared.");
                lastNPCCam.SetActive( false );
                Destroy( solitaireGameInstance );

                OnGameOverEvent?.Invoke();
                OnSolitaireModeEnds?.Invoke();
            }
        }
        #endregion


        #region Private methods
        private void StartNextStage() {
            Debug.Log( "currentStage " + currentStage );

            if( currentStage < solitaireStagesPrefabs.Count ) {
                if( currentStage > 0 ) {
                    Destroy( solitaireGameInstance );
                }

                solitaireGameInstance = Instantiate( solitaireStagesPrefabs[currentStage],
                                                    solitaireGameParent );
                gameMode = solitaireGameInstance.GetComponent<AbstractGameMode>();
                deckController = solitaireGameInstance.GetComponent<DeckController>();

                currentStage++;

                StartGame();
                OnSolitaireModeStarts.Invoke();

            } else {
                Debug.Log( "***********************" );
                Debug.Log( "Solitaire game cleared!" );
                Debug.Log( "***********************" );
            }
        }


        private void StartGame() {
            deckController.onCardsCleared += EndClearedGame;
            gameMode.OnCardsCleared.AddListener( deckController.RemoveCardsFromGame );
            gameMode.Initialize( deckController.InitializeCards( gameMode.Suits,
                                                                gameMode.AmountOfEachSuit ) );
        }
        #endregion
    }
}