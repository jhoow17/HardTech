using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace conexao
{
    public class Conexao
    {
        public static SqlConnection getConnection()
        {
            SqlConnection cnn = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Bd_Conexao;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            return cnn;
        }
    }
}
