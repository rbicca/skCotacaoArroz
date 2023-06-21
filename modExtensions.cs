namespace skCotacaoArroz.WindowsService;
using System;

public static class Extensions
{
    [System.Diagnostics.DebuggerStepThrough()]
    public static string ToSQL(this decimal d)
    {
        string Valor = d.ToString();
        Valor = Valor.Replace(".", "");
        Valor = Valor.Replace(",", ".");

        if (Valor.Trim() == "")
            return "NULL";
        else
            return Valor;
    }

    [System.Diagnostics.DebuggerStepThrough()]
    public static string ToSQL(this DateTime d)
    {
        return d.ToString("MM/dd/yyyy");
    }

    [System.Diagnostics.DebuggerStepThrough()]
    public static string ToSQL(this string s)
    {
        if (s == null)
            return "";
        else
            return s.Replace("'", "''").Trim();
    }

    [System.Diagnostics.DebuggerStepThrough()]
    public static string ToSQL(this bool b)
    {
        if (b) return "1"; else return "0";
    }

    [System.Diagnostics.DebuggerStepThrough()]
    public static bool IsAlpha(this string strToCheck)
    {
        System.Text.RegularExpressions.Regex objAlphaPattern = new System.Text.RegularExpressions.Regex("[^a-zA-Z]");
        return !objAlphaPattern.IsMatch(strToCheck);
    }

}