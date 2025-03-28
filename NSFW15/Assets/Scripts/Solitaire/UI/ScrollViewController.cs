/*
* Author:	Iris Bermudez
* Date:		13/08/2024
*/


using UnityEngine;
using UnityEngine.UI;


namespace UI {
	public class ScrollViewController : MonoBehaviour {
		#region Variables
		[SerializeField]
		private Vector2 offset = new Vector2( 200, 200 );
        [SerializeField]
        private Vector2 minLimit = new Vector2( 0, 0 );
        [SerializeField]
        private Vector2 maxLimit = new Vector2( 400, 0 );
        [SerializeField]
        private RectTransform contentRectTransform;
        #endregion


        #region Public methods
        public void ScrollToTheLeft() {
            Scroll( new Vector2( offset.x, 0 ) );
        }

        public void ScrollToTheRight() {
            Scroll( new Vector2( -offset.x, 0 ) );
        }

        public void ScrollUp() {
            Scroll( new Vector2( 0, offset.y ) );
        }

        public void ScrollDown() {
            Scroll( new Vector2( 0, -offset.y ) );
        }
        #endregion


        #region Private methods
        private void Scroll( Vector2 _distance ) {
            // contentRectTransform.anchoredPosition += _distance;

            
            if( ( contentRectTransform.anchoredPosition.x + _distance.x ) >= minLimit.x
                    && ( contentRectTransform.anchoredPosition.x + _distance.x ) <= maxLimit.x
                    && ( contentRectTransform.anchoredPosition.y + _distance.y ) >= minLimit.y
                    && ( contentRectTransform.anchoredPosition.y + _distance.y ) <= maxLimit.y ) {

                contentRectTransform.anchoredPosition += _distance;
            }
        }
        #endregion
    }
}