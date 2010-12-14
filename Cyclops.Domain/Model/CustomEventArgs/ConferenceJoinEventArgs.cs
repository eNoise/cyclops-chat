using Cyclops.Core.Model.Enums;

namespace Cyclops.Core.Model.CustomEventArgs
{
    /// <summary>
    /// Disconnect event args
    /// </summary>
    public class ConferenceJoinEventArgs : OperationResult<ConferenceJoinErrorKind>
    {
        public ConferenceJoinEventArgs()
        {
        }

        public ConferenceJoinEventArgs(ConferenceJoinErrorKind errorKind, string errorMessage) :
            base(errorKind, errorMessage)
        {
        }
    }
}