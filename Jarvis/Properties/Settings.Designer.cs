﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Jarvis.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.9.0.0")]
    internal sealed partial class Settings : global::System.Configuration.ApplicationSettingsBase {
        
        private static Settings defaultInstance = ((Settings)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new Settings())));
        
        public static Settings Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\Program Files\\Jarvis\\Resources\\exePaths.json")]
        public string ExePathsJsonPath {
            get {
                return ((string)(this["ExePathsJsonPath"]));
            }
            set {
                this["ExePathsJsonPath"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("ProjectJarvis\\Resources\\VoskModels\\vosk-model-small-ru-0.22")]
        public string relativePathVoskModel {
            get {
                return ((string)(this["relativePathVoskModel"]));
            }
            set {
                this["relativePathVoskModel"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("C:\\WINDOWS\\Microsoft.Net\\assembly\\GAC_MSIL\\Accessibility\\v4.0_4.0.0.0__b03f5f7f11" +
            "d50a3a\\Accessibility.dll")]
        public string AssemblyDllPath {
            get {
                return ((string)(this["AssemblyDllPath"]));
            }
            set {
                this["AssemblyDllPath"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("e327b05bf5e93f0560363f8bc007f7e2")]
        public string APIkeyWeather {
            get {
                return ((string)(this["APIkeyWeather"]));
            }
            set {
                this["APIkeyWeather"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("True")]
        public bool AntiPiracyStatus {
            get {
                return ((bool)(this["AntiPiracyStatus"]));
            }
            set {
                this["AntiPiracyStatus"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("10")]
        public int RecognitionWaitTime {
            get {
                return ((int)(this["RecognitionWaitTime"]));
            }
            set {
                this["RecognitionWaitTime"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("8")]
        public int RecognitionPostCommandWaitTime {
            get {
                return ((int)(this["RecognitionPostCommandWaitTime"]));
            }
            set {
                this["RecognitionPostCommandWaitTime"] = value;
            }
        }
    }
}
