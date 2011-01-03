using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Cyclops.Client.Configuration;
using Cyclops.Core.Model;
using NUnit.Framework;

namespace Cyclops.Tests
{
    [TestFixture]
    public class ProfileSystemTests
    {
        [Test]
        public void CommonTest()
        {
            string profileTestsDir = "ProfileTests";

            if (Directory.Exists(profileTestsDir))
                Directory.Delete(profileTestsDir);

            Directory.CreateDirectory(profileTestsDir);

            ProfileManager manager = new ProfileManager(profileTestsDir);

            manager.SaveProfile(CreateFakeProfile("cyclops"));
            manager.SaveProfile(CreateFakeProfile("2"));
            manager.SaveProfile(CreateFakeProfile("3"));

            var profiles = manager.GetSavedProfiles().ToArray();

            Assert.AreEqual(3, profiles.Count());
            Assert.AreEqual("1", manager.GetSavedProfiles().OrderBy(i => i.Name).First().ConnectionConfig.
                                DecodePassword());

            manager.GetSavedProfiles().ForEach(manager.RemoveProfile);
        }

        private Profile CreateFakeProfile(string name)
        {
            Profile profile = new Profile
                                  {
                                      Name = name,
                                      ConnectionConfig = new ConnectionConfig
                                                             {
                                                                 Server = "jabber.uruchie.org",
                                                                 User = name,
                                                                 Password = name,
                                                                 Port = 5222
                                                             }
                                  };
            return profile;
        }
    }
}
