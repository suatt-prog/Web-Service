using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace webServiceC
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]

    public class WebService1 : System.Web.Services.WebService
    {
        public string connString = "Data Source=DESKTOP-1TM9SIN"+@"\SUATAD;Initial Catalog=webService;User ID=sa;password=ad2022+-;Persist Security Info=true";
        private static SqlConnection connection = new SqlConnection();
        private static SqlCommand command = new SqlCommand();
        private static SqlDataReader DbReader;
        private static SqlDataAdapter adapter = new SqlDataAdapter();
        public SqlTransaction DbTran;
        DataTable table = new DataTable();
        [WebMethod]
        public string HelloWorld()
        {
            return "Merhaba Dünya";
        }

        public void openConn(string message)
        {
            if (connection.State == ConnectionState.Closed)
            {
                connection.ConnectionString = message;
                connection.Open();
            }
        }
        [WebMethod]
        public string countries()
        {
            table.Columns.Add("name");
            table.Columns.Add("contenent");
            table.Rows.Add("Türkiye", "Asia");
            table.Rows.Add("Azerbaycan", "Asia");
            table.Rows.Add("Özbekistan", "Asia");
            return JsonConvert.SerializeObject(table);
        }
        public void read(string query , DataTable d)
        {
            try
            {
                openConn(connString);
                command.Connection = connection;
                command.CommandText = query;
                command.CommandType = CommandType.Text;
                adapter = new SqlDataAdapter();
                adapter.Fill(d);
            }
            catch(Exception A) { }
        }
        public SqlDataReader readWithReader(string query)
        {
            try
            {
                openConn(connString);
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = query;
            }
            catch(Exception s) { }
            return command.ExecuteReader();
        }
        public int executeQuery(SqlCommand co)
        {
            try
            {
                co.Connection = connection;
                co.CommandType = CommandType.Text;
            }
            catch(Exception g) { }
            return co.ExecuteNonQuery();
        }
        [WebMethod]
        public void update(string ulke, string kita,string id,string baskent)
        {
            openConn(connString);
            string quer = "update ulkeler set isim='"+ulke+"' ,kita='"+kita+"',baskent='"+baskent+"' where id='"+id+"'";
            try
            {
                command.Connection = connection;
                command.CommandText = quer;
                command.CommandType = CommandType.Text;
                command.ExecuteNonQuery();
            }
            catch (Exception a) { }
        }
        [WebMethod]
        public void insert(string ulke,string kita,string baskent)
        {
            openConn(connString);
            string quer = "insert into ulkeler (isim,kita,baskent) values('"+ulke+"','"+kita+"','"+baskent+"')";
            try
            {
                command.Connection = connection;
                command.CommandText = quer;
                command.CommandType = CommandType.Text;
                command.ExecuteNonQuery();
            }
            catch(Exception a) { }
        }
        [WebMethod]
        public void delete(string id)
        {
            openConn(connString);
            string quer = "delete from ulkeler where id='"+id+"'";
            try
            {
                command.Connection = connection;
                command.CommandText = quer;
                command.CommandType = CommandType.Text;
                command.ExecuteNonQuery();
            }
            catch (Exception a) { }
        }
        [WebMethod]
        public string search(string ulke,string kita)
        {
            string que = "select isim,kita,baskent,id from ulkeler where isim like '%"+ulke+"%' and kita like '%"+kita+"%'";
            DbReader = readWithReader(que);
            table.Columns.Add("name");
            table.Columns.Add("contenent");
            table.Columns.Add("capitol");
            table.Columns.Add("id");
            while (DbReader.Read())
            {
                table.Rows.Add(DbReader["isim"], DbReader["kita"],DbReader["baskent"],DbReader["id"]);
            }
            DbReader.Close();
            return JsonConvert.SerializeObject(table);
        }
        [WebMethod]
        public string select(string id)
        {
            string query = "select * from ulkeler where id='"+id+"'";
            DbReader= readWithReader(query);
            table.Columns.Add("name");
            table.Columns.Add("contenent");
            table.Columns.Add("capitol");
            table.Columns.Add("id");
            while (DbReader.Read())
            {
                table.Rows.Add(DbReader["isim"],DbReader["kita"],DbReader["baskent"], DbReader["id"]);
            }
            DbReader.Close();
            return JsonConvert.SerializeObject(table);
        }
        [WebMethod]
        public string baskent(string kita)
        {
            string que = "select isim from sehir where kita='"+kita+"'";
            DbReader = readWithReader(que);
            table.Columns.Add("name");
            while (DbReader.Read())
            {
                table.Rows.Add(DbReader["isim"]);
            }
            DbReader.Close();
            return JsonConvert.SerializeObject(table);
        }
    }
}