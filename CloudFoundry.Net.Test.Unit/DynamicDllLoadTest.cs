﻿using System;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Uhuru.CloudFoundry.Server.DEA;
using Uhuru.CloudFoundry.Server.DEA.PluginBase;
using System.Windows.Forms;


namespace CloudFoundry.Net.Test.Unit
{
    [TestClass]
    [DeploymentItem("TestDLLToLoad.dll")]
    public class DynamicDllLoadTest
    {
        IAgentPlugin agent;
        string dllFolderPath = Path.GetFullPath(@"..\Out");
        string dllFileName = "TestDLLToLoad.dll";

        string resultFilePath = @"D:/file.txt";


        public DynamicDllLoadTest()
        { 
            ////copy the dll file in the base folder
            //string destinationFileName = Path.Combine(dllFolderPath, dllFileName);
            //if (File.Exists(destinationFileName)) File.Delete(destinationFileName);

            //File.Copy(Path.Combine(dllFolderPath, "lib", dllFileName), destinationFileName);
        }

        [TestInitialize]
        public void Setup()
        {
            //get an IAgentPlugin
            Guid guid = PluginHost.LoadPlugin(Path.Combine(dllFolderPath, dllFileName), "TheDLLToLoad.TestClass");
            agent = PluginHost.CreateInstance(guid);
        }

        [TestCleanup]
        public void Teardown()
        {
            //clear file
            if (File.Exists(resultFilePath)) File.Delete(resultFilePath); 
        }

        [TestMethod]
        public void T001CallStartApplication()
        {
            agent.StartApplication();

            Assert.IsTrue(File.Exists(resultFilePath)); //the file should have been created

            string[] content = File.ReadAllLines(resultFilePath);
            Assert.IsTrue(content.Contains("StartApplication")); //file should contain this string
        }

        [TestMethod]
        public void T002CallStopApplication()
        {
            agent.StopApplication();

            Assert.IsTrue(File.Exists(resultFilePath)); //the file should have been created

            string[] content = File.ReadAllLines(resultFilePath);
            Assert.IsTrue(content.Contains("StopApplication")); //file should contain this string
        }

        [TestMethod]
        public void T003CallKillApplication()
        {
            agent.KillApplication();

            Assert.IsTrue(File.Exists(resultFilePath)); //the file should have been created

            string[] content = File.ReadAllLines(resultFilePath);
            Assert.IsTrue(content.Contains("KillApplication")); //file should contain this string
        }

        [TestMethod]
        public void T004CallRemoveInstance()
        {
            PluginHost.RemoveInstance(agent);

            Assert.IsTrue(File.Exists(resultFilePath)); //the file should have been created

            string[] content = File.ReadAllLines(resultFilePath);
            Assert.IsTrue(content.Contains("StopApplication")); //file should contain this string, as StopApplication was called on the app
        }

        [TestMethod]
        public void T005CallConfigureDebug()
        {
            string firstParameter = "param1";
            string secondParameter = "param2";

            agent.ConfigureDebug(firstParameter, secondParameter, null);//new ApplicationVariable[0]);

            Assert.IsTrue(File.Exists(resultFilePath)); //the file should have been created
            string[] content = File.ReadAllLines(resultFilePath);

            string row = content.Where(r => r.StartsWith("ConfigureDebug", StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

            Assert.AreNotEqual(row, default(string)); //a row fulfilling the condition should be found

            string[] parts = row.Split(' ');

            Assert.AreEqual(parts.Length, 3);
            Assert.AreEqual(parts[1], firstParameter);
            Assert.AreEqual(parts[2], secondParameter);
            
        }


    }
}
