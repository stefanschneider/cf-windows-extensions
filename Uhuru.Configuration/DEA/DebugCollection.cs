﻿using System.Configuration;

namespace Uhuru.Configuration.DEA
{
    /// <summary>
    /// This class is a collection of DebugElement.
    /// </summary>
    [ConfigurationCollection(typeof(DebugElement), CollectionType = ConfigurationElementCollectionType.BasicMap)]
    public class DebugCollection : ConfigurationElementCollection
    {
        #region Constructors
        static DebugCollection()
        {
            properties = new ConfigurationPropertyCollection();
        }

        /// <summary>
        /// Public parameterless constructor.
        /// </summary>
        public DebugCollection()
        {
        }
        #endregion

        #region Fields
        private static ConfigurationPropertyCollection properties;
        #endregion

        #region Properties

        /// <summary>
        /// Defines the configuration properties available for a DebugCollection
        /// </summary>
        protected override ConfigurationPropertyCollection Properties
        {
            get { return properties; }
        }

        /// <summary>
        /// Defines the collection type (BasicMap) for DebugCollection
        /// </summary>
        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.BasicMap; }
        }

        /// <summary>
        /// Gets the element name for a DebugCollection
        /// </summary>
        protected override string ElementName
        {
            get { return "debugConfiguration"; }
        }

        #endregion

        #region Indexers

        /// <summary>
        /// Gets a debug configuration by index.
        /// </summary>
        /// <param name="index">Zero-based index of a debug configuration.</param>
        /// <returns>The DebugElement at the specified index.</returns>
        public DebugElement this[int index]
        {
            get { return (DebugElement)base.BaseGet(index); }
            set
            {
                if (base.BaseGet(index) != null)
                {
                    base.BaseRemoveAt(index);
                }
                base.BaseAdd(index, value);
            }
        }

        /// <summary>
        /// Gets a debug configuration by its name.
        /// </summary>
        /// <param name="name">String specifying the debug configuration name.</param>
        /// <returns>The DebugElement with the specified name.</returns>
        public new DebugElement this[string name]
        {
            get { return (DebugElement)base.BaseGet(name); }
        }
        #endregion

        #region Overrides

        /// <summary>
        /// This method creates a new DebugElement.
        /// </summary>
        /// <returns>A new DebugElement.</returns>
        protected override ConfigurationElement CreateNewElement()
        {
            return new DebugElement();
        }

        /// <summary>
        /// This method gets an element key name for a DebugElement.
        /// </summary>
        /// <param name="element">The DebugElement for which to get the key.</param>
        /// <returns>A string that is the name of the debug configuration.</returns>
        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as DebugElement).Name;
        }
        #endregion
    }
}
