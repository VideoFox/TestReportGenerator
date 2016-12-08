﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
// ReSharper disable All

namespace TestReportGenerator
{
   public class ClassLibrary
    {
        /// <summary>
        /// Создание файла настроек (если вдруг нет)
        /// </summary>
        /// <param name="xmlPath">Путь к xml файлу</param>
        /// <param name="mainNode">Наименование родительской Ноды</param>
        public static void createXML(string xmlPath, string mainNode)
        {
            if (File.Exists(xmlPath)) return;
            XmlDocument xmlDoc = new XmlDocument();
            XmlDeclaration xmlDecl = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
            XmlElement entriesElement = xmlDoc.CreateElement(mainNode);
            xmlDoc.AppendChild(entriesElement);
            xmlDoc.Save(xmlPath);
        }
        /// <summary>
        /// Метод записи в XML файл
        /// </summary>
        /// <param name="xmlPath">XML файл</param>
        /// <param name="mainNodeName">Главная нода(специальность)</param>
        /// <param name="parentNodeName">Нода программы</param>
        /// <param name="entries">Массив со значениями</param>
        public static void WriteXML(string xmlPath, string mainNodeName,
                                    string parentNodeName, XMLrec[] entries)
        {
            XmlDocument xmlDoc = new XmlDocument();

            if (File.Exists(xmlPath))
            {
                xmlDoc.Load(xmlPath);
            }
            else
            {
                createXML(xmlPath, mainNodeName);
            }

            // Родительская нода
            XmlNode parentXml = xmlDoc.SelectSingleNode("/" + mainNodeName);

            // Раздел
            XmlNode parentEntries;

            // Проверка на наличие раздела программы и создание , если нет
            if (xmlDoc.SelectSingleNode(mainNodeName + "/" + parentNodeName) == null)
            {
                parentEntries = xmlDoc.CreateElement(parentNodeName);
                parentXml.AppendChild(parentEntries);
            }
            else
            {
                parentEntries = xmlDoc.SelectSingleNode(mainNodeName + "/" + parentNodeName);
            }

            // Разделы из массива
            foreach (XMLrec item in entries)
            {
                XmlNode parentNode = xmlDoc.SelectSingleNode(mainNodeName + "/" + parentNodeName + "/" +
                                                             item.nodePath + item.nameElement);
                XmlNode nodePath;
                if (item.nodePath != "")
                {
                    nodePath = xmlDoc.SelectSingleNode(mainNodeName + "/" + parentNodeName + "/"
                                                             + item.nodePath);
                }
                else
                {
                    nodePath = parentEntries;
                }

                if (parentNode == null)
                {
                    XmlNode entr = xmlDoc.CreateElement(item.nameElement);
                    // Запись значения Ноды
                    entr.InnerText = item.textElement;
                    // Запись атрибута
                    int y = 0;
                    if (item.attrElement != null)
                    {
                        foreach (var attrib in item.attrElement)
                        {
                            XmlAttribute xmlAttr = item.attrElement[y];
                            XmlNode attr = xmlDoc.CreateNode(XmlNodeType.Attribute, xmlAttr.Name, null);
                            attr.Value = xmlAttr.Value;
                            XmlNode root = entr;
                            root.Attributes.SetNamedItem(attr);
                            y++;
                        }
                    }
                    nodePath.AppendChild(entr);
                }
                else
                {
                    XmlNode entr = xmlDoc.SelectSingleNode(mainNodeName + "/" + parentNodeName + "/" +
                                   item.nodePath + item.nameElement);
                    // Запись атрибута
                    int y = 0;
                    if (item.attrElement != null)
                    {
                        foreach (var attrib in item.attrElement)
                        {
                            XmlAttribute xmlAttr = item.attrElement[y];
                            XmlNode attr = xmlDoc.CreateNode(XmlNodeType.Attribute, xmlAttr.Name, null);
                            attr.Value = xmlAttr.Value;
                            XmlNode root = entr;
                            root.Attributes.SetNamedItem(attr);
                            y++;
                        }
                    }
                    // Запись значения Ноды
                    entr.InnerText = item.textElement;
                }
            }
            // Сохранение xml
            xmlDoc.Save(xmlPath);
        }
        /// <summary>
        /// Метод чтения XML . Возвращает массив со значениями 
        /// </summary>
        /// <param name="xmlPath">XML файл</param>
        /// <param name="mainNodeName">Главная нода(специальность)</param>
        /// <param name="parentNodeName">Нода программы</param>
        public static List<XMLrec> ReadXML(string xmlPath, string mainNodeName, string parentNodeName)
        {
            // Список данных на вывод
            List<XMLrec> entries = new List<XMLrec>();

            XmlDocument xmlDoc = new XmlDocument();
            // Проверка на наличие XML и считывание в случае его наличия
            if (File.Exists(xmlPath))
            {
                xmlDoc.Load(xmlPath);
            }
            else
            {
                return entries = null;
            }
            // Главная нода для текущей программы
            XmlNode parentNode = xmlDoc.SelectSingleNode(mainNodeName + "/" + parentNodeName);
            // Если нет записи программы
            if (parentNode == null)
            {
                return entries = null;
            }

            int i = 0;
            // Считывание тегов в разделе программы
            foreach (XmlNode childnode in parentNode.ChildNodes)
            {
                for (int g = 0; g < childnode.ChildNodes.Count; g++)
                {
                    XMLrec ent = new XMLrec("", childnode.ChildNodes[g].Name,
                                                childnode.ChildNodes[g].InnerText,
                                                childnode.ChildNodes[g].Attributes);
                    entries.Add(ent);
                    i++;
                }
            }
            // Возврат массива значений
            return entries;
        }
    }
    public class XMLrec
    {
        public string nameElement;
        public string textElement;
        public XmlAttributeCollection attrElement;
        public string nodePath;
        public XMLrec(string nodePath, string nameElement, string textElement, XmlAttributeCollection attrElement)
        {
            this.nameElement = nameElement;
            this.textElement = textElement;
            this.attrElement = attrElement;
            this.nodePath = nodePath;
        }
    }
    public class GroupByGrid : DataGridView
    {
        protected override void OnCellFormatting(DataGridViewCellFormattingEventArgs args)
        {
            base.OnCellFormatting(args);
            if (args.RowIndex == 0)
                return;

            if (IsRepeatedCellValue(args.RowIndex, args.ColumnIndex))
            {
                args.Value = string.Empty;
                args.FormattingApplied = true;
            }
        }
        private bool IsRepeatedCellValue(int rowIndex, int colIndex)
        {
            DataGridViewCell currCell = Rows[rowIndex].Cells[colIndex];
            DataGridViewCell prevCell = Rows[rowIndex - 1].Cells[colIndex];
            return ((currCell.Value == prevCell.Value) || (currCell.Value != null && prevCell.Value != null &&
                currCell.Value.ToString() == prevCell.Value.ToString()));
        }
        protected override void OnCellPainting(DataGridViewCellPaintingEventArgs args)
        {
            base.OnCellPainting(args);
            args.AdvancedBorderStyle.Bottom = DataGridViewAdvancedCellBorderStyle.None;
            if (args.RowIndex < 1 || args.ColumnIndex < 0)
                return;
            if (IsRepeatedCellValue(args.RowIndex, args.ColumnIndex))
                args.AdvancedBorderStyle.Top = DataGridViewAdvancedCellBorderStyle.None;
            else
                args.AdvancedBorderStyle.Top = AdvancedCellBorderStyle.Top;
        }
    }
    public static class FileService
    {
        /// <summary>
        /// Копирование файлов шаблона
        /// </summary>
        /// <param name="pathFolder">Папка с шаблонами</param>
        /// <returns>Возвращает путь к новой папке</returns>
        public static string CopyTemplate(string pathFolder)
        {
            // Список файлов отчета
            string[] fileNames = { "title.tex", "config.tex", "legends_and_colors.tex", "main.tex", "preamble.tex" };
            // Путь для новой папки
            string nameNewFolder = pathFolder + "\\" + DateTime.Now.ToString("yyyy_MM_dd_hh.m.s");
            Directory.CreateDirectory(nameNewFolder);
            // Копирование файлов отчета в новую папку
            foreach (var item in fileNames)
            {
                string sourceFile = Path.Combine(Application.StartupPath + "\\tex_templates", item);
                string destFile = Path.Combine(nameNewFolder, item);
                File.Copy(sourceFile, destFile, true);
            }
            return nameNewFolder;
        }
        /// <summary>
        /// Считывание Тех и Мос
        /// </summary>
        /// <param name="nameNewFolder">Путь к папке</param>
        /// <param name="techMosString">Тип процесса</param>
        /// <param name="strFromTitle">Строка из Тайтла</param>
        /// <returns>Возвращает считанный тип процесса</returns>
        public static string TechMosRead(string nameNewFolder, string techMosString, string strFromTitle)
        {
            // Считывание шаблонов TECH и MOS
            StreamReader reader = new StreamReader(nameNewFolder + "\\legends_and_colors.tex");
            string content = reader.ReadToEnd();
            reader.Close();

            string strTech = content.Substring(content.IndexOf("%"+ techMosString) + techMosString.Length+1, 
                                               content.IndexOf("%END"+ techMosString) - techMosString.Length + 1);
            strTech = Regex.Replace(strTech, @"\r\n", "");
            strTech = Regex.Replace(strTech, @"%", "");
            string[] arrayStringTech = strTech.Split(' ');

            bool flagTech = false;
            string techString = "";
            foreach (string item in arrayStringTech)
            {
                if (item != "" && strFromTitle.IndexOf(item) != -1)
                {
                    flagTech = true;
                    techString = item;
                }
            }

            if (!flagTech)
            {
                MessageBox.Show(techMosString+@" не определен! Проверьте входные данные!");
                techString = "???";
            }
            return techString;
        }
    }
}
