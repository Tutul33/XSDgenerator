using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace WpfAppSln
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Selects a folder to save the generated XSD file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectFolder_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    TxtFolder.Text = dialog.SelectedPath;
                }
            }
        }
        /// <summary>
        /// Generates an XSD file based on the schema of the specified stored procedure.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GenerateXsd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string server = TxtServer.Text;
                string database = TxtDatabase.Text;
                string user = TxtUser.Text;
                string password = TxtPassword.Password;
                string storedProc = TxtStoredProc.Text;
                string folder = TxtFolder.Text;

                if (string.IsNullOrEmpty(folder))
                {
                    System.Windows.MessageBox.Show("Please select a folder.");
                    return;
                }

                string connStr = $"Server={server};Database={database};User Id={user};Password={password};";
                string outputFile = System.IO.Path.Combine(folder, storedProc + ".xsd");

                using (SqlConnection conn = new SqlConnection(connStr))
                using (SqlCommand cmd = new SqlCommand(storedProc, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SchemaOnly))
                    {
                        DataTable schema = reader.GetSchemaTable();

                        using (XmlWriter writer = XmlWriter.Create(outputFile, new XmlWriterSettings { Indent = true }))
                        {
                            writer.WriteStartDocument();
                            writer.WriteStartElement("xs", "schema", "http://www.w3.org/2001/XMLSchema");

                            writer.WriteStartElement("xs", "element", null);
                            writer.WriteAttributeString("name", storedProc);

                            writer.WriteStartElement("xs", "complexType", null);
                            writer.WriteStartElement("xs", "sequence", null);

                            foreach (DataRow row in schema.Rows)
                            {
                                string colName = row["ColumnName"].ToString();
                                string dataType = MapToXsdType((Type)row["DataType"]);

                                writer.WriteStartElement("xs", "element", null);
                                writer.WriteAttributeString("name", colName);
                                writer.WriteAttributeString("type", dataType);
                                writer.WriteAttributeString("nillable", "true");
                                writer.WriteEndElement();
                            }

                            writer.WriteEndElement(); // sequence
                            writer.WriteEndElement(); // complexType
                            writer.WriteEndElement(); // element
                            writer.WriteEndElement(); // schema
                            writer.WriteEndDocument();
                        }
                    }
                }

                System.Windows.MessageBox.Show($"XSD file generated at:\n{outputFile}");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Error: " + ex.Message);
            }
        }
        /// <summary>
        /// Maps .NET types to corresponding XSD types.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private string MapToXsdType(Type type)
        {
            if (type == typeof(int) || type == typeof(long) || type == typeof(short)) return "xs:int";
            if (type == typeof(string)) return "xs:string";
            if (type == typeof(DateTime)) return "xs:dateTime";
            if (type == typeof(bool)) return "xs:boolean";
            if (type == typeof(decimal) || type == typeof(double) || type == typeof(float)) return "xs:decimal";
            if (type == typeof(byte)) return "xs:byte";
            return "xs:string"; // default
        }
    }
}
