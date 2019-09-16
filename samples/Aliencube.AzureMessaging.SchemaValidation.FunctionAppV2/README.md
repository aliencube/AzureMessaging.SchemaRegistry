# Schema Validation Azure Function App (v2) for Azure Service Bus ##

This sample Azure Function (v2) app sends a message to Azure Service Bus and receives the message from the Azure Service Bus. While sending and receiving the message, it is validated against the schema stored at the local file system.


## Prerequisites ##

In order to run this console application, the following Azure services **MUST** be ready to use:

* Azure Service Bus topic and subscription, and its connection string.


## Getting Started ##

1. Choose the configuration &ndash; `Debug` or `Release`.
2. Update `local.settings.json` with:
    ```json
    {
      "IsEncrypted": false,
      "Values": {
        "AzureWebJobsStorage": "UseDevelopmentStorage=true",
        "FUNCTIONS_WORKER_RUNTIME": "dotnet",

        "AzureServiceBusConnectionString": "[AZURE_SEVICE_BUS_CONNECTION_STRING]",
        "ServiceBusTopic": "[AZURE_SERVICE_BUS_TOPIC]",
        "ServiceBusTopicSubscription": "[AZURE_SERVICE_BUS_SUBSCRIPTION]"
      }
    }
    ```
3. Put some break points wherever preferred.
4. Run the function app locally, and send a request to the following endpoint:
    ```bash
    http://localhost:7071/api/servicebus/publish
    ```
