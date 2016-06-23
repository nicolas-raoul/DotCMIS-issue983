using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotCMIS;
using DotCMIS.Client.Impl;
using DotCMIS.Client;
using System.IO;
using DotCMIS.Data.Impl;
using System.Net;

/**
 * Code demonstrating DotCMIS issue 983.
 * Modify AtomPubUrl, User, Password, formerToken
 */
namespace dotcmis_issue689
{
    class Program
    {
        static void Main(string[] args)
        {
            var token = "24420784"; // MODIFY HERE. Use a token from yesterday for instance, so that there are a few changes to get.

            // Create session.
            var parameters = new Dictionary<string, string>();
            parameters[SessionParameter.BindingType] = BindingType.AtomPub;
            parameters[SessionParameter.AtomPubUrl] = "https://localhost:8080/alfresco/api/-default-/public/cmis/versions/1.1/atom"; // MODIFY HERE
            parameters[SessionParameter.User] = "nicolas.raoul"; // MODIFY HERE
            parameters[SessionParameter.Password] = "put password here"; // MODIFY HERE
            var factory = SessionFactory.NewInstance();
            var repository = factory.GetRepositories(parameters)[0];
            var session = repository.CreateSession();
            
            Console.WriteLine("Latest token on repository: " + repository.LatestChangeLogToken);

            // Get changes.
            IChangeEvents changes;
            do
            {
                changes = session.GetContentChanges(token, false, 3);

                Console.WriteLine(changes.ChangeEventList.Count + " changes, token=" + token);

                token = changes.LatestChangeLogToken;
            }
            while (changes.HasMoreItems ?? false);
        }
    }
}
