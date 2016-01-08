using System;
using System.Reflection;

namespace WindowsPhone81TVNL.Helpers
{
    public class AssemblyInformation
    {
        readonly Assembly _assembly;

        public AssemblyInformation()
        {
            _assembly = this.GetType().GetTypeInfo().Assembly;
        }

        public AssemblyInformation(Assembly assembly)
        {
            _assembly = assembly;
            
        }

        public string Title
        {
            get { return GetValue<AssemblyTitleAttribute>(a => a.Title); }
        }

        public string Product
        {
            get { return GetValue<AssemblyProductAttribute>(a => a.Product); }
        }

        public string Copyright
        {
            get { return GetValue<AssemblyCopyrightAttribute>(a => a.Copyright); }
        }

        public string Company
        {
            get { return GetValue<AssemblyCompanyAttribute>(a => a.Company); }
        }

        public string Description
        {
            get { return GetValue<AssemblyDescriptionAttribute>(a => a.Description); }
        }

        public string Trademark
        {
            get { return GetValue<AssemblyTrademarkAttribute>(a => a.Trademark); }
        }

        public string Configuration
        {
            get { return GetValue<AssemblyConfigurationAttribute>(a => a.Configuration); }
        }

        public Version Version
        {
            get { return new AssemblyName(_assembly.FullName).Version; }
        }

        public string FileVersion
        {
            get { return GetValue<AssemblyFileVersionAttribute>(a => a.Version); }
        }

        public string InformationalVersion
        {
            get { return GetValue<AssemblyInformationalVersionAttribute>(a => a.InformationalVersion); }
        }

        string GetValue<T>(Func<T, string> getValue) where T : Attribute
        {
            var a = _assembly.GetCustomAttribute<T>();
            return a == null ? "" : getValue(a);
        }
    }
}
