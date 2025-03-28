/*
* Author:	Iris Bermudez
* Date:		06/12/2023
*/



using System.Collections.Generic;
using UnityEngine;



namespace Solitaire.Gameplay.Common {

    [CreateAssetMenu(fileName = "New CardSpritesScriptableObject",
                                menuName = "Solitaire/Card Sprites")]
    public class CardSpritesScriptableObject : ScriptableObject {
        #region Variables
        [SerializeField]
        private CardThemeSpritesData cardThemeData;
        public Sprite BackSprite {
            get => cardThemeData.BackSprite;
        }
        #endregion


        #region Public methods
        public void SetNewSprites( CardThemeSpritesData _cardThemeData ) {
            cardThemeData = _cardThemeData;
        }


        public List<Sprite> GetSuitCardsSprites( BasicSuitData _suitData ) {
            foreach( CompleteSuitData auxSuit in cardThemeData.Suits ) {
                if( auxSuit.suitName.ToUpper().Equals( _suitData.suitName.ToUpper() ) )
                    return auxSuit.sprites;
            }

            throw new System.Exception( $"Suit key {_suitData.suitName.ToUpper()} not found." );
        }
        #endregion
    }
}