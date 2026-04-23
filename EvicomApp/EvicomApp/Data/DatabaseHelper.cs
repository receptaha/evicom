using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace EvicomApp.Data
{
    internal class DatabaseHelper
    {
        private static string dbPath = "evicom.db";
        private static string connectionString = $"Data Source={dbPath};Version=3;";
    }
}
