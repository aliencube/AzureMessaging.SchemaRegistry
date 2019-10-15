# Schema Registry Console App for Local File System ##

This sample console application generates a JSON schema and stores it into a local file system through [`FileSystemSchemaSink`](../docs/schema-registry-sinks-file-system.md).


## Getting Started ##

1. Choose the target framework &ndash; .NET Framework (4.6.1+) or .NET Core (2.1+).
2. Choose the configuration &ndash; `Debug` or `Release`.
3. Choose the base location of the local file system as the schema registry.
4. Choose the file name and path representing the schema file to be stored.
5. Run the following command based on the target framework and configuration.
    ```bash
    dotnet run . \
           --configuration Debug \
           --framework netcoreapp2.1 \
           -- \
           --blob-connectionstring [STORAGE_ACCOUNT_CONNECTION_STRING] \
           --blob-baseuri https://[STORAGE_ACCOUNT_INSTANCE_NAME].blob.core.windows.net/ \
           --blob-container [BLOB_CONTAINER_NAME] \
           --file-baselocation /etc/schema-registry \
           --file-path v1/schema.json
    ```
