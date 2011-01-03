using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cyclops.Core.Model;
using Cyclops.Core.Mvvm;

namespace Cyclops.Client.Configuration
{
    public class Profile : PropertyChangedBase
    {
        public Profile()
        {
            Theme = "Default";
            ConnectionConfig = new ConnectionConfig();
        }

        private string name;

        /// <summary>
        /// Name of the profile
        /// </summary>
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                RaisePropertyChanged("Name");
            }
        }

        private string theme;

        /// <summary>
        /// Default theme for current profile. Themes are stored in %APPFOLDER%\Themes folder
        /// </summary>
        public string Theme
        {
            get { return theme; }
            set
            {
                theme = value;
                RaisePropertyChanged("Theme");
            }
        }

        private ConnectionConfig connectionConfig;

        /// <summary>
        /// Assigned configuration
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

        public override string ToString()
        {
            return Name;
        }
    }
}
