/*
* Author:	Iris Bermudez
* Date:		17/08/2024
*/



namespace Misc.Command {
	public class SingletonCommandQueue : CommandQueue {
		#region Variables
		private static SingletonCommandQueue instance;
		public static SingletonCommandQueue Instance => instance ??
									( instance = new SingletonCommandQueue() );
		#endregion
	}
}