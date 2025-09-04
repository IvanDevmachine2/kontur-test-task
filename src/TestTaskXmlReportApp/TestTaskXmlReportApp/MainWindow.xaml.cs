using System.Windows;
using System.IO;
using TestTaskXmlReportApp.Services;
using System.Xml.Linq;
using System.Windows.Controls;
using System;

namespace TestTaskXmlReportApp
{
    public partial class MainWindow : Window
    {
        private readonly IXmlCalculatorService _calculatorService;
        private readonly IXmlDataProcessorService _dataProcessorService;

        public MainWindow()
        {
            InitializeComponent();

            _calculatorService = new XmlCalculatorService();
            _dataProcessorService = new XmlDataProcessorService();
        }

        private void CalculateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var selectedItem = SourceFileComboBox.SelectedItem as ComboBoxItem;
                if (selectedItem == null)
                {
                    MessageBox.Show("Файл для расчёта");
                    return;
                }

                string inputFileName = selectedItem.Content.ToString();

                // Работа с путями - предполагается, что в данном решении входящие xml-файлы будут находиться в каталоге
                // Data/Input рядом с исполняющим файлом программы
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                string inputXmlPath = Path.Combine(baseDirectory, "Data", "Input", inputFileName);
                string outputXmlPath = Path.Combine(baseDirectory, "Data", "Output", "Employees.xml");

                if (!File.Exists(inputXmlPath))
                {
                    MessageBox.Show($"Файл не найден: {inputXmlPath}", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                string resultPath = _calculatorService.CalculateDataToEmployees(inputXmlPath, outputXmlPath);

                _dataProcessorService.AddTotalSalaryAttribute(resultPath);

                if (inputFileName == "Data1.xml")
                {
                    _dataProcessorService.AddTotalAmountToData1(inputXmlPath);
                }

                LoadResultsToDataGrid(resultPath);

                MessageBox.Show("Преобразование завершено успешно!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddItemButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(NewNameTextBox.Text) ||
                    string.IsNullOrWhiteSpace(NewSurnameTextBox.Text) ||
                    string.IsNullOrWhiteSpace(NewAmountTextBox.Text) ||
                    NewMountComboBox.SelectedItem == null)
                {
                    MessageBox.Show("Заполните все поля для добавления записи");
                    return;
                }

                string mount = (NewMountComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString();
                if (string.IsNullOrEmpty(mount))
                {
                    MessageBox.Show("Выберите месяц");
                    return;
                }

                AddNewItemToData1(
                    NewNameTextBox.Text.Trim(),
                    NewSurnameTextBox.Text.Trim(),
                    NewAmountTextBox.Text.Trim(),
                    mount
                );

                // Очистка полей ввода новых данных о сотрудниках после нажатия на кнопку добавления данных нового сотрудника
                NewNameTextBox.Clear();
                NewSurnameTextBox.Clear();
                NewAmountTextBox.Clear();
                NewMountComboBox.SelectedIndex = 0;

                CalculateButton_Click(sender, e);

                MessageBox.Show("Запись добавлена и данные пересчитаны!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении записи: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddNewItemToData1(string name, string surname, string amount, string mount)
        {
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string data1Path = Path.Combine(baseDirectory, "Data", "Input", "Data1.xml");

            XDocument doc = XDocument.Load(data1Path);

            XElement newItem = new XElement("item",
                new XAttribute("name", name),
                new XAttribute("surname", surname),
                new XAttribute("amount", amount),
                new XAttribute("mount", mount)
            );

            doc.Root?.Add(newItem);
            doc.Save(data1Path);
        }

        private void LoadResultsToDataGrid(string xmlPath)
        {
            XDocument doc = XDocument.Load(xmlPath);

            var employees = doc.Descendants("Employee")
                .Select(e => new
                {
                    Name = e.Attribute("name")?.Value,
                    Surname = e.Attribute("surname")?.Value,
                    TotalSalary = e.Attribute("salary_sum")?.Value,
                    Salaries = string.Join("; ", e.Elements("salary")
                        .Select(s => $"{s.Attribute("mount")?.Value}: {s.Attribute("amount")?.Value}"))
                }).ToList();

            EmployeesDataGrid.ItemsSource = employees;
        }
    }
}