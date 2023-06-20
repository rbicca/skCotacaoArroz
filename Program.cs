//Artigo base
//https://learn.microsoft.com/en-us/dotnet/core/extensions/windows-service?pivots=dotnet-7-0

//Para compilar e publicr
//dotnet publish --output "C:\Tubo\skCotacaoArroz_Pub" 

//Porém, para criar o serviço, usei o cmd como admin (e não powershell) com o seguinte comando:
//sc create "Serviço SK - Cotação do Arroz" binpath="c:\tubo\skCotacaoArroz_Pub\skCotacaoArroz.exe"

using skCotacaoArroz.WindowsService;
using Microsoft.Extensions.Logging.Configuration;
using Microsoft.Extensions.Logging.EventLog;


HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Services.AddWindowsService(options =>
{
    options.ServiceName = "Serviço SK - Cotação do Arroz";
});

LoggerProviderOptions.RegisterProviderOptions<EventLogSettings, EventLogLoggerProvider>(builder.Services);

builder.Services.AddSingleton<JokeService>();
builder.Services.AddHostedService<EsalqueService>();

// See: https://github.com/dotnet/runtime/issues/47303
builder.Logging.AddConfiguration(
    builder.Configuration.GetSection("Logging"));

IHost host = builder.Build();
host.Run();