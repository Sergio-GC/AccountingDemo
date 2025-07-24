using Microsoft.Extensions.Localization;
using System.Reflection;

namespace WebApp.src
{
    public class LocaleService
    {
        private readonly IStringLocalizer _stringLocalizer;

        public LocaleService(IStringLocalizerFactory factory)
        {
            var type = typeof(SharedLocale);
            var assemblyName = new AssemblyName(type.Assembly.FullName);

            _stringLocalizer = factory.Create("SharedLocale", assemblyName.Name);
        }

        public LocalizedString GetLocalizedString(string key)
        {
            return _stringLocalizer[key];
        }
    }
}
