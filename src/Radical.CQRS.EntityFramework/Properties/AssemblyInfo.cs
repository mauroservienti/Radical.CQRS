using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("Radical.CQRS.EntityFramework")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("Radical.CQRS.EntityFramework")]
[assembly: AssemblyCopyright("Copyright ©  2015")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
[assembly: Guid("4dd2d422-6754-42e9-9ca9-44d2b0213efb")]

[assembly: AssemblyVersion( Consts.version )]
[assembly: AssemblyFileVersion( Consts.version )]
[assembly: AssemblyInformationalVersion( Consts.version + Consts.preRelease )]

class Consts
{
    public const string version = "0.1.1";
    public const string preRelease = "-RC1";
}