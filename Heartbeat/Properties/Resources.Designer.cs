﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Heartbeat.Properties {
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
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Heartbeat.Properties.Resources", typeof(Resources).Assembly);
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
        ///   Looks up a localized string similar to Downstream.
        /// </summary>
        internal static string File_Downstream {
            get {
                return ResourceManager.GetString("File_Downstream", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to SignalStats.
        /// </summary>
        internal static string File_SignalStats {
            get {
                return ResourceManager.GetString("File_SignalStats", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Upstream.
        /// </summary>
        internal static string File_Upstream {
            get {
                return ResourceManager.GetString("File_Upstream", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to C:\Users\User\Downloads\Heartbeat\{0}.{1}.csv.
        /// </summary>
        internal static string FileNameFormat {
            get {
                return ResourceManager.GetString("FileNameFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [{0} {1}] - Unable to reach page. Status: &apos;{2} - {3}&apos;.
        /// </summary>
        internal static string Format_StatusMessageConsole_Error {
            get {
                return ResourceManager.GetString("Format_StatusMessageConsole_Error", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to [{0} {1}] - Success.
        /// </summary>
        internal static string Format_StatusMessageConsole_Success {
            get {
                return ResourceManager.GetString("Format_StatusMessageConsole_Success", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Unable to reach page. Status: &apos;{0} - {1}&apos;.
        /// </summary>
        internal static string Format_StatusMessageLog_Error {
            get {
                return ResourceManager.GetString("Format_StatusMessageLog_Error", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Success.
        /// </summary>
        internal static string Format_StatusMessageLog_Success {
            get {
                return ResourceManager.GetString("Format_StatusMessageLog_Success", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Timestamp,Channel ID,Frequency (Hz),Signal to Noise Ratio (dB),Downstream Modulation,Power Level (dBmV).
        /// </summary>
        internal static string Header_Downstream {
            get {
                return ResourceManager.GetString("Header_Downstream", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Timestamp,Channel ID,Total Unerrored Codewords,Total Correctable Codewords,Total Uncorrectable Codewords.
        /// </summary>
        internal static string Header_SignalStats {
            get {
                return ResourceManager.GetString("Header_SignalStats", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Timestamp,Channel ID,Frequency (Hz),Ranging Service ID,Symbol Rate (Msym/sec),Power Level (dBmV),Upstream Modulation,Ranging Status.
        /// </summary>
        internal static string Header_Upstream {
            get {
                return ResourceManager.GetString("Header_Upstream", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to /html[1]/body[1]/center[1]/table[1] //td.
        /// </summary>
        internal static string NodeSelection_Downstream {
            get {
                return ResourceManager.GetString("NodeSelection_Downstream", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to /html[1]/body[1]/center[3]/table[1] //td.
        /// </summary>
        internal static string NodeSelection_SignalStats {
            get {
                return ResourceManager.GetString("NodeSelection_SignalStats", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to /html[1]/body[1]/center[2]/table[1] //td.
        /// </summary>
        internal static string NodeSelection_Upstream {
            get {
                return ResourceManager.GetString("NodeSelection_Upstream", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to http://192.168.100.1/cmSignalData.htm.
        /// </summary>
        internal static string URL {
            get {
                return ResourceManager.GetString("URL", resourceCulture);
            }
        }
    }
}
