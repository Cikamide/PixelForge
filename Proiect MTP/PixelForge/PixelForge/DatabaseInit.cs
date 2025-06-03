using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Data.SQLite;
using System.IO;
using System.Data;

namespace PixelForge
{
    public class DatabaseInit
    {
        public static void Initializare()
        {
            if (!File.Exists("DataBase.db"))
            {
                SQLiteConnection.CreateFile("DataBase.db");
            }
            using var connection = new SQLiteConnection("Data Source=DataBase.db");
            connection.Open();

            string createTableProcesor = @"
            CREATE TABLE IF NOT EXISTS Procesor (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Nume TEXT UNIQUE NOT NULL,
            Socket TEXT NOT NULL,
            FrecventaGhz REAL NOT NULL,
            CoolerInclus BOOLEAN NOT NULL,
            Pret REAL NOT NULL);";

            string createTableCooler = @"
            CREATE TABLE IF NOT EXISTS Cooler (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Nume TEXT UNIQUE NOT NULL,
            AIO BOOLEAN NOT NULL,
            RPM INTEGER NOT NULL,
            Pret REAL NOT NULL);";

            string createTableMB = @"
            CREATE TABLE IF NOT EXISTS PlacaDeBaza (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Nume TEXT UNIQUE NOT NULL,
            Socket TEXT NOT NULL,
            Memorie INTEGER NOT NULL,
            SloturiRam INTEGER NOT NULL,
            SloturiM2 INTEGER NOT NULL,
            Pret REAL NOT NULL);";

            string createTableRami = @"
            CREATE TABLE IF NOT EXISTS Rami (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Nume TEXT UNIQUE NOT NULL,
            Viteza INTEGER NOT NULL,
            Capacitate INTEGER NOT NULL,
            NrModule INTEGER NOT NULL,
            Pret REAL NOT NULL);";

            string createTableStocare = @"
            CREATE TABLE IF NOT EXISTS Stocare (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Nume TEXT UNIQUE NOT NULL,
            Capacitate INTEGER NOT NULL,
            Tip TEXT NOT NULL,
            Pret REAL NOT NULL);";

            string createTableGPU = @"
            CREATE TABLE IF NOT EXISTS GPU (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Nume TEXT UNIQUE NOT NULL,
            MemorieGB INTEGER NOT NULL,
            Pret REAL NOT NULL);";

            string createTableCase = @"
            CREATE TABLE IF NOT EXISTS Carcasa (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Nume TEXT UNIQUE NOT NULL,
            Volum REAL NOT NULL,
            Pret REAL NOT NULL);";

            string createTablePSU = @"
            CREATE TABLE IF NOT EXISTS Sursa (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Nume TEXT UNIQUE NOT NULL,
            Eficienta TEXT NOT NULL,
            PutereW INTEGER NOT NULL,
            Pret REAL NOT NULL);";

             string createTableUtilizator = @"
             CREATE TABLE IF NOT EXISTS Utilizator (
             Id INTEGER PRIMARY KEY AUTOINCREMENT,
             Username TEXT NOT NULL UNIQUE,
             Parola TEXT NOT NULL,
             Tip TEXT NOT NULL CHECK(Tip IN ('STAFF', 'CLIENT')) );";

             string inserareStaff = @"
             INSERT INTO Utilizator (Username, Parola, Tip)
             SELECT 'admin', 'parola123', 'STAFF'
             WHERE NOT EXISTS ( SELECT 1 FROM Utilizator WHERE Username = 'admin' );";

             string createTableComenzi = @"
             CREATE TABLE IF NOT EXISTS Comenzi (
             Id INTEGER PRIMARY KEY AUTOINCREMENT,
             Procesor TEXT NOT NULL,
             Cooler TEXT NOT NULL,
             MB TEXT NOT NULL,
             Rami TEXT NOT NULL,
             Stocare TEXT NOT NULL,
             GPU TEXT NOT NULL,
             Carcasa TEXT NOT NULL,
             PSU TEXT NOT NULL,
             Validat BOOLEAN NOT NULL,
             PretTotal REAL NOT NULL,
             Client TEXT NOT NULL);";
         

            using var cmdProcesor = new SQLiteCommand(createTableProcesor, connection);
            using var cmdCooler = new SQLiteCommand(createTableCooler, connection);
            using var cmdMB = new SQLiteCommand(createTableMB, connection);
            using var cmdRami = new SQLiteCommand(createTableRami, connection);
            using var cmdStocare = new SQLiteCommand(createTableStocare, connection);
            using var cmdGPU = new SQLiteCommand(createTableGPU, connection);
            using var cmdCase = new SQLiteCommand(createTableCase, connection);
            using var cmdPSU = new SQLiteCommand(createTablePSU, connection);
            using var cmdUtilizator = new SQLiteCommand(createTableUtilizator, connection);
            using var cmdInserareStaff = new SQLiteCommand(inserareStaff, connection);
            using var cmdInserareComenzi = new SQLiteCommand(createTableComenzi, connection);

            cmdProcesor.ExecuteNonQuery();
            cmdCooler.ExecuteNonQuery();
            cmdMB.ExecuteNonQuery();
            cmdRami.ExecuteNonQuery();
            cmdStocare.ExecuteNonQuery();
            cmdGPU.ExecuteNonQuery();
            cmdCase.ExecuteNonQuery();
            cmdPSU.ExecuteNonQuery();
            cmdUtilizator.ExecuteNonQuery();
            cmdInserareStaff.ExecuteNonQuery();
            cmdInserareComenzi.ExecuteNonQuery();
        }
    }
}
