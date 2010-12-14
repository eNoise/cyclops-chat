using System.ComponentModel.DataAnnotations;
using Cyclops.Core.Resources;

namespace Cyclops.Core.Model
{
    /// <summary>
    /// Connection configuration
    /// </summary>
    public class ConnectionConfig
    {
        private string networkHost = string.Empty;

        public ConnectionConfig()
        {
            Port = 5222;
        }

        /// <summary>
        /// Server address
        /// </summary>
        [Required(ErrorMessageResourceType = typeof (ErrorMessageResources),
            ErrorMessageResourceName = "RequiredErrorMessage")]
        [Display(ResourceType = typeof (ConnectionConfigResources), Name = "ServerLabel", Order = 3)]
        //[RegularExpression("")]
            public string Server { get; set; }

        /// <summary>
        /// Physical server address (if differs with Server)
        /// </summary>
        [Display(ResourceType = typeof (ConnectionConfigResources), Name = "NetworkHostLabel",
            Description = "NetworkHostDescription", Order = 5)]
        //[RegularExpression("")]
            public string NetworkHost
        {
            get { return string.IsNullOrEmpty(networkHost) ? Server : networkHost; }
            set { networkHost = value; }
        }

        /// <summary>
        /// Port
        /// </summary>
        [Display(ResourceType = typeof (ConnectionConfigResources), Name = "PortLabel", Order = 4)]
        [Range(0, 65535)]
        public int Port { get; set; }

        /// <summary>
        /// User
        /// </summary>
        [Required(ErrorMessageResourceType = typeof (ErrorMessageResources),
            ErrorMessageResourceName = "RequiredErrorMessage")]
        [Display(ResourceType = typeof (ConnectionConfigResources), Name = "UserLabel", Order = 1)]
        //[RegularExpression("")]
            public string User { get; set; }

        /// <summary>
        /// Password
        /// </summary>
        [Required(ErrorMessageResourceType = typeof (ErrorMessageResources),
            ErrorMessageResourceName = "RequiredErrorMessage")]
        [Display(ResourceType = typeof (ConnectionConfigResources), Name = "PasswordLabel", Order = 2)]
        //[RegularExpression("")]
            public string Password { get; set; }

        /*
         * TODO: Proxy 
         */

        public override string ToString()
        {
            return string.Format("Server: {0}; NetworkHost: {1}; User: {2}; Port: {3}", Server, NetworkHost, User, Port);
        }
    }
}