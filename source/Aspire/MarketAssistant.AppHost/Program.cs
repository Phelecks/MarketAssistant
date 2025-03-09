using Microsoft.Extensions.Configuration;

var builder = DistributedApplication.CreateBuilder(args);

// Add a secret parameters
var discordBotToken = builder.AddParameter("DISCORD-BOT-TOKEN", secret: true);
var identitySecret = builder.AddParameter("IDENTITY-SECRET", secret: true);
var sqlPassword = builder.AddParameter("SQL-SA-PASSWORD", secret: true);
var rabbitUsername = builder.AddParameter("RABBIT-USERNAME", secret: true);
var rabbitPassword = builder.AddParameter("RABBIT-PASSWORD", secret: true);

var redis = builder.AddRedis("cache").WithDataVolume();

var rabbit = builder.AddRabbitMQ("messaging")
    .WithDataVolume().WithManagementPlugin();

// var postgresdb = builder.AddPostgres("postgresdb")
//                         .WithHealthCheck(key: "postgresdb_healthcheck")
//                         .WithPgAdmin()
//                         .WithDataVolume()
//                         .AddDatabase("marketassitant_db", "marketassitant_db");

var sql = builder.AddSqlServer("sql", password: sqlPassword).WithDataVolume().WithImageTag("latest");

var informingDb = sql.AddDatabase(name: "informingdb", databaseName: "informing");
var identityDb = sql.AddDatabase(name: "identitydb", databaseName: "identity");
var blockProcessorDb = sql.AddDatabase(name: "blockprocessordb", databaseName: "blockProcessor");

//var informing = builder.AddProject<Projects.Informing_Grpc>("informing")
//    // .WithEndpoint(endpointName: "http", options => 
//    // {
//    //     options.Protocol = System.Net.Sockets.ProtocolType.Tcp;
//    //     options.Transport = HttpProtocol.Http3;
//    //     //options.Port = 80;
//    //     options.TargetPort = 80;
//    // }, createIfNotExists: true)
//    // .WithEndpoint(endpointName: "grpc", options => 
//    // {
//    //     options.Protocol = System.Net.Sockets.ProtocolType.Tcp;
//    //     options.Transport = HttpProtocol.Http2;
//    //     //options.Port = 81;
//    //     options.TargetPort = 81;
//    // }, createIfNotExists: true)
//    .WithReference(redis)
//    .WithReference(rabbit)
//    .WithReference(informingDb)
//    .WithEnvironment(name: "USE_INMEMORY_DATABASE", value: builder.Configuration.GetValue("USE_INMEMORY_DATABASE", "true"))
//    .WithEnvironment(name: "ENSURE_DELETED_DATABASE_ON_STARTUP", value: builder.Configuration.GetValue("ENSURE_DELETED_DATABASE_ON_STARTUP", "false"))
//    .WithEnvironment(name: "APPLICATION_NAME", value: "Informing")
//    .WithEnvironment(name: "TOKEN_ISSUER", value: builder.Configuration.GetValue("TOKEN_ISSUER", "https://identity.contoso.com"))
//    .WithEnvironment(name: "IDENTITY_SECRET", identitySecret)
//    .WithEnvironment(name: "DISCORD_BOT_TOKEN", discordBotToken)
//    .WaitFor(redis)
//    .WaitFor(rabbit)
//    .WaitFor(informingDb);

//var identity = builder.AddProject<Projects.BlockChainIdentity_Grpc>("identity")
//    // .WithEndpoint(endpointName: "http", options => 
//    // {
//    //     options.Protocol = System.Net.Sockets.ProtocolType.Tcp;
//    //     options.Transport = HttpProtocol.Http3;
//    // }, createIfNotExists: true)
//    // .WithEndpoint(endpointName: "grpc", options => 
//    // {
//    //     options.Protocol = System.Net.Sockets.ProtocolType.Tcp;
//    //     options.Transport = HttpProtocol.Http2;
//    // }, createIfNotExists: true)
//    .WithReference(redis)
//    .WithReference(rabbit)
//    .WithReference(identityDb)
//    .WithEnvironment(name: "USE_INMEMORY_DATABASE", value: builder.Configuration.GetValue("USE_INMEMORY_DATABASE", "true"))
//    .WithEnvironment(name: "ENSURE_DELETED_DATABASE_ON_STARTUP", value: builder.Configuration.GetValue("ENSURE_DELETED_DATABASE_ON_STARTUP", "false"))
//    .WithEnvironment(name: "APPLICATION_NAME", value: "Identity")
//    .WithEnvironment(name: "TOKEN_ISSUER", value: builder.Configuration.GetValue("TOKEN_ISSUER", "https://identity.contoso.com"))
//    .WithEnvironment(name: "IDENTITY_SECRET", identitySecret)
//    .WaitFor(redis)
//    .WaitFor(rabbit)
//    .WaitFor(identityDb);

var blockProcessor = builder.AddProject<Projects.BlockProcessor_Api>("blockprocessor")
    .WithReference(redis)
    .WithReference(rabbit)
    .WithReference(blockProcessorDb)
    .WithEnvironment(name: "USE_INMEMORY_DATABASE", value: builder.Configuration.GetValue("USE_INMEMORY_DATABASE", "true"))
    .WithEnvironment(name: "ENSURE_DELETED_DATABASE_ON_STARTUP", value: builder.Configuration.GetValue("ENSURE_DELETED_DATABASE_ON_STARTUP", "false"))
    .WithEnvironment(name: "APPLICATION_NAME", value: "BlockProcessor")
    .WithEnvironment(name: "TOKEN_ISSUER", value: builder.Configuration.GetValue("TOKEN_ISSUER", "https://identity.contoso.com"))
    .WithEnvironment(name: "IDENTITY_SECRET", identitySecret)
    .WaitFor(redis)
    .WaitFor(rabbit)
    .WaitFor(blockProcessorDb);

//builder.AddProject<Projects.WalletTracker_Api>("wallettracker")
//    .WithReference(redis)
//    .WithReference(rabbit)
//    .WaitFor(redis)
//    .WaitFor(rabbit);

//var applicationGateway = builder.AddProject<Projects.Application_Gateway>("applicationgateway")
//    // .WithEndpoint(endpointName: "http", options => 
//    // {
//    //     options.Protocol = System.Net.Sockets.ProtocolType.Tcp;
//    //     options.Transport = HttpProtocol.Http3;
//    // }, createIfNotExists: true)
//    // .WithEndpoint(endpointName: "grpc", options => 
//    // {
//    //     options.Protocol = System.Net.Sockets.ProtocolType.Tcp;
//    //     options.Transport = HttpProtocol.Http2;
//    // }, createIfNotExists: true)
//    .WithReference(redis)
//    .WithReference(identity)
//    .WithReference(informing)
//    .WithEnvironment(name: "APPLICATION_NAME", value: "Application.Gateway")
//    .WaitFor(redis)
//    .WaitFor(rabbit)
//    .WaitFor(identityDb);

//var adminGateway = builder.AddProject<Projects.Admin_Gateway>("admingateway")
//    // .WithEndpoint(endpointName: "http", options => 
//    // {
//    //     options.Protocol = System.Net.Sockets.ProtocolType.Tcp;
//    //     options.Transport = HttpProtocol.Http3;
//    // }, createIfNotExists: true)
//    // .WithEndpoint(endpointName: "grpc", options => 
//    // {
//    //     options.Protocol = System.Net.Sockets.ProtocolType.Tcp;
//    //     options.Transport = HttpProtocol.Http2;
//    // }, createIfNotExists: true)
//    .WithReference(redis)
//    .WithReference(identity)
//    .WithReference(informing)
//    .WithEnvironment(name: "APPLICATION_NAME", value: "Admin.Gateway")
//    .WaitFor(redis)
//    .WaitFor(rabbit)
//    .WaitFor(identityDb);

//builder.AddProject<Projects.ReverseProxy_Gateway>("reverseproxy-gateway")
//    .WithReference(redis)
//    .WithReference(identity)
//    .WithReference(informing)
//    .WithEnvironment(name: "APPLICATION_NAME", value: "ReverseProxy.Gateway")
//    .WaitFor(redis)
//    .WaitFor(rabbit)
//    .WaitFor(identityDb);

await builder.Build().RunAsync();
