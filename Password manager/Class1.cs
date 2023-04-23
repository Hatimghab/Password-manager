using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Password_manager
{
    class Class1
    {
        public static DataTable getallPasswords() 
        {
            
            DataTable dataTable = new DataTable();
            SqlConnection cs = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Integrated Security=True");
                
                    try
                    {
                        SqlCommand cmd = new SqlCommand("Select * from Passwords", cs);
                        cs.Open();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dataTable);
                        cs.Close();
                        //
                        Console.WriteLine("passwords list  is generated.");
                        return dataTable;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error occurred when generating a passwords list.");
                        return null;
                    }

                
            }
        public static DataTable getpassword(int id)
        {
            DataTable dataTable = new DataTable();
            SqlConnection cs = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Integrated Security=True");
            SqlCommand cmd = new SqlCommand("Select * from Passwords where id = " + id, cs);
            cs.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dataTable);
            cs.Close();
            
            if (dataTable.Rows.Count != 0)
            {
                return dataTable;
               
            }
            else
            {
                Console.WriteLine("ein Fehler ist aufgetreten");
                return null;
            }
}
        public static DataTable getdecryptedPassword(int id) {
            DataTable dataTable = new DataTable();
            dataTable = getpassword(id);
            String pass =dataTable.Rows[0][4].ToString();
            byte[] data = Convert.FromBase64String(pass);
            string decodedpass = Encoding.UTF8.GetString(data);
            dataTable.Rows[0][4]=decodedpass;

            if (dataTable.Rows.Count != 0)
            {
                return dataTable;

            }
            else
            {
                Console.WriteLine("We couldn't find this password. Please try again");
                return null;
            }
        }
        public static void addPassword(String cat,string app, string user,string encpass) 
        {
       
            SqlConnection cs = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Integrated Security=True");
            SqlCommand cmd = new SqlCommand("Insert into Passwords (category,app,username,encryptedpassword) values (@cat,@app,@user,@encpass)", cs);
            cmd.Parameters.AddWithValue("@cat", cat);
            cmd.Parameters.AddWithValue("@app", app);
            cmd.Parameters.AddWithValue("@user", user);
            cmd.Parameters.AddWithValue("@encpass", encpass);
            cs.Open();
            cmd.ExecuteNonQuery();
            cs.Close();
        }
        public static void deletePassword(String password)
        { 

            SqlConnection cs = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Integrated Security=True");
            cs.Open();
            SqlCommand cmd = new SqlCommand("delete  from Passwords where encryptedpassword = '" + password + "'", cs);
            cmd.ExecuteNonQuery();
            cs.Close();
        } 
        public static void updatePassword(int id) 
        {
            SqlConnection cs = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Integrated Security=True");
            cs.Open();
            Console.WriteLine("Geben Sie die neue Kategorie vom Password ?");
            string ct = Console.ReadLine();
            Console.WriteLine("Geben Sie die neue App vom Password ?");
            string ap = Console.ReadLine();
            Console.WriteLine("Geben Sie die neue Username vom Password ?");
            string us = Console.ReadLine();
            Console.WriteLine("Geben Sie das neue Password ein ?");
            string pa = Console.ReadLine();
            SqlCommand cmd = new SqlCommand("UPDATE Passwords SET category = '"+ct+"', app = '"+ap+ "', username ='"+us+ "',encryptedpassword ='"+pa+"' WHERE id ="+id,cs);
            cmd.ExecuteNonQuery();
            cs.Close() ;
        }
        static void Main(string[] args)
        {
            //addPassword("work", "whatsapp", "test@test.com", "1234");
            //DataTable dt = new DataTable();
            //dt = getallPasswords();
            //for(int i=0;i<=dt.Rows.Count-1;i++)
            //{
            //Console.WriteLine("ID: "+dt.Rows[i][0].ToString()+ ", CATEGORY: " + dt.Rows[i][1].ToString() + ", APP: " + dt.Rows[i][2].ToString() + ", USERNAME: " + dt.Rows[i][3].ToString() + ", ENCRYPTEDPASSWORD: " + dt.Rows[i][4].ToString());

            //}
            //deletePassword("1234");
            //dt = getallPasswords();
            //for (int i = 0; i <= dt.Rows.Count - 1; i++)
            //{
            //    Console.WriteLine("ID: " + dt.Rows[i][0].ToString() + ", CATEGORY: " + dt.Rows[i][1].ToString() + ", APP: " + dt.Rows[i][2].ToString() + ", USERNAME: " + dt.Rows[i][3].ToString() + ", ENCRYPTEDPASSWORD: " + dt.Rows[i][4].ToString());

            //}
            //dt = getallPasswords();

            Console.WriteLine("Was wollen Sie machen ?");
            while (true)
            {
                Console.WriteLine("                          ");
                Console.WriteLine("***************************");
                Console.WriteLine("Available commands: ");
                Console.WriteLine("q : quit program");
                Console.WriteLine("g : get all encrypted password");
                Console.WriteLine("a : add a password");
                Console.WriteLine("r : remove a password");
                Console.WriteLine("u : update a password");
                Console.WriteLine("s : get a single password");
                Console.WriteLine("d : get a decrypted password");
                Console.WriteLine("***************************");

                string service = Console.ReadLine();
                switch (service)
                {
                    case "g":
                        DataTable dataTable = new DataTable();
                        dataTable = getallPasswords();
                        for (int i = 0; i <= dataTable.Rows.Count - 1; i++)
                        {
                            Console.WriteLine("ID: " + dataTable.Rows[i][0].ToString() + ", CATEGORY: " + dataTable.Rows[i][1].ToString() + ", APP: " + dataTable.Rows[i][2].ToString() + ", USERNAME: " + dataTable.Rows[i][3].ToString() + ", ENCRYPTEDPASSWORD: " + dataTable.Rows[i][4].ToString());
                        }
                            break;
                    case "a":
                        
                        Console.WriteLine("was ist die Kategorie vom Password ?");
                        string k = Console.ReadLine();
                        Console.WriteLine("was ist die App vom Password ?");
                        string a = Console.ReadLine();
                        Console.WriteLine("was ist der Username vom Password ?");
                        string u = Console.ReadLine();
                        Console.WriteLine("Geben Sie das Password ein ?");
                        string p = Console.ReadLine();
                        addPassword(k, a, u, p);
                        break;
                    case "r":

                        Console.WriteLine("Geben Sie das Password , das wollen sie loeschen ein");
                        String pass = Console.ReadLine();
                        deletePassword(pass);
                        break;
                    case "s":

                        Console.WriteLine("Geben Sie das ID des Passwords , das wollen sie abrufen ein");
                        int id = Convert.ToInt32(Console.ReadLine());
                        DataTable dataTable2 = new DataTable(); 
                        dataTable2=getpassword(id);
                        for (int i = 0; i <= dataTable2.Rows.Count - 1; i++)
                        {
                            Console.WriteLine("ID: " + dataTable2.Rows[i][0].ToString() + ", CATEGORY: " + dataTable2.Rows[i][1].ToString() + ", APP: " + dataTable2.Rows[i][2].ToString() + ", USERNAME: " + dataTable2.Rows[i][3].ToString() + ", ENCRYPTEDPASSWORD: " + dataTable2.Rows[i][4].ToString());
                        }
                        break;
                    case "d":
                        Console.WriteLine("Geben Sie das ID des Passwords , das wollen sie als decryted password abrufen ein");
                        int id2 = Convert.ToInt32(Console.ReadLine());
                        DataTable dataTable1 = new DataTable();
                        dataTable1 = getdecryptedPassword(id2);
                        for (int i = 0; i <= dataTable1.Rows.Count - 1; i++)
                        {
                            Console.WriteLine("ID: " + dataTable1.Rows[i][0].ToString() + ", CATEGORY: " + dataTable1.Rows[i][1].ToString() + ", APP: " + dataTable1.Rows[i][2].ToString() + ", USERNAME: " + dataTable1.Rows[i][3].ToString() + ", ENCRYPTEDPASSWORD: " + dataTable1.Rows[i][4].ToString());
                        }
                        break;
                    case "u":

                        Console.WriteLine("was ist der Id vom Password , das wollen sie bearbeiten ?");
                        int idb  = Convert.ToInt32(Console.ReadLine());
           
                       updatePassword(idb);
                        break;
                    case "q":
                        break;

                }

                if (service == "q")
                    break;

            }







        }
    }

    }

