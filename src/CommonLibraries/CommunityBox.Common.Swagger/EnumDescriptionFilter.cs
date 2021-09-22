using System;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CommunityBox.Common.Swagger
{
    /// <summary>
    ///     Фильтр для вывода имени членов перечислений.
    /// </summary>
    public class EnumDescriptionFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            foreach (var openApiSchema in context.SchemaRepository.Schemas)
            {
                foreach (var property in openApiSchema.Value.Properties)
                {
                    if (property.Value.Enum != null)
                    {
                        ModifyEnums(property.Value);
                    }
                }

                if (openApiSchema.Value.Enum != null)
                {
                    ModifyEnums(openApiSchema.Value);
                }
            }
        }

        private static void ModifyEnums(OpenApiSchema schema)
        {
            var values = schema.Enum.Select(en =>
            {
                var enType = en.GetType();
                if (!enType.IsEnum)
                    return string.Empty;

                var enu = (Enum) en;
                return string.Concat(
                    $"{Convert.ChangeType(enu, enu.GetTypeCode(), CultureInfo.CurrentCulture)}",
                    $" = {Enum.GetName(enType, en)}");
            });

            var sb = new StringBuilder();
            foreach (var value in values)
            {
                sb.AppendLine(value);
            }
            schema.Description = sb.ToString();
        }
    }
}