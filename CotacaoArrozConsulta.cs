namespace skCotacaoArroz.WindowsService;

using System.Threading.Tasks;
using NPOI.HSSF.UserModel;

public readonly record struct DadosCotacaoArroz(string? dataCotacao, decimal valorReal, decimal valorDolar);

public sealed class CotacaoArrozConsulta
{
    private static readonly HttpClient _httpClient = new HttpClient();

    public async Task<DadosCotacaoArroz> GetCotacaoArroz(DateTime data)
    {
        int tabela_id = 91;     // 91 - referente ao arroz - INDICADOR DO ARROZ EM CASCA CEPEA/IRGA-RS
        int periodicidade = 1;  // 1 - período diário
        string data_inicial = data.ToString("dd/MM/yyyy");
        string data_final = data.ToString("dd/MM/yyyy");

        string urlReq = $"{Settings.URL_CEPEA}?tabela_id={tabela_id}&data_inicial={data_inicial}&data_final={data_final}&periodicidade={periodicidade}";
        HttpResponseMessage response =  await _httpClient.GetAsync(urlReq);

        if (response.IsSuccessStatusCode)
        {
            var jsonString = JsonConvert.DeserializeObject<rtArquivoCepeaDTO>(await response.Content.ReadAsStringAsync());
            string url = jsonString!.arquivo!;
            
            HttpResponseMessage response1 = await _httpClient.GetAsync(url);

            if (response1.IsSuccessStatusCode)
            {
                using (var stream = response1.Content.ReadAsStreamAsync().Result)
                {
                    var workbook = new HSSFWorkbook(stream);
                    var worksheet = workbook.GetSheetAt(0); // Assume que a planilha desejada é a primeira (índice 0)

                    var dataCotacao = worksheet.GetRow(4)?.GetCell(0)?.StringCellValue;
                    var valor = worksheet.GetRow(4)?.GetCell(1)?.StringCellValue;
                    var valorDolar = worksheet.GetRow(4)?.GetCell(2)?.StringCellValue;

                    if (dataCotacao == null) 
                    {
                        return new DadosCotacaoArroz(null, 0.0M, 0.0M);
                    }
                    return new DadosCotacaoArroz(dataCotacao!,  Convert.ToDecimal(valor), Convert.ToDecimal(valorDolar));
                }
            }
            
        }

        return new DadosCotacaoArroz(null, 0.0M, 0.0M);
    }

}   
