using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TestReportGenerator
{
    public partial class mForm : Form
    {
        public mForm()
        {
            InitializeComponent();
        }

        private void dtcConfig_Click(object sender, EventArgs e)
        {

        }

        private void dtcDescr_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Загрузка данных с тестера
        /// </summary>
        private void button15_Click(object sender, EventArgs e)
        {
            // Создание экземпляра диалога открытия
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = "D:\\",
                Filter = @"txt files (*.txt)|*.txt|All files (*.*)|*.*",
                FilterIndex = 2,
                RestoreDirectory = true
            };

            List<string[]> arrayString = new List<string[]>();

            if (openFileDialog.ShowDialog() != DialogResult.OK) return;
            try
            {
                // Поток для текста
                FileStream file = (FileStream)openFileDialog.OpenFile();
                using (file)
                {
                    // Создаем поток для чтения данных из файла.
                    using (StreamReader sr = new StreamReader(file))
                    {

                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            arrayString.Add(line.Split('\t'));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(@"Ошибка при загрузке данных! " + ex.Message);
            }

            // Если нет данных
            if (arrayString.Count == 0)
            {
                MessageBox.Show(@"Увы! Нет ничего в этом файле! ");
                return;
            }
            // Запись данных в таблицу
            progressBar1.Maximum = arrayString.Count + 1;
            foreach (var item in arrayString)
            {
                progressBar1.Value += 1;
                dataGridView5.Rows.Add(item[0], item[1], item[2], item[3], item[4]);
            }
            progressBar1.Value = 1;
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
             
        }
    }
}
