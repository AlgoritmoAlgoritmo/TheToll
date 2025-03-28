/*
* Author:	Iris Bermudez
* Date:		27/03/2024
*/



using System.Threading.Tasks;



public static class TaskExtension {
    public static async void WrapErrors( this Task _task ) {
        await _task;
    }
}
