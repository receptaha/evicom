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
        public static void initializeDatabase()
        {
            string resultMessage = "";

            try
            {
                if (File.Exists(dbPath))
                {
                    File.Delete(dbPath);
                }

                SQLiteConnection.CreateFile(dbPath);
                createTables();
                createIndexes();
                seedDatabase();

                Dictionary<string, int> dbState = new Dictionary<string, int>();
                using(var conn = GetConnection())
                {
                    conn.Open();
                    string query = @"SELECT name, seq FROM sqlite_sequence";
                    using (var cmd = new SQLiteCommand(query, conn))
                    {
                        using(var read = cmd.ExecuteReader())
                        {
                            while (read.Read())
                            {
                                dbState.Add(read.GetString(0), read.GetInt32(1));
                            }
                        }
                    }
                }

                if(dbState.Count > 0)
                {
                    resultMessage += "Veritabanı başarıyla hazırlandı ve veriler yüklendi.";
                    foreach (var state in dbState)
                    {
                        resultMessage += "\n";
                        resultMessage += $"{state.Key} : {state.Value}";
                    }
                    MessageBox.Show(resultMessage, "Başarılı");
                }else
                {
                    MessageBox.Show("Veritabanına hiçbir veri yüklenmedi");
                }
                
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message}", "Veritabanı Hatası");
            }
        }
        private static SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(connectionString);
        }
        private static void createTables()
        {
            try 
            {
                createUserTable();
                createCityTable();
                createCategoryTable();

                createDistrictTable();
                createHouseTable();
                createAdTable();
                createHousePropertyTable();

                createRentalTable();
                createCommentTable();
                createLikeTable();
                createImageTable();
            }
            catch(Exception e)
            {
                throw new Exception("Tablolar oluşturulurken bir hata meydana geldi: " + e.Message);
            }
        }

        private static void createIndexes()
        {
            using (var conn = GetConnection())
            {
                conn.Open();

                string idxDistrictCity = "CREATE INDEX IF NOT EXISTS idx_district_city ON Districts(city_id);";

                string idxHouseDistrict = "CREATE INDEX IF NOT EXISTS idx_house_district ON Houses(district_id);";
                string idxHouseCategory = "CREATE INDEX IF NOT EXISTS idx_house_category ON Houses(category_id);";

                string idxAdHouse = "CREATE INDEX IF NOT EXISTS idx_ad_house ON Ads(house_id);";
                string idxAdPrice = "CREATE INDEX IF NOT EXISTS idx_ad_price ON Ads(price);";

                string idxCommentSource = "CREATE INDEX IF NOT EXISTS idx_comment_source ON Comments(source_type, source_id);";
                string idxLikeSource = "CREATE INDEX IF NOT EXISTS idx_like_source ON Likes(source_type, source_id);";
                string idxImageSource = "CREATE INDEX IF NOT EXISTS idx_image_source ON Images(source_type, source_id);";

                string idxRentalDates = "CREATE INDEX IF NOT EXISTS idx_rental_dates ON Rentals(start_date, end_date);";

                string idxPropertyKey = "CREATE INDEX IF NOT EXISTS idx_prop_key ON HouseProperties(property_key);";

                string allIndexes = idxDistrictCity + idxHouseDistrict + idxHouseCategory +
                                    idxAdHouse + idxAdPrice + idxCommentSource +
                                    idxLikeSource + idxImageSource + idxRentalDates + idxPropertyKey;

                using (var cmd = new SQLiteCommand(allIndexes, conn)) cmd.ExecuteNonQuery();
            }
        }

        private static void createUserTable()
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string query = @"CREATE TABLE IF NOT EXISTS Users (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    username TEXT NOT NULL UNIQUE,
                    email TEXT NOT NULL UNIQUE,
                    password TEXT NOT NULL,
                    role TEXT NOT NULL DEFAULT 'user',
                    created_at INTEGER DEFAULT (strftime('%s', 'now'))
                );";
                using (var cmd = new SQLiteCommand(query, conn)) cmd.ExecuteNonQuery();
            }
        }

        private static void createCityTable()
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string query = "CREATE TABLE IF NOT EXISTS Cities (id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT NOT NULL UNIQUE);";
                using (var cmd = new SQLiteCommand(query, conn)) cmd.ExecuteNonQuery();
            }
        }

        private static void createCategoryTable()
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string query = "CREATE TABLE IF NOT EXISTS Categories (id INTEGER PRIMARY KEY AUTOINCREMENT, name TEXT NOT NULL UNIQUE);";
                using (var cmd = new SQLiteCommand(query, conn)) cmd.ExecuteNonQuery();
            }
        }

        private static void createDistrictTable()
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string query = @"CREATE TABLE IF NOT EXISTS Districts (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    city_id INTEGER NOT NULL,
                    name TEXT NOT NULL,
                    FOREIGN KEY (city_id) REFERENCES Cities(id) ON DELETE CASCADE
                );";
                using (var cmd = new SQLiteCommand(query, conn)) cmd.ExecuteNonQuery();
            }
        }

        private static void createHouseTable()
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string query = @"CREATE TABLE IF NOT EXISTS Houses (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    owner_id INTEGER NOT NULL,
                    category_id INTEGER NOT NULL,
                    district_id INTEGER NOT NULL,
                    title TEXT NOT NULL,
                    description TEXT,
                    full_address TEXT NOT NULL,
                    created_at INTEGER DEFAULT (strftime('%s', 'now')),
                    FOREIGN KEY (owner_id) REFERENCES Users(id) ON DELETE CASCADE,
                    FOREIGN KEY (category_id) REFERENCES Categories(id),
                    FOREIGN KEY (district_id) REFERENCES Districts(id)
                );";
                using (var cmd = new SQLiteCommand(query, conn)) cmd.ExecuteNonQuery();
            }
        }

        private static void createAdTable()
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string query = @"CREATE TABLE IF NOT EXISTS Ads (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    house_id INTEGER UNIQUE NOT NULL,
                    title TEXT NOT NULL,
                    price DECIMAL(10,2) NOT NULL,
                    status TEXT DEFAULT 'active',
                    created_at INTEGER DEFAULT (strftime('%s', 'now')),
                    FOREIGN KEY (house_id) REFERENCES Houses(id) ON DELETE CASCADE
                );";
                using (var cmd = new SQLiteCommand(query, conn)) cmd.ExecuteNonQuery();
            }
        }

        private static void createHousePropertyTable()
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string query = @"CREATE TABLE IF NOT EXISTS HouseProperties (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    house_id INTEGER NOT NULL,
                    property_key TEXT NOT NULL,
                    property_value TEXT NOT NULL,
                    FOREIGN KEY (house_id) REFERENCES Houses(id) ON DELETE CASCADE
                );";
                using (var cmd = new SQLiteCommand(query, conn)) cmd.ExecuteNonQuery();
            }
        }

        private static void createRentalTable()
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string query = @"CREATE TABLE IF NOT EXISTS Rentals (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    ad_id INTEGER NOT NULL,
                    tenant_id INTEGER NOT NULL,
                    start_date INTEGER,
                    end_date INTEGER,
                    total_price DECIMAL(10,2) NOT NULL,
                    is_active BOOLEAN DEFAULT 1,
                    created_at INTEGER DEFAULT (strftime('%s', 'now')),
                    FOREIGN KEY (ad_id) REFERENCES Ads(id),
                    FOREIGN KEY (tenant_id) REFERENCES Users(id)
                );";
                using (var cmd = new SQLiteCommand(query, conn)) cmd.ExecuteNonQuery();
            }
        }

        private static void createCommentTable()
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string query = @"CREATE TABLE IF NOT EXISTS Comments (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    user_id INTEGER NOT NULL,
                    source_id INTEGER NOT NULL,
                    source_type TEXT NOT NULL,
                    content TEXT NOT NULL,
                    created_at INTEGER DEFAULT (strftime('%s', 'now')),
                    FOREIGN KEY (user_id) REFERENCES Users(id) ON DELETE CASCADE
                );";
                using (var cmd = new SQLiteCommand(query, conn)) cmd.ExecuteNonQuery();
            }
        }

        private static void createLikeTable()
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string query = @"CREATE TABLE IF NOT EXISTS Likes (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    user_id INTEGER NOT NULL,
                    source_id INTEGER NOT NULL,
                    source_type TEXT NOT NULL,
                    FOREIGN KEY (user_id) REFERENCES Users(id) ON DELETE CASCADE
                );";
                using (var cmd = new SQLiteCommand(query, conn)) cmd.ExecuteNonQuery();
            }
        }

        private static void createImageTable()
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string query = @"CREATE TABLE IF NOT EXISTS Images (
                    id INTEGER PRIMARY KEY AUTOINCREMENT,
                    source_id INTEGER NOT NULL,
                    source_type TEXT NOT NULL,
                    image_path TEXT NOT NULL,
                    created_at INTEGER DEFAULT (strftime('%s', 'now'))
                );";
                using (var cmd = new SQLiteCommand(query, conn)) cmd.ExecuteNonQuery();
            }
        }

        public static void seedDatabase()
        {
            using(var conn = GetConnection())
            {
                conn.Open();
                DatabaseSeeder.Seed(conn);
            }
        }
    }
}
