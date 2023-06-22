namespace skCotacaoArroz.WindowsService;

using System.Data;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;

public class CotacaoArrozDB{

    public static void InsertCotacaoArroz(DadosCotacaoArroz dados){

        SqlConnection cn = new SqlConnection(Settings.CS);
        cn.Open();
        
        string sql = $"select COUNT(*) from CotacaoCPEA where data = '{dados!.dataCotacao}'";
        SqlCommand cmd = new SqlCommand(sql,cn);
        int qtd = int.Parse(cmd.ExecuteScalar().ToString()!);

        if(qtd <=0 ){
            sql = "INSERT CotacaoCPEA (data,valor,valorDolar,logData,logHora) VALUES (";
            sql += "'" + dados!.dataCotacao + "', ";
            sql += dados.valorReal.ToSQL() + ",";
            sql += dados.valorDolar.ToSQL() + ",";
            sql += "'" + DateTime.Now.ToString("MM/dd/yyyy") + "', ";
            sql += "'" + DateTime.Now.ToString("HH:mm") + "')";

            cmd.CommandText = sql;
        } else {
            sql = "UPDATE CotacaoCPEA set ";
            sql += $"valor = {dados.valorReal.ToSQL()}, ";
            sql += $"valorDolar = {dados.valorDolar.ToSQL()}, ";
            sql += $"logData = '{DateTime.Now.ToString("MM/dd/yyyy")}', ";
            sql += $"logHora = '{DateTime.Now.ToString("HH:mm")}' ";
            sql += $"where data = '{dados!.dataCotacao}'";

            cmd.CommandText = sql;
        }    

        cmd.ExecuteNonQuery();

        cn.Close();

    }

}