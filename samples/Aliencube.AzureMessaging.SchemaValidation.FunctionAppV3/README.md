# Schema Validation Azure Function App (v3) ##

This sample Azure Function (v3) app validates the message payload.


## Getting Started ##

1. Choose the configuration &ndash; `Debug` or `Release`.
2. Update `local.settings.json` with:
    ```json
    {
      "IsEncrypted": false,
      "Values": {
        "AzureWebJobsStorage": "UseDevelopmentStorage=true",
        "FUNCTIONS_WORKER_RUNTIME": "dotnet",

        "SchemaVersion": "[SCHEMA_VERSION]",
        "SchemaFilename": "[SCHEMA_FILENAME]"
      }
    }
    ```
3. Put some break points wherever preferred.
4. Run the function app locally, and send a request to the following endpoint:
    ```bash
    POST http://localhost:7071/api/schema/validate
    ```
