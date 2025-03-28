/*
* Author:	Iris Bermudez
* Date:		18/03/2024
*/


using UnityEngine;


namespace Misc {
    public class ApplicationController : MonoBehaviour {
        #region Public methods
        public void QuitApplication() {
            Application.Quit();
        }

        public void ToggleScreenMode() {
            if ( Screen.fullScreenMode == FullScreenMode.ExclusiveFullScreen)
                Screen.fullScreenMode = FullScreenMode.Windowed;
            else
                Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
        }
        #endregion
    }
}