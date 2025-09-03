using System.Xml.Linq;
using System.Globalization;

namespace TestTaskXmlReportApp.Services
{
    public class XmlDataProcessorService : IXmlDataProcessorService
    {
        public void AddTotalSalaryAttribute(string xmlPath)
        {
            XDocument doc = XDocument.Load(xmlPath);

            foreach (var employeeElem in doc.Descendants("Employee"))
            {
                decimal salarySum = 0;
                foreach (var salaryElem in employeeElem.Elements("salary"))
                {
                    string amountValue = salaryElem.Attribute("amount")?.Value;
                    if (decimal.TryParse(amountValue, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal amount))
                    {
                        salarySum += amount;
                    }
                }

                employeeElem.SetAttributeValue("salary_sum", salarySum.ToString(CultureInfo.InvariantCulture));
            }

            doc.Save(xmlPath);
        }

        public void AddTotalAmountToData1(string data1Path)
        {
            XDocument doc = XDocument.Load(data1Path);

            decimal amountSum = 0;
            foreach (var itemElem in doc.Descendants("item"))
            {
                string amountValue = itemElem.Attribute("amount")?.Value;
                if (decimal.TryParse(amountValue, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal amount))
                {
                    amountSum += amount;
                }
            }

            doc.Root?.SetAttributeValue("amount_sum", amountSum.ToString(CultureInfo.InvariantCulture));

            doc.Save(data1Path);
        }
    }
}