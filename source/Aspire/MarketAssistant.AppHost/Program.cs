using Microsoft.Extensions.Configuration;

var builder = DistributedApplication.CreateBuilder(args);

var redis = builder.AddRedis("cache").WithDataVolume();

var rabbit = builder.AddRabbitMQ("messaging").WithDataVolume();

// var postgresdb = builder.AddPostgres("postgresdb")
//                         .WithHealthCheck(key: "postgresdb_healthcheck")
//                         .WithPgAdmin()
//                         .WithDataVolume()
//                         .AddDatabase("marketassitant_db", "marketassitant_db");

var sql = builder.AddSqlServer("sql").WithDataVolume().WithImageTag("latest");
var informingDb = sql.AddDatabase(name: "informingdb", databaseName: "informing");
var identityDb = sql.AddDatabase(name: "identitydb", databaseName: "identity");

var informing = builder.AddProject<Projects.Informing_Grpc>("informing")
    // .WithEndpoint(endpointName: "http", options => 
    // {
    //     options.Protocol = System.Net.Sockets.ProtocolType.Tcp;
    //     options.Transport = HttpProtocol.Http3;
    //     //options.Port = 80;
    //     options.TargetPort = 80;
    // }, createIfNotExists: true)
    // .WithEndpoint(endpointName: "grpc", options => 
    // {
    //     options.Protocol = System.Net.Sockets.ProtocolType.Tcp;
    //     options.Transport = HttpProtocol.Http2;
    //     //options.Port = 81;
    //     options.TargetPort = 81;
    // }, createIfNotExists: true)
    .WithReference(redis)
    .WithReference(rabbit)
    .WithReference(informingDb)
    .WithEnvironment(name: "BLOCKCHAIN_IDENTITY_AUTHORITY", value: builder.Configuration.GetValue("BLOCKCHAIN_IDENTITY_AUTHORITY", ""))
    .WithEnvironment(name: "USE_INMEMORY_DATABASE", value: builder.Configuration.GetValue("USE_INMEMORY_DATABASE", "true"))
    .WithEnvironment(name: "APPLICATION_NAME", value: "Informing");
    //.WaitFor(redis)
    //.WaitFor(rabbit)
    //.WaitFor(informingDb);

var identity = builder.AddProject<Projects.BlockChainIdentity_Grpc>("identity")
    // .WithEndpoint(endpointName: "http", options => 
    // {
    //     options.Protocol = System.Net.Sockets.ProtocolType.Tcp;
    //     options.Transport = HttpProtocol.Http3;
    // }, createIfNotExists: true)
    // .WithEndpoint(endpointName: "grpc", options => 
    // {
    //     options.Protocol = System.Net.Sockets.ProtocolType.Tcp;
    //     options.Transport = HttpProtocol.Http2;
    // }, createIfNotExists: true)
    .WithReference(redis)
    .WithReference(rabbit)
    .WithReference(identityDb)
    .WithEnvironment(name: "BLOCKCHAIN_IDENTITY_AUTHORITY", value: builder.Configuration.GetValue("BLOCKCHAIN_IDENTITY_AUTHORITY", ""))
    .WithEnvironment(name: "TOKEN_ISSUER", value: builder.Configuration.GetValue("TOKEN_ISSUER", ""))
    .WithEnvironment(name: "USE_INMEMORY_DATABASE", value: builder.Configuration.GetValue("USE_INMEMORY_DATABASE", "true"))
    .WithEnvironment(name: "APPLICATION_NAME", value: "Identity");
    //.WaitFor(redis)
    //.WaitFor(rabbit)
    //.WaitFor(identityDb);

builder.Build().Run();
