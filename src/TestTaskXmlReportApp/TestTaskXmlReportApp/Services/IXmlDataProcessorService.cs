namespace TestTaskXmlReportApp.Services
{
    /// <summary>
    /// Сервис для добавления атрибутов в XML-файлы
    /// </summary>
    public interface IXmlDataProcessorService
    {
        /// <summary>
        /// Добавление в группы Employee итогового Employees.xml атрибута с суммой заработных плат
        /// </summary>
        void AddTotalSalaryAttribute(string xmlPath);

        /// <summary>
        /// Добавление в исходный файл Data1.xml атрибут с указанием суммы всех указанных заработных плат
        /// </summary>
        void AddTotalAmountToData1(string data1Path);
    }
}