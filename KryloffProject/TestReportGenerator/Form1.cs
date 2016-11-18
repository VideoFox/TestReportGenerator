using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace TestReportGenerator
{
    public partial class mForm : Form
    {
        // Имя сохраненного файла
        string nameFile = "";
        // Диалог сохранения
        readonly SaveFileDialog saveFileDialog1 = new SaveFileDialog();

        public mForm()
        {
            InitializeComponent();
            saveFileDialog1.Filter = @"rgf files (*.rgf)|*.rgf|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;
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
            OpenFileDialog fileDialog = new OpenFileDialog();
            List<XMLrec> ents = new List<XMLrec>();

            fileDialog.InitialDirectory = "c:\\";
            fileDialog.Filter = @"rgf files (*.rgf)|*.rgf|All files (*.*)|*.*";
            fileDialog.FilterIndex = 1;
            fileDialog.RestoreDirectory = true;

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    nameFile = fileDialog.FileName;
                    ents = ClassLibrary.ReadXML(nameFile, "TextReportGenerator", "TextReportGeneratorData");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(@"Ошибка при загрузке данных! Возможно, неверный формат файла! " + ex.Message);
                }

                // Заполнение полей
                if (ents.Count != 0)
                {
                    List<XMLrec> dtcDescription = new List<XMLrec>();
                    List<XMLrec> pins = new List<XMLrec>();
                    List<XMLrec> textPatterns = new List<XMLrec>();
                    List<XMLrec> funcTestConfigig = new List<XMLrec>();
                    List<XMLrec> simulResult = new List<XMLrec>();

                    foreach (XMLrec item in ents)
                    {
                        #region Заполнение Title
                        if (item.nameElement == "ChipID") tbox1.Text = item.textElement;
                        if (item.nameElement == "MWPID") tbox2.Text = item.textElement;
                        if (item.nameElement == "BatchID") tbox3.Text = item.textElement;
                        if (item.nameElement == "LotID") tbox4.Text = item.textElement;
                        if (item.nameElement == "Process") tbox5.Text = item.textElement;
                        if (item.nameElement == "IOlib") tbox6.Text = item.textElement;
                        if (item.nameElement == "CoreLib") tbox7.Text = item.textElement;

                        if (item.nameElement == "Temperature") tbox8.Text = item.textElement;
                        if (item.nameElement == "Pacaging") tbox9.Text = item.textElement;
                        if (item.nameElement == "Type") tbox10.Text = item.textElement;
                        if (item.nameElement == "WaferNo") tbox11.Text = item.textElement;
                        if (item.nameElement == "NumberOfDies") tbox12.Text = item.textElement;
                        if (item.nameElement == "Author") tbox13.Text = item.textElement;
                        if (item.nameElement == "Measurement") tbox14.Text = item.textElement;
                        if (item.nameElement == "Date") tbox15.Text = item.textElement;
                        if (item.nameElement == "Version") tbox16.Text = item.textElement;

                        if (item.nameElement == "VDD_IO-1") tbox17.Text = item.textElement;
                        if (item.nameElement == "VDD_IO-2") tbox18.Text = item.textElement;
                        if (item.nameElement == "VDD_IO-3") tbox19.Text = item.textElement;
                        if (item.nameElement == "VDD_Core-1") tbox20.Text = item.textElement;
                        if (item.nameElement == "VDD_Core-2") tbox21.Text = item.textElement;
                        if (item.nameElement == "VDD_Core-3") tbox22.Text = item.textElement;
                        #endregion

                        // Массивы данных таблиц
                        if (item.nameElement.Contains("DTCdescription"))
                        {
                            dtcDescription.Add(item);
                        }
                        if (item.nameElement.Contains("SupplyPins"))
                        {
                            pins.Add(item);
                        }
                        if (item.nameElement.Contains("TextPatterns"))
                        {
                            textPatterns.Add(item);
                        }
                        if (item.nameElement.Contains("FunctionalTestConfig"))
                        {
                            funcTestConfigig.Add(item);
                        }
                        if (item.nameElement.Contains("SimulationResult"))
                        {
                            simulResult.Add(item);
                        }
                    }

                    // Сортировка данных таблиц
                    if (dtcDescription.Count != 0)
                    {
                        int j = 0;
                        for (int i = 0; i < dtcDescription.Count/2; i++)
                        {
                            dataGridView1.Rows.Add(dtcDescription[j].textElement, dtcDescription[j+1].textElement);
                            j += 2;
                        }
                    }

                    if (pins.Count != 0)
                    {
                        int j = 0;
                        for (int i = 0; i < pins.Count / 3; i++)
                        {
                            dataGridView2.Rows.Add(pins[j].textElement, pins[j + 1].textElement,
                                                   pins[j + 2].textElement);
                            j += 3;
                        }
                    }

                    if (textPatterns.Count != 0)
                    {
                        int j = 0;
                        for (int i = 0; i < textPatterns.Count / 2; i++)
                        {
                            dataGridView3.Rows.Add(textPatterns[j].textElement, textPatterns[j + 1].textElement);
                            j += 2;
                        }
                    }
                    if (funcTestConfigig.Count != 0)
                    {
                        int j = 0;
                        for (int i = 0; i < funcTestConfigig.Count / 2; i++)
                        {
                            dataGridView4.Rows.Add(funcTestConfigig[j].textElement, funcTestConfigig[j + 1].textElement);
                            j += 2;
                        }
                    }

                    if (simulResult.Count != 0)
                    {
                        int j = 0;
                        for (int i = 0; i < simulResult.Count / 4; i++)
                        {
                            dataGridView6.Rows.Add(simulResult[j].textElement, simulResult[j + 1].textElement,
                                                   simulResult[j + 2].textElement, simulResult[j + 3].textElement);
                            j += 4;
                        }
                    }
                    
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

        XMLrec[] BuildDataArrayGrid(DataGridView dataGridView, string titleNode)
        {
            if (dataGridView.RowCount > 1)
            {
                List<string> arrayList = new List<string>();
                for (int i = 0; i < dataGridView.RowCount; i++)
                {
                    for (int j = 0; j < dataGridView.ColumnCount; j++)
                    {
                        if (dataGridView[j, i].Value == null)
                            arrayList.Add("");
                        else
                            arrayList.Add(dataGridView[j, i].Value as string);
                    }
                }
                XMLrec[] entries = new XMLrec[arrayList.Count + 1];
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
        /// <summary>
        /// Очистка первой страницы
        /// </summary>
        private void button17_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(@"Очистить поля?",
                                         @"Подтверждение удаления", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                tbox1.Text = "";
                tbox2.Text = "";
                tbox3.Text = "";
                tbox4.Text = "";
                tbox5.Text = "";
                tbox6.Text = "";
                tbox7.Text = "";
                tbox8.Text = "";
                tbox9.Text = "";
                tbox10.Text = "";
                tbox11.Text = "";
                tbox12.Text = "";
                tbox13.Text = "";
                tbox14.Text = "";
                tbox15.Text = "";
                tbox16.Text = "";
                tbox17.Text = "";
                tbox18.Text = "";
                tbox19.Text = "";
                tbox20.Text = "";
                tbox21.Text = "";
                tbox22.Text = "";
            }
        }
        #region Очистка таблиц
        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                if (MessageBox.Show(@"Очистить все строки?",
                    @"Подтверждение удаления", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    for (int i = 0; i < dataGridView1.RowCount; i++)
                    {
                        for (int j = 0; j < dataGridView1.ColumnCount; j++)
                        {
                            dataGridView1[j, i].Value = "";
                        }
                    }
                }
            }
        }
        private void button8_Click(object sender, EventArgs e)
        {
            if (dataGridView2.CurrentRow != null)
            {
                if (MessageBox.Show(@"Очистить все строки?",
                    @"Подтверждение удаления", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    for (int i = 0; i < dataGridView2.RowCount; i++)
                    {
                        for (int j = 0; j < dataGridView2.ColumnCount; j++)
                        {
                            dataGridView2[j, i].Value = "";
                        }
                    }
                }
            }
        }
        private void button11_Click(object sender, EventArgs e)
        {
            if (dataGridView3.CurrentRow != null)
            {
                if (MessageBox.Show(@"Очистить все строки?",
                    @"Подтверждение удаления", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    for (int i = 0; i < dataGridView3.RowCount; i++)
                    {
                        for (int j = 0; j < dataGridView3.ColumnCount; j++)
                        {
                            dataGridView3[j, i].Value = "";
                        }
                    }
                }
            }
        }
        private void button14_Click(object sender, EventArgs e)
        {
            if (dataGridView4.CurrentRow != null)
            {
                if (MessageBox.Show(@"Очистить все строки?",
                    @"Подтверждение удаления", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    for (int i = 0; i < dataGridView4.RowCount; i++)
                    {
                        for (int j = 0; j < dataGridView4.ColumnCount; j++)
                        {
                            dataGridView4[j, i].Value = "";
                        }
                    }
                }
            }
        }
        private void button18_Click(object sender, EventArgs e)
        {
            if (dataGridView6.CurrentRow != null)
            {
                if (MessageBox.Show(@"Очистить все строки?",
                    @"Подтверждение удаления", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    for (int i = 0; i < dataGridView6.RowCount; i++)
                    {
                        for (int j = 0; j < dataGridView6.ColumnCount; j++)
                        {
                            dataGridView6[j, i].Value = "";
                        }
                    }
                }
            }
        }
        #endregion
        #region Добавить строку в таблицу
        private void button5_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Add();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 3; i++)
            {
                dataGridView6.Rows.Add();
            }
        }
        private void button7_Click(object sender, EventArgs e)
        {
            dataGridView2.Rows.Add();
        }
        private void button10_Click(object sender, EventArgs e)
        {
            dataGridView3.Rows.Add();
        }
        private void button13_Click(object sender, EventArgs e)
        {
            dataGridView4.Rows.Add();
        }
        #endregion
        #region Удаление строк
        private void button6_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                if (MessageBox.Show(@"Удалить строку?",
                             @"Подтверждение удаления", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    int i = dataGridView1.CurrentRow.Index;
                    dataGridView1.Rows.RemoveAt(i);
                }
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView2.CurrentRow != null)
            {
                if (MessageBox.Show(@"Удалить строку?",
                             @"Подтверждение удаления", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    int i = dataGridView2.CurrentRow.Index;
                    dataGridView2.Rows.RemoveAt(i);
                }
            }
        }
        private void button9_Click(object sender, EventArgs e)
        {
            if (dataGridView3.CurrentRow != null)
            {
                if (MessageBox.Show(@"Удалить строку?",
                             @"Подтверждение удаления", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    int i = dataGridView3.CurrentRow.Index;
                    dataGridView3.Rows.RemoveAt(i);
                }
            }
        }
        private void button12_Click(object sender, EventArgs e)
        {
            if (dataGridView4.CurrentRow != null)
            {
                if (MessageBox.Show(@"Удалить строку?",
                             @"Подтверждение удаления", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    int i = dataGridView4.CurrentRow.Index;
                    dataGridView4.Rows.RemoveAt(i);
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (dataGridView6.CurrentRow != null)
            {
                if (MessageBox.Show(@"Удалить строку?",
                             @"Подтверждение удаления", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    int i = dataGridView6.CurrentRow.Index;
                    dataGridView6.Rows.RemoveAt(i);
                }
            }
        }
        #endregion

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 about=new AboutBox1();
            about.ShowDialog(this);
        }
    }
}
