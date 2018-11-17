using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Microsoft.SqlServer.Management.Smo;
using System.Configuration;


namespace ClassMaker2
{
    public partial class Form1 : Form
    {

        Server svr = new Server(Program.Server);
        public int TableCount;
        public string Connection = "\"Connection\"";
        public string IdentityColumn = "ID";

        public Form1()
        {


            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textEdit1.Text == string.Empty)
            {
                MessageBox.Show("Lütfen Veri Tabanının İsmini Girin", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                splashScreenManager1.ShowWaitForm();
                listBoxControl1.Items.Clear();
                Program.Database = textEdit1.Text;
                Program.ConnectionString = string.Format("Server={0};Database={1};User Id={2};Password={3}", Program.Server, Program.Database, Program.Username, Program.Password);

                List<string> Tables = new List<string>();


                TableCount = svr.Databases[Program.Database].Tables.Count;

                //File.OpenWrite("C:\\Users\\Basri\\Documents\\visual studio 2013\\Projects\\ClassMaker2\\ClassMaker2\\Class\\Class1.cs");

                //StreamWriter s = new StreamWriter("C:\\Users\\Basri\\Documents\\visual studio 2013\\Projects\\ClassMaker2\\ClassMaker2\\Class\\Class1.cs");
                //s.WriteLine("hasan");
                //s.Close();


                if (listBoxControl1.Items.Count == 0)
                {
                    for (int i = 0; i < TableCount; i++)
                    {
                        //MessageBox.Show(svr.Databases[Program.Database].Tables[i].Columns[i].DataType.ToString());
                        listBoxControl1.Items.Add(svr.Databases[Program.Database].Tables[i].Name);

                    }

                }
                splashScreenManager1.CloseWaitForm();

            }
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            if (txtProjectName.Text == string.Empty)
            {
                MessageBox.Show("Proje İsmi Boş Bırakılamaz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (TableCount == 0)
            {
                MessageBox.Show("Öncelikle Bağlanmak İstediğiniz Veri Tabanını Yazıp Listelemelisiniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            else
            {
                Program.ProjectName = txtProjectName.Text;

                FileStream config = File.Create(string.Format("C:\\Users\\okhas\\Desktop\\Classes\\{0}.config", "App"));
                config.Close();

                StreamWriter stream = new StreamWriter(string.Format("C:\\Users\\okhas\\Desktop\\Classes\\{0}.config", "App"));

                stream.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
                stream.WriteLine("<configuration>");
                stream.WriteLine("<connectionStrings>");
                stream.WriteLine(string.Format("<add name=\"Connection\" connectionString=\"{0}\"/>", Program.ConnectionString));
                stream.WriteLine("</connectionStrings>");
                stream.WriteLine("</configuration>");

                stream.Close();

                

                splashScreenManager1.ShowWaitForm();

                for (int i = 0; i < TableCount; i++)
                {

                    FileStream fs = File.Create(string.Format("C:\\Users\\okhas\\Desktop\\Classes\\{0}.cs", svr.Databases[Program.Database].Tables[i].Name));
                    fs.Close();
                    StreamWriter SW = new StreamWriter(string.Format("C:\\Users\\okhas\\Desktop\\Classes\\{0}.cs", svr.Databases[Program.Database].Tables[i].Name));
                    SW.WriteLine("using System;");
                    SW.WriteLine("using System.Collections.Generic;");
                    SW.WriteLine("using System.Linq;");
                    SW.WriteLine("using System.Text;");
                    SW.WriteLine("using System.Threading.Tasks;");
                    SW.WriteLine("using System.Data;");
                    SW.WriteLine("using System.Data.SqlClient;");
                    SW.WriteLine("using System.Configuration;");
                    SW.WriteLine("\n");

                    SW.WriteLine("namespace " + Program.ProjectName.ToString());
                    SW.WriteLine("{");
                    SW.WriteLine("public class " + svr.Databases[Program.Database].Tables[i].Name);
                    SW.WriteLine("{");


                    int ColumnCount = svr.Databases[Program.Database].Tables[i].Columns.Count;
                    //Identity olan kolonu buluyoruz
                    //delete ve update işlemleri için gerekli
                    for (int k = 0; k < ColumnCount; k++)
                    {
                        if (svr.Databases[Program.Database].Tables[i].Columns[k].Identity)
                        {
                            IdentityColumn = svr.Databases[Program.Database].Tables[i].Columns[k].Name.ToString();
                        }
                    }

                    string[] Columns2 = new string[ColumnCount - 1];
                    List<string> Columns = new List<string>();

                    for (int j = 0; j < ColumnCount; j++)
                    {
                        //kolon nullable değil ise değişken tanımlama olayı burda gerçekleşecek
                        #region nullable olmayan tip tanımlama
                        if (!svr.Databases[Program.Database].Tables[i].Columns[j].Nullable)
                        {
                            if (svr.Databases[Program.Database].Tables[i].Columns[j].DataType.ToString() == "varchar" || svr.Databases[Program.Database].Tables[i].Columns[j].DataType.ToString() == "nvarchar")
                            {
                                SW.WriteLine("public " + "string" + " " + svr.Databases[Program.Database].Tables[i].Columns[j].Name.ToString() + " {get; set;}");
                            }
                            else if (svr.Databases[Program.Database].Tables[i].Columns[j].DataType.ToString() == "smallint")
                            {
                                SW.WriteLine("public " + "short" + " " + svr.Databases[Program.Database].Tables[i].Columns[j].Name.ToString() + " {get; set;}");
                            }
                            else if (svr.Databases[Program.Database].Tables[i].Columns[j].DataType.ToString()=="Guid")
                            {
                                SW.WriteLine("public " + "GUID" + " " + svr.Databases[Program.Database].Tables[i].Columns[j].Name.ToString() + " {get; set;}");
                            }
                            else if (svr.Databases[Program.Database].Tables[i].Columns[j].DataType.ToString() == "GUID")
                            {
                                SW.WriteLine("public " + "GUID" + " " + svr.Databases[Program.Database].Tables[i].Columns[j].Name.ToString() + " {get; set;}");
                            }
                            else if (svr.Databases[Program.Database].Tables[i].Columns[j].DataType.ToString() == "ntext")
                            {
                                SW.WriteLine("public " + "string" + " " + svr.Databases[Program.Database].Tables[i].Columns[j].Name.ToString() + " {get; set;}");
                            }
                            else if (svr.Databases[Program.Database].Tables[i].Columns[j].DataType.ToString() == "char")
                            {
                                SW.WriteLine("public " + "char" + " " + svr.Databases[Program.Database].Tables[i].Columns[j].Name.ToString() + " {get; set;}");
                            }
                            else if (svr.Databases[Program.Database].Tables[i].Columns[j].DataType.ToString() == "bit")
                            {
                                SW.WriteLine("public " + "bool" + " " + svr.Databases[Program.Database].Tables[i].Columns[j].Name.ToString() + " {get; set;}");

                            }
                            else if (svr.Databases[Program.Database].Tables[i].Columns[j].DataType.ToString() == "money")
                            {
                                SW.WriteLine("public " + "decimal" + " " + svr.Databases[Program.Database].Tables[i].Columns[j].Name.ToString() + " {get; set;}");

                            }
                            else if (svr.Databases[Program.Database].Tables[i].Columns[j].DataType.ToString() == "sql variant")
                            {
                                SW.WriteLine("public " + "object" + " " + svr.Databases[Program.Database].Tables[i].Columns[j].Name.ToString() + " {get; set;}");

                            }
                            else if (svr.Databases[Program.Database].Tables[i].Columns[j].DataType.ToString() == "tinyint")
                            {
                                SW.WriteLine("public " + "byte" + " " + svr.Databases[Program.Database].Tables[i].Columns[j].Name.ToString() + " {get; set;}");
                            }
                            else if (svr.Databases[Program.Database].Tables[i].Columns[j].DataType.ToString() == "uniqueidentifier")
                            {
                                SW.WriteLine("public " + "GUID" + " " + svr.Databases[Program.Database].Tables[i].Columns[j].Name.ToString() + " {get; set;}");

                            }
                            else if (svr.Databases[Program.Database].Tables[i].Columns[j].DataType.ToString() == "datetime")
                            {
                                SW.WriteLine("public " + "DateTime" + " " + svr.Databases[Program.Database].Tables[i].Columns[j].Name.ToString() + " {get; set;}");
                            }
                            else if (svr.Databases[Program.Database].Tables[i].Columns[j].DataType.ToString() == "uniqueidentifier")
                            {
                                SW.WriteLine("public " + "GUID?" + " " + svr.Databases[Program.Database].Tables[i].Columns[j].Name.ToString() + " {get; set;}");

                            }
                            else
                            {
                                SW.WriteLine("public " + svr.Databases[Program.Database].Tables[i].Columns[j].DataType.ToString() + " " + svr.Databases[Program.Database].Tables[i].Columns[j].Name.ToString() + " {get; set;}");
                            }


                        }
                        #endregion
                        //kolon nullable ise değişken tanımlama olayı
                        #region nullable olan tip tanımlama
                        else if (svr.Databases[Program.Database].Tables[i].Columns[j].Nullable)
                        {
                            if (svr.Databases[Program.Database].Tables[i].Columns[j].DataType.ToString() == "varchar" || svr.Databases[Program.Database].Tables[i].Columns[j].DataType.ToString() == "nvarchar")
                            {
                                SW.WriteLine("public " + "string" + " " + svr.Databases[Program.Database].Tables[i].Columns[j].Name.ToString() + " {get; set;}");
                            }
                            else if (svr.Databases[Program.Database].Tables[i].Columns[j].DataType.ToString() == "Guid")
                            {
                                SW.WriteLine("public " + "GUID" + " " + svr.Databases[Program.Database].Tables[i].Columns[j].Name.ToString() + " {get; set;}");
                            }
                            else if (svr.Databases[Program.Database].Tables[i].Columns[j].DataType.ToString() == "GUID")
                            {
                                SW.WriteLine("public " + "GUID" + " " + svr.Databases[Program.Database].Tables[i].Columns[j].Name.ToString() + " {get; set;}");
                            }
                            else if (svr.Databases[Program.Database].Tables[i].Columns[j].DataType.ToString() == "smallint")
                            {
                                SW.WriteLine("public " + "short?" + " " + svr.Databases[Program.Database].Tables[i].Columns[j].Name.ToString() + " {get; set;}");
                            }
                            else if (svr.Databases[Program.Database].Tables[i].Columns[j].DataType.ToString() == "ntext")
                            {
                                SW.WriteLine("public " + "string" + " " + svr.Databases[Program.Database].Tables[i].Columns[j].Name.ToString() + " {get; set;}");
                            }
                            else if (svr.Databases[Program.Database].Tables[i].Columns[j].DataType.ToString() == "char")
                            {
                                SW.WriteLine("public " + "char?" + " " + svr.Databases[Program.Database].Tables[i].Columns[j].Name.ToString() + " {get; set;}");
                            }
                            else if (svr.Databases[Program.Database].Tables[i].Columns[j].DataType.ToString() == "bit")
                            {
                                SW.WriteLine("public " + "bool?" + " " + svr.Databases[Program.Database].Tables[i].Columns[j].Name.ToString() + " {get; set;}");

                            }
                            else if (svr.Databases[Program.Database].Tables[i].Columns[j].DataType.ToString() == "money")
                            {
                                SW.WriteLine("public " + "decimal?" + " " + svr.Databases[Program.Database].Tables[i].Columns[j].Name.ToString() + " {get; set;}");

                            }
                            else if (svr.Databases[Program.Database].Tables[i].Columns[j].DataType.ToString() == "sql variant")
                            {
                                SW.WriteLine("public " + "object?" + " " + svr.Databases[Program.Database].Tables[i].Columns[j].Name.ToString() + " {get; set;}");

                            }
                            else if (svr.Databases[Program.Database].Tables[i].Columns[j].DataType.ToString() == "tinyint")
                            {
                                SW.WriteLine("public " + "byte?" + " " + svr.Databases[Program.Database].Tables[i].Columns[j].Name.ToString() + " {get; set;}");
                            }
                            else if (svr.Databases[Program.Database].Tables[i].Columns[j].DataType.ToString() == "uniqueidentifier")
                            {
                                SW.WriteLine("public " + "GUID?" + " " + svr.Databases[Program.Database].Tables[i].Columns[j].Name.ToString() + " {get; set;}");

                            }
                            else if (svr.Databases[Program.Database].Tables[i].Columns[j].DataType.ToString() == "datetime")
                            {
                                SW.WriteLine("public " + "DateTime?" + " " + svr.Databases[Program.Database].Tables[i].Columns[j].Name.ToString() + " {get; set;}");
                            }
                            else
                            {
                                SW.WriteLine("public " + svr.Databases[Program.Database].Tables[i].Columns[j].DataType.ToString() + "? " + svr.Databases[Program.Database].Tables[i].Columns[j].Name.ToString() + " {get; set;}");
                            }


                        }
                        #endregion





                    }
                    for (int j = 1; j < ColumnCount; j++)
                    {
                        if (!svr.Databases[Program.Database].Tables[i].Columns[j].Identity)
                        {
                            Columns2[j - 1] = svr.Databases[Program.Database].Tables[i].Columns[j].Name.ToString();
                            Columns.Add(svr.Databases[Program.Database].Tables[i].Columns[j].Name.ToString());
                        }
                    }

                    string ColumnsCommand = string.Empty;
                    string ColumnsParameters = string.Empty;

                    for (int k = 0; k < Columns2.Length; k++)
                    {

                        if (k == Columns2.Length - 1)
                        {
                            ColumnsCommand += Columns2[k];
                        }
                        else
                        {
                            ColumnsCommand += Columns2[k] + ",";
                        }
                    }

                    for (int k = 0; k < Columns2.Length; k++)
                    {

                        if (k == Columns2.Length - 1)
                        {
                            ColumnsParameters += "@" + Columns2[k];
                        }
                        else
                        {
                            ColumnsParameters += "@" + Columns2[k] + ",";
                        }
                    }

                    //foreach (var item in Columns)
                    //{

                    //    ColumnsCommand += item + ",";
                    //}

                    //foreach (var item in Columns)
                    //{
                    //    ColumnsParameters += "@" + item + ",";
                    //}



                    //Veritabanına insert etme işlemi için metod tanımlıyoruz

                    #region InsertToDatabase

                    SW.WriteLine("public void Insert({0} entity)", svr.Databases[Program.Database].Tables[i].Name);
                    SW.WriteLine("{");
                    //connection ve command cümleciklerini tanımlıyoruz
                    SW.WriteLine("string ConnectionString=ConfigurationManager.ConnectionStrings[{0}].ConnectionString;", Connection);
                    SW.WriteLine("string CommandString=\"INSERT INTO {0} ({1}) VALUES({2})\";", svr.Databases[Program.Database].Tables[i].Name.ToString(), ColumnsCommand, ColumnsParameters);

                    SW.WriteLine("SqlConnection con=new SqlConnection(ConnectionString);");
                    SW.WriteLine("con.Open();");
                    SW.WriteLine("SqlCommand cmd=new SqlCommand(CommandString,con);");
                    SW.WriteLine("cmd.CommandType=CommandType.Text;");

                    //identity olan değeri parametre olarak vermiyoruz,otomatik artıyor
                    for (int k = 0; k < svr.Databases[Program.Database].Tables[i].Columns.Count; k++)
                    {
                        if (!svr.Databases[Program.Database].Tables[i].Columns[k].Identity)
                        {
                            SW.WriteLine("if(entity.{0}!=null)", svr.Databases[Program.Database].Tables[i].Columns[k].Name.ToString());
                            SW.WriteLine("{");
                            SW.WriteLine("cmd.Parameters.AddWithValue(\"@{0}\",entity.{0});", svr.Databases[Program.Database].Tables[i].Columns[k].Name.ToString());
                            SW.WriteLine("}");
                            SW.WriteLine("else");
                            SW.WriteLine("{");
                            switch (svr.Databases[Program.Database].Tables[i].Columns[k].DataType.ToString())
                            {

                                case "GUID":
                                    SW.WriteLine("cmd.Parameters.AddWithValue(\"@{0}\",Guid.Empty);", svr.Databases[Program.Database].Tables[i].Columns[k].Name.ToString());
                                    break;
                                case "uniqueidentifier":
                                    SW.WriteLine("cmd.Parameters.AddWithValue(\"@{0}\",Guid.Empty);", svr.Databases[Program.Database].Tables[i].Columns[k].Name.ToString());
                                    break;
                                case "varchar":
                                    SW.WriteLine("cmd.Parameters.AddWithValue(\"@{0}\",string.Empty);", svr.Databases[Program.Database].Tables[i].Columns[k].Name.ToString());
                                    break;
                                case "nvarchar":
                                    SW.WriteLine("cmd.Parameters.AddWithValue(\"@{0}\",string.Empty);", svr.Databases[Program.Database].Tables[i].Columns[k].Name.ToString());
                                    break;
                                case "int":
                                    SW.WriteLine("cmd.Parameters.AddWithValue(\"@{0}\",default(int));", svr.Databases[Program.Database].Tables[i].Columns[k].Name.ToString());
                                    break;
                                case"float":
                                    SW.WriteLine("cmd.Parameters.AddWithValue(\"@{0}\",default(float));", svr.Databases[Program.Database].Tables[i].Columns[k].Name.ToString());
                                    break;
                                case "double":
                                    SW.WriteLine("cmd.Parameters.AddWithValue(\"@{0}\",default(double));", svr.Databases[Program.Database].Tables[i].Columns[k].Name.ToString());
                                    break;
                                case "smallint":
                                    SW.WriteLine("cmd.Parameters.AddWithValue(\"@{0}\",default(int));", svr.Databases[Program.Database].Tables[i].Columns[k].Name.ToString());
                                    break;
                                case "datetime":
                                      SW.WriteLine("cmd.Parameters.AddWithValue(\"@{0}\",DateTime.Now);", svr.Databases[Program.Database].Tables[i].Columns[k].Name.ToString());
                                    break;
                                case "money":
                                    SW.WriteLine("cmd.Parameters.AddWithValue(\"@{0}\",default(decimal));", svr.Databases[Program.Database].Tables[i].Columns[k].Name.ToString());
                                    break;
                                default:
                                    break;
                            }

                            SW.WriteLine("}");
                        }
                    }

                    SW.WriteLine("cmd.ExecuteNonQuery();");
                    SW.WriteLine("}");
                    //SW.WriteLine("}");
                    //SW.WriteLine("}"); 
                    #endregion


                    #region UpdateDatabase

                    string updateCommand = string.Empty;


                    for (int k = 0; k < Columns2.Length; k++)
                    {
                        if (k == Columns2.Length - 1)
                        {
                            updateCommand += Columns2[k] + "=" + "@" + Columns2[k];
                        }
                        else
                        {
                            updateCommand += Columns2[k] + "=" + "@" + Columns2[k] + " AND ";

                        }

                    }

                    SW.WriteLine("public void Update({0} entity)", svr.Databases[Program.Database].Tables[i].Name);
                    SW.WriteLine("{");
                    SW.WriteLine("string ConnectionString=ConfigurationManager.ConnectionStrings[{0}].ConnectionString;", Connection);
                    SW.WriteLine("string CommandString=\"UPDATE {0} SET {1} WHERE {2}=@{2}\";", svr.Databases[Program.Database].Tables[i].Name.ToString(), updateCommand, IdentityColumn);
                    SW.WriteLine("SqlConnection con=new SqlConnection(ConnectionString);");
                    SW.WriteLine("con.Open();");
                    SW.WriteLine("SqlCommand cmd=new SqlCommand(CommandString,con);");
                    SW.WriteLine("cmd.CommandType=CommandType.Text;");

                    //identity olan değeri parametre olarak vermiyoruz!!!!!!
                    for (int k = 0; k < svr.Databases[Program.Database].Tables[i].Columns.Count; k++)
                    {
                        if (!svr.Databases[Program.Database].Tables[i].Columns[k].Identity)
                        {
                            SW.WriteLine("if(entity.{0}!=null)", svr.Databases[Program.Database].Tables[i].Columns[k].Name.ToString());
                            SW.WriteLine("{");
                            SW.WriteLine("cmd.Parameters.AddWithValue(\"@{0}\",entity.{0});", svr.Databases[Program.Database].Tables[i].Columns[k].Name.ToString());
                            SW.WriteLine("}");
                            SW.WriteLine("else");
                            SW.WriteLine("{");
                            switch (svr.Databases[Program.Database].Tables[i].Columns[k].DataType.ToString())
                            {

                                case "GUID":
                                    SW.WriteLine("cmd.Parameters.AddWithValue(\"@{0}\",Guid.Empty);", svr.Databases[Program.Database].Tables[i].Columns[k].Name.ToString());
                                    break;
                                case "uniqueidentifier":
                                    SW.WriteLine("cmd.Parameters.AddWithValue(\"@{0}\",Guid.Empty);", svr.Databases[Program.Database].Tables[i].Columns[k].Name.ToString());
                                    break;
                                case "varchar":
                                    SW.WriteLine("cmd.Parameters.AddWithValue(\"@{0}\",string.Empty);", svr.Databases[Program.Database].Tables[i].Columns[k].Name.ToString());
                                    break;
                                case "nvarchar":
                                    SW.WriteLine("cmd.Parameters.AddWithValue(\"@{0}\",string.Empty);", svr.Databases[Program.Database].Tables[i].Columns[k].Name.ToString());
                                    break;
                                case "int":
                                    SW.WriteLine("cmd.Parameters.AddWithValue(\"@{0}\",default(int));", svr.Databases[Program.Database].Tables[i].Columns[k].Name.ToString());
                                    break;
                                case "float":
                                    SW.WriteLine("cmd.Parameters.AddWithValue(\"@{0}\",default(float));", svr.Databases[Program.Database].Tables[i].Columns[k].Name.ToString());
                                    break;
                                case "double":
                                    SW.WriteLine("cmd.Parameters.AddWithValue(\"@{0}\",default(double));", svr.Databases[Program.Database].Tables[i].Columns[k].Name.ToString());
                                    break;
                                case "smallint":
                                    SW.WriteLine("cmd.Parameters.AddWithValue(\"@{0}\",default(int));", svr.Databases[Program.Database].Tables[i].Columns[k].Name.ToString());
                                    break;
                                case "datetime":
                                    SW.WriteLine("cmd.Parameters.AddWithValue(\"@{0}\",DateTime.Now);", svr.Databases[Program.Database].Tables[i].Columns[k].Name.ToString());
                                    break;
                                case "money":
                                    SW.WriteLine("cmd.Parameters.AddWithValue(\"@{0}\",default(decimal));", svr.Databases[Program.Database].Tables[i].Columns[k].Name.ToString());
                                    break;
                                default:
                                    break;
                            }
                        }


                    }
                    SW.WriteLine("cmd.Parameters.AddWithValue(\"@{0}\",entity.{0});", IdentityColumn);
                    SW.WriteLine("cmd.ExecuteNonQuery();");
                    SW.WriteLine("}");

                    #endregion

                    #region DeleteFromDatabase



                    SW.WriteLine("public void Delete({0} entity)", svr.Databases[Program.Database].Tables[i].Name);
                    SW.WriteLine("{");
                    SW.WriteLine("string ConnectionString=ConfigurationManager.ConnectionStrings[{0}].ConnectionString;", Connection);
                    SW.WriteLine("string CommandString=\"DELETE FROM {0}  WHERE {1}=@{1}\";", svr.Databases[Program.Database].Tables[i].Name.ToString(), IdentityColumn);
                    SW.WriteLine("SqlConnection con=new SqlConnection(ConnectionString);");
                    SW.WriteLine("con.Open();");
                    SW.WriteLine("SqlCommand cmd=new SqlCommand(CommandString,con);");
                    SW.WriteLine("cmd.CommandType=CommandType.Text;");
                    SW.WriteLine("cmd.Parameters.AddWithValue(\"@{0}\",entity.{0});", IdentityColumn);
                    SW.WriteLine("cmd.ExecuteNonQuery();");
                    SW.WriteLine("}");

                    #endregion

                    #region GetFromDatabase
                    SW.WriteLine("public List<{0}> Read()", svr.Databases[Program.Database].Tables[i].Name.ToString());
                    SW.WriteLine("{");
                    SW.WriteLine("List<{0}> items=new List<{0}>();", svr.Databases[Program.Database].Tables[i].Name.ToString());
                    SW.WriteLine("string ConnectionString=ConfigurationManager.ConnectionStrings[{0}].ConnectionString;", Connection);
                    SW.WriteLine("string CommandString=\"SELECT * FROM {0}\";", svr.Databases[Program.Database].Tables[i].Name.ToString());
                    SW.WriteLine("SqlConnection con=new SqlConnection(ConnectionString);");
                    SW.WriteLine("con.Open();");
                    SW.WriteLine("SqlCommand cmd=new SqlCommand(CommandString,con);");
                    SW.WriteLine("cmd.CommandType=CommandType.Text;");
                    SW.WriteLine("SqlDataReader dr=cmd.ExecuteReader();");
                    SW.WriteLine("\n");
                    SW.WriteLine("while(dr.Read())");
                    SW.WriteLine("{");
                    SW.WriteLine("{0} item=new {0}();", svr.Databases[Program.Database].Tables[i].Name.ToString());

                    for (int k = 0; k < svr.Databases[Program.Database].Tables[i].Columns.Count; k++)
                    {
                        if (!svr.Databases[Program.Database].Tables[i].Columns[k].Identity)
                        {
                            if (!svr.Databases[Program.Database].Tables[i].Columns[k].Nullable)
                            {
                                switch (svr.Databases[Program.Database].Tables[i].Columns[k].DataType.ToString())
                                {
                                    case "int":
                                        SW.WriteLine("item.{0}=(int)dr[\"{0}\"];", svr.Databases[Program.Database].Tables[i].Columns[k].Name.ToString());
                                        break;
                                    case "varchar":
                                        SW.WriteLine("item.{0}=dr[\"{0}\"].ToString();", svr.Databases[Program.Database].Tables[i].Columns[k].Name.ToString());
                                        break;
                                    case "nvarchar":
                                        SW.WriteLine("item.{0}=dr[\"{0}\"].ToString();", svr.Databases[Program.Database].Tables[i].Columns[k].Name.ToString());
                                        break;
                                    case "ntext":
                                        SW.WriteLine("item.{0}=dr[\"{0}\"].ToString();", svr.Databases[Program.Database].Tables[i].Columns[k].Name.ToString());
                                        break;
                                    case "char":
                                        SW.WriteLine("item.{0}=Convert.ToChar(dr[\"{0}\"]);", svr.Databases[Program.Database].Tables[i].Columns[k].Name.ToString());
                                        break;
                                    case "double":
                                        SW.WriteLine("item.{0}=(double)dr[\"{0}\"];", svr.Databases[Program.Database].Tables[i].Columns[k].Name.ToString());
                                        break;
                                    case "bit":
                                        SW.WriteLine("item.{0}=Convert.ToBoolean(dr[\"{0}\"]);", svr.Databases[Program.Database].Tables[i].Columns[k].Name.ToString());
                                        break;
                                    case "datetime":
                                        SW.WriteLine("item.{0}=Convert.ToDateTime(dr[\"{0}\"]);", svr.Databases[Program.Database].Tables[i].Columns[k].Name.ToString());
                                        break;
                                    case "decimal":
                                        SW.WriteLine("item.{0}=(decimal)dr[\"{0}\"];", svr.Databases[Program.Database].Tables[i].Columns[k].Name.ToString());
                                        break;
                                    case "float":
                                        SW.WriteLine("item.{0}=(float)dr[\"{0}\"];", svr.Databases[Program.Database].Tables[i].Columns[k].Name.ToString());
                                        break;
                                    case "money":
                                        SW.WriteLine("item.{0}=(decimal)dr[\"{0}\"];", svr.Databases[Program.Database].Tables[i].Columns[k].Name.ToString());
                                        break;
                                    case "smallint":
                                        SW.WriteLine("item.{0}=(short)dr[\"{0}\"];", svr.Databases[Program.Database].Tables[i].Columns[k].Name.ToString());
                                        break;
                                    case "sql variant":
                                        SW.WriteLine("item.{0}=(object)dr[\"{0}\"];", svr.Databases[Program.Database].Tables[i].Columns[k].Name.ToString());
                                        break;
                                    case "tinyint":
                                        SW.WriteLine("item.{0}=Convert.ToByte(dr[\"{0}\"]);", svr.Databases[Program.Database].Tables[i].Columns[k].Name.ToString());
                                        break;
                                    case "uniqueidentifier":
                                        SW.WriteLine("item.{0}=(GUID)dr[\"{0}\"];", svr.Databases[Program.Database].Tables[i].Columns[k].Name.ToString());
                                        break;

                                    default:
                                        break;
                                }

                            }
                            else if (svr.Databases[Program.Database].Tables[i].Columns[k].Nullable)
                            {
                                switch (svr.Databases[Program.Database].Tables[i].Columns[k].DataType.ToString())
                                {
                                    case "int":
                                        SW.WriteLine("item.{0}=dr[\"{0}\"]==DBNull.Value ? null:(int?)dr[\"{0}\"];", svr.Databases[Program.Database].Tables[i].Columns[k].Name.ToString());
                                        break;
                                    case "varchar":
                                        SW.WriteLine("item.{0}=string.IsNullOrEmpty(dr[\"{0}\"].ToString()) ? string.Empty:dr[\"{0}\"].ToString();", svr.Databases[Program.Database].Tables[i].Columns[k].Name.ToString());
                                        break;
                                    case "nvarchar":
                                        SW.WriteLine("item.{0}=string.IsNullOrEmpty(dr[\"{0}\"].ToString()) ? string.Empty:dr[\"{0}\"].ToString();", svr.Databases[Program.Database].Tables[i].Columns[k].Name.ToString());
                                        break;
                                    case "ntext":
                                        SW.WriteLine("item.{0}=string.IsNullOrEmpty(dr[\"{0}\"].ToString()) ? string.Empty:dr[\"{0}\"].ToString();", svr.Databases[Program.Database].Tables[i].Columns[k].Name.ToString());
                                        break;
                                    case "char":
                                        SW.WriteLine("item.{0}=Convert.ToChar(dr[\"{0}\"])==DBNull.Value ? null:Convert.ToChar(dr[\"{0}\"]);", svr.Databases[Program.Database].Tables[i].Columns[k].Name.ToString());
                                        break;
                                    case "double":
                                        SW.WriteLine("item.{0}=dr[\"{0}\"]==DBNull.Value ? default(double) :(double?)dr[\"{0}\"] ;", svr.Databases[Program.Database].Tables[i].Columns[k].Name.ToString());
                                        break;
                                    case "bit":
                                        SW.WriteLine("item.{0}=Convert.ToBoolean(dr[\"{0}\"]);", svr.Databases[Program.Database].Tables[i].Columns[k].Name.ToString());
                                        break;
                                    case "datetime":
                                        SW.WriteLine("item.{0}=(dr[\"{0}\"])==DBNull.Value ? null : (DateTime?)((dr[\"{0}\"]));", svr.Databases[Program.Database].Tables[i].Columns[k].Name.ToString());
                                        break;
                                    case "decimal":
                                        SW.WriteLine("item.{0}=dr[\"{0}\"]==DBNull.Value ? default(decimal) : (decimal?)dr[\"{0}\"];", svr.Databases[Program.Database].Tables[i].Columns[k].Name.ToString());
                                        break;
                                    case "float":
                                        SW.WriteLine("item.{0}=dr[\"{0}\"]==DBNull.Value ? default(float) : (float?)dr[\"{0}\"];", svr.Databases[Program.Database].Tables[i].Columns[k].Name.ToString());
                                        break;
                                    case "money":
                                        SW.WriteLine("item.{0}=dr[\"{0}\"]==DBNull.Value ? default(decimal) : (decimal?)dr[\"{0}\"];", svr.Databases[Program.Database].Tables[i].Columns[k].Name.ToString());
                                        break;
                                    case "smallint":
                                        SW.WriteLine("item.{0}=dr[\"{0}\"]==DBNull.Value? default(short) : (short?)dr[\"{0}\"];", svr.Databases[Program.Database].Tables[i].Columns[k].Name.ToString());
                                        break;
                                    case "sql variant":
                                        SW.WriteLine("item.{0}=(object)dr[\"{0}\"];", svr.Databases[Program.Database].Tables[i].Columns[k].Name.ToString());
                                        break;
                                    case "tinyint":
                                        SW.WriteLine("item.{0}=Convert.ToByte(dr[\"{0}\"]);", svr.Databases[Program.Database].Tables[i].Columns[k].Name.ToString());
                                        break;
                                    case "uniqueidentifier":
                                        SW.WriteLine("item.{0}=(GUID)dr[\"{0}\"];", svr.Databases[Program.Database].Tables[i].Columns[k].Name.ToString());
                                        break;

                                    default:
                                        break;
                                }
                            }


                        }
                    }
                    SW.WriteLine("items.Add(item);");
                    SW.WriteLine("}");
                    SW.WriteLine("return items;");
                    SW.WriteLine("}");
                    SW.WriteLine("}");
                    SW.WriteLine("}");

                    #endregion


                    SW.Close();
                }
                splashScreenManager1.CloseWaitForm();
            }
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            splashScreenManager1.ShowWaitForm();
            if (listBoxControl1.SelectedItem == null)
            {
                MessageBox.Show("Kolon Bilgilerini Listelemek İstediğiniz Tablo Adını Seçiniz", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            string TableName = listBoxControl1.SelectedItem.ToString();

            List<TabloBilgileri> items = new List<TabloBilgileri>();
            for (int i = 0; i < svr.Databases[Program.Database].Tables[TableName].Columns.Count; i++)
            {
                TabloBilgileri item = new TabloBilgileri();
                item.ColumnName = svr.Databases[Program.Database].Tables[TableName].Columns[i].Name.ToString();
                item.Type = svr.Databases[Program.Database].Tables[TableName].Columns[i].DataType.ToString();
                item.Nullable = svr.Databases[Program.Database].Tables[TableName].Columns[i].Nullable;

                items.Add(item);
            }

            gridControl1.DataSource = items;
            gridView1.Columns["ColumnName"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["Type"].OptionsColumn.AllowEdit = false;
            gridView1.Columns["Nullable"].OptionsColumn.AllowEdit = false;
            splashScreenManager1.CloseWaitForm();



        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (Program.Password == string.Empty || Program.Username == string.Empty || Program.Server == string.Empty)
            {
                frmServer frm = new frmServer();
                frm.ShowDialog();
            }
        }
    }
}
