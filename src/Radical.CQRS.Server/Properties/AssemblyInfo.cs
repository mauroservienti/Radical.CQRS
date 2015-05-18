using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle( "Radical.CQRS.Server" )]
[assembly: AssemblyDescription( "" )]
[assembly: AssemblyConfiguration( "" )]
[assembly: AssemblyCompany( "" )]
[assembly: AssemblyProduct( "Radical.CQRS.Server" )]
[assembly: AssemblyCopyright( "Copyright ©  2015" )]
[assembly: AssemblyTrademark( "" )]
[assembly: AssemblyCulture( "" )]
[assembly: ComVisible( false )]
[assembly: Guid( "858182a7-c4d1-47a3-bbfd-52ff52bf1d42" )]
[assembly: AssemblyVersion( Consts.version )]
[assembly: AssemblyFileVersion( Consts.version )]
[assembly: AssemblyInformationalVersion( Consts.version + Consts.preRelease )]

class Consts
{
	public const string version = "0.0.0.1";
	public const string preRelease = "-Alfa";
}