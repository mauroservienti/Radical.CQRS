using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("Radical.CQRS.Client")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("Radical.CQRS.Client")]
[assembly: AssemblyCopyright("Copyright ©  2015")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
[assembly: Guid("1d1b36e0-f47f-4802-9f58-44f841b13488")]
[assembly: AssemblyVersion( Consts.version )]
[assembly: AssemblyFileVersion( Consts.version )]
[assembly: AssemblyInformationalVersion( Consts.version + Consts.preRelease )]

class Consts
{
	public const string version = "0.0.0.1";
	public const string preRelease = "-Alfa-1";
}