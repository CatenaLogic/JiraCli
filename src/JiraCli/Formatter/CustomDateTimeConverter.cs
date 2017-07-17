using Newtonsoft.Json.Converters;

namespace JiraCli.Models.Formatter
{
    class CustomDateTimeConverter : IsoDateTimeConverter
    {
        public CustomDateTimeConverter()
        {
            base.DateTimeFormat = "yyyy-MM-dd";
        }
    }
}
