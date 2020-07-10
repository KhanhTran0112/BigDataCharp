using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DemoBigData
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        static string sql = @"Data Source=LAPTOP-DG8REI13;Initial Catalog=JSON-CSharp;Integrated Security=True";

        public IEnumerable<Product> getProduct()
        {
            using (var connection = new SqlConnection(sql))
            {
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM PRODUCT_TABLE";
                    connection.Open();
                    var reader = cmd.ExecuteReader();
                    using (reader)
                    {
                        while (reader.Read())
                        {
                            var products = new Product
                            {
                                id = reader.GetInt32(0).ToString(),
                                nameProduct = reader.GetString(1),
                                urlProduct = reader.GetString(2),
                                idProduct = reader.GetString(3),
                                nameCategory = reader.GetString(4)
                            };

                            yield return products;
                        }
                    }
                }
            }
        }
        IDisposable subcription;

        private void btnStart_Click(object sender, EventArgs e)
        {
            List<Product> compaies = new List<Product>();
            var bindingList = new BindingList<Product>(compaies);
            var source = new BindingSource(bindingList, null);
            source.ResetBindings(false);
            dataGridView1.DataSource = source;
            bindingList.ResetBindings();
            lbLoading.Text = string.Format("Loading...");
            var obverse = getProduct().ToObservable(ThreadPoolScheduler.Instance).Buffer(Convert.ToInt32(txtBuffer.Text)).ObserveOn(SynchronizationContext.Current);

            subcription = obverse.Subscribe(loadData =>
            {
                compaies.AddRange(loadData);
                source.ResetBindings(false);
                lbLoading.Text = string.Format("Loading " + compaies.Count.ToString() + " rows");

            },
            exception => { lbLoading.Text = exception.Message; },
            () => { lbLoading.Text = string.Format("Finished loading data"); });
        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            subcription.Dispose();
        }
    }
}
