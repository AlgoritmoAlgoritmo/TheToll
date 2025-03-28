/*
* Author:	Iris Bermudez
* Date:		12/12/2023
*/



using System.Collections.Generic;
using UnityEngine;



namespace Solitaire.Gameplay.Common {
    [System.Serializable]
    public class CompleteSuitData : BasicSuitData {
        #region Variables
        public List<Sprite> sprites;

        public CompleteSuitData( string _suitName, string _color,
                            List<Sprite> _sprites) : base( _suitName, _color ) {
            sprites = _sprites;
        }
        #endregion
    }
}