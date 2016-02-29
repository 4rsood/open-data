using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading;
using System.Xml;
using System.Xml.Linq;

namespace pensions.Service
{
    public class Service1 : IService1
    {
        public const string En = "1";
        public const string Fr = "2";
        CultureInfo cultureEn = CultureInfo.CreateSpecificCulture("en-CA");
        CultureInfo cultureFr = CultureInfo.CreateSpecificCulture("fr-CA");
                
        ////////////////////////////////////////////////////
        // Statistical Table 1.1. Pensions in Pay         //
        ///////////////////////////////////////////////////

        public XmlDocument p11XMLEn(string pensionYear)
        {
            return processp11Data(pensionYear, En);
        }

        public XmlDocument p11XMLFr(string pensionYear)
        {
            return processp11Data(pensionYear, Fr);
        }

        public XmlDocument processp11Data(string pensionYear, string langID)
        {
            XmlNode rootNode;
            XmlNode node;
            String _topLevelNode = String.Empty;

            if (langID.Equals(Fr))
            {
                Thread.CurrentThread.CurrentCulture = cultureFr;
                Thread.CurrentThread.CurrentUICulture = cultureFr;
            }
            else
            {
                Thread.CurrentThread.CurrentCulture = cultureEn;
                Thread.CurrentThread.CurrentUICulture = cultureEn;
            }

            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);

            XmlNode topLevelNode = doc.CreateElement(Resources.Resource1.topLevelNode);
            doc.AppendChild(topLevelNode);

            pensionsDataContext od = new pensionsDataContext();
            // make a call to the stored proceduare in the database
            // and populate the xml nodes 
            // the XML element names are defined in the resource files, e.g.,Resources.Resource1.year
            foreach (usp_Get_Pension_NumResult p in od.usp_Get_Pension_Num(Int16.Parse(pensionYear), Int16.Parse(langID)))
            {
                rootNode = doc.CreateElement(Resources.Resource1.rootNode);
                topLevelNode.AppendChild(rootNode);
                
                node = doc.CreateElement(Resources.Resource1.year);
                node.AppendChild(doc.CreateTextNode(p.Pension_Year.ToString()));
                rootNode.AppendChild(node);

                node = doc.CreateElement(Resources.Resource1.pensions);
                node.AppendChild(doc.CreateTextNode(p.Pension_Number.ToString()));
                rootNode.AppendChild(node);

                node = doc.CreateElement(Resources.Resource1.survivorPensions);
                node.AppendChild(doc.CreateTextNode(p.Pension_Number.ToString()));
                rootNode.AppendChild(node);

                node = doc.CreateElement(Resources.Resource1.total);
                node.AppendChild(doc.CreateTextNode(p.Pension_Number.ToString()));
                rootNode.AppendChild(node);
            }

            return doc;
        }

     
        public System.IO.Stream p11CSV(string pensionYear, string langID)
        {
            System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
            CultureInfo culture;
            
            if (langID.Equals(Fr))
            {
                Thread.CurrentThread.CurrentCulture = cultureFr;
                Thread.CurrentThread.CurrentUICulture = cultureFr;
                culture = cultureFr;
            }
            else
            {
                Thread.CurrentThread.CurrentCulture = cultureEn;
                Thread.CurrentThread.CurrentUICulture = cultureEn;
                culture = cultureEn;
            }

            processp11CSVData(sb1, pensionYear, langID, culture);

            byte[] resultBytes = Encoding.UTF8.GetBytes(sb1.ToString());
            return new MemoryStream(resultBytes);
        }

        public System.IO.Stream p11TXT(string tableID, string langID)
        {
            DataDictionary dd = new DataDictionary();

            if (langID.Equals(Fr))
            {
                Thread.CurrentThread.CurrentCulture = cultureFr;
                Thread.CurrentThread.CurrentUICulture = cultureFr;
            }
            else
            {
                Thread.CurrentThread.CurrentCulture = cultureEn;
                Thread.CurrentThread.CurrentUICulture = cultureEn;
            } 
            return (dd.pTXT(tableID, langID, Resources.Resource1.filename11));
        }


        public System.Text.StringBuilder processp11CSVData(System.Text.StringBuilder sb1, string pensionYear, string langID, CultureInfo culture)
        {
            pensionsDataContext od = new pensionsDataContext();
            sb1.Append(Resources.Resource1.header);
            
            sb1.Append(System.Environment.NewLine);
            foreach (usp_Get_Pension_NumResult p in od.usp_Get_Pension_Num(Int16.Parse(pensionYear), Int16.Parse(langID)))
            {
                sb1.Append(p.Pension_Year);
                sb1.Append(",");
                sb1.Append(p.Pension_Number);
                sb1.Append(",");
                sb1.Append(p.Survivor_Pension_Number);
                sb1.Append(",");
                sb1.Append(p.Total_Number);
                sb1.Append(System.Environment.NewLine);
            }

            WebOperationContext.Current.OutgoingResponse.ContentType = "text/csv";
            WebOperationContext.Current.OutgoingResponse.Headers.Add("content-disposition", "attachment; filename=" + Resources.Resource1.filename11 + ".csv");
            return sb1;
        }


        public XmlDocument p12XMLEn(string pensionYear)
        {
            return processp12Data(pensionYear, En); 

        }
        public XmlDocument p12XMLFr(string pensionYear)
        {
            return processp12Data(pensionYear, Fr); 
        }

        public XmlDocument processp12Data(string pensionYear, string langID)
        {
            XmlNode rootNode;
            XmlNode node;
            String _topLevelNode = String.Empty;

            if (langID.Equals(Fr))
            {
                Thread.CurrentThread.CurrentCulture = cultureFr;
                Thread.CurrentThread.CurrentUICulture = cultureFr;
            }
            else
            {
                Thread.CurrentThread.CurrentCulture = cultureEn;
                Thread.CurrentThread.CurrentUICulture = cultureEn;
            }

            XmlDocument doc = new XmlDocument();
            XmlNode docNode = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(docNode);

            XmlNode topLevelNode = doc.CreateElement(Resources.Resource1.topLevelNode);
            doc.AppendChild(topLevelNode);

            pensionsDataContext od = new pensionsDataContext();

            foreach (usp_Get_Pension_AverageResult p in od.usp_Get_Pension_Average(Int16.Parse(pensionYear)))
            {
                rootNode = doc.CreateElement(Resources.Resource1.rootNode2);
                topLevelNode.AppendChild(rootNode);

                node = doc.CreateElement(Resources.Resource1.year);
                node.AppendChild(doc.CreateTextNode(p.Pension_Year.ToString()));
                rootNode.AppendChild(node);

                node = doc.CreateElement(Resources.Resource1.averageAnnualAmountMen);
                node.AppendChild(doc.CreateTextNode(p.Men_Average_Amount.ToString()));
                rootNode.AppendChild(node);

                node = doc.CreateElement(Resources.Resource1.averageAnnualAmountWomen);
                node.AppendChild(doc.CreateTextNode(p.Women_Average_Amount.ToString()));
                rootNode.AppendChild(node);

                node = doc.CreateElement(Resources.Resource1.averageAnnualAmountTotal);
                node.AppendChild(doc.CreateTextNode(p.Total_Average_Amount.ToString()));
                rootNode.AppendChild(node);

                node = doc.CreateElement(Resources.Resource1.averageAnnualAmountSpouseCommonLawPartner);
                node.AppendChild(doc.CreateTextNode(p.Spouse_Average_Amount.ToString()));
                rootNode.AppendChild(node);

                node = doc.CreateElement(Resources.Resource1.averageAnnualAmountChildren);
                node.AppendChild(doc.CreateTextNode(p.Children_Average_Amount.ToString()));
                rootNode.AppendChild(node);

                node = doc.CreateElement(Resources.Resource1.averageAnnualAmountStudents);
                node.AppendChild(doc.CreateTextNode(p.Student_Average_Amount.ToString()));
                rootNode.AppendChild(node);

                node = doc.CreateElement(Resources.Resource1.averageAgeMen);
                node.AppendChild(doc.CreateTextNode(p.Men_Age_Average.ToString()));
                rootNode.AppendChild(node);

                node = doc.CreateElement(Resources.Resource1.averageAgeWomen);
                node.AppendChild(doc.CreateTextNode(p.Women_Age_Average.ToString()));
                rootNode.AppendChild(node);

                node = doc.CreateElement(Resources.Resource1.averageAgeTotal);
                node.AppendChild(doc.CreateTextNode(p.Total_Age_Average.ToString()));
                rootNode.AppendChild(node);

                node = doc.CreateElement(Resources.Resource1.averageAgeSpouseCommonLawPartner);
                node.AppendChild(doc.CreateTextNode(p.Spouse_Age_Average.ToString()));
                rootNode.AppendChild(node);

                node = doc.CreateElement(Resources.Resource1.averageAgeChildren);
                node.AppendChild(doc.CreateTextNode(Resources.Resource1.na));
                rootNode.AppendChild(node);

                node = doc.CreateElement(Resources.Resource1.averageAgeStudents);
                node.AppendChild(doc.CreateTextNode(Resources.Resource1.na));
                rootNode.AppendChild(node);

                node = doc.CreateElement(Resources.Resource1.averagePensionableServiceYearsMen);
                node.AppendChild(doc.CreateTextNode(p.Men_Service_Year_Average.ToString()));
                rootNode.AppendChild(node);

                node = doc.CreateElement(Resources.Resource1.averagePensionableServiceYearsWomen);
                node.AppendChild(doc.CreateTextNode(p.Women_Service_Year_Average.ToString()));
                rootNode.AppendChild(node);

                node = doc.CreateElement(Resources.Resource1.averagePensionableServiceYearsTotal);
                node.AppendChild(doc.CreateTextNode(p.Total_Service_Year_Average.ToString()));
                rootNode.AppendChild(node);

                node = doc.CreateElement(Resources.Resource1.averagePensionableServiceSpouseCommonLawPartner);
                node.AppendChild(doc.CreateTextNode(p.Spouse_Service_Year_Average.ToString()));
                rootNode.AppendChild(node);

                node = doc.CreateElement(Resources.Resource1.averagePensionableServiceChildren);
                node.AppendChild(doc.CreateTextNode(Resources.Resource1.na));
                rootNode.AppendChild(node);

                node = doc.CreateElement(Resources.Resource1.averagePensionableServiceStudents);
                node.AppendChild(doc.CreateTextNode(Resources.Resource1.na));
                rootNode.AppendChild(node);
            }

            return doc;
        }

        public List<AverageAnnualAmountEn> processp12DataEn(List<AverageAnnualAmountEn> results, string pensionYear)
        {
            pensionsDataContext od = new pensionsDataContext();
            var culture = System.Globalization.CultureInfo.GetCultureInfo("fr-CA");
            String.Format(culture, "{0:0.0}", 4.3);

            foreach (usp_Get_Pension_AverageResult p in od.usp_Get_Pension_Average(Int16.Parse(pensionYear)))
                {
                    results.Add(new AverageAnnualAmountEn()
                    {
                        pensionyearEn = p.Pension_Year.ToString(),
                        pensionsMenEn = p.Men_Average_Amount,
                        pensionsWomenEn = p.Women_Average_Amount,
                        pensionsTotalEn = p.Total_Average_Amount,
                        pensionsSpouseEn = p.Spouse_Average_Amount,
                        pensionsChildrenEn = p.Children_Average_Amount,
                        pensionsStudentsEn = p.Student_Average_Amount,
                        averageAgeMenEn = p.Men_Age_Average.ToString(),
                        averageAgeWomenEn = p.Women_Age_Average,
                        averageAgeTotalEn = p.Total_Age_Average,
                        averageAgeSpouseEn = p.Spouse_Age_Average,
                        averageAgeChildrenEn = "N/A",
                        averageAgeStudentsEn = "N/A",
                        averageServiceMenEn = p.Men_Service_Year_Average,
                        averageServiceWomenEn = p.Women_Service_Year_Average,
                        averageServiceTotalEn = p.Total_Service_Year_Average,
                        averageServiceSpouseEn = p.Spouse_Service_Year_Average,
                        averageServiceChildrenEn = "N/A",
                        averageServiceStudentsEn = "N/A"
                    });
                }
            
            return results;
        }

        public List<AverageAnnualAmountFr> processp12DataFr(List<AverageAnnualAmountFr> results, string pensionYear)
        {

            CultureInfo culture;
            Thread.CurrentThread.CurrentCulture = cultureFr;
            Thread.CurrentThread.CurrentUICulture = cultureFr;
            culture = cultureFr;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("fr-CA");
            CultureInfo cf = new CultureInfo("fr-CA");

            pensionsDataContext od = new pensionsDataContext();

            foreach (usp_Get_Pension_AverageResult p in od.usp_Get_Pension_Average(Int16.Parse(pensionYear)))
                {                
                results.Add(new AverageAnnualAmountFr()
                    {
                        pensionyearFr = p.Pension_Year.ToString(),
                        pensionsMenFr = p.Men_Average_Amount,
                        pensionsWomenFr = p.Women_Average_Amount,
                        pensionsTotalFr = p.Total_Average_Amount,
                        pensionsSpouseFr = p.Spouse_Average_Amount,
                        pensionsChildrenFr = p.Children_Average_Amount,
                        pensionsStudentsFr = p.Student_Average_Amount,
                        averageAgeMenFr = String.Format("{0:0.0}", p.Men_Age_Average, cultureFr),
                        averageAgeWomenFr = String.Format("{0:0.0}", p.Women_Age_Average, cultureFr),
                        averageAgeTotalFr = String.Format("{0:0.0}", p.Total_Age_Average, cultureFr),
                        averageAgeSpouseFr = String.Format("{0:0.0}", p.Spouse_Age_Average, cultureFr),
                        averageAgeChildrenFr = "s/o",
                        averageAgeStudentsFr = "s/o",
                        averageServiceMenFr = String.Format("{0:0.0}", p.Men_Service_Year_Average, cultureFr),
                        averageServiceWomenFr = String.Format("{0:0.0}", p.Women_Service_Year_Average, cultureFr),
                        averageServiceTotalFr = String.Format("{0:0.0}", p.Total_Service_Year_Average, cultureFr),
                        averageServiceSpouseFr = String.Format("{0:0.0}", p.Spouse_Service_Year_Average, cultureFr),
                        averageServiceChildrenFr = "s/o",
                        averageServiceStudentsFr = "s/o"
                    });
                }
            return results;
        }

        public System.IO.Stream p12CSV(string pensionYear, string langID)
        {
            System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
            CultureInfo culture;

            if (langID.Equals(Fr))
            {
                Thread.CurrentThread.CurrentCulture = cultureFr;
                Thread.CurrentThread.CurrentUICulture = cultureFr;
                culture = cultureFr;
            }
            else
            {
                Thread.CurrentThread.CurrentCulture = cultureEn;
                Thread.CurrentThread.CurrentUICulture = cultureEn;
                culture = cultureEn;
            }
            sb1.Append(Resources.Resource1.header123);
            sb1.Append(System.Environment.NewLine);
            processp12CSVData(sb1, pensionYear, langID, culture);
            byte[] resultBytes = Encoding.UTF8.GetBytes(sb1.ToString());
            return new MemoryStream(resultBytes);
        }


        public System.Text.StringBuilder processp12CSVData(System.Text.StringBuilder sb1, string pensionYear, string langID, CultureInfo culture)
        {
            pensionsDataContext od = new pensionsDataContext();

            foreach (usp_Get_Pension_AverageResult p in od.usp_Get_Pension_Average(Int16.Parse(pensionYear)))
            {
                sb1.Append(p.Pension_Year);
                sb1.Append(",");
                
                sb1.Append(Resources.Resource1.averageAnnualAmount);
                sb1.Append(",");

                sb1.Append(String.Format("\"{0}\"", p.Men_Average_Amount, culture));
                sb1.Append(",");
                sb1.Append(String.Format("\"{0}\"", p.Women_Average_Amount, culture));
                sb1.Append(",");
                sb1.Append(String.Format("\"{0}\"", p.Total_Average_Amount, culture));
                sb1.Append(",");
                sb1.Append(String.Format("\"{0}\"", p.Spouse_Average_Amount, culture));
                sb1.Append(",");
                sb1.Append(String.Format("\"{0}\"", p.Children_Average_Amount, culture));
                sb1.Append(",");
                sb1.Append(String.Format("\"{0}\"", p.Student_Average_Amount, culture));
                sb1.Append(System.Environment.NewLine);
                sb1.Append(",");
                sb1.Append(Resources.Resource1.averageAge);
                sb1.Append(",");
                sb1.Append(String.Format("\"{0:0.00}\"", p.Men_Age_Average, culture));
                sb1.Append(",");
                sb1.Append(String.Format("\"{0:0.00}\"", p.Women_Age_Average, culture));
                sb1.Append(",");
                sb1.Append(String.Format("\"{0:0.00}\"", p.Total_Age_Average, culture));
                sb1.Append(",");
                sb1.Append(String.Format("\"{0:0.00}\"", p.Spouse_Age_Average, culture));
                sb1.Append(",");
                sb1.Append(Resources.Resource1.na);
                sb1.Append(",");
                sb1.Append(Resources.Resource1.na);
                sb1.Append(System.Environment.NewLine);
               
                sb1.Append(",");               
                sb1.Append(Resources.Resource1.averagePensionableService);
                sb1.Append(",");
                sb1.Append(String.Format("\"{0:0.00}\"", p.Men_Service_Year_Average, culture));
                sb1.Append(",");
                sb1.Append(String.Format("\"{0:0.00}\"", p.Women_Service_Year_Average, culture));
                sb1.Append(",");
                sb1.Append(String.Format("\"{0:0.00}\"", p.Total_Service_Year_Average, culture));
                sb1.Append(",");
                sb1.Append(String.Format("\"{0:0.00}\"", p.Spouse_Service_Year_Average, culture));
                sb1.Append(",");
                sb1.Append(Resources.Resource1.na);
                sb1.Append(",");
                sb1.Append(Resources.Resource1.na);
                sb1.Append(System.Environment.NewLine);
            }

            WebOperationContext.Current.OutgoingResponse.ContentType = "text/csv";
            WebOperationContext.Current.OutgoingResponse.Headers.Add("content-disposition", "attachment; filename=" + Resources.Resource1.filename12 + ".csv");
            return sb1;
        }

        public System.IO.Stream p12TXT(string tableID, string langID)
        {
            DataDictionary dd = new DataDictionary();
 
            if (langID.Equals(Fr))
            {
                Thread.CurrentThread.CurrentCulture = cultureFr;
                Thread.CurrentThread.CurrentUICulture = cultureFr;
            }
            else
            {
                Thread.CurrentThread.CurrentCulture = cultureEn;
                Thread.CurrentThread.CurrentUICulture = cultureEn;
            } 
            return (dd.pTXT(tableID, langID, Resources.Resource1.filename12));
        }


        public System.IO.Stream pTestCSV(string pensionYear, string langID)
        {
            System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
            CultureInfo culture;

            if (langID.Equals(Fr))
            {
                Thread.CurrentThread.CurrentCulture = cultureFr;
                Thread.CurrentThread.CurrentUICulture = cultureFr;
                culture = cultureFr;
            }
            else
            {
                Thread.CurrentThread.CurrentCulture = cultureEn;
                Thread.CurrentThread.CurrentUICulture = cultureEn;
                culture = cultureEn;
            }

            processp11CSVData(sb1, pensionYear, langID, culture);

            byte[] resultBytes = Encoding.UTF8.GetBytes(sb1.ToString());
            return new MemoryStream(resultBytes);
        }










    }    
}
