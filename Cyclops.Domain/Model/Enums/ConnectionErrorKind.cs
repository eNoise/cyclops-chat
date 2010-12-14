namespace Cyclops.Core.Model.Enums
{
    public enum ConnectionErrorKind
    {
        NoError = -1,
        InvalidPasswordOrUserName = 0,
        InvalidConfig,
        ConnectionError,
        RequestedByUser,
        UnknownError
    }
}