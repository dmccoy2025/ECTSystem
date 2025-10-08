var builder = DistributedApplication.CreateBuilder(args);

var client = builder.AddProject<Projects.AF_ECT_Client>("client");
var server = builder.AddProject<Projects.AF_ECT_Server>("server");

builder.Build().Run();
