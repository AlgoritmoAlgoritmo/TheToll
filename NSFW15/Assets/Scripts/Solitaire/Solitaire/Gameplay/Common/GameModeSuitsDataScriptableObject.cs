/*
* Author:	Iris Bermudez
* Date:		12/09/2024
*/



using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Solitaire.Gameplay.Common {
    [CreateAssetMenu( fileName = "New GameModeSuitsDataScriptableObject",
                        menuName = "Solitaire/GameModeSuitsDataScriptableObject" )]
    public class GameModeSuitsDataScriptableObject : ScriptableObject {
        #region Variables
        [SerializeField]
        protected List<BasicSuitData> suits;
        public List<BasicSuitData> Suits {
            get => suits;
            set => suits = value;
        }

      

        [SerializeField]
        protected short amountOfEachSuit;
        public short AmountOfEachSuit {
            get => amountOfEachSuit;
            set => amountOfEachSuit = value;
        }
        #endregion
    }
}