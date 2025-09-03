namespace TestTaskXmlReportApp.Services
{
    /// <summary>
    /// Сервис для выполнения XSLT-преобразований данных о сотрудниках
    /// </summary>
    public interface IXmlCalculatorService
    {
        /// <summary>
        /// Преобразует переданный XML-файл в структуру Employees.xml, автоматически определяя нужный XSLT-преобразователь
        /// </summary>
        /// <param name="inputXmlPath">Путь к исходному XML-файлу (Data1.xml или Data2.xml)</param>
        /// <param name="outputXmlPath">Путь для сохранения результата - файла Employees.xml</param>
        /// <returns>Путь к созданному Employees.xml</returns>
        string CalculateDataToEmployees(string inputXmlPath, string outputXmlPath);
    }
}