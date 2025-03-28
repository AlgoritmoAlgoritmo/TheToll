/*
* Author:	Iris Bermudez
* Date:		20/03/2024
*/



using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;



namespace Settings {
    public class VolumeSettingsController : MonoBehaviour {
        #region Variables
        [SerializeField]
        private float defaultValue = .5f;
        [SerializeField]
        private AudioMixer audioMixer;
        [SerializeField]
        private Slider volumeSlider;
        [SerializeField]
        private string soundType;
        [SerializeField]
        private float testingTime;
        #endregion


        #region MonoBehaviour methods
        private void Start() {
            PlayerPrefs.SetFloat( soundType, GetActualVolumeValue( volumeSlider.value ) );
        }
        #endregion


        #region Public methods
        public void SetVolume( float _sliderValue ) {
            if( !audioMixer.SetFloat( soundType, GetActualVolumeValue( _sliderValue ) ) ) {
                throw new System.NullReferenceException( soundType + " not found in " 
                                                            + audioMixer.name);
            }

            PlayerPrefs.SetFloat( soundType, defaultValue );
        }
        #endregion


        #region Private methods
        private float GetActualVolumeValue( float _value ) {
            return Mathf.Log10(_value) * 20;
        }
        #endregion
    }
}