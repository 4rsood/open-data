using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.IO;
using System.Web.UI;
using System.Data;
using System.Data.SqlClient;

namespace pensions
{
    public class DataDictionary
    {
        public const string En = "1";
        public const string Fr = "2";

        public System.IO.Stream pTXT(string tableID, string langID, string filename)
        {
            System.Text.StringBuilder sb1 = new System.Text.StringBuilder();
            if (langID.Equals(Fr))
                processpTXTData(sb1, tableID, Fr, "fr-CA", filename);
            else
                processpTXTData(sb1, tableID, En, "en-CA", filename);
            byte[] resultBytes = Encoding.UTF8.GetBytes(sb1.ToString()); 
            return new MemoryStream(resultBytes);
        }
        
        public System.Text.StringBuilder processpTXTData(System.Text.StringBuilder sb1, string tableID, string langID, string culture, string filename)
        {
            pensionsDataContext od = new pensionsDataContext();
            string header = "";
            if (langID.Equals(Fr))
                header = "\"Attribut\",\"Définition\", \"Type de données\", \"Longueur maximale\"\n";
            else
                header = "\"Attribute\",\"Definition\", \"Data Type\", \"Max Length\"\n";

            sb1.Append(header);
            sb1.Append(System.Environment.NewLine);
            // data dictionary contents are retrieved from the database
            foreach (usp_Get_Data_Dictionary_EntryResult p in od.usp_Get_Data_Dictionary_Entry(Int16.Parse(tableID), Int16.Parse(langID)))
            {

                if (p.Attribute != null)
                {
                    sb1.Append("\"" + p.Attribute + "\"" + ',' + "\"" + p.value + "\"" + ',' + "\"" + p.DATA_TYPE + "\"" + ',' + "\"" + p.max_length + "\"");
                    sb1.Append(System.Environment.NewLine);
                }
            }
            WebOperationContext.Current.OutgoingResponse.ContentType = "text";
            WebOperationContext.Current.OutgoingResponse.Headers.Add("content-disposition", "attachment; filename=" + filename + ".txt");

            return sb1;
        }
    }
}
