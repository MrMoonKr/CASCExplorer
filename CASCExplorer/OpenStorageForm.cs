using CASCLib;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Windows.Forms;

namespace CASCExplorer
{
    public partial class OpenStorageForm : Form
    {
        public string StoragePath { get; private set; }
        public string Product { get; private set; }

        private List<TACTProduct> products = new List<TACTProduct>();

        public OpenStorageForm()
        {
            InitializeComponent();

            NameValueCollection onlineStorageList = (NameValueCollection)ConfigurationManager.GetSection("OnlineStorageList");

            if (onlineStorageList != null)
            {
                foreach (string game in onlineStorageList)
                {
                    products.Add(new TACTProduct { Id = game, Name = onlineStorageList[game] });
                }
            }

            productComboBox.DataSource = products;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            if (storageFolderBrowserDialog.ShowDialog() != DialogResult.OK)
            {
                DialogResult = DialogResult.Cancel;
                return;
            }

            string path = storageFolderBrowserDialog.SelectedPath;

            textBox1.Text = path;
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            if (productComboBox.SelectedIndex == -1)
            {
                MessageBox.Show("Must select type of game product!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            TACTProduct selectedProduct = productComboBox.SelectedItem as TACTProduct;
            try
            {
                CASCConfig.LoadLocalStorageConfig(textBox1.Text, selectedProduct.Id);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            StoragePath = textBox1.Text;
            Product = selectedProduct.Id;

            DialogResult = DialogResult.OK;
            Close();
        }

        class TACTProduct
        {
            public string Id { get; set; }
            public string Name { get; set; }

            public override string ToString() => $"{Name} ({Id})";
        }
    }
}
