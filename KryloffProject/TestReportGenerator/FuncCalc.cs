using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public static string VDDcore1;
        public static string VDDcore2;
        public static string VDDcore3;

        public FuncCalc()
        {
            InitializeComponent();
            // Получение данных с главной формы
            ArrayString = mForm.arrayString;
            TesterSortCollections = mForm.testerSortCollections;
            TextAliases = mForm.textAliases;
            VDDcore1 = mForm.VDDcore1;
            VDDcore2 = mForm.VDDcore2;
            VDDcore3 = mForm.VDDcore3;
            ColumnAdd();
            RowAdd();
        }

        void ColumnAdd()
        {
            // Добавление столбцов в таблицу по числу алиасов и напряжений
            for (int i = 0; i < TesterSortCollections.Count; i++)
            {
                var dgvAge = new DataGridViewTextBoxColumn
                {
                    Name = "Tester",
                    HeaderText = TesterSortCollections[i][0][2] + "_" + VDDcore1,
                    Width = 100
                };

                var dgvAge2 = new DataGridViewTextBoxColumn
                {
                    Name = "Tester2",
                    HeaderText = TesterSortCollections[i][0][2] + "_" + VDDcore2,
                    Width = 100
                };
                var dgvAge3 = new DataGridViewTextBoxColumn
                {
                    Name = "Tester3",
                    HeaderText = TesterSortCollections[i][0][2] + "_" + VDDcore3,
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
                for (int j = 0; j < TesterSortCollections[i].Count; j++)
                {
                    // Текущий массив
                    string[] arrStrings = TesterSortCollections[i][j];

                    string value = "";
                    dataGridView1.Rows[j].Cells[0].Value = arrStrings[1];

                    value = Convert.ToDouble(arrStrings[4]) > Convert.ToDouble(VDDcore1) ? "1" : "0";
                    if (arrStrings[4] == "-1") value = "1";
                    dataGridView1.Rows[j].Cells[y].Value = value;

                    value = Convert.ToDouble(arrStrings[4]) > Convert.ToDouble(VDDcore2) ? "1" : "0";
                    if (arrStrings[4] == "-1") value = "1";
                    dataGridView1.Rows[j].Cells[y + 1].Value = value;

                    value = Convert.ToDouble(arrStrings[4]) > Convert.ToDouble(VDDcore3) ? "1" : "0";
                    if (arrStrings[4] == "-1") value = "1";
                    dataGridView1.Rows[j].Cells[y + 2].Value = value;
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

        private void button1_Click(object sender, EventArgs e)
        {
            // Путь для сохранения отчета по умолчанию
            string programFiles = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            using (var folderDialog = new FolderBrowserDialog())
            {
                folderDialog.SelectedPath = programFiles;
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    string nameReport = "ChipID_MPWID_LotID_Waffer " +  DateTime.Now.ToString("yyyy_MM_dd_hh.m.s")+".txt";

                    string[] lines = new string[dataGridView1.RowCount];

                    for (int i = 0; i < dataGridView1.RowCount; i++)
                    {
                        lines[i] = "";
                        for (int j = 0; j < dataGridView1.ColumnCount; j++)
                        {
                            lines[i] += dataGridView1.Rows[i].Cells[j].Value + "\t";
                        }
                    }

                    System.IO.File.WriteAllLines(folderDialog.SelectedPath+"\\"+ nameReport, lines);

                }
            }
        }
    }
}
