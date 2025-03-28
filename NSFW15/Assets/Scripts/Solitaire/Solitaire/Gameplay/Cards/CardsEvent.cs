/*
* Author:	Iris Bermudez
* Date:		08/12/2023
*/



using System;
using System.Collections.Generic;
using Solitaire.Gameplay.Cards;



namespace Solitaire.Gameplay {
    [Serializable]
    public class CardsEvent : UnityEngine.Events.UnityEvent<List<CardFacade>> { }
}