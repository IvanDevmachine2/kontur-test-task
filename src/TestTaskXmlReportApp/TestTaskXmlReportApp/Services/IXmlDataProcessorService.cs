namespace TestTaskXmlReportApp.Services
{
    public interface IXmlDataProcessorService
    {
        void AddTotalSalaryAttribute(string xmlPath);
        void AddTotalAmountToData1(string data1Path);
    }
}