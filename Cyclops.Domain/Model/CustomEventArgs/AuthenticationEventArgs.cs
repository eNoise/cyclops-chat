using Cyclops.Core.Model.Enums;

namespace Cyclops.Core.Model.CustomEventArgs
{
    /// <summary>
    /// Authentication event arguments
    /// </summary>
    public class AuthenticationEventArgs : OperationResult<ConnectionErrorKind>
    {
        public AuthenticationEventArgs()
        {
        }

        public AuthenticationEventArgs(ConnectionErrorKind errorKind, string errorMessage) :
            base(errorKind, errorMessage)
        {
        }
    }
}