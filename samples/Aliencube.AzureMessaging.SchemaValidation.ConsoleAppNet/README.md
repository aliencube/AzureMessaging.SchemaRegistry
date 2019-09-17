# Schema Validation Console App over Azure Service Bus for .NET Framework ##

This sample .NET console application generates a JSON schema, stores it into Azure Blob Storage, sends a message to Azure Service Bus and receives the message from the Azure Service Bus. While sending and receiving the message, it is validated against the schema stored at Azure Blob Storage.


## Prerequisites ##

In order to run this console application, the following Azure services **MUST** be ready to use:

* Azure Blob Storage and its connection string
* Azure Service Bus topic and subscription, and its connection string.


## Getting Started ##

1. Choose the configuration &ndash; `Debug` or `Release`.
2. Run the following command based on the configuration.
    ```bash
    dotnet run . \
           --configuration Debug \
           -- \
           --blob-connectionstring [STORAGE_ACCOUNT_CONNECTION_STRING] \
           --blob-baseuri https://[STORAGE_ACCOUNT_INSTANCE_NAME].blob.core.windows.net/ \
           --container [BLOB_CONTAINER_NAME] \
           --file-path [SCHEMA_FILE_NAME_WITH_PROPER_VERSIONING] \
           --servicebus-connectionstring [SERVICE_BUS_CONNECTION-STRING] \
           --topic [TOPIC_NAME] \
           --subscription [SUBSCRIPTION_NAME]
    ```
