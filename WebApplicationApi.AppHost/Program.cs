var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.WebApplicationApi>("webapplicationapi");

builder.Build().Run();
