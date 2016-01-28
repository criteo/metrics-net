using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("metrics")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("metrics")]
[assembly: AssemblyCopyright("Copyright © Daniel Crenna 2011")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: AllowPartiallyTrustedCallers]
#if COREFX
#else
[assembly: SecurityRules(SecurityRuleSet.Level1)]
#endif

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("922ec375-7f03-46b9-95d7-d990c6f7795c")]

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
[assembly: InternalsVisibleTo("metrics.Tests, PublicKey=0024000004800000940000000602000000240000525341310004000001000100a720382e6cdd8f44505b6a075d38de5b2787ae099625d7261920db489f7db803602bc6e2353d4151eb6d55d98848cff7c4eee03e9fbdc6936a6616cc5d626c96676672d7d41a23e15a624db9d48308547797876af10c13f09af9dd5236e3a5f9b6362dc24714b087ce24d3628b2f3e3e09665896aef203d1abaeebcdd09858b7")]
