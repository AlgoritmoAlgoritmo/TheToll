/*
* Author:	Iris Bermudez
* Date:		05/12/2023
*/




namespace Solitaire.Gameplay.Cards {

    [System.Serializable]
    public class CardData {
        #region Variables
        public short number;
        public string suit;
        public string color;

        public string id;
        #endregion


        #region Constructor
        public CardData( short _number, string _suit, string _cardColour, string _id ) {
            number = _number;
            suit = _suit;
            color = _cardColour;
            id = _id;
        }
        #endregion
    }
}