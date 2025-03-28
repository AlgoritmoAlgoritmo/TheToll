/*
* Author:	Iris Bermudez
* Date:		16/02/2024
*/



using System.Collections.Generic;
using UnityEngine;



namespace Solitaire.Gameplay.Common {
    [System.Serializable]
    public class CardThemeSpritesData {
        #region Variables
        [SerializeField]
        private Sprite backSprite;
        public Sprite BackSprite {
            get => backSprite;
        }

        [SerializeField]
        private List<CompleteSuitData> suits;
        public List<CompleteSuitData> Suits {
            get => suits;
        }
        #endregion


        #region Constructor methods
        public CardThemeSpritesData() {
            suits = new List<CompleteSuitData>();
        }
        #endregion
    }
}