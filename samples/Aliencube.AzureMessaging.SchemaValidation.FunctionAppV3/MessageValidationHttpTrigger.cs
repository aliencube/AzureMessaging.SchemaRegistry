using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;

using Aliencube.AzureMessaging.SchemaValidation.Extensions;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

namespace Aliencube.AzureMessaging.SchemaValidation.FunctionAppV3
{
    [ExcludeFromCodeCoverage]
    public class MessageValidationHttpTrigger
    {
        private const string SchemaVersionKey = "SchemaVersion";
        private const string SchemaFilenameKey = "SchemaFilename";

        private readonly JsonSerializerSettings _settings;
        private readonly ISchemaValidator _validator;

        public MessageValidationHttpTrigger(JsonSerializerSettings settings, ISchemaValidator validator)
        {
            this._settings = settings ?? throw new ArgumentNullException(nameof(settings));
            this._validator = validator ?? throw new ArgumentNullException(nameof(validator));
        }

        [FunctionName(nameof(MessageValidationHttpTrigger))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "schema/validate")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var version = Environment.GetEnvironmentVariable(SchemaVersionKey);
            var filename = Environment.GetEnvironmentVariable(SchemaFilenameKey);
            var path = $"{version}/{filename}";

            var payload = default(string);
            using (var reader = new StreamReader(req.Body))
            {
                payload = await reader.ReadToEndAsync().ConfigureAwait(false);
            }

            var result = new ValidationResult();
            try
            {
                await payload.ValidateAsStringAsync(this._validator, path).ConfigureAwait(false);
                result.Validated = true;
            }
            catch
            {
                result.Validated = false;
            }

            var response = new OkObjectResult(result);

            return response;
        }
    }
}
