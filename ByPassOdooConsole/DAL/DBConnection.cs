using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace ByPassOdooConsole.DAL
{
    class DBConnection
    {
        public static DBConnection Instance;
        private string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"bypassdb.db");

        private DBConnection()
        {

        }

        static DBConnection()
        {
            Instance = Instance??new DBConnection();
        }

        public SQLiteConnection GetConnection()
        {
            return new SQLiteConnection($"Data Source = {dbPath}; Version = 3; ");
        }

        public void CloseConnection(SQLiteConnection con)
        {
            if (con != null)
            {
                con.Close();
                con.Dispose();
            }
        }
    }
}
