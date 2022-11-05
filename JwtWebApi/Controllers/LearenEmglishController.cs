using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static JwtWebApi.Controllers.MessagesController;
using System.Dynamic;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics.Metrics;
using System.Xml;
using NHibernate.Impl;
using Microsoft.AspNetCore.Authorization;

namespace JwtWebApi.Controllers
{
    public class xmlHelp
    {
        public string xmlReturn  { get; set; }
    }
    [Route("/LearenEmglish/")]
    [ApiController]
    public class LearenEmglishController : ControllerBase
    {
        [HttpPost("trasformDataToXml"), Authorize(Roles = "ROLE_MANAGER")]
        public async Task<ActionResult<Object>> trasformDataToXml(List<LearenEnglsData> data)
        {
            string returnXml = CreateXml(data);
            xmlHelp xmlhelp = new xmlHelp();
            xmlhelp.xmlReturn = returnXml;
            return Ok(xmlhelp);
        }

        [HttpGet("getMessageData/{id}"), Authorize(Roles = "ROLE_MANAGER")]
        public async Task<ActionResult<List<LEDataXmlByMsg>>> trasformDataByMessage(int id)
        {
            List<LEDataXmlByMsg> converXml = ConvertXmlToDataByMesage(id);
            return Ok(converXml);
        }   

        public static List<LEDataXmlByMsg> ConvertXmlToDataByMesage(int id)
        {
            LEDataXmlByMsg xmlByMsg = new LEDataXmlByMsg();
            List<LEDataXmlByMsg> allXmlByMsgForReturn = new List<LEDataXmlByMsg>();
            string varEnV = "", varHeV = "";
            
            XmlDocument xml = new XmlDocument();
            xml.Load("C:\\XML\\React App\\"+id+".xml");

            XmlNodeList nodeList = xml.SelectNodes("//MessageData//word");
            XmlElement root = xml.DocumentElement;
            string article = root.Attributes["article"].Value;

            foreach (XmlNode node in nodeList)
            {
                XmlNode heV = node.SelectSingleNode("heV");
                XmlNode enV = node.SelectSingleNode("enV");
                varEnV = enV.InnerText;
                varHeV = heV.InnerText;
                xmlByMsg.enV = varEnV;
                xmlByMsg.heV = varHeV;
                xmlByMsg.msgId = id;
                xmlByMsg.newArticle = article;
                allXmlByMsgForReturn.Add(xmlByMsg);
                xmlByMsg = new LEDataXmlByMsg();
            }
            return allXmlByMsgForReturn;
        }

        public static string CreateXml(List<LearenEnglsData> data)
        {
            XmlDocument doc = new XmlDocument();
            int index = data.Count() -1;
                using (XmlWriter objWriter = doc.CreateNavigator().AppendChild())
                {
                    objWriter.WriteStartDocument();
                        objWriter.WriteStartElement("MessageData");
                        objWriter.WriteAttributeString("article", data[0].newArticle);
                        while (index >= 0) {
                            objWriter.WriteStartElement("word");
                                objWriter.WriteStartElement("enV");
                                objWriter.WriteValue(data[index].engilshValue);
                                objWriter.WriteEndElement();

                                objWriter.WriteStartElement("heV");
                                objWriter.WriteValue(data[index].hebrwoValue);
                                objWriter.WriteEndElement();
                            objWriter.WriteEndElement(); 
                            index--;
                        }
                        objWriter.WriteEndElement();
                    objWriter.WriteEndDocument(); 
                };
            StringWriter sw = new StringWriter();
            XmlTextWriter xw = new XmlTextWriter(sw);
            doc.WriteTo(xw);
            return sw.ToString();
        }
        public class LearenEnglsData
        {
            public string hebrwoValue { get; set; }
            public string newArticle { get; set; }
            public string engilshValue { get; set; }
        }
        public class LEDataXmlByMsg
        {
            public string heV { get; set; }
            public int msgId { get; set; }
            public string enV { get; set; }
            public string newArticle { get; set; }

        }
    }
}
