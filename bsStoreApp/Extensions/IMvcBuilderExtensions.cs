using bsStoreApp.Utilities.Formatters;

namespace bsStoreApp.Extensions;

    public static class IMvcBuilderExtensions
    {
        public static IMvcBuilder AddCustomCsvFormatter(this IMvcBuilder builder) => builder.AddMvcOptions(configure => configure.OutputFormatters.Add(new CsvOutputFormatter()));
    }
