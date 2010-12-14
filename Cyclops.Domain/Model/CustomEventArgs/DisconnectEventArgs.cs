using Cyclops.Core.Model.Enums;

namespace Cyclops.Core.Model.CustomEventArgs
{
    /// <summary>
    /// Disconnect event args
    /// </summary>
    public class DisconnectEventArgs : OperationResult<ConnectionErrorKind>
    {
        public DisconnectEventArgs()
        {
            ErrorKind = ConnectionErrorKind.RequestedByUser;
        }

        public DisconnectEventArgs(ConnectionErrorKind errorKind, string errorMessage) :
            base(errorKind, errorMessage)
        {
        }
    }
}