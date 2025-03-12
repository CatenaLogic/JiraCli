namespace JiraCli.Models.Formatters
{
    using Newtonsoft.Json.Converters;

    class CustomDateTimeConverter : IsoDateTimeConverter
    {
        public CustomDateTimeConverter()
        {
            base.DateTimeFormat = "yyyy-MM-dd";
        }
    }
}