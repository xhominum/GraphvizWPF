﻿using System;
using System.Reflection;
using System.Runtime.InteropServices;
using PostSharp.Samples.CustomLogging.Aspects;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("Graphviz4Net.Core")]
[assembly: AssemblyDescription("Graphviz4Net Core Library")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("...")]
[assembly: AssemblyProduct("Graphviz4Net.Core")]
[assembly: AssemblyCopyright("Copyright Steve Sindelar © 2011")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: CLSCompliant(true)]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("a144dbb8-4d2f-4c5e-82dd-abb619b13b7b")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

//[assembly: LogMethod(AttributePriority = 0)]

////Remove logging from the Aspects namespace to avoid infinite recursions(logging would log itself).

//[assembly:
//    LogMethod(AttributePriority = 1, AttributeExclude = true,
//        AttributeTargetTypes = "PostSharp.Samples.CustomLogging.Aspects.*")]

//[assembly:
//    LogMethod(AttributePriority = 1, AttributeExclude = true,
//        AttributeTargetTypes = "PostSharp.Samples.CustomLogging.Helpers.*")]

//[assembly:
//    LogMethod(AttributePriority = 1, AttributeExclude = true,
//        AttributeTargetTypes = "*GetHashCode*")]
