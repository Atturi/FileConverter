using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;

namespace FileConverter
{
    //Control data and conversion process
    public static class DataManage
    {
        //CSV delimiter
        public static char separator = ',';

        /* Determines wheter to print XML node attributes to CSV file.
         *   If true, node attributes will follow node value.
         *   If false, not to print node attributes.
         */
        public static bool printAttributes = false;

        /* Determines whether to print header to CSV.
         *   If true, print header line to first CSV line.
         *   If false, not to print header line.
         */
        public static bool printHeader = false;

        /* Determines how to interpret the first line in CSV.
         *   If true, the first line in CSV will be interpreted as header. 
         *     It`s values will be converted into node names on the third nested level of XML.
         *   If false, the first line in CSV will be interpreted as data.
         */
        public static bool CSVHasHeader = false;

        //Name of output file
        public static string outFileName;

        //Name of root element in new XML file
        public static string rootName = String.Empty;

        //Name of second nested level nodes
        public static string nodeName = "Node";

        //New CSV file
        public static CSVFile csvFile;

        //New XML file
        public static XMLFile xmlFile;

        //Conversion direction
        public static ConversionType conversionType;

        //Conversion errors
        public static List<string> conversionErrors = new List<string>();

        /* Load chosen file.
         * Parameters:
         *   sender — OpenFileDialog object connected to file
         *   e — event arguments
         */
        public static void LoadFile(object sender, CancelEventArgs e)
        {
            OpenFileDialog dialog = sender as OpenFileDialog;

            if (dialog.SafeFileName.EndsWith(".csv"))
            {
                DataManage.conversionType = ConversionType.CSVToXML;

                using (StreamReader reader = new StreamReader(dialog.FileName))
                {
                    MessageBox.Show("Кодировка StreamReader: " + reader.CurrentEncoding.ToString());
                    DataManage.csvFile = new CSVFile();
                    DataManage.csvFile.LoadFile(reader);
                }
            }
            else if(dialog.SafeFileName.EndsWith(".xml"))
            {
                DataManage.conversionType = ConversionType.XMLToCSV;
                DataManage.xmlFile = new XMLFile();
                DataManage.xmlFile.LoadFile(dialog.SafeFileName);
            }
        }
    }
}
