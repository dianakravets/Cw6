
using System.Data.SqlClient;

namespace Cw6.Services
{
    public class Dbservice : IDbservice
    {
        private const string ConString =
            "Data Source=db-mssql16.pjwstk.edu.pl;Initial Catalog=s19054;User ID=apbds19054;Password=admin";

        public bool ckeckIndex(string index)
        {

            using (var con = new SqlConnection(ConString))
            using (var com = new SqlCommand())
            {
                com.Connection = con;
                con.Open();

                com.CommandText = "select IndexNumber from Student where IndexNumber=@id ";
                com.Parameters.AddWithValue("id", index);
                var dr = com.ExecuteReader();
                if (!dr.Read())
                {
                    return false;
                }

            }

            return true;
        }
    }
}