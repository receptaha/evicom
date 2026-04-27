using EvicomApp.Models;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Deployment.Internal;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace EvicomApp.Data
{
    internal class DatabaseSeeder
    {
        private static Random rng = new Random();

        public static void Seed(SQLiteConnection conn)
        {
            if (conn.State != System.Data.ConnectionState.Open) conn.Open();

            using (var transaction = conn.BeginTransaction())
            {
                try
                {
                    SeedUsers(conn, 100);
                    SeedCategories(conn);
                    SeedAllTurkey(conn);
                    SeedHouses(conn, 100);
                    SeedHousesProperties(conn, 100);
                    SeedAds(conn, "active", 50);
                    SeedAds(conn, "passive", 50);
                    SeedRentals(conn, 50);

                    transaction.Commit();
                }
                catch(Exception e)
                {
                    transaction.Rollback();
                    throw new Exception("Veriler veritabanına işlenirken (Seed) bir hata oluştu: " + e.Message);
                }
            }     
        }

        private static void SeedUsers(SQLiteConnection conn, uint count = 10)
        {
            if (conn.State != System.Data.ConnectionState.Open) conn.Open();

            using (var cmd = new SQLiteCommand("INSERT INTO Users (username, email, password, role) VALUES ('admin', 'admin@gmail.com', 'admin', 'admin')", conn)) cmd.ExecuteNonQuery();

            for (int i = 1; i <= count; i++)
            {
                string username = $"user_{i}";
                string email = $"user_{i}@gmail.com";
                string password = $"password_{i}";
                string role = "user";

                string query = $"INSERT INTO Users (username, email, password, role) VALUES ('{username}', '{email}', '{password}', '{role}')";
                using (var cmd = new SQLiteCommand(query, conn)) cmd.ExecuteNonQuery();
            }
        }
            
        private static void SeedCategories(SQLiteConnection conn)
        {
            if (conn.State != System.Data.ConnectionState.Open) conn.Open();

            foreach (var category in House.Categories)
            {
                string name = category;

                string query = $"INSERT INTO Categories (name) VALUES ('{name}')";
                using (var cmd = new SQLiteCommand(query, conn)) cmd.ExecuteNonQuery();
            }

        }

        private static void SeedAllTurkey(SQLiteConnection conn)
        {
            if (conn.State != System.Data.ConnectionState.Open) conn.Open();

            int cityIdCounter = 1;

            foreach(var cityDistricts in City.CitiesDistricts)
            {
                string[] parts = cityDistricts.Split(':');
                string city = parts[0];
                string[] districts = parts[1].Split(',');

                string cityQuery = $"INSERT INTO Cities (name) VALUES ('{city}')";
                using(var cmd = new SQLiteCommand(cityQuery, conn)) cmd.ExecuteNonQuery();
                
                foreach(string district in districts)
                {
                    string districtQuery = $"INSERT INTO Districts (city_id, name) VALUES ({cityIdCounter} ,'{district}')";
                    using (var cmd = new SQLiteCommand(districtQuery, conn)) cmd.ExecuteNonQuery();
                }

                cityIdCounter++;
            }
        }

        private static void SeedHouses(SQLiteConnection conn, uint count = 10)
        {
            if (conn.State != System.Data.ConnectionState.Open) conn.Open();

            int userCount = Convert.ToInt32(new SQLiteCommand("SELECT COUNT(*) FROM Users", conn).ExecuteScalar());
            int categoryCount = Convert.ToInt32(new SQLiteCommand("SELECT COUNT(*) FROM Categories", conn).ExecuteScalar());
            int districtCount = Convert.ToInt32(new SQLiteCommand("SELECT COUNT(*) FROM Districts", conn).ExecuteScalar());

            if (userCount == 0 || categoryCount == 0 || districtCount == 0)
            {
                throw new Exception("Evleri eklemek için önce Users, Categories ve Districts tablolarında veri olmalıdır!");
            }

            for (int i = 1; i <= count; i++)
            {
                int ownerId = rng.Next(1, userCount + 1);
                int catId = rng.Next(1, categoryCount + 1);
                int distId = rng.Next(1, districtCount + 1);

                string title = $"Örnek title #{i}";
                string address = $"Örnek Mahallesi, No:{rng.Next(1, 150)} Daire:{rng.Next(1, 20)}";

                string query = $@"INSERT INTO Houses (owner_id, category_id, district_id, title, description, full_address) 
                     VALUES ({ownerId}, {catId}, {distId}, '{title}', 'Tüm detayları düşünülmüş konforlu yaşam alanı.', '{address}')";

                using (var cmd = new SQLiteCommand(query, conn)) cmd.ExecuteNonQuery();
            }
        }

        private static void SeedHousesProperties(SQLiteConnection conn, uint hCount = 10)
        {
            if (conn.State != System.Data.ConnectionState.Open) conn.Open();

            int houseCount = Convert.ToInt32(new SQLiteCommand("SELECT COUNT(*) FROM Houses", conn).ExecuteScalar());

            if (houseCount == 0 || houseCount < hCount)
            {
                throw new Exception("Ev özellikleri eklemek için önce Houses tablosunda veri olmalıdır!");
            }

            var houseIds = new List<int>();
            string houseIdsQuery = "SELECT id FROM Houses";
            using(var cmd = new SQLiteCommand(houseIdsQuery, conn))
            {
                using(var reader  = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        houseIds.Add(reader.GetInt32(0));
                    }
                }
            }

            houseIds = houseIds.OrderBy(x => rng.Next()).ToList();

            for (int i = 0; i < hCount; i++)
            {
                int houseId = houseIds[i];
                foreach(var keyValues in HouseProperty.KeysValues)
                {
                    string key = keyValues.Key;
                    string value = keyValues.Value[rng.Next(keyValues.Value.Count)];
                    string insertQuery = $"INSERT INTO HouseProperties (house_id, property_key, property_value) VALUES ({houseId}, '{key}', '{value}')";
                    new SQLiteCommand(insertQuery, conn).ExecuteNonQuery();
                }
            }
        }

        private static void SeedAds(SQLiteConnection conn, string status, uint count = 10)
        {
            if (conn.State != System.Data.ConnectionState.Open) conn.Open();
            if (!Ad.statuses.Contains(status)) throw new Exception($"İlan oluşturmak için geçersiz durum (status): {status}");


            List<int> availableHouseIds = new List<int>();

            string availableHousesQuery = "SELECT DISTINCT id FROM Houses WHERE id NOT IN (SELECT DISTINCT house_id FROM Ads)";
            using (var cmd = new SQLiteCommand(availableHousesQuery, conn))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read()) availableHouseIds.Add(reader.GetInt32(0));
                }
            }


            if (count > availableHouseIds.Count) throw new Exception($"{count} adet {status} ilan oluşturmak için yeterli ev bulunamadı");
            availableHouseIds = availableHouseIds.OrderBy(x => rng.Next()).ToList();
            

            for (int i = 1; i <= count; i++)
            {
                int price = rng.Next(1, 50) * 1000;
                int houseId = availableHouseIds[i - 1];

                string query = $"INSERT INTO Ads (house_id, title, price, status) VALUES ({houseId}, 'Sahibinden Kiralık', {price}, '{status}')";
                using (var cmd = new SQLiteCommand(query, conn)) cmd.ExecuteNonQuery();
            } 
        }

        private static void SeedRentals(SQLiteConnection conn, uint count = 10)
        {
            if (conn.State != System.Data.ConnectionState.Open) conn.Open();

            List<int> rentableIds = new List<int>();
            List<int> tenantableIds = new List<int>();

            string rentableAdsQuery = "SELECT id FROM Ads WHERE status = 'active'";
            using (var cmd = new SQLiteCommand(rentableAdsQuery, conn))
            {
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read()) rentableIds.Add(reader.GetInt32(0));
                }
            }

            if (count > rentableIds.Count) throw new Exception($"{count} adet kiralanabilecek aktif ilan bulunamadı, lütfen ilan ekleyiniz.");

            rentableIds = rentableIds.OrderBy(x => rng.Next()).ToList();

            for (int i = 0; i < count; i++)
            {
                int adId = rentableIds[i % rentableIds.Count];
                bool insertable = true;
                int  attemps = 0;

                while(attemps < 50)
                {                   
                    DateTime startDate = DateTime.Now.AddDays(rng.Next(1, 30));
                    int rentDay = rng.Next(1, 15);
                    DateTime endDate = startDate.AddDays(rentDay);
                    long sTimestamp = new DateTimeOffset(startDate).ToUnixTimeSeconds();
                    long eTimestamp = new DateTimeOffset(endDate).ToUnixTimeSeconds();

                    string insertableQuery = $"SELECT COUNT(*) FROM Rentals WHERE ad_id = {adId} AND start_date < {eTimestamp} AND end_date > {sTimestamp}";
                    insertable = Convert.ToInt32(new SQLiteCommand(insertableQuery, conn).ExecuteScalar()) == 0;

                    tenantableIds.Clear();
                    string tenantableQuery = $"SELECT id FROM Users WHERE id NOT IN (SELECT tenant_id FROM Rentals WHERE start_date < {eTimestamp} AND end_date > {sTimestamp})";
                    using (var cmd = new SQLiteCommand(tenantableQuery, conn)) 
                    {
                        using (var reader = cmd.ExecuteReader()) 
                        {
                            while (reader.Read())
                            { 
                                tenantableIds.Add(reader.GetInt32(0));
                            }
                        }
                    }

                    if (insertable && tenantableIds.Count > 0)
                    {
                        string adQuery = $"SELECT price FROM Ads WHERE id = {adId}";
                        decimal totalPrice = Convert.ToDecimal(new SQLiteCommand(adQuery, conn).ExecuteScalar()) * rentDay;
                        int tenantId = tenantableIds[rng.Next(tenantableIds.Count)];

                        string insertQuery = $"INSERT INTO Rentals (ad_id, tenant_id, start_date, end_date, total_price) VALUES ({adId}, {tenantId}, {sTimestamp}, {eTimestamp}, {totalPrice})";
                        new SQLiteCommand(insertQuery, conn).ExecuteNonQuery();
                    }

                    attemps++;
                }
            }
        }
    }
}
