/*
* Author:	Iris Bermudez
* Date:		13/03/2024
*/



using System.Collections;
using UnityEngine;
using UnityEngine.Audio;



namespace Sound {
    public class BGMPlayer : MonoBehaviour {
        #region  Variables
        [SerializeField]
        private AudioClip[] bgmTracks;
        [SerializeField]
        private AudioMixerGroup audioMixerGroup;
        [SerializeField]
        private bool autoStart;
        [SerializeField]
        private bool isCyclicModeOn;
        [SerializeField]
        private float waitingTimeToRefreshInSeconds = 1f;

        private AudioSource audioSource;

        private bool isPlaying = false;
        private int counter = 0;
        #endregion

        #region MonoBehavior functions
        private void Start() {
            audioSource = GetComponent<AudioSource>();

            if (!audioSource)
                audioSource = gameObject.AddComponent<AudioSource>();

            audioSource.outputAudioMixerGroup = audioMixerGroup;
            audioSource.clip = bgmTracks[0];

            if (autoStart) {
                if (isCyclicModeOn) {
                    StartCoroutine("CheckCyclicModePlaying");

                } else {
                    StartPlaying();
                }
            }
        }
        #endregion


        #region Public functions
        public virtual void StartPlaying() {
            if (!isPlaying) {
                if (counter == bgmTracks.Length - 1) {
                    counter = 0;
                }

                audioSource.clip = bgmTracks[counter];
                audioSource.Play();

                counter++;
                isPlaying = true;
            }
        }

        public virtual void StopPlaying() {
            if (isPlaying) {
                audioSource.Stop();
                isPlaying = false;
            }
        }

        public void PlayNextSong() {
            if (counter >= bgmTracks.Length
                        || counter == bgmTracks.Length - 1) {
                counter = 0;
            }

            audioSource.clip = bgmTracks[counter];
            audioSource.Play();

            counter++;
        }

        public void Pause() {
            audioSource.Pause();
        }

        public void Resume() {
            audioSource.UnPause();
        }
        #endregion


        #region Private functions
        private int GetCurrentIndex() {
            if (counter == 0)
                return bgmTracks.Length - 1;
            else
                return counter - 1;
        }
        #endregion


        #region Coroutines
        public IEnumerator CheckCyclicModePlaying() {
            isPlaying = true;

            while (isPlaying) {
                if (!audioSource.isPlaying)
                    PlayNextSong();

                yield return new WaitForSeconds(waitingTimeToRefreshInSeconds);
            }
        }
        #endregion
    }
}
