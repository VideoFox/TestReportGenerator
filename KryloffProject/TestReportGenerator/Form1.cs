using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
// ReSharper disable All

namespace TestReportGenerator
{
    public partial class mForm : Form
    {
        // Имя сохраненного файла
        string nameFile = "";
        // Массив данных с тестера
        public static List<string[]> arrayString = new List<string[]>();
        // Коллекция из отсортированных коллекций из тестердаты
        public static List<List<string[]>> testerSortCollections = new List<List<string[]>>();
        // Коллекция алиасов
        public static List<string> textAliases = new List<string>();
        // Коллекция уровней текстовых алиасов
        public static Dictionary<string,string> LevelDictionary { get; set; }
        // VDD core
        public static string VDDcore1;
        public static string VDDcore2;
        public static string VDDcore3;

        private string pathFile;
        private string testerPath;

        // Диалог сохранения
        readonly SaveFileDialog saveFileDialog1 = new SaveFileDialog();

        public mForm()
        {
            InitializeComponent();
            saveFileDialog1.Filter = @"rgf files (*.rgf)|*.rgf|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;
            DataGridViewComboBoxColumn cmb = (DataGridViewComboBoxColumn) dataGridView2.Columns[4];
            cmb.Items.Add("IO");
            cmb.Items.Add("Core");
            cmb.Items.Add("Memory");
            cmb.FlatStyle = FlatStyle.Flat;

            cmb.DefaultCellStyle.NullValue = "IO";
            // Чтение путей к директории
            pathFile = ConfigSettings.ReadSetting("AppPath");
            testerPath= ConfigSettings.ReadSetting("TesterPath");
            
        }

        /// <summary>
        /// Загрузка данных с тестера
        /// </summary>
        private void button15_Click(object sender, EventArgs e)
        {
            // Создание экземпляра диалога открытия
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                InitialDirectory = testerPath,
                Filter = @"txt files (*.txt)|*.txt|All files (*.*)|*.*",
                FilterIndex = 1,
                RestoreDirectory = true
            };


            if (openFileDialog.ShowDialog() != DialogResult.OK) return;
            try
            {
                // Поток для текста
                FileStream file = (FileStream)openFileDialog.OpenFile();

                // Запись пути к директории
                ConfigSettings.WriteSetting("TesterPath", Path.GetDirectoryName(file.Name));

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
            
            fileDialog.InitialDirectory = pathFile;
            fileDialog.Filter = @"rgf files (*.rgf)|*.rgf|All files (*.*)|*.*";
            fileDialog.FilterIndex = 1;
            fileDialog.RestoreDirectory = true;

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    nameFile = fileDialog.FileName;
                    // Запись пути к директории
                    ConfigSettings.WriteSetting("AppPath", Path.GetDirectoryName(nameFile));

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

                        if (item.nameElement == "Email") tbox23.Text = item.textElement;

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
                        for (int i = 0; i < dtcDescription.Count / 2; i++)
                        {
                            dataGridView1.Rows.Add(dtcDescription[j].textElement, dtcDescription[j + 1].textElement);
                            j += 2;
                        }
                    }

                    if (pins.Count != 0)
                    {
                        int j = 0;
                        for (int i = 0; i < pins.Count / 4; i++)
                        {
                            dataGridView2.Rows.Add(pins[j].textElement,
                                                   pins[j + 1].textElement,
                                                   pins[j + 2].textElement,
                                                   pins[j + 3].textElement);
                            j += 4;
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
                        for (int i = 0; i < funcTestConfigig.Count / 3; i++)
                        {
                            dataGridView4.Rows.Add(funcTestConfigig[j].textElement, funcTestConfigig[j + 1].textElement, funcTestConfigig[j + 2].textElement);
                            j += 3;
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
                new XMLrec("Title","Email", tbox23.Text ,null),
                new XMLrec("Title","VDD_IO-1", tbox17.Text ,null),
                new XMLrec("Title","VDD_IO-2", tbox18.Text ,null),
                new XMLrec("Title","VDD_IO-3", tbox19.Text ,null),
                new XMLrec("Title","VDD_Core-1", tbox20.Text ,null),
                new XMLrec("Title","VDD_Core-2", tbox21.Text ,null),
                new XMLrec("Title","VDD_Core-3", tbox22.Text ,null),
               new XMLrec("","Path","",null),
               new XMLrec("Path","LastFile", nameFile ,null)

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
                tbox23.Text = "";
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

            //    MessageBox.Show(dataGridView1.Rows[0].Cells[0].Value.ToString());

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
            AboutBox1 about = new AboutBox1();
            about.ShowDialog(this);
        }

        private void reportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Путь для сохранения отчета по умолчанию
            string programFiles = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            using (var folderDialog = new FolderBrowserDialog())
            {
                folderDialog.SelectedPath = programFiles;
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    // Копирование файлов отчета в новую папку
                    string nameNewFolder = FileService.CopyTemplate(folderDialog.SelectedPath);

                    // Считывание шаблонов TECH и MOS
                    string techString = "";
                    techString = FileService.TechMosRead(nameNewFolder, "TECH", tbox5.Text);
                    string mosString = "";
                    mosString = FileService.TechMosRead(nameNewFolder, "MOS", tbox5.Text);


                    // Заполнение config
                    StreamReader readerConfig = new StreamReader(nameNewFolder + "\\config.tex");
                    string contentConfig = readerConfig.ReadToEnd();
                    readerConfig.Close();

                    if (!string.IsNullOrEmpty(techString))
                        contentConfig = contentConfig.Replace("$Tech$", techString);
                    if (!string.IsNullOrEmpty(mosString))
                        contentConfig = contentConfig.Replace("$MOS$", mosString);
       
                    string tbox = tbox1.Text.Replace("_", @"\_");
                    contentConfig = contentConfig.Replace("$ChipID$", tbox);
                    tbox = tbox2.Text.Replace("_", @"\_");
                    contentConfig = contentConfig.Replace("$MPWID$", tbox);

                    tbox = tbox3.Text.Replace("_", @"\_");
                    contentConfig = contentConfig.Replace("$BatchID$", tbox);
                    tbox = tbox4.Text.Replace("_", @"\_");
                    contentConfig = contentConfig.Replace("$LotID$", tbox);
                    tbox = tbox5.Text.Replace("_", @"\_");
                    contentConfig = contentConfig.Replace("$Process$", tbox);
                    tbox = tbox6.Text.Replace("_", @"\_");
                    contentConfig = contentConfig.Replace("$IOLib$", tbox);
                    tbox = tbox7.Text.Replace("_", @"\_");
                    contentConfig = contentConfig.Replace("$CoreLib$", tbox);
                    tbox = tbox9.Text.Replace("_", @"\_");
                    contentConfig = contentConfig.Replace("$Packaging$", tbox);
                    tbox = tbox10.Text.Replace("_", @"\_");
                    contentConfig = contentConfig.Replace("$TestType$", tbox);
                    tbox = tbox11.Text.Replace("_", @"\_");
                    contentConfig = contentConfig.Replace("$Wafer$", tbox);
                    tbox = tbox12.Text.Replace("_", @"\_");
                    contentConfig = contentConfig.Replace("$Dies$", tbox);
                    tbox = tbox13.Text.Replace("_", @"\_");
                    contentConfig = contentConfig.Replace("$Author$", tbox);
                    tbox = tbox14.Text.Replace("_", @"\_");
                    contentConfig = contentConfig.Replace("$Measured$", tbox);
                    tbox = tbox23.Text.Replace("_", @"\_");
                    contentConfig = contentConfig.Replace("$Email$", tbox);
                    tbox = tbox16.Text.Replace("_", @"\_");
                    contentConfig = contentConfig.Replace("$Version$", tbox);

                    StreamWriter writerConfig = new StreamWriter(nameNewFolder + "\\config.tex");
                    writerConfig.Write(contentConfig);
                    writerConfig.Close();
                    
                    // Заполнение DTC description
                    StreamReader reader = new StreamReader(nameNewFolder + "\\main.tex");
                    string content = reader.ReadToEnd();
                    reader.Close();

                    string startString = "%Start DTC description";
                    int dtcStart = content.IndexOf(startString) + startString.Length;
                    if (dataGridView1.RowCount != 0)
                    {
                        for (int i = dataGridView1.RowCount - 1; i >= 0; i--)
                        {
                            string txt1 = dataGridView1.Rows[i].Cells[0].Value.ToString().Replace("_", @"\_");
                            string txt2 = dataGridView1.Rows[i].Cells[1].Value.ToString().Replace("_", @"\_");

                            content = content.Insert(dtcStart, "\n\\item " + txt1 + " " + txt2);
                        }
                    }
                    // Заполнение тест паттерна
                    startString = "%Start test patterns";
                    dtcStart = content.IndexOf(startString) + startString.Length;
                    if (dataGridView3.RowCount != 0)
                    {
                        for (int i = dataGridView3.RowCount - 1; i >= 0; i--)
                        {
                            string txt1 = dataGridView3.Rows[i].Cells[0].Value.ToString().Replace("_", @"\_");
                            string txt2 = dataGridView3.Rows[i].Cells[1].Value.ToString().Replace("_", @"\_");

                            content = content.Insert(dtcStart, "\n\\item " + txt1 + " " + txt2);
                        }
                    }
                    // Заполнение supply pins
                    startString = "%Start supply pins";
                    dtcStart = content.IndexOf(startString) + startString.Length;
                    if (dataGridView2.RowCount != 0)
                    {
                        for (int i = dataGridView2.RowCount - 1; i >= 0; i--)
                        {
                            string txt1= dataGridView2.Rows[i].Cells[1].Value.ToString().Replace("_", @"\_");
                            string txt2 = dataGridView2.Rows[i].Cells[2].Value.ToString().Replace("_", @"\_");
                            string txt3 = dataGridView2.Rows[i].Cells[3].Value.ToString().Replace("_", @"\_");

                            content = content.Insert(dtcStart, "\n" + txt1 + " & \\" + txt2 + " & " + txt3 + @" \\ \hline");
                        }
                    }

                    writerConfig = new StreamWriter(nameNewFolder + "\\main.tex");
                    writerConfig.Write(content);
                    writerConfig.Close();


                    // Заполнение supply pins в Config.tex
                    reader = new StreamReader(nameNewFolder + "\\config.tex");
                    content = reader.ReadToEnd();
                    reader.Close();

                    startString = "%Start supply pins";
                    dtcStart = content.IndexOf(startString) + startString.Length;
                    if (dataGridView2.RowCount != 0)
                    {
                        for (int i = dataGridView2.RowCount - 1; i >= 0; i--)
                        {
                            string txt1 = dataGridView2.Rows[i].Cells[0].Value.ToString().Replace("_", @"\_");
                            string txt2 = dataGridView2.Rows[i].Cells[2].Value.ToString().Replace("_", @"\_");

                            content = content.Insert(dtcStart, "\n" + @"\newcommand{\" + txt1 + "}{" + txt2 + "}");
                        }
                    }

                    writerConfig = new StreamWriter(nameNewFolder + "\\config.tex");
                    writerConfig.Write(content);
                    writerConfig.Close();

                }
            }
        }

        /// <summary>
        /// Формирование и сортировка ланных с тестера и тесталиасов
        /// </summary>
        private void funcCulcToolStripMenuItem_Click(object sender, EventArgs e)
        {
            testerSortCollections.Clear();
            textAliases.Clear();
            LevelDictionary=new Dictionary<string, string>();
            // Составление коллекции по тесталиасам
            if (arrayString.Count != 0)
            {
                if (dataGridView4.RowCount != 0)
                {
                    for (int i = 0; i < dataGridView4.RowCount; i++)
                    {
                        textAliases.Add(dataGridView4.Rows[i].Cells[0].Value.ToString());
                        // Заполнение коллекции уровней
                        LevelDictionary.Add(dataGridView4.Rows[i].Cells[0].Value.ToString(),
                                            dataGridView4.Rows[i].Cells[2].Value.ToString());
                    }
                    foreach (var item in textAliases)
                    {
                        List<string[]> tempList = new List<string[]>();
                        foreach (var testerItem in arrayString)
                        {
                            if (item == testerItem[2])
                            {
                                string[] tmpArray = { testerItem[0], testerItem[1], testerItem[2], testerItem[3], testerItem[4] };
                                tempList.Add(tmpArray);
                            }
                        }
                        if (tempList.Count != 0)
                        {
                            // Сортировка данных по Die
                            var tempLstSort = from elem in tempList
                                orderby Convert.ToInt32(elem[1])
                                select  elem
                            ;
                            List<string[]> tempListSort = new List<string[]>();
                            foreach (var itm in tempLstSort)
                            {
                                tempListSort.Add(itm);
                            }

                            // Добавляем отсортированную коллекцию
                            testerSortCollections.Add(tempListSort);
                        }
                        tempList = new List<string[]>();
                    }
                }
            }
            else
            {
                MessageBox.Show("Данные с тестера не загружены!");
                return;
            }




            // Проверка на правильность VDD core
            VDDcore1 = tbox20.Text;
            VDDcore2 = tbox21.Text;
            VDDcore3 = tbox22.Text;
            double res;
            if (!double.TryParse(VDDcore1, out res))
            {
                MessageBox.Show("Неверный формат VDDcore1");
                return;
            }
            if (!double.TryParse(VDDcore2, out res))
            {
                MessageBox.Show("Неверный формат VDDcore2");
                return;
            }
            if (!double.TryParse(VDDcore3, out res))
            {
                MessageBox.Show("Неверный формат VDDcore3");
                return;
            }
            // Форма с таблицей
            FuncCalc funcCalc = new FuncCalc();
            funcCalc.ShowDialog();


        }
    }
}
