﻿
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OnlineNeuralNetworkTrainer
{
    public class DataBaseManager
    {
        public string database_file;
        public string database_table = "recorded_data";
        public string database_format = " (date_collection DATETIME, feature1 DOUBLE(8,2), feature2 DOUBLE(8,2), feature3 DOUBLE(8,2), feature4 DOUBLE(8,2), feature5 DOUBLE(8,2), result DOUBLE(8,2))";
        public string database_format_cmd = "(date_collection, feature1, feature2, feature3, feature4, feature5, result) values (";
        public List<List<double>> selectedData;
        public SQLiteConnection db_connection;
        public void LoadDatabase()
        {
            try
            {
                if (this.database_file != null && this.database_file.Trim() != "")
                {
                    if (File.Exists(this.database_file))
                    {
                        // load database
                        OpenDatabase();
                    }
                    else
                    {
                        // create database
                        CreateDatabase();
                    }
                }
                else
                {
                    CreateDatabase();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("load (creat) data base file failed");
                Console.WriteLine("[DEBUG] at DataBaseManager.cs, in LoadDataBase(), ex: " + e.Message);
            }
        }
        // create database
        public void CreateDatabase()
        {
            try
            {
                SQLiteConnection.CreateFile(this.database_file);
                this.OpenDatabase();
                string sql = "CREATE TABLE IF NOT EXISTS " + this.database_table + this.database_format;
                SQLiteCommand cmd = new SQLiteCommand(sql, this.db_connection);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                if (ex.InnerException != null)
                {
                    message = ex.InnerException.Message;
                }
                Console.Write("[DEBUG]: at DataBaseManager.cs, in CreateDatabase(), ex: " + message);
                MessageBox.Show("Create data base failed");
            }
        }
        // open database
        public void OpenDatabase()
        {
            try
            {
                string connection_string = "Data Source = " + this.database_file + "; Version = 3;";
                this.db_connection = new SQLiteConnection(connection_string);
                this.db_connection.Open();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                if (ex.InnerException != null)
                {
                    message = ex.InnerException.Message;
                }
                Console.Write("[DEBUG]: at DataBaseManager.cs, in OpenDatabase(), ex: " + message);
                MessageBox.Show("open data base failed");
            }
        }
        public void ExportToCSV()
        {
            // To Be Implemeted
        }
        public void ImportFromCSV(string filename, BackgroundWorker bgw)
        {
            try
            {
                string[] lines = File.ReadAllLines(filename);
                for (int i = 1; i < lines.Length; i++)
                {
                    // skip first line (headers)
                    string[] line_splited = lines[i].Split(',');
                    double[] dataFromLine = Array.ConvertAll<string, double>(line_splited, d => Convert.ToDouble(d));
                    AddToDB(dataFromLine.SubArray(1, 6));
                    int progress = i / lines.Length;
                    string progress_str = "Adding to database: " + (i + 1).ToString() + "/" + lines.Length.ToString();
                    bgw.ReportProgress(progress,progress_str);
                }
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                if (ex.InnerException != null)
                {
                    message = ex.InnerException.Message;
                }
                Console.Write("[DEBUG]: at DataBaseManager.cs, in ImportFromCSV(), ex: " + message);
                MessageBox.Show("Import csv failed, please check csv format");
            }
        }
        public void AddToDB(double[] inputsArray)
        {
            try
            {
                string valueStr = SQLString(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff", null)) + ", "
                    + inputsArray[0].ToString("0.##") + ", "
                    + inputsArray[1].ToString("0.##") + ", "
                    + inputsArray[2].ToString("0.##") + ", "
                    + inputsArray[3].ToString("0.##") + ", "
                    + inputsArray[4].ToString("0.##") + ", "
                    + inputsArray[5].ToString("0.##");
                string sql_cmd = "insert into " + this.database_table + this.database_format_cmd + valueStr + ")";
                SQLiteCommand cmd = new SQLiteCommand(sql_cmd, this.db_connection);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                if (ex.InnerException != null)
                {
                    message = ex.InnerException.Message;
                }
                Console.Write("[DEBUG]: at DataBaseManager.cs, in AddToDatabase(), ex: " + message);
                MessageBox.Show("add to data base failed");
            }
        }
        public string SQLString(string str)
        {
            return "'" + str.Replace("\0", string.Empty) + "'";
        }
        public void SelectAllData()
        {
            string sql = "select * from " + this.database_table;
            SQLiteCommand command = new SQLiteCommand(sql, this.db_connection);
            SQLiteDataReader reader = command.ExecuteReader();
            List<double> lst_Feature1 = new List<double>();
            List<double> lst_Feature2 = new List<double>();
            List<double> lst_Feature3 = new List<double>();
            List<double> lst_Feature4 = new List<double>();
            List<double> lst_Feature5 = new List<double>();
            List<double> lst_result = new List<double>();
            while (reader.Read())
            {
                lst_Feature1.Add(Convert.ToDouble(reader[1].ToString()));
                lst_Feature2.Add(Convert.ToDouble(reader[2].ToString()));
                lst_Feature3.Add(Convert.ToDouble(reader[3].ToString()));
                lst_Feature4.Add(Convert.ToDouble(reader[4].ToString()));
                lst_Feature5.Add(Convert.ToDouble(reader[5].ToString()));
                lst_result.Add(Convert.ToDouble(reader[6].ToString()));
            }
            selectedData = new List<List<double>>();
            selectedData.Add(lst_Feature1);
            selectedData.Add(lst_Feature2);
            selectedData.Add(lst_Feature3);
            selectedData.Add(lst_Feature4);
            selectedData.Add(lst_Feature5);
            selectedData.Add(lst_result);
        }
    }
}