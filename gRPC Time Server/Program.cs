using gRPC_Time_Server.Middleware;
using gRPC_Time_Server.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using System.Security.Cryptography.X509Certificates;

#region Builder
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
#endregion

#region Custom Certificate
/*
 * Dev comes with integrated certificates.
 * If we want a custom certificate, it will create and use that.
 */
builder.WebHost.ConfigureKestrel(opt =>
{
    string file = "server_certificate.pfx";
    string password = "Password!";

    // Get the path to the desktop
    string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

    // Combine the desktop path with the file name
    string filePath = Path.Combine(desktopPath, file);

    if (File.Exists(filePath))
    {
        // Load the certificate
        var cert = new X509Certificate2(filePath, password);

        opt.ConfigureHttpsDefaults(h =>
        {
            h.ClientCertificateMode = Microsoft.AspNetCore.Server.Kestrel.Https.ClientCertificateMode.AllowCertificate;

            h.CheckCertificateRevocation = false;
            h.ServerCertificate = cert;
        });
    }
});
#endregion

var app = builder.Build();

#region Middleware
// Adding middleware to ensure https method for retrieval of DB times.
app.UseMiddleware<RequireHttpsMiddleware>();
#endregion


#region HTTP Pipelines
// Configure the HTTP request pipeline.
app.MapGrpcService<GreeterService>();
app.MapGrpcService<TimeService>();
#endregion

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
