using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using CsvHelper;
using System.IO;
using CsvHelper.Configuration;
using System.Globalization;

namespace FileConverter
{
    //Represents XML file
    public class XMLFile
    {
        //Represents XML content
        public List<UDO_XmlNode> Content;

        public XMLFile()
        {
            this.Content = new List<UDO_XmlNode>();
        }

        /* Load .xml file into class instance.
         * Parameters:
         *   fileName — File name with .xml extension
         */
        public void LoadFile(string fileName)
        {
            XmlDocument document = new XmlDocument();

            document.Load(fileName);

            XmlElement rootELement = document.DocumentElement;

            if(rootELement != null)
            {
                this.GetXmlNode(rootELement, 1);
            }
        }

        /* Recursively passe the file
         *   and write it`s content into current instance.
         * Parameters:
         *   node — Every single node of xml file
         *   level — Nesting level of node, starting from root node(level 1)
         */
        public void GetXmlNode(XmlNode node, int level)
        {
            UDO_XmlNode udoNode = new UDO_XmlNode();
            udoNode.Level = level;
            udoNode.Name = node.Name;
            this.Content.Add(udoNode);

            if (node.HasChildNodes == true)
            {
                if (!(node.FirstChild is XmlElement))
                {
                    udoNode.NodeValue = node.InnerText;
                }
                else
                {
                    foreach (XmlElement e in node.ChildNodes)
                    {
                        this.GetXmlNode(e, level + 1);
                    }
                }
            }

            foreach (XmlAttribute attribute in node.Attributes)
            {
                udoNode.Attributes[attribute.Name] = attribute.Value;
            }
        }

        /* Convert XML to CSV.
         * Create new CSV file where every line contains one node from source XML file. 
         * Parameters:
         *   fileName — File name without .csv extension
         *   printAttributes — Determines whether to print node attributes to CSV.
         *     If true, every line in output file will have following format: nodeName,nodeValue[,attribute1Name,attribute1Value,...]
         *     If false, every line in output file will have followind format: noveName,nodeValue
         *   printHeader — Determines whether to print header line to CSV.
         *     If true, first row in CSV will contain header.
         *     If false, CSV will contain only data, starting from first row.
         */
        public void XMLToCSVConversion(string fileName, bool printAttributes = false, bool printHeader = false)
        {
            using (StreamWriter writer = new StreamWriter(fileName + ".csv"))
            using (CsvWriter csvWriter = new CsvWriter(writer, new CsvConfiguration(CultureInfo.CurrentCulture){Encoding = Encoding.UTF8, Delimiter = DataManage.separator.ToString()}))
            {
                if(printHeader == true)
                {
                    csvWriter.WriteField("NodeName");
                    csvWriter.WriteField("NodeValue");
                    csvWriter.WriteField("Attributes");
                    csvWriter.NextRecord();
                }

                foreach(UDO_XmlNode node in this.Content)
                {
                    csvWriter.WriteField(node.Name);
                    if(node.NodeValue != String.Empty)
                    {
                        csvWriter.WriteField(node.NodeValue);
                    }
                    
                    if(printAttributes == true)
                    {
                        foreach (KeyValuePair<string, string> attribute in node.Attributes)
                        {
                            csvWriter.WriteField(attribute.Key);
                            csvWriter.WriteField(attribute.Value);
                        }
                    }

                    csvWriter.NextRecord();
                }
            }
        }
    }
}
