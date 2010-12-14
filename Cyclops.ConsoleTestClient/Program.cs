using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using Cyclops.Core.Model;
using Cyclops.Core.Model.CustomEventArgs;

namespace Cyclops.ConsoleTestClient
{
    class Program
    {
        static void Main(string[] args)
        {
            string data = "11111";
            string sel = new string(data.Skip(2).Take(3).ToArray());
            JabberSession session = new JabberSession();
            session.Authenticated += Authenticated;

            Console.WriteLine(@"Connecting...");
            session.BeginAuthentication(new ConnectionConfig {
                                                User = "cyclops", 
                                                Server = "jabber.uruchie.org",
                                                Password = "cyclops"
                                            });
            session.Conferences.Add(new jabber.JID("main", "conference.jabber.uruchie.org", "CyclopsIsAlive"));
            Console.ReadKey();
        }

        static void Authenticated(object sender, AuthenticationEventArgs e)
        {
            Console.WriteLine(@"Connected ({0})", e);
        }
    }
}
