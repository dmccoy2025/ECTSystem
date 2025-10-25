namespace AF.ECT.AppHost;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = DistributedApplication.CreateBuilder(args);

        var client = builder.AddProject<Projects.AF_ECT_WebClient>("client");
        var server = builder.AddProject<Projects.AF_ECT_Server>("server");
        var wiki = builder.AddProject<Projects.AF_ECT_Wiki>("wiki");

        builder.Build().Run();
    }
}