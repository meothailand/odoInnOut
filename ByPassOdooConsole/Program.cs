using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ByPassOdooConsole.DAL;

namespace ByPassOdooConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var accounts = DAL.DAL.GetAvailableAccounts();
            Console.WriteLine("Available accounts");
            foreach(var acc in accounts)
            {
                Console.WriteLine($"Id: {acc.Id} Username: {acc.Username} Password: {acc.Password}");
            }
            Console.Read();
        }
    }

    

    class ByPassOdooService
    {
        CookieContainer cockieContainer = new CookieContainer();

        public void Login(string username, string password)
        {
            var token = getLoginToken();
            var url = "https://odoo.blockchainlabs.asia/web/login";
            string loginParram =
                        String.Format("csrf_token={0}&login={1}&password={2}&redirect=", token, System.Web.HttpUtility.UrlEncode(username), password);
            string referel = $"https://odoo.blockchainlabs.asia/web/login";
            byte[] loginParramByte = Encoding.UTF8.GetBytes(loginParram);
            var response = RestClient.Send(url, "POST", cockieContainer, true, loginParramByte, referel);
        }

        private string getLoginToken()
        {
            string url = "https://odoo.blockchainlabs.asia/web/login";
            var response = RestClient.Send(url, "GET", cockieContainer, true, null);
            var regex = "(?<=csrf_token: \").{41}";
            Regex rx = new Regex(regex, RegexOptions.Compiled | RegexOptions.IgnoreCase);
            var token = rx.Match(response.Result);
            return token.Value;
        }

        public void CheckIn()
        {
            var url = "https://odoo.blockchainlabs.asia/web/dataset/call_kw/hr.employee/attendance_manual";
            string loginParram1 = "{\"jsonrpc\":\"2.0\",\"method\":\"call\",\"params\":{\"args\":[[657],\"hr_attendance.hr_attendance_action_my_attendances\"],\"model\":\"hr.employee\",\"method\":\"attendance_manual\",\"kwargs\":{}},\"id\":" + getRpcId() + "}";

            byte[] loginParramByte = Encoding.UTF8.GetBytes(loginParram1);
            var response = RestClient.SendRpc(url, "POST", cockieContainer, true, loginParramByte);
        }

        private string getRpcId()
        {
            return (DateTime.Now.ToFileTime() % 999999999).ToString();
        }
    }
}
