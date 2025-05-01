using Microsoft.Extensions.Configuration;

var builder = DistributedApplication.CreateBuilder(args);

const string UseInMemoryDatabase = "USE-INMEMORY-DATABASE";
const string EnsureDeletedDatabaseOnStartup = "ENSURE-DELETED-DATABASE-ON-STARTUP";
const string IdentitySecret = "IDENTITY_SECRET";
const string TokenIssuer = "TOKEN-ISSUER";
const string DatabaseEncryptionKey = "DATABASE-ENCRYPTION-KEY";
const string ApplicationName = "APPLICATION-NAME";
const string RpcUrls = "RPC-URLS";
const string IdentityIssuer = "https://identity.contoso.com";

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
var blockChainDb = sql.AddDatabase(name: "blockchaindb", databaseName: "blockChain");
var logProcessorDb = sql.AddDatabase(name: "logprocessordb", databaseName: "logProcessor");

var informingMigration = builder.AddProject<Projects.Informing_MigrationWorker>("informing-migrations")
        .WithReference(informingDb)
        .WithEnvironment(name: UseInMemoryDatabase, value: builder.Configuration.GetValue(UseInMemoryDatabase, "true"))
        .WithEnvironment(name: EnsureDeletedDatabaseOnStartup, value: builder.Configuration.GetValue(EnsureDeletedDatabaseOnStartup, "false"))
        .WithEnvironment(name: DatabaseEncryptionKey, parameter: databaseEncryptionKey)
        .WithEnvironment(name: "DISCORD-BOT-TOKEN", discordBotToken)
        .WaitFor(identityDb);
var informing = builder.AddProject<Projects.Informing_Grpc>("informing")
   .WithReference(redis)
   .WithReference(rabbit)
   .WithReference(informingDb)
   .WithEnvironment(name: UseInMemoryDatabase, value: builder.Configuration.GetValue(UseInMemoryDatabase, "true"))
   .WithEnvironment(name: EnsureDeletedDatabaseOnStartup, value: builder.Configuration.GetValue(EnsureDeletedDatabaseOnStartup, "false"))
   .WithEnvironment(name: ApplicationName, value: "Informing")
   .WithEnvironment(name: TokenIssuer, value: builder.Configuration.GetValue(TokenIssuer, IdentityIssuer))
   .WithEnvironment(name: IdentitySecret, identitySecret)
   .WaitFor(redis)
   .WaitFor(rabbit)
   .WaitFor(informingDb)
   .WaitForCompletion(informingMigration)
   .WithReplicas(1);

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
   .WithEnvironment(name: TokenIssuer, value: builder.Configuration.GetValue(TokenIssuer, IdentityIssuer))
   .WithEnvironment(name: IdentitySecret, identitySecret)
   .WithEnvironment(name: DatabaseEncryptionKey, value: builder.Configuration.GetValue<string>(DatabaseEncryptionKey))
   .WaitFor(redis)
   .WaitFor(rabbit)
   .WaitFor(identityDb)
   .WaitForCompletion(identityMigration)
   .WithReplicas(1);

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
    .WithEnvironment(name: TokenIssuer, value: builder.Configuration.GetValue(TokenIssuer, IdentityIssuer))
    .WithEnvironment(name: IdentitySecret, parameter: identitySecret)
    .WithEnvironment(name: DatabaseEncryptionKey, parameter: databaseEncryptionKey)
    .WaitFor(redis)
    .WaitFor(rabbit)
    .WaitFor(blockProcessorDb)
    .WaitForCompletion(blockProcessorMigration)
    .WaitFor(identity)
    .WaitFor(informing)
    .WithReplicas(1);

var blockChainMigration = builder.AddProject<Projects.BlockChain_MigrationWorker>("blockchain-migrations")
    .WithReference(blockChainDb)
    .WithEnvironment(name: UseInMemoryDatabase, value: builder.Configuration.GetValue(UseInMemoryDatabase, "true"))
    .WithEnvironment(name: EnsureDeletedDatabaseOnStartup, value: builder.Configuration.GetValue(EnsureDeletedDatabaseOnStartup, "false"))
    .WithEnvironment(name: DatabaseEncryptionKey, parameter: databaseEncryptionKey)
    .WaitFor(blockChainDb);
var blockChain = builder.AddProject<Projects.BlockChain_Api>("blockchain")
    .WithReference(redis)
    .WithReference(rabbit)
    .WithReference(blockChainDb)
    .WithEnvironment(name: UseInMemoryDatabase, value: builder.Configuration.GetValue(UseInMemoryDatabase, "true"))
    .WithEnvironment(name: EnsureDeletedDatabaseOnStartup, value: builder.Configuration.GetValue(EnsureDeletedDatabaseOnStartup, "false"))
    .WithEnvironment(name: ApplicationName, value: "BlockChain")
    .WithEnvironment(name: TokenIssuer, value: builder.Configuration.GetValue(TokenIssuer, IdentityIssuer))
    .WithEnvironment(name: IdentitySecret, parameter: identitySecret)
    .WithEnvironment(name: DatabaseEncryptionKey, parameter: databaseEncryptionKey)
    .WaitFor(redis)
    .WaitFor(rabbit)
    .WaitFor(blockChainDb)
    .WaitForCompletion(blockChainMigration)
    .WaitFor(identity)
    .WithReplicas(1);

var logProcessorMigration = builder.AddProject<Projects.LogProcessor_MigrationWorker>("logprocessor-migrations")
    .WithReference(logProcessorDb)
    .WithEnvironment(name: UseInMemoryDatabase, value: builder.Configuration.GetValue(UseInMemoryDatabase, "true"))
    .WithEnvironment(name: EnsureDeletedDatabaseOnStartup, value: builder.Configuration.GetValue(EnsureDeletedDatabaseOnStartup, "false"))
    .WithEnvironment(name: RpcUrls, value: builder.Configuration.GetValue<string>(RpcUrls))
    .WithEnvironment(name: DatabaseEncryptionKey, parameter: databaseEncryptionKey)
    .WaitFor(logProcessorDb);
var logProcessor = builder.AddProject<Projects.LogProcessor_Api>("logprocessor")
    .WithReference(redis)
    .WithReference(rabbit)
    .WithReference(logProcessorDb)
    .WithEnvironment(name: UseInMemoryDatabase, value: builder.Configuration.GetValue(UseInMemoryDatabase, "true"))
    .WithEnvironment(name: EnsureDeletedDatabaseOnStartup, value: builder.Configuration.GetValue(EnsureDeletedDatabaseOnStartup, "false"))
    .WithEnvironment(name: ApplicationName, value: "logProcessor")
    .WithEnvironment(name: TokenIssuer, value: builder.Configuration.GetValue(TokenIssuer, IdentityIssuer))
    .WithEnvironment(name: IdentitySecret, parameter: identitySecret)
    .WithEnvironment(name: DatabaseEncryptionKey, parameter: databaseEncryptionKey)
    .WaitFor(redis)
    .WaitFor(rabbit)
    .WaitFor(logProcessorDb)
    .WaitForCompletion(logProcessorMigration)
    .WaitFor(identity)
    .WaitFor(informing)
    .WithReplicas(1);

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
   .WithReference(blockChain)
   .WithEnvironment(name: ApplicationName, value: "ReverseProxy.Gateway")
   .WaitFor(redis)
   .WaitFor(rabbit)
   .WaitFor(identity)
   .WaitFor(informing)
   .WaitFor(blockProcessor)
   .WaitFor(logProcessor)
   .WaitFor(blockChain);

await builder.Build().RunAsync();
