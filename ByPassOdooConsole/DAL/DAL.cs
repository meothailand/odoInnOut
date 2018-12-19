using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ByPassOdooConsole.DAL
{
    public class DAL
    {
        public static List<Account> GetAvailableAccounts()
        {
            using(var con = DBConnection.Instance.GetConnection())
            {
                con.Open();
                SQLiteCommand cmdAccount = new SQLiteCommand("SELECT * FROM Account", con);
                SQLiteCommand cmdLeave = new SQLiteCommand("SELECT AccountId FROM Leave WHERE date(LeaveDate, 'localtime') = DATE('now', 'localtime')", con);
                cmdAccount.Parameters.Add(new SQLiteParameter("@today", DateTime.Now));
                var readerAccount = cmdAccount.ExecuteReader();
                var readerLeave = cmdLeave.ExecuteReader();
                List<Account> accounts = new List<Account>();
                List<int> leaves = new List<int>();
                while (readerLeave.Read())
                {
                    leaves.Add(readerLeave.GetInt32(0));
                }
                while (readerAccount.Read())
                {
                    accounts.Add(new Account()
                    {
                        Id = readerAccount.GetInt32(0),
                        Username = readerAccount.GetString(1),
                        Password = readerAccount.GetString(2)
                    });                   
                }
                con.Close();
                return accounts.SkipWhile(a => leaves.Contains(a.Id)).ToList();
            }
        }

        public static void SaveCheckInTime(int accountId, DateTime time, string error)
        {
            using(var con = DBConnection.Instance.GetConnection())
            {
                SQLiteCommand insertCmd = new SQLiteCommand("INSERT INTO CheckInOutLog (AccountId, CheckInTime, ErrorLog) VALUES (@accountId, datetime(@checkInTime, 'localtime'), @error)", con);
                insertCmd.Parameters.Add(new SQLiteParameter("@accountId", accountId));
                insertCmd.Parameters.Add(new SQLiteParameter("@checkInTime", time));
                insertCmd.Parameters.Add(new SQLiteParameter("@error", error));
                con.Open();
                insertCmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public static void SaveCheckOutTime(int accountId, DateTime time, string error)
        {
            using (var con = DBConnection.Instance.GetConnection())
            {
                SQLiteCommand insertCmd = new SQLiteCommand("INSERT INTO CheckInOutLog (AccountId, CheckOutTime, ErrorLog) VALUES (@accountId, datetime(@checkOutTime, 'localtime'), @error)", con);
                insertCmd.Parameters.Add(new SQLiteParameter("@accountId", accountId));
                insertCmd.Parameters.Add(new SQLiteParameter("@checkOutTime", time));
                insertCmd.Parameters.Add(new SQLiteParameter("@error", error));
                con.Open();
                insertCmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public static List<CheckInOutLog> GetUnCheckOutAccounts()
        {
            using(var con = DBConnection.Instance.GetConnection())
            {
                var availableAccounts = GetAvailableAccounts().Select(i => i.Id).ToArray();
                SQLiteCommand checkCmd = new SQLiteCommand($"SELECT Id, CheckInTime, AccountId FROM CheckInOutLog WHERE date(CheckInTime, 'localtime') = date('now') AND CheckOutTime IS NULL", con);
                con.Open();
                var reader = checkCmd.ExecuteReader();
                List<CheckInOutLog> logs = new List<CheckInOutLog>();
                while(reader.Read())
                {
                    logs.Add(new CheckInOutLog()
                    {
                        Id = reader.GetInt32(0),
                        CheckInTime = reader.GetDateTime(1),
                        CheckOutTime = null,
                        AccountId = reader.GetInt32(2)
                    });
                }
                return logs;
            }
        }
    }

    public class Account
    {
        public int Id;
        public string Username;
        public string Password;
    }

    public class CheckInOutLog
    {
        public int Id;
        public DateTime? CheckInTime;
        public DateTime? CheckOutTime;
        public string ErrorLog;
        public int AccountId { get; set; }
    }

    public class Leave
    {
        public int Id { get; set; }
        public DateTime LeaveDate { get; set; }
        public int AccountId { get; set; }
    }
}
