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

        private void LoadResultsToDataGrid(string xmlPath)
        {
            XDocument doc = XDocument.Load(xmlPath);

            var employees = doc.Descendants("Employee")
                .Select(e => new
                {
                    Name = e.Attribute("name")?.Value,
                    Surname = e.Attribute("surname")?.Value,
                    TotalSalary = e.Attribute("total_salary")?.Value,
                    Salaries = string.Join("; ", e.Elements("salary")
                        .Select(s => $"{s.Attribute("mount")?.Value}: {s.Attribute("amount")?.Value}"))
                }).ToList();

            EmployeesDataGrid.ItemsSource = employees;
        }
    }
}