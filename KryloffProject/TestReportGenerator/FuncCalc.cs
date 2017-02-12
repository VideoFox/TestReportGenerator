using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace TestReportGenerator
{
    public partial class FuncCalc : Form
    {
        // Массив данных с тестера
        public static List<string[]> ArrayString = new List<string[]>();
        // Коллекция из отсортированных коллекций из тестердаты
        public static List<List<string[]>> TesterSortCollections = new List<List<string[]>>();
        // Коллекция алиасов
        public static List<string> TextAliases = new List<string>();

        public static string VdDcore1;
        public static string VdDcore2;
        public static string VdDcore3;

        public FuncCalc()
        {
            InitializeComponent();
            // Получение данных с главной формы
            ArrayString = mForm.arrayString;
            TesterSortCollections = mForm.testerSortCollections;
            TextAliases = mForm.textAliases;
            VdDcore1 = mForm.VDDcore1;
            VdDcore2 = mForm.VDDcore2;
            VdDcore3 = mForm.VDDcore3;
            
            ColumnAdd();
            SummaryColumnAdd();
            RowAdd();
            
        }
        // Вывод столбцов для сумм
        private void SummaryColumnAdd()
        {
            var dgvAge = new DataGridViewTextBoxColumn
            {
                Name = "Tester",
                HeaderText = @"Summ" + @"_" + VdDcore1,
                Width = 100
            };

            var dgvAge2 = new DataGridViewTextBoxColumn
            {
                Name = "Tester2",
                HeaderText = @"Summ" + @"_" + VdDcore2,
                Width = 100
            };
            var dgvAge3 = new DataGridViewTextBoxColumn
            {
                Name = "Tester3",
                HeaderText = @"Summ" + @"_" + VdDcore3,
                Width = 100
            };

            //добавили колонку
            dataGridView1.Columns.Add(dgvAge);
            dataGridView1.Columns.Add(dgvAge2);
            dataGridView1.Columns.Add(dgvAge3);
        }
        // Столбцы для тестов
        void ColumnAdd()
        {
            // Добавление столбцов в таблицу по числу алиасов и напряжений
            foreach (var t in TesterSortCollections)
            {
                var dgvAge = new DataGridViewTextBoxColumn
                {
                    Name = "Tester",
                    HeaderText = t[0][2] + @"_" + VdDcore1,
                    Width = 100
                };

                var dgvAge2 = new DataGridViewTextBoxColumn
                {
                    Name = "Tester2",
                    HeaderText = t[0][2] + @"_" + VdDcore2,
                    Width = 100
                };
                var dgvAge3 = new DataGridViewTextBoxColumn
                {
                    Name = "Tester3",
                    HeaderText = t[0][2] + @"_" + VdDcore3,
                    Width = 100
                };

                //добавили колонку
                dataGridView1.Columns.Add(dgvAge);
                dataGridView1.Columns.Add(dgvAge2);
                dataGridView1.Columns.Add(dgvAge3);
            }
        }

        void RowAdd()
        {
            // Определение максимального количества строк
            if (TesterSortCollections.Count == 0)
            {
                MessageBox.Show(@"Нет данных! Произошла ошибка.");
                return;
            }
            int maxRowCount = TesterSortCollections[0].Count;

            foreach (var item in TesterSortCollections)
            {
                if (item.Count != maxRowCount)
                {
                    MessageBox.Show(@"Разное число кристаллов!");
                    return;
                }
            }
            // Добавление пустых строчек
            for (int i = 0; i < maxRowCount; i++)
            {
                dataGridView1.Rows.Add();
            }
            // Добавление данных
            int y = 1; // Для текущего столбца VDDcore
            for (int i = 0; i < TesterSortCollections.Count; i++)
            {
                // Названия для элементов сравнения
                List<string> parentCollect = new List<string>();
                string nameChild = "";
                string lvl = "";

                foreach (var itm in mForm.LevelDictionary)
                {
                    if (itm.Key == TesterSortCollections[i][0][2])
                    {
                        nameChild = itm.Key;
                        lvl = itm.Value;
                    }
                }

                if (!string.IsNullOrEmpty(nameChild) && lvl.Length > 1)
                {
                    foreach (var itm in mForm.LevelDictionary)
                    {
                        if (TesterSortCollections[i][0][2] != itm.Key && // Исключение самого себя
                            itm.Value.Length == 1 &&  // Главный уровень
                            (itm.Value == "0" ||
                             itm.Value == lvl.Substring(0, 1))
                        )
                        {
                            parentCollect.Add(itm.Key);
                        }
                    }
                }

                for (var j = 0; j < TesterSortCollections[i].Count; j++)
                {
                    // Текущий массив
                    var arrStrings = TesterSortCollections[i][j];
                    dataGridView1.Rows[j].Cells[0].Value = arrStrings[1];

                    var value = Convert.ToDouble(arrStrings[4]) > Convert.ToDouble(VdDcore1) ? "1" : "0";
                    if (arrStrings[4] == "-1") value = "1";
                    dataGridView1.Rows[j].Cells[y].Value = value;

                    value = Convert.ToDouble(arrStrings[4]) > Convert.ToDouble(VdDcore2) ? "1" : "0";
                    if (arrStrings[4] == "-1") value = "1";
                    dataGridView1.Rows[j].Cells[y + 1].Value = value;

                    value = Convert.ToDouble(arrStrings[4]) > Convert.ToDouble(VdDcore3) ? "1" : "0";
                    if (arrStrings[4] == "-1") value = "1";
                    dataGridView1.Rows[j].Cells[y + 2].Value = value;

                   // TesterSortCollections[0][0][4] = "1,7";
                    //TesterSortCollections[0][1][4] = "100";
                    //TesterSortCollections[1][0][4] = "100";

                    if (parentCollect.Count > 0)
                    {
                        foreach (var itm in parentCollect)
                        {
                            foreach (var t in TesterSortCollections)
                            {
                                if (itm != t[0][2]) continue;
                                if (Convert.ToDouble(t[j][4]) > Convert.ToDouble(VdDcore1))
                                {
                                    dataGridView1.Rows[j].Cells[y].Value = "2";
                                }
                                if (Convert.ToDouble(t[j][4]) > Convert.ToDouble(VdDcore2))
                                {
                                    dataGridView1.Rows[j].Cells[y + 1].Value = "2";
                                }
                                if (Convert.ToDouble(t[j][4]) > Convert.ToDouble(VdDcore3))
                                {
                                    dataGridView1.Rows[j].Cells[y + 2].Value = "2";
                                }
                            }
                        }
                    }
                    
                    // Суммирование строки
                    for (var k = 1; k < dataGridView1.ColumnCount - 3; k++)
                    {
                        if ((string)dataGridView1.Rows[j].Cells[k].Value == "1" ||
                            (string)dataGridView1.Rows[j].Cells[k].Value == "2")
                        {
                            if (dataGridView1.Columns[k].Name == "Tester")
                                dataGridView1.Rows[j].Cells[dataGridView1.ColumnCount - 3].Value = "1";
                            if (dataGridView1.Columns[k].Name == "Tester2")
                                dataGridView1.Rows[j].Cells[dataGridView1.ColumnCount - 2].Value = "1";
                            if (dataGridView1.Columns[k].Name == "Tester3")
                                dataGridView1.Rows[j].Cells[dataGridView1.ColumnCount - 1].Value = "1";
                        }
                        else
                        {
                            if (dataGridView1.Columns[k].Name == "Tester")
                                dataGridView1.Rows[j].Cells[dataGridView1.ColumnCount - 3].Value = "0";
                            if (dataGridView1.Columns[k].Name == "Tester2")
                                dataGridView1.Rows[j].Cells[dataGridView1.ColumnCount - 2].Value = "0";
                            if (dataGridView1.Columns[k].Name == "Tester3")
                                dataGridView1.Rows[j].Cells[dataGridView1.ColumnCount - 1].Value = "0";
                        }
                    }

                    //  dataGridView1.Columns[5].Name

                }
                y += 3;
            }
        }

        private void FuncCalc_FormClosing(object sender, FormClosingEventArgs e)
        {
            for (int i = 1; i < dataGridView1.ColumnCount; i++)
            {
                dataGridView1.Columns.RemoveAt(i);
            }

        }

        // Export RAW Data
        private void button1_Click(object sender, EventArgs e)
        {
            // Путь для сохранения отчета по умолчанию
            string programFiles = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            using (var folderDialog = new FolderBrowserDialog())
            {
                folderDialog.SelectedPath = programFiles;
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    string nameReport = "ChipID_MPWID_LotID_Waffer " + DateTime.Now.ToString("yyyy_MM_dd_hh.m.s") + ".txt";

                    string[] lines = new string[dataGridView1.RowCount + 1];

                    // Заголовки
                    lines[0] = "\t";
                    for (int i = 0; i < TesterSortCollections.Count; i++)
                    {
                        lines[0] += TesterSortCollections[i][0][2] + "_" + VdDcore1 + "\t" +
                                    TesterSortCollections[i][0][2] + "_" + VdDcore2 + "\t" +
                                    TesterSortCollections[i][0][2] + "_" + VdDcore3 + "\t";

                    }
                    lines[0] += "Summ_" + VdDcore1 + "\t" + "Summ_" + VdDcore2 + "\t" + "Summ_" + VdDcore3 + "\t";
                    // Строки данных
                    for (int i = 0; i < dataGridView1.RowCount; i++)
                    {
                        lines[i + 1] = "";
                        for (int j = 0; j < dataGridView1.ColumnCount; j++)
                        {
                            lines[i + 1] += dataGridView1.Rows[i].Cells[j].Value + "\t";
                        }
                    }

                    System.IO.File.WriteAllLines(folderDialog.SelectedPath + "\\" + nameReport, lines);

                }
            }
        }
        // Export Func. Yield
        private void button2_Click(object sender, EventArgs e)
        {
            // Путь для сохранения отчета по умолчанию
            string programFiles = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            using (var folderDialog = new FolderBrowserDialog())
            {
                folderDialog.SelectedPath = programFiles;
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    string nameReport = "ChipID_Functional_Yield " + DateTime.Now.ToString("yyyy_MM_dd_hh.m.s") + ".txt";

                    // Строки для разных напряжений
                    string[] lines1 = new string[TesterSortCollections.Count + 2]; // + строка суммы+ строка пробела
                    string[] lines2 = new string[TesterSortCollections.Count + 2]; // + строка суммы+ строка пробела
                    string[] lines3 = new string[TesterSortCollections.Count + 2]; // + строка суммы+ строка пробела
                                                                                   //  var allLines = new []{lines1, lines2, lines3};

                    int cnt = 0;
                    //  string[] nameVDDcore = { VDDcore1 , VDDcore2 , VDDcore3 };

                    int cnt1 = 0, cnt2 = 0, cnt3 = 0;
                    for (int i = 1; i < dataGridView1.ColumnCount; i++)
                    {
                        List<int> lst0 = new List<int>();
                        List<int> lst1 = new List<int>();

                        for (int y = 0; y < dataGridView1.RowCount; y++)
                        {
                            if ((string)dataGridView1.Rows[y].Cells[i].Value == "0")
                            {
                                lst0.Add(0);
                            }
                            if ((string)dataGridView1.Rows[y].Cells[i].Value == "1")
                            {
                                lst1.Add(1);
                            }
                        }
                        string outString = dataGridView1.Columns[i].HeaderText + "\t" +
                                          (lst0.Count + lst1.Count) + "\t" + lst0.Count + "\t" +
                                          lst1.Count + "\t" + ((100 * lst0.Count) / (lst0.Count + lst1.Count));
                        if (cnt == 0)
                        {
                            lines1[cnt1] = outString;
                            cnt1++;
                        }
                        if (cnt == 1)
                        {
                            lines2[cnt2] = outString;
                            cnt2++;
                        }
                        if (cnt == 2)
                        {
                            lines3[cnt3] = outString;
                            cnt3++;
                        }
                        cnt++;
                        if (cnt > 2) cnt = 0;
                    }
                    lines1[cnt1] = " ";
                    lines2[cnt2] = " ";
                    lines3[cnt3] = " ";


                    string[] linesAll = new string[lines1.Length + lines2.Length + lines3.Length-1];

                    for (int i = 0; i < lines1.Length; i++)
                    {
                        linesAll[i] = lines1[i];
                        linesAll[i+ lines1.Length-1] = lines2[i];
                        linesAll[i + lines1.Length + lines2.Length-1] = lines3[i];
                    }


                    System.IO.File.WriteAllLines(folderDialog.SelectedPath + "\\" + nameReport, linesAll);

                   

                }
            }
        }
    }
}
