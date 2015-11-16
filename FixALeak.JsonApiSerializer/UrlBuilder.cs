using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using System.Text;

namespace FixALeak.JsonApiSerializer
{
    public class UrlBuilder
    {
        private StringBuilder _sb;
        private static PluralizationService PluralizationService
        {
            get
            {
                return PluralizationService.CreateService(new CultureInfo("en-US"));
            }
        }

        private UrlBuilder(StringBuilder sb)
        {
            _sb = sb;
        }

        public static UrlBuilder Initialize()
        {
            var sb = new StringBuilder();
            if (!string.IsNullOrEmpty(SerializerConfiguration.Prefix))
            {
                sb.Append("/");
                sb.Append(SerializerConfiguration.Prefix);
            }

            return new UrlBuilder(sb);
        }

        public UrlBuilder Resource(string resoourceName)
        {
            _sb.Append("/");
            _sb.Append(PluralizationService.Pluralize(resoourceName));
            return this;
        }

        public UrlBuilder Id(int id)
        {
            _sb.Append("/");
            _sb.Append(id);
            return this;
        }

        public override string ToString()
        {
            return _sb.ToString();
        }
    }

}
