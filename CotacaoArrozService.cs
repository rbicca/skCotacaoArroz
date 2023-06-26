namespace skCotacaoArroz.WindowsService;

public class ConsultaArrozService : BackgroundService
{
    private readonly CotacaoArrozConsulta _cotacao;
    private readonly ILogger<ConsultaArrozService> _logger;

    public ConsultaArrozService(CotacaoArrozConsulta consulta, ILogger<ConsultaArrozService> logger) => (_cotacao, _logger) = (consulta, logger);

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try{
                    int qtdAnt = 1;
                    if (DateTime.Now.DayOfWeek == DayOfWeek.Monday) qtdAnt = 3;
                    if (DateTime.Now.DayOfWeek == DayOfWeek.Sunday) qtdAnt = 2;

                    DateTime diaAnterior = DateTime.Now.AddDays(-1 * qtdAnt);
                    
                    var cotacaoHora = await _cotacao.GetCotacaoArroz(diaAnterior);
                    CotacaoArrozDB.InsertCotacaoArroz(cotacaoHora);
                    _logger.LogWarning("{CotacaoArroz}",$"{cotacaoHora.dataCotacao} {cotacaoHora.valorReal}  - {DateTime.Now.ToString("dd/MM/yy-HH:mm")}");

                } catch(Exception e){
                    _logger.LogError("{CotacaoArroz-Erro}",e.Message);
                }

                await Task.Delay(TimeSpan.FromMinutes(60), stoppingToken);
            }
        }
        catch (TaskCanceledException)
        {
            // When the stopping token is canceled, for example, a call made from services.msc,
            // we shouldn't exit with a non-zero exit code. In other words, this is expected...
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "{Message}", ex.Message);

            // Terminates this process and returns an exit code to the operating system.
            // This is required to avoid the 'BackgroundServiceExceptionBehavior', which
            // performs one of two scenarios:
            // 1. When set to "Ignore": will do nothing at all, errors cause zombie services.
            // 2. When set to "StopHost": will cleanly stop the host, and log errors.
            //
            // In order for the Windows Service Management system to leverage configured
            // recovery options, we need to terminate the process with a non-zero exit code.
            Environment.Exit(1);
        }
    
    }
}
