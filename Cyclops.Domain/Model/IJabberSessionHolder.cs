namespace Cyclops.Core.Model
{
    public interface IJabberSessionHolder
    {
        /// <summary>
        /// Session object
        /// </summary>
        JabberSession JabberSession { get; }
    }
}