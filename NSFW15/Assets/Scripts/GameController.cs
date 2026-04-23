/*
* GitHub: https://github.com/AlgoritmoAlgoritmo
* Date: 18/03/2025 (DD/MM/YYYY)
*/



using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using UnityEngine.SceneManagement;
using Solitaire.Gameplay;
using Solitaire.Gameplay.GameMode;
using Solitaire.Gameplay.Cards;
using Jam15.Interactions;



namespace Jam15 {
	public class GameController : MonoBehaviour {
		#region Variables
        [Header("General")]
		[SerializeField]
		private RectTransform solitaireGameParent;
        [SerializeField]
        private InteractableNPC interactableNPCForTesting;
        [SerializeField]
        private BarAnimationController[] barAnimationControllers;

        
        [Space]
        [Header("Timeline properties")]
        [SerializeField]
        private PlayableDirector playableDirector;        
        [SerializeField]
        private TimelineAsset backTimeline;
        [SerializeField]
        private TimelineAsset frontTimeline;
        [SerializeField]
        private TimelineAsset mouthTimeline;
        [SerializeField]
        private TimelineAsset handTimeline;


        [Space]
        [Header("Events")]
        public UnityEvent OnGameOverEvent = new UnityEvent();
        public UnityEvent OnSolitaireModeStarts = new UnityEvent();
        public UnityEvent OnSolitaireModeEnds = new UnityEvent();
        public UnityEvent OnCardEvent = new UnityEvent();


        // Private properties
        private GameObject solitaireGameInstance;
        private AbstractGameMode gameMode;
        private DeckController deckController;

        private List<GameObject> solitaireStagesPrefabs;
        private short currentStage = 0;
        #endregion


        #region MonoBehaviour methods
        private void Start() {
            DisplaySolitaireUI( false );
        }


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

            StartNextStage();
            OnSolitaireModeStarts.Invoke();
        }

        public void EndClearedGame( object _object, System.EventArgs _args ) {
            if( currentStage < solitaireStagesPrefabs.Count ) {
                StartNextStage();

            } else {
                Debug.Log("Solitaire game cleared.");
                // npcCam.SetActive( false );
                Destroy( solitaireGameInstance );

                OnGameOverEvent?.Invoke();
                OnSolitaireModeEnds?.Invoke();
            }
        }

        public void RestartSolitaireGame() {
        
        }

        public void PlayAgain() {
            SceneManager.LoadScene( SceneManager.GetSceneAt(0).name );
        }

        public void QuitGame() {
            Debug.Log( "Closing game..." );
            Application.Quit();
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

                gameMode.OnCardsCleared.AddListener( UpdateIndividualStuitScores );
                gameMode.OnCardsCleared.AddListener( delegate{ OnCardEvent.Invoke(); } );

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
            DisplaySolitaireUI( true );
        }


        private void DisplaySolitaireUI( bool _areDisplayed ) {
            foreach( var auxBar in barAnimationControllers ) {
                auxBar.gameObject.SetActive( _areDisplayed );
            }
        }


        private void UpdateIndividualStuitScores( List<CardFacade> _cards ) {
            foreach( var auxBar in barAnimationControllers ) {
                // Updating scores after cards are cleared
                auxBar.IncreaseProgression( _cards );

                // Checking if any of them reached 100% progress
                if( auxBar.IsFull() ) {
                    Debug.Log( auxBar.SuitID );
                    // Ending game
                    EndClearedGame( this, EventArgs.Empty );

                    TimelineAsset timeline = frontTimeline;

                    if( auxBar.SexPositionID == "BACK" ) {
                        timeline = backTimeline;

                    } else if( auxBar.SexPositionID == "MOUTH" ) {
                        timeline = mouthTimeline;

                    } else if( auxBar.SexPositionID == "HAND" ) {
                        timeline = handTimeline;
                    }

                    playableDirector.playableAsset = timeline;
                    playableDirector.Play();
                }
            }
        }
        
        private void PlayTimeline( TimelineAsset _timeline ) {
            playableDirector.playableAsset = _timeline;
            playableDirector.Play();
        }
        #endregion
    }
}