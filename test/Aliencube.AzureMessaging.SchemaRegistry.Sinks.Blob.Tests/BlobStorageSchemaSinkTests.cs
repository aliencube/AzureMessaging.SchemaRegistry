using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Threading.Tasks;

using FluentAssertions;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Aliencube.AzureMessaging.SchemaRegistry.Sinks.Blob.Tests
{
    [TestClass]
    [SuppressMessage("Usage", "CA1806:Do not ignore method results")]
    public class BlobStorageSchemaSinkTests
    {
        [TestMethod]
        public void Given_Type_Then_It_Should_Inherit_BaseClass()
        {
            typeof(BlobStorageSchemaSink).Should().BeDerivedFrom<SchemaSink>();
        }

        [TestMethod]
        public void Given_Type_Then_It_Should_Implement_Interfaces()
        {
            typeof(BlobStorageSchemaSink)
                .Should().Implement<ISchemaSink>()
                .And.Implement<IBlobStorageSchemaSink>()
                ;
        }

        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Constructors()
        {
            typeof(BlobStorageSchemaSink)
                .Should().HaveDefaultConstructor()
                .And.HaveConstructor(new[] { typeof(string) })
                .And.HaveConstructor(new[] { typeof(CloudBlobClient) })
                .And.HaveConstructor(new[] { typeof(string), typeof(CloudBlobClient) })
                ;
        }

        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Properties()
        {
            typeof(BlobStorageSchemaSink)
                .Should().HaveProperty<string>("Container")
                    .Which.Should().BeReadable()
                          .And.BeWritable()
                          .And.BeVirtual()
                          ;
        }

        [TestMethod]
        public void Given_Type_Then_It_Should_Have_Methods()
        {
            typeof(BlobStorageSchemaSink)
                .Should().HaveMethod("WithBlobClient", new[] { typeof(CloudBlobClient) })
                    .Which.Should().BeVirtual()
                        .And.Return<ISchemaSink>();

            typeof(BlobStorageSchemaSink)
                .Should().HaveMethod("WithContainer", new[] { typeof(string) })
                    .Which.Should().BeVirtual()
                        .And.Return<ISchemaSink>();

            typeof(BlobStorageSchemaSink)
                .Should().HaveMethod("GetSchemaAsync", new[] { typeof(string) })
                    .Which.Should().BeVirtual()
                        .And.BeAsync()
                        .And.Return<Task<string>>();

            typeof(BlobStorageSchemaSink)
                .Should().HaveMethod("SetSchemaAsync", new[] { typeof(string), typeof(string) })
                    .Which.Should().BeVirtual()
                        .And.BeAsync()
                        .And.Return<Task<bool>>();
        }

        [TestMethod]
        public void Given_Default_When_Instantiated_Then_It_Should_Return_Value()
        {
            var instance = new BlobStorageSchemaSink();

            instance.BaseLocation.Should().BeEmpty();
            instance.Container.Should().BeEmpty();
        }

        [TestMethod]
        public void Given_Null_Parameters_When_Instantiated_Then_It_Should_Throw_Exception()
        {
            var action = default(Action);

            action = () => new BlobStorageSchemaSink(location: null);
            action.Should().Throw<ArgumentNullException>();

            action = () => new BlobStorageSchemaSink(blobClient: null);
            action.Should().Throw<ArgumentNullException>();

            action = () => new BlobStorageSchemaSink(null, null);
            action.Should().Throw<ArgumentNullException>();

            action = () => new BlobStorageSchemaSink("hello-world", null);
            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_Null_BlobClient_When_WithBlobClient_Invoked_Then_It_Should_Throw_Exception()
        {
            var instance = new BlobStorageSchemaSink();

            Action action = () => instance.WithBlobClient(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [TestMethod]
        public void Given_BlobClient_When_WithBlobClient_Invoked_Then_It_Should_Return_Result()
        {
            var uri = new Uri("http://localhost");
            var blobClient = new CloudBlobClient(uri);
            var instance = new BlobStorageSchemaSink();

            var result = instance.WithBlobClient(blobClient);

            var field = typeof(BlobStorageSchemaSink).GetField("_blobClient", BindingFlags.NonPublic | BindingFlags.Instance);

            field.GetValue(instance).Should().Be(blobClient);
        }

        [TestMethod]
        public void Given_Null_Container_When_WithContainer_Invoked_Then_It_Should_Throw_Exception()
        {
            var instance = new BlobStorageSchemaSink();

            Action action = () => instance.WithContainer(null);

            action.Should().Throw<ArgumentNullException>();
        }

        [DataTestMethod]
        [DataRow("hello-world")]
        public void Given_Container_When_WithContainer_Invoked_Then_It_Should_Return_Value(string container)
        {
            var instance = new BlobStorageSchemaSink();

            instance.WithContainer(container);

            instance.Container.Should().Be(container);
        }

        [TestMethod]
        public void Given_Null_Path_When_GetSchemaAsync_Invoked_Then_It_Should_Throw_Exception()
        {
            var instance = new BlobStorageSchemaSink();

            Func<Task> func = async () => await instance.GetSchemaAsync(null).ConfigureAwait(false);

            func.Should().Throw<ArgumentNullException>();
        }

        [DataTestMethod]
        [DataRow("hello", "default.json")]
        public async Task Given_Invalid_Container_When_GetSchemaAsync_Invoked_Then_It_Should_Throw_Exception(string container, string path)
        {
            var account = CloudStorageAccount.DevelopmentStorageAccount;
            var blobClient = account.CreateCloudBlobClient();
            await blobClient.GetContainerReference(container)
                            .DeleteIfExistsAsync()
                            .ConfigureAwait(false);

            var instance = new BlobStorageSchemaSink(blobClient)
                               .WithContainer(container);

            Func<Task> func = async () => await instance.GetSchemaAsync(path).ConfigureAwait(false);

            func.Should().Throw<BlobContainerNotFoundException>();
        }

        [DataTestMethod]
        [DataRow("world", "default.json")]
        public async Task Given_Invalid_Blob_When_GetSchemaAsync_Invoked_Then_It_Should_Throw_Exception(string container, string path)
        {
            var account = CloudStorageAccount.DevelopmentStorageAccount;
            var blobClient = account.CreateCloudBlobClient();
            await blobClient.GetContainerReference(container)
                            .CreateIfNotExistsAsync()
                            .ConfigureAwait(false);

            var instance = new BlobStorageSchemaSink(blobClient)
                               .WithContainer(container);

            Func<Task> func = async () => await instance.GetSchemaAsync(path).ConfigureAwait(false);

            func.Should().Throw<BlobNotFoundException>();

            await blobClient.GetContainerReference(container)
                            .DeleteIfExistsAsync()
                            .ConfigureAwait(false);
        }

        [DataTestMethod]
        [DataRow("lorem", "default.json", "{ \"lorem\": \"ipsum\" }")]
        public async Task Given_Blob_When_GetSchemaAsync_Invoked_Then_It_Should_Return_Result(string container, string path, string schema)
        {
            var account = CloudStorageAccount.DevelopmentStorageAccount;
            var blobClient = account.CreateCloudBlobClient();
            var blobContainer = blobClient.GetContainerReference(container);
            await blobContainer.CreateIfNotExistsAsync().ConfigureAwait(false);

            var blob = blobContainer.GetBlockBlobReference(path);
            await blob.UploadTextAsync(schema).ConfigureAwait(false);

            var instance = new BlobStorageSchemaSink(blobClient)
                               .WithContainer(container);

            var result = await instance.GetSchemaAsync(path).ConfigureAwait(false);

            result.Should().Be(schema);

            await blob.DeleteIfExistsAsync().ConfigureAwait(false);
            await blobClient.GetContainerReference(container)
                            .DeleteIfExistsAsync()
                            .ConfigureAwait(false);
        }

        [TestMethod]
        public void Given_Null_Parameters_When_SetSchemaAsync_Invoked_Then_It_Should_Throw_Exception()
        {
            var schema = "{ \"lorem\": \"ipsum\" }";
            var instance = new BlobStorageSchemaSink();

            var func = default(Func<Task>);

            func = async () => await instance.SetSchemaAsync(null, null).ConfigureAwait(false);
            func.Should().Throw<ArgumentNullException>();

            func = async () => await instance.SetSchemaAsync(schema, null).ConfigureAwait(false);
            func.Should().Throw<ArgumentNullException>();
        }

        [DataTestMethod]
        [DataRow("ipsum", "{ \"lorem\": \"ipsum\" }", "default.json")]
        public async Task Given_Blob_When_SetSchemaAsync_Invoked_Then_It_Should_Return_Result(string container, string schema, string path)
        {
            var account = CloudStorageAccount.DevelopmentStorageAccount;
            var blobClient = account.CreateCloudBlobClient();
            var blobContainer = blobClient.GetContainerReference(container);
            await blobContainer.CreateIfNotExistsAsync().ConfigureAwait(false);

            var blob = blobContainer.GetBlockBlobReference(path);
            await blob.UploadTextAsync(schema).ConfigureAwait(false);

            var instance = new BlobStorageSchemaSink(blobClient)
                               .WithContainer(container);

            var result = await instance.SetSchemaAsync(schema, path).ConfigureAwait(false);

            result.Should().BeTrue();

            await blob.DeleteIfExistsAsync().ConfigureAwait(false);
            await blobClient.GetContainerReference(container)
                            .DeleteIfExistsAsync()
                            .ConfigureAwait(false);
        }
    }
}
