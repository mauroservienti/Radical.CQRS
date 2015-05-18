using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle( "Radical.CQRS" )]
[assembly: AssemblyDescription( "" )]
[assembly: AssemblyConfiguration( "" )]
[assembly: AssemblyCompany( "" )]
[assembly: AssemblyProduct( "Radical.CQRS" )]
[assembly: AssemblyCopyright( "Copyright ©  2015" )]
[assembly: AssemblyTrademark( "" )]
[assembly: AssemblyCulture( "" )]
[assembly: ComVisible( false )]
[assembly: Guid( "a5d46977-9328-4cb8-8d06-c0c9203bb24f" )]
[assembly: AssemblyVersion( Consts.version )]
[assembly: AssemblyFileVersion( Consts.version )]
[assembly: AssemblyInformationalVersion( Consts.version + Consts.preRelease )]

class Consts
{
	public const string version = "0.0.0.1";
	public const string preRelease = "-Alfa";
}