/*
* Author:	Iris Bermudez
* Date:		21/03/2024
*/



using System.Collections.Generic;
using UnityEngine;



namespace Settings {
    public class ScreenSettingsController : MonoBehaviour {
        #region Variables
        [SerializeField]
        private List<Vector2> resolutionOptions = new List<Vector2>();
        #endregion


        #region Public methods
        public void SetResolution( int _id ) {
            SetResolution(resolutionOptions[_id] );
        }

        public void SetResolution( Vector2 _resolutionValues ) {
            Screen.SetResolution( (int)_resolutionValues.x,
                                  (int)_resolutionValues.y,
                                  true );
        }

        public void SetQuality( int _qualityIndex ) {
            QualitySettings.SetQualityLevel(_qualityIndex);
        }
        #endregion
    }
}
