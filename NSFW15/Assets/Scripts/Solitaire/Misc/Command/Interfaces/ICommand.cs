/*
* Author:	Iris Bermudez
* Date:		27/03/2024
*/



using System.Threading.Tasks;



namespace Misc.Command {
	public interface ICommand {
		#region Abstract methods
		Task Execute();
		#endregion
	}
}