namespace TestTaskXmlReportApp.Services
{
    public class XmlCalculatorService : IXmlCalculatorService
    {
        public string TransformData1ToEmployees(string inputXmlPath, string xsltPath, string outputXmlPath)
        {
            // Первое преобразование
            return outputXmlPath;
        }

        public string TransformData2ToEmployees(string inputXmlPath, string xsltPath, string outputXmlPath)
        {
            // Второе преобразование
            return outputXmlPath;
        }
    }
}
