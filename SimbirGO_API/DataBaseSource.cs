using SimbirGO_API.Models;
using Npgsql;
using System.Data;
using System.Diagnostics;

namespace SimbirGO_API
{
    public class DataBaseSource
    {
        //псевдоБазаДанных

        public static List<Client> clients = new List<Client>
        {
          
        };

        public static List<Transport> transports = new List<Transport>
        {

        };

        public static List<Rental> rentals = new List<Rental>
        {

        };


        public static NpgsqlConnection connecting = new NpgsqlConnection(@"Host=localhost;Port=5432;Database=SimbirGO;Username=postgres;Password=1122334455");

        internal static DataTable WorkTable(string query)
        {
            DataTable dataTable = new DataTable();
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(query, connecting);
            adapter.Fill(dataTable);
            return dataTable;
        }
        internal static DataSet Upload(string query)
        {
            DataSet dataset = new DataSet();
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(query, connecting);
            adapter.Fill(dataset);
            return dataset;
        }

        internal static void Excet(string query)
        {
            DataSet dataset = new DataSet();
            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter(query, connecting);
            adapter.Fill(dataset);
        }

    }
}
