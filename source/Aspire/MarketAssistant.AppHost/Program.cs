var builder = DistributedApplication.CreateBuilder(args);

var redis = builder.AddRedis("cache").WithDataVolume();

var rabbit = builder.AddRabbitMQ("messaging").WithDataVolume();

var postgresdb = builder.AddPostgres("postgresdb")
                        .WithPgAdmin()
                        .WithDataVolume()
                        .AddDatabase("marketassitant_db", "marketassitant_db");

builder.Build().Run();
