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
        // Имя сохраненного файла
        string nameFile = "";
        // Диалог сохранения
        SaveFileDialog saveFileDialog1 = new SaveFileDialog();

        public mForm()
        {
            InitializeComponent();

            saveFileDialog1.Filter = "rgf files (*.rgf)|*.rgf|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;

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
                FilterIndex = 1,
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

                try
                {
                    dataGridView5.Rows.Add(item[0], item[1], item[2], item[3], item[4]);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(@"Ошибка при загрузке данных! Возможно, неверный формат файла! " + ex);
                    return;
                }

            }
            progressBar1.Value = 1;
        }
        /// <summary>
        /// ОТкрыть сохранение
        /// </summary>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            List<XMLrec> ents=new List<XMLrec>();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "rgf files (*.rgf)|*.rgf|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    nameFile = openFileDialog1.FileName;
                    ents = ClassLibrary.ReadXML(nameFile, "TextReportGenerator", "TextReportGeneratorData");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(@"Ошибка при загрузке данных! Возможно, неверный формат файла! " + ex.Message);
                }
            }
        }
        /// <summary>
        /// Сохранить
        /// </summary>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (nameFile == "")
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    nameFile = saveFileDialog1.FileName;
                }
            }
            ClassLibrary.WriteXML(nameFile, "TextReportGenerator", "TextReportGeneratorData", BuildDataArrayTitle());
            if(dataGridView1.RowCount>1)
             ClassLibrary.WriteXML(nameFile, "TextReportGenerator", "TextReportGeneratorData", BuildDataArrayGrid(dataGridView1, "DTCdescription"));
            if (dataGridView2.RowCount > 1)
                ClassLibrary.WriteXML(nameFile, "TextReportGenerator", "TextReportGeneratorData", BuildDataArrayGrid(dataGridView2, "SupplyPins"));
            if (dataGridView3.RowCount > 1)
                ClassLibrary.WriteXML(nameFile, "TextReportGenerator", "TextReportGeneratorData", BuildDataArrayGrid(dataGridView3, "TextPatterns"));
            if (dataGridView4.RowCount > 1)
                ClassLibrary.WriteXML(nameFile, "TextReportGenerator", "TextReportGeneratorData", BuildDataArrayGrid(dataGridView4, "FunctionalTestConfig"));
            if (dataGridView6.RowCount > 1)
                ClassLibrary.WriteXML(nameFile, "TextReportGenerator", "TextReportGeneratorData", BuildDataArrayGrid(dataGridView6, "SimulationResult"));

        }
        /// <summary>
        /// Метод формирования массива данных
        /// </summary>
        /// <returns></returns>
        XMLrec[] BuildDataArrayTitle()
        {
            // Проверка на наличие файла настроек
            ClassLibrary.createXML(nameFile, "TextReportGenerator");
            XMLrec[] entries = {
                // Запись размеров окна
                new XMLrec("","Title","",null),
                new XMLrec("Title","ChipID", tbox1.Text ,null),
                new XMLrec("Title","MWPID", tbox2.Text ,null),
                new XMLrec("Title","BatchID", tbox3.Text ,null),
                new XMLrec("Title","LotID", tbox4.Text ,null),
                new XMLrec("Title","Process", tbox5.Text ,null),
                new XMLrec("Title","IOlib", tbox6.Text ,null),
                new XMLrec("Title","CoreLib", tbox7.Text ,null),
                new XMLrec("Title","Temperature", tbox8.Text ,null),
                new XMLrec("Title","Pacaging", tbox9.Text ,null),
                new XMLrec("Title","Type", tbox10.Text ,null),
                new XMLrec("Title","WaferNo", tbox11.Text ,null),
                new XMLrec("Title","NumberOfDies", tbox12.Text ,null),
                new XMLrec("Title","Author", tbox13.Text ,null),
                new XMLrec("Title","Measurement", tbox14.Text ,null),
                new XMLrec("Title","Date", tbox15.Text ,null),
                new XMLrec("Title","Version", tbox16.Text ,null),
                new XMLrec("Title","VDD_IO-1", tbox17.Text ,null),
                new XMLrec("Title","VDD_IO-2", tbox18.Text ,null),
                new XMLrec("Title","VDD_IO-3", tbox19.Text ,null),
                new XMLrec("Title","VDD_Core-1", tbox20.Text ,null),
                new XMLrec("Title","VDD_Core-2", tbox21.Text ,null),
                new XMLrec("Title","VDD_Core-3", tbox22.Text ,null)
            };
            return entries;
        }

        XMLrec[] BuildDataArrayGrid(DataGridView dataGridView ,string titleNode )
        {
            if (dataGridView.RowCount > 1)
            {
                List<string> arrayList = new List<string>();
                for (int i = 0; i < dataGridView.RowCount; i++)
                {

                    for (int j = 0; j < dataGridView.ColumnCount; j++)
                    {
                        arrayList.Add(dataGridView[j, i].Value as string);
                    }
                }
                XMLrec[] entries = new XMLrec[arrayList.Count];
                entries[0] = new XMLrec("", titleNode, "", null);

                for (int i = 1; i <= arrayList.Count; i++)
                {
                    entries[i] = new XMLrec(titleNode, titleNode + "_" + i, arrayList[i - 1], null);
                }
                return entries;
            }
            return null;
        }

        /// <summary>
        /// Сохранить как
        /// </summary>
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                nameFile = saveFileDialog1.FileName;
            }
            ClassLibrary.WriteXML(nameFile, "TextReportGenerator", "TextReportGeneratorData", BuildDataArrayTitle());
            if (dataGridView1.RowCount > 1)
                ClassLibrary.WriteXML(nameFile, "TextReportGenerator", "TextReportGeneratorData", BuildDataArrayGrid(dataGridView1, "DTCdescription"));
            if (dataGridView2.RowCount > 1)
                ClassLibrary.WriteXML(nameFile, "TextReportGenerator", "TextReportGeneratorData", BuildDataArrayGrid(dataGridView2, "SupplyPins"));
            if (dataGridView3.RowCount > 1)
                ClassLibrary.WriteXML(nameFile, "TextReportGenerator", "TextReportGeneratorData", BuildDataArrayGrid(dataGridView3, "TextPatterns"));
            if (dataGridView4.RowCount > 1)
                ClassLibrary.WriteXML(nameFile, "TextReportGenerator", "TextReportGeneratorData", BuildDataArrayGrid(dataGridView4, "FunctionalTestConfig"));
            if (dataGridView6.RowCount > 1)
                ClassLibrary.WriteXML(nameFile, "TextReportGenerator", "TextReportGeneratorData", BuildDataArrayGrid(dataGridView6, "SimulationResult"));

        }
        /// <summary>
        /// Выход
        /// </summary>
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
           for (int i = 0; i < 3; i++)
            {
                dataGridView6.Rows.Add();
            }




        }
    }
}
