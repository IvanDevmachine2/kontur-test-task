using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTaskXmlReportApp.Services
{
    public interface IXmlCalculatorService
    {
        string TransformData1ToEmployees(string inputXmlPath, string xsltPath, string outputXmlPath);
        string TransformData2ToEmployees(string inputXmlPath, string xsltPath, string outputXmlPath);
    }
}
