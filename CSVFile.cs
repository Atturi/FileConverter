using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;


namespace FileConverter
{
    //Represents CSV file
    public class CSVFile
    {
        //Represents CSV content
        public List<string[]> Content;

        public CSVFile()
        {
            this.Content = new List<string[]>();
        }

        /* Load CSV file into class instance.
         * Parameters:
         *   reader — stream connected with CSV file
         */
        public void LoadFile(StreamReader reader)
        {
            while(!reader.EndOfStream)
            {
                this.Content.Add(this.SplitLine(reader.ReadLine()));
            }
        }

        /* Split line from CSV file into values by delimiter.
         * Parameters:
         *   line — line from CSV file
         * Out value:
         *   array of values from line
         */
        private string[] SplitLine(string line)
        {
            if (line[0] == '\"')
            {
                return line.Substring(1, line.Length - 2).Replace("\"\"", "\"").Replace(@"\u0022", "\"").Split(DataManage.separator);
            }

            return line.Split(DataManage.separator);
        }

        /* Convert CSV to XML.
         *   Create new XML file.
         *   The most nested nodes in file are values from one CSV line.
         *   The second nesting level nodes represent one line from CSV.
         * Parameters:
         *   fileName — Name of new XML file without .xml extenstion.
         *   rootName — Name of root node in new XML file.
         *   nodeName — Name of node on second nesting level.
         *   hasHeader — Determines the most nested level nodes names.
         *     If true, read nodes names from CSV header line.
         *     If false, generate name for every node.
         */
        public void CSVToXMLConversion(string fileName, string rootName, string nodeName, bool hasHeader)
        {
            if(DataManage.csvFile.Content == null || DataManage.csvFile.Content.Count == 0)
            {
                throw new ArgumentException("Error during conversion! CSV object is null or has no elements.");
            }
            XmlDocument document = new XmlDocument();
            XmlElement rootElement;
            document.AppendChild(document.CreateXmlDeclaration("1.0", "utf-8", null));
            
            if(rootName == null || rootName == String.Empty)
            {
                rootElement = document.CreateElement(fileName);
            }
            else
            {
                rootElement = document.CreateElement(rootName);
            }

            int startLine = (hasHeader == true && DataManage.csvFile.Content.Count > 1 ? 1 : 0);

            for(int i = startLine; i < DataManage.csvFile.Content.Count; i++)
            {
                XmlElement element = document.CreateElement(nodeName);

                uint nodeNumber = 1;

                for(int j = 0; j < DataManage.csvFile.Content[i].Length; j++)
                {
                    string nameTmp = (hasHeader == true ? DataManage.csvFile.Content[0][j] : "Node" + nodeNumber);

                    try
                    {
                        XmlElement nestedElement = document.CreateElement(nameTmp);
                        nestedElement.InnerText = DataManage.csvFile.Content[i][j];
                        element.AppendChild(nestedElement);
                        nodeNumber++;
                    }
                    catch(InvalidOperationException e)
                    {
                        DataManage.conversionErrors.Add(e.Message + $"\nNode name: {nameTmp}; Node value: {DataManage.csvFile.Content[i][j]}");
                    }

                    catch(XmlException e)
                    {
                        DataManage.conversionErrors.Add(e.Message + $"\nNode name: {nameTmp}; Node value: {DataManage.csvFile.Content[i][j]}");
                    }
                }

                rootElement.AppendChild(element);
            }

            document.AppendChild(rootElement);
            document.Save(fileName + ".xml");
        }
    }
}
