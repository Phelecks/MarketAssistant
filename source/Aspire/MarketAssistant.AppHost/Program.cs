using Microsoft.Extensions.Configuration;

var builder = DistributedApplication.CreateBuilder(args);

const string UseInMemoryDatabase = "USE-INMEMORY-DATABASE";
const string EnsureDeletedDatabaseOnStartup = "ENSURE-DELETED-DATABASE-ON-STARTUP";
const string IdentitySecret = "IDENTITY_SECRET";
const string TokenIssuer = "TOKEN-ISSUER";
const string DatabaseEncryptionKey = "DATABASE-ENCRYPTION-KEY";
const string ApplicationName = "APPLICATION-NAME";
const string RpcUrls = "RPC-URLS";

// Add a secret parameters
var discordBotToken = builder.AddParameter("DISCORD-BOT-TOKEN", secret: true);
var identitySecret = builder.AddParameter("IDENTITY-SECRET", secret: true);
var sqlPassword = builder.AddParameter("SQL-SA-PASSWORD", secret: true);
//var rabbitUsername = builder.AddParameter("RABBIT-USERNAME", secret: true);
//var rabbitPassword = builder.AddParameter("RABBIT-PASSWORD", secret: true);
var databaseEncryptionKey = builder.AddParameter("DATABASE-ENCRYPTION-KEY", secret: true);

var redis = builder.AddRedis("cache").WithDataVolume();

var rabbit = builder.AddRabbitMQ("messaging")
    .WithDataVolume().WithManagementPlugin();

var sql = builder.AddSqlServer("sql", password: sqlPassword, 1433).WithDataVolume().WithImageTag("latest");

var informingDb = sql.AddDatabase(name: "informingdb", databaseName: "informing");
var identityDb = sql.AddDatabase(name: "identitydb", databaseName: "identity");
var blockProcessorDb = sql.AddDatabase(name: "blockprocessordb", databaseName: "blockProcessor");

var informingMigration = builder.AddProject<Projects.Informing_MigrationWorker>("informing-migrations")
        .WithReference(informingDb)
        .WithEnvironment(name: UseInMemoryDatabase, value: builder.Configuration.GetValue(UseInMemoryDatabase, "true"))
        .WithEnvironment(name: EnsureDeletedDatabaseOnStartup, value: builder.Configuration.GetValue(EnsureDeletedDatabaseOnStartup, "false"))
        .WithEnvironment(name: DatabaseEncryptionKey, parameter: databaseEncryptionKey)
        .WaitFor(identityDb);
var informing = builder.AddProject<Projects.Informing_Grpc>("informing")
   .WithReference(redis)
   .WithReference(rabbit)
   .WithReference(informingDb)
   .WithEnvironment(name: UseInMemoryDatabase, value: builder.Configuration.GetValue(UseInMemoryDatabase, "true"))
   .WithEnvironment(name: EnsureDeletedDatabaseOnStartup, value: builder.Configuration.GetValue(EnsureDeletedDatabaseOnStartup, "false"))
   .WithEnvironment(name: ApplicationName, value: "Informing")
   .WithEnvironment(name: TokenIssuer, value: builder.Configuration.GetValue(TokenIssuer, "https://identity.contoso.com"))
   .WithEnvironment(name: IdentitySecret, identitySecret)
   .WithEnvironment(name: "DISCORD_BOT_TOKEN", discordBotToken)
   .WaitFor(redis)
   .WaitFor(rabbit)
   .WaitFor(informingDb)
   .WaitFor(informingMigration);

var identityMigration = builder.AddProject<Projects.BlockChainIdentity_MigrationWorker>("blockchainidentity-migrations")
        .WithReference(identityDb)
        .WithEnvironment(name: UseInMemoryDatabase, value: builder.Configuration.GetValue(UseInMemoryDatabase, "true"))
        .WithEnvironment(name: EnsureDeletedDatabaseOnStartup, value: builder.Configuration.GetValue(EnsureDeletedDatabaseOnStartup, "false"))
        .WithEnvironment(name: DatabaseEncryptionKey, parameter: databaseEncryptionKey)
        .WithEnvironment(name: RpcUrls, value: builder.Configuration.GetValue<string>(RpcUrls))
        .WaitFor(identityDb);
var identity = builder.AddProject<Projects.BlockChainIdentity_Grpc>("identity")
   .WithReference(redis)
   .WithReference(rabbit)
   .WithReference(identityDb)
   .WithEnvironment(name: UseInMemoryDatabase, value: builder.Configuration.GetValue(UseInMemoryDatabase, "true"))
   .WithEnvironment(name: EnsureDeletedDatabaseOnStartup, value: builder.Configuration.GetValue(EnsureDeletedDatabaseOnStartup, "false"))
   .WithEnvironment(name: ApplicationName, value: "Identity")
   .WithEnvironment(name: TokenIssuer, value: builder.Configuration.GetValue(TokenIssuer, "https://identity.contoso.com"))
   .WithEnvironment(name: IdentitySecret, identitySecret)
   .WithEnvironment(name: DatabaseEncryptionKey, value: builder.Configuration.GetValue<string>(DatabaseEncryptionKey))
   .WaitFor(redis)
   .WaitFor(rabbit)
   .WaitFor(identityDb)
   .WaitFor(identityMigration);

var blockProcessorMigration = builder.AddProject<Projects.BlockProcessor_MigrationWorker>("blockprocessor-migrations")
    .WithReference(blockProcessorDb)
    .WithEnvironment(name: UseInMemoryDatabase, value: builder.Configuration.GetValue(UseInMemoryDatabase, "true"))
    .WithEnvironment(name: EnsureDeletedDatabaseOnStartup, value: builder.Configuration.GetValue(EnsureDeletedDatabaseOnStartup, "false"))
    .WithEnvironment(name: RpcUrls, value: builder.Configuration.GetValue<string>(RpcUrls))
    .WithEnvironment(name: DatabaseEncryptionKey, parameter: databaseEncryptionKey)
    .WaitFor(blockProcessorDb);
var blockProcessor = builder.AddProject<Projects.BlockProcessor_Api>("blockprocessor")
    .WithReference(redis)
    .WithReference(rabbit)
    .WithReference(blockProcessorDb)
    .WithEnvironment(name: UseInMemoryDatabase, value: builder.Configuration.GetValue(UseInMemoryDatabase, "true"))
    .WithEnvironment(name: EnsureDeletedDatabaseOnStartup, value: builder.Configuration.GetValue(EnsureDeletedDatabaseOnStartup, "false"))
    .WithEnvironment(name: ApplicationName, value: "BlockProcessor")
    .WithEnvironment(name: TokenIssuer, value: builder.Configuration.GetValue(TokenIssuer, "https://identity.contoso.com"))
    .WithEnvironment(name: IdentitySecret, parameter: identitySecret)
    .WithEnvironment(name: DatabaseEncryptionKey, parameter: databaseEncryptionKey)
    .WaitFor(redis)
    .WaitFor(rabbit)
    .WaitFor(blockProcessorDb)
    .WaitFor(blockProcessorMigration)
    .WaitFor(identity)
    .WaitFor(informing)
    .WithReplicas(2);

bool RunWalletTracker = builder.Configuration.GetValue("RUN-WALLET-TRACKER", false);
if(RunWalletTracker)
    builder.AddProject<Projects.WalletTracker_Api>("wallettracker")
       .WithReference(redis)
       .WithReference(rabbit)
       .WaitFor(redis)
       .WaitFor(rabbit);

builder.AddProject<Projects.ReverseProxy_Gateway>("reverseproxy-gateway")
   .WithReference(redis)
   .WithReference(identity)
   .WithReference(informing)
   .WithReference(blockProcessor)
   .WithEnvironment(name: ApplicationName, value: "ReverseProxy.Gateway")
   .WaitFor(redis)
   .WaitFor(rabbit)
   .WaitFor(identity)
   .WaitFor(informing)
   .WaitFor(blockProcessor);

await builder.Build().RunAsync();
