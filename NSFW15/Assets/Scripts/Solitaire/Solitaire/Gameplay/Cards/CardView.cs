/*
* Author:	Iris Bermudez
* Date:		06/12/2023
*/



using UnityEngine;
using UnityEngine.UI;



namespace Solitaire.Gameplay.Cards {

    [System.Serializable]
    public class CardView {
        #region Variables
        [SerializeField]
        private Sprite frontSprite;
        [SerializeField]
        private Sprite backSprite;
        [SerializeField]
        private Image imageComponent;

        private bool isFacingUp;
        public bool IsFacingUp {
            get => isFacingUp;
        }
        #endregion


        #region Constructors
        public CardView() {}
        #endregion


        #region Public methods
        public void SetFrontSprite( Sprite _frontSprite ) {
            frontSprite = _frontSprite;
        }


        public void SetBackSprite( Sprite _backSprite ) {
            backSprite = _backSprite;
        }


        public void FlipCard( bool _facingUp ) {
            if( imageComponent  ) {
                if( _facingUp ) {
                    imageComponent.sprite = frontSprite;

                } else {
                    imageComponent.sprite = backSprite;
                }


                isFacingUp = _facingUp;

            } else {
                throw new System.Exception( "imageComponent reference is missing." );
            }
        }


        public void RenderOnTop( Transform _transform ) {
            Transform auxParent = _transform.parent;

            if (_transform.parent && _transform.parent.parent) {
                _transform?.SetParent(_transform.parent.parent);
                _transform?.SetParent(auxParent);
            }
        }


        public void SetInteractable( bool _interactable) {
            imageComponent.raycastTarget = _interactable;
        }
        #endregion
    }
}