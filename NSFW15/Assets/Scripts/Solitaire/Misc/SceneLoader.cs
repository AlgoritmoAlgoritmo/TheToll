/*
* Author:	Iris Bermudez
* Date:		14/08/2024
*/


using UnityEngine;
using UnityEngine.SceneManagement;


namespace Misc {
	public class SceneLoader : MonoBehaviour {
		#region Public methods
		public void LoadScene( string _sceneName ) {
			SceneManager.LoadScene( _sceneName );
		}

		public void RestartCurrentScene() {
			SceneManager.LoadScene( SceneManager.GetActiveScene().name );
		}
		#endregion
	}
}