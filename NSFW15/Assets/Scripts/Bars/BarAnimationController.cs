/*
* GitHub: https://github.com/AlgoritmoAlgoritmo
* Date: 11/04/2026 (DD/MM/YYYY)
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Solitaire.Gameplay.Cards;


namespace Jam15 {
    public class BarAnimationController : MonoBehaviour {
        #region Variables
        [SerializeField]
        private UnityEngine.UI.Image fillBarImage;
        [SerializeField]
        private int maxProgression = 100;
        [SerializeField]
        private int startingProgressionAmount = 0;
        [SerializeField]
        private float pointsMultiplier = 1f;
        [SerializeField]
        private string suitID = "HEARTS";
        public string SuitID {
            get {
                return suitID;
            }
        }

        [SerializeField]
        private string sexPositionID = "MOUTHY";
        public string SexPositionID {
            get {
                return sexPositionID;
            }
        }

        private float currentProgression = 0;
        #endregion


        #region MonoBehaviour methods
        private void Awake() {
            currentProgression = startingProgressionAmount;
            UpdateView();
        }
        #endregion


        #region Public methods
        public void IncreaseProgression( List<CardFacade> _cards ) {
            foreach( var auxCard in _cards ) {
                if( suitID.Equals( auxCard.GetSuit() ) ) {
                    IncreaseProgression( auxCard.GetCardNumber() );
                }
            }
        }

        public void IncreaseProgression( int _amount ) {
            currentProgression += _amount;
            UpdateView();
        }

        public void DecreaseProgression( int _amount ) {
            currentProgression -= _amount;
            UpdateView();
        }

        public float GetProgressionPercentage() {
            return ( (float)( currentProgression * pointsMultiplier ) / (float)maxProgression ) * 100f;
        }

        public bool IsFull() {
            return GetProgressionPercentage() >= 100f;
        }

        public void Reset() {
            currentProgression = startingProgressionAmount;
            UpdateView();
        }
        #endregion


        #region Private methods
        private void UpdateView() {
            fillBarImage.fillAmount = ( currentProgression * pointsMultiplier ) / (float)maxProgression;
        }
        #endregion
    }
}