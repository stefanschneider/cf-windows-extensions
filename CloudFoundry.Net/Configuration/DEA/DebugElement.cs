﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace CloudFoundry.Net.Configuration.DEA
{
    public class DebugElement : ConfigurationElement
    {
           #region Constructors

        static DebugElement()
        {
            propertyName = new ConfigurationProperty(
                "name",
                typeof(string),
                null,
                ConfigurationPropertyOptions.IsRequired
            );

            propertyEnvironment = new ConfigurationProperty(
                "environment",
                typeof(EnvironmentCollection),
                new EnvironmentCollection(),
                ConfigurationPropertyOptions.IsRequired
            );

            properties = new ConfigurationPropertyCollection();

            properties.Add(propertyName);
            properties.Add(propertyEnvironment);
        }
        
        #endregion

        #region Static Fields

        private static ConfigurationProperty propertyName;
        private static ConfigurationProperty propertyEnvironment;

        private static ConfigurationPropertyCollection properties;
        #endregion

        #region Properties


        [ConfigurationProperty("name", IsRequired = true, DefaultValue = null)]
        public string Name
        {
            get
            {
                return (string)base[propertyName];
            }
        }

        [ConfigurationProperty("environment", IsRequired = true, DefaultValue = null)]
        public EnvironmentCollection Environment
        {
            get
            {
                return (EnvironmentCollection)base[propertyEnvironment];
            }
        }

        /// <summary>
        /// Override the Properties collection and return our custom one.
        /// </summary>
        protected override ConfigurationPropertyCollection Properties
        {
            get { return properties; }
        }

        #endregion
    }
}
