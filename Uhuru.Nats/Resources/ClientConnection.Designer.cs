﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.239
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Uhuru.NatsClient.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class ClientConnection {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ClientConnection() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Uhuru.NatsClient.Resources.ClientConnection", typeof(ClientConnection).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to \r\n.
        /// </summary>
        internal static string CR_LF {
            get {
                return ResourceManager.GetString("CR_LF", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to \A-ERR\s+(&apos;.+&apos;)?\r\n.
        /// </summary>
        internal static string ERR {
            get {
                return ResourceManager.GetString("ERR", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Reactor.
        /// </summary>
        internal static string EventSource {
            get {
                return ResourceManager.GetString("EventSource", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to \AINFO\s+([^\r\n]+)\r\n.
        /// </summary>
        internal static string INFO {
            get {
                return ResourceManager.GetString("INFO", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to NatsClient.
        /// </summary>
        internal static string LOGSOURCE {
            get {
                return ResourceManager.GetString("LOGSOURCE", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to 1.
        /// </summary>
        internal static string MAX_RECONNECT_ATTEMPTS {
            get {
                return ResourceManager.GetString("MAX_RECONNECT_ATTEMPTS", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to \AMSG\s+([^\s]+)\s+([^\s]+)\s+(([^\s]+)[^\S\r\n]+)?(\d+)\r\n.
        /// </summary>
        internal static string MSG {
            get {
                return ResourceManager.GetString("MSG", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to \A\+OK\s*\r\n.
        /// </summary>
        internal static string OK {
            get {
                return ResourceManager.GetString("OK", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to \APING\s*\r\n.
        /// </summary>
        internal static string PING {
            get {
                return ResourceManager.GetString("PING", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to PING{0}.
        /// </summary>
        internal static string PING_REQUEST {
            get {
                return ResourceManager.GetString("PING_REQUEST", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to \APONG\s*\r\n.
        /// </summary>
        internal static string PONG {
            get {
                return ResourceManager.GetString("PONG", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to PONG{0}.
        /// </summary>
        internal static string PONG_RESPONSE {
            get {
                return ResourceManager.GetString("PONG_RESPONSE", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to \A(.*)\r\n.
        /// </summary>
        internal static string UNKNOWN {
            get {
                return ResourceManager.GetString("UNKNOWN", resourceCulture);
            }
        }
    }
}
