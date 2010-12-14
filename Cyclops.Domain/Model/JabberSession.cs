using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Threading;
using System.Xml;
using Cyclops.Core.Model.CustomEventArgs;
using Cyclops.Core.Model.Enums;
using Cyclops.Core.Resources;
using jabber;
using jabber.client;
using jabber.connection;
using jabber.protocol.stream;

namespace Cyclops.Core.Model
{
    /// <summary>
    /// Jabber-net session
    /// </summary>
    public class JabberSession : INotifyPropertyChanged
    {
        /// <summary>
        /// ConferenceManager object. 
        /// </summary>
        private readonly ConferenceManager conferenceManager;

        /// <summary>
        /// Jabber-net client session object
        /// </summary>
        private readonly JabberClient jabberClient;

        /// <summary>
        /// Timer for reconnect
        /// </summary>
        private readonly DispatcherTimer reconnectTimer;

        private ConferenceCollection conferences;

        /// <summary>
        /// Connection configuration
        /// </summary>
        private ConnectionConfig connectionConfig;

        /// <summary>
        /// Indicates is the authentication in progress
        /// </summary>
        private bool isAuthenticating;

        /// <summary>
        /// Current JID
        /// </summary>
        private JID jid;

        /// <summary>
        /// Create a jabber session
        /// </summary>
        public JabberSession(Dispatcher dispatcher = null)
        {
            Dispatcher = dispatcher;

            jabberClient = new JabberClient();
            conferenceManager = new ConferenceManager {Stream = jabberClient};
            Conferences = new ConferenceCollection(this);

            jabberClient.OnAuthenticate += s => Invoke(() => jabberClient_OnAuthenticate(s));
            jabberClient.OnAuthError += (s, rp) => Invoke(() => jabberClient_OnAuthError(s, rp));
            jabberClient.OnConnect += (s, stream) => Invoke(() => jabberClient_OnConnect(s, stream));
            jabberClient.OnDisconnect += s => Invoke(() => jabberClient_OnDisconnect(s));
            jabberClient.OnError += (s, exc) => Invoke(() => jabberClient_OnError(s, exc));
            jabberClient.OnInvalidCertificate += jabberClient_OnInvalidCertificate;
            jabberClient.OnStreamError += (s, rp) => Invoke(() => jabberClient_OnStreamError(s, rp));

            reconnectTimer = new DispatcherTimer {Interval = TimeSpan.FromSeconds(10)};
            reconnectTimer.Tick += reconnectTimer_Tick;
        }

        /// <summary>
        /// Thread dispatcher
        /// </summary>
        public Dispatcher Dispatcher { get; private set; }

        /// <summary>
        /// </summary>
        public bool IsAuthenticating
        {
            get { return isAuthenticating; }
            private set
            {
                isAuthenticating = value;
                RaisePropertyChanged("IsAuthenticating");
            }
        }

        /// <summary>
        /// Current JID
        /// </summary>
        public JID Jid
        {
            get { return jid; }
            set
            {
                jid = value;
                RaisePropertyChanged("Jid");
            }
        }

        public ConferenceCollection Conferences
        {
            get { return conferences; }
            set
            {
                conferences = value;
                RaisePropertyChanged("Conferences");
            }
        }

        /// <summary>
        /// Connection configuration
        /// </summary>
        public ConnectionConfig ConnectionConfig
        {
            get { return connectionConfig; }
            set
            {
                connectionConfig = value;
                RaisePropertyChanged("ConnectionConfig");
            }
        }

        /// <summary>
        /// Are we currently authenticated?
        /// </summary>
        public bool IsAuthenticated
        {
            get { return jabberClient.IsAuthenticated; }
        }

        internal ConferenceManager ConferenceManager
        {
            get { return conferenceManager; }
        }

        /// <summary>
        /// Switch to UI thread and invoke an action
        /// </summary>
        public void Invoke(Action action)
        {
            if (Dispatcher == null)
                action();
            else
                Dispatcher.BeginInvoke(action);//.Invoke(action);
        }

        private void reconnectTimer_Tick(object sender, EventArgs e)
        {
            reconnectTimer.Stop();
            Reconnect();
        }

        /// <summary>
        /// Start authentication proccess.
        /// You have to add handler for <code>OnAuthenticated</code> to recieve the result
        /// </summary>
        public void BeginAuthentication(ConnectionConfig info)
        {
            if (!ValidateConnectionConfig(info))
                return;

            jabberClient.Server = info.Server;
            jabberClient.NetworkHost = info.NetworkHost;
            jabberClient.User = info.User;
            jabberClient.Password = info.Password;
            jabberClient.Port = info.Port;
            jabberClient.Resource = "cyclops v." + Assembly.GetAssembly(GetType()).GetName().Version.ToString(3);


            Jid = new JID(info.User, info.Server, jabberClient.Resource);
            ConnectionConfig = info;

            // some default settings
            jabberClient.AutoReconnect = -1;


            jabberClient[Options.SASL_MECHANISMS] = MechanismType.PLAIN;
            jabberClient.KeepAlive = 20F;

            //let's go!
            IsAuthenticating = true;
            jabberClient.Connect();
        }

        /// <summary>
        /// Close session
        /// </summary>
        public void Close()
        {
            try
            {
                if (jabberClient.IsAuthenticated)
                    jabberClient.Close(true);
            }
            catch (Exception exc)
            {
                //TODO: log the exception
            }
        }

        /// <summary>
        /// Reconnect
        /// </summary>
        public void Reconnect()
        {
            if (ConnectionConfig == null)
                throw new InvalidOperationException(
                    "You can't call Reconnect() before at least one BeginAuthentication() calling");
            BeginAuthentication(ConnectionConfig);
        }

        /// <summary>
        /// Raised when authentication complete (success or not)
        /// </summary>
        public event EventHandler<AuthenticationEventArgs> Authenticated = delegate { };

        /// <summary>
        /// Raised when authentication complete (success or not)
        /// </summary>
        public event EventHandler<AuthenticationEventArgs> ConnectionDropped = delegate { };

        private bool ValidateConnectionConfig(ConnectionConfig config)
        {
            var context = new ValidationContext(config, null, null);
            var results = new List<ValidationResult>();
            bool result = Validator.TryValidateObject(config, context, results);

            if (!result)
                Authenticated(this, new AuthenticationEventArgs(ConnectionErrorKind.InvalidConfig,
                                                                results.CollectionToString(i => i.ErrorMessage)));

            return result;
        }

        #region Implementation of INotifyPropertyChanged

        /// <summary>
        /// Property changed event
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        /// <summary>
        /// Raise a PropertyChanged event for given property
        /// </summary>
        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region JabberClient events

        private void jabberClient_OnStreamError(object sender, XmlElement rp)
        {
            if (IsAuthenticating)
            {
                IsAuthenticating = false;
                Authenticated(this, new AuthenticationEventArgs(
                                        ConnectionErrorKind.UnknownError,
                                        ErrorMessageResources.CommonAuthenticationErrorMessage));
            }
            else
                ConnectionDropped(this, new AuthenticationEventArgs(
                                            ConnectionErrorKind.ConnectionError,
                                            ErrorMessageResources.CommonAuthenticationErrorMessage));
        }

        private bool jabberClient_OnInvalidCertificate(object sender, X509Certificate certificate, X509Chain chain,
                                                       SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        private void jabberClient_OnError(object sender, Exception ex)
        {
            if (IsAuthenticating)
            {
                IsAuthenticating = false;
                Authenticated(this, new AuthenticationEventArgs(ConnectionErrorKind.ConnectionError, ex.Message));
            }
            else
                ConnectionDropped(this, new AuthenticationEventArgs(ConnectionErrorKind.ConnectionError, ex.Message));

            reconnectTimer.Start();
        }

        private void jabberClient_OnDisconnect(object sender)
        {
        }

        private void jabberClient_OnConnect(object sender, StanzaStream stream)
        {
        }

        private void jabberClient_OnAuthError(object sender, XmlElement rp)
        {
            Authenticated(sender,
                          new AuthenticationEventArgs(ConnectionErrorKind.InvalidPasswordOrUserName,
                                                      ErrorMessageResources.InvalidLoginOrPasswordMessage));
        }

        private void jabberClient_OnAuthenticate(object sender)
        {
            //Channels.ForEach(i => i.OnConnected());
            IsAuthenticating = true;
            Authenticated(sender, new AuthenticationEventArgs());
        }

        #endregion
    }
}