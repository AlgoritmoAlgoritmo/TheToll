/*
* Author:	Iris Bermudez
* Date:		18/09/2024
*/



using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;



namespace Misc.Command {
    public interface IUndoableCommand : ICommand {
        #region Public methods
        Task Undo();
        #endregion
    }
}
