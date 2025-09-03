using System.Xml.Xsl;
using System.IO;

namespace TestTaskXmlReportApp.Services
{
    public class XmlCalculatorService : IXmlCalculatorService
    {
        public string CalculateDataToEmployees(string inputXmlPath, string outputXmlPath)
        {
            string xsltPath = DetermineXsltPath(inputXmlPath);

            XslCompiledTransform xslt = new XslCompiledTransform();

            xslt.Load(xsltPath);

            string outputDirectory = Path.GetDirectoryName(outputXmlPath);

            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            xslt.Transform(inputXmlPath, outputXmlPath);

            return outputXmlPath;
        }

        /// <summary>
        /// Выбор подходящего xslt по названию переданного файла
        /// </summary>
        private string DetermineXsltPath(string inputXmlPath)
        {
            string fileName = Path.GetFileName(inputXmlPath).ToLower();

            return fileName switch
            {
                "Data1.xml" => "Data/Transformers/Data1ToEmployees.xslt",
                "Data2.xml" => "Data/Transformers/Data2ToEmployees.xslt",
                _ => throw new ArgumentException($"Невалидное название файла {fileName}")
            };
        }
    }
}