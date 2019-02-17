using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace PB2B
{
    public class baglanti
    {
        public static string GFirma = "006";
        public static string GDonem = "04";

        public static SqlConnection DapperConnection()
        {
            return new SqlConnection(ConfigurationManager.ConnectionStrings["LKSDBDAPPER"].ConnectionString);
        }

        public static string getStokSTR(double sayi) {
            if (sayi>100)
            {
                return "100+";
            }
            else if (sayi < 0)
            {
                return "0";
            }
            return sayi.ToString();
        }
    }
}