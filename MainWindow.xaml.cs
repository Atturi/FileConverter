using System;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;

namespace FileConverter
{
    /// <summary>
    /// MainWindow.xaml logic
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonSelectFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            fileDialog.InitialDirectory = Environment.CurrentDirectory;
            fileDialog.Filter = "csv files (*.csv)|*.csv|xml files (*.xml)|*.xml";
            fileDialog.FileOk += DataManage.LoadFile;
            fileDialog.ShowDialog();
        }
        
        private void ButtonConvert_Click(object sender, RoutedEventArgs e)
        {
            if(DataManage.conversionType == ConversionType.None)
            {
                MessageBox.Show("Conversion direction is not set.");
            }
            else if(DataManage.conversionType == ConversionType.XMLToCSV)
            {
                if(DataManage.xmlFile == null)
                {
                    throw new NullReferenceException("Error during XML to CSV conversion. XML object is null.");
                }

                if(this.TxtBxOutFileName.Text != "")
                {
                    DataManage.outFileName = TxtBxOutFileName.Text;
                }
                else
                {
                    DataManage.outFileName = "outFile";
                }

                DataManage.xmlFile.XMLToCSVConversion(DataManage.outFileName, DataManage.printAttributes, DataManage.printHeader);
            }
            else if(DataManage.conversionType == ConversionType.CSVToXML)
            {
                if(DataManage.csvFile == null)
                {
                    throw new NullReferenceException("Error during XML to CSV conversion. CSV object is null.");
                }

                if(this.TxtBxOutFileName.Text != "")
                {
                    DataManage.outFileName = TxtBxOutFileName.Text;
                }
                else
                {
                    DataManage.outFileName = "outFile";
                }

                if(this.TxtBxRootName.Text != "")
                {
                    DataManage.rootName = this.TxtBxRootName.Text;
                }

                if(this.TxtBxNodeName.Text != "")
                {
                    DataManage.nodeName = this.TxtBxNodeName.Text;
                }
                else
                {
                    DataManage.nodeName = "Node";
                }

                DataManage.csvFile.CSVToXMLConversion(DataManage.outFileName, DataManage.rootName, DataManage.nodeName, DataManage.CSVHasHeader);

                if(DataManage.conversionErrors.Count > 0)
                {
                    for(int i = 0; i < DataManage.conversionErrors.Count; i++)
                    {
                        this.ErrorTextBlock.Text += (i + 1) + "  " + DataManage.conversionErrors[i] + "\n";
                    }
                }
            }
        }

        private void TxtBxSeparator_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(this.TxtBxSeparator.Text == "")
            {
                DataManage.separator = ',';
            }
            else
            {
                DataManage.separator = this.TxtBxSeparator.Text[0];
            }
        }

        private void ChBxPrintAttributes_Checked(object sender, RoutedEventArgs e)
        {
            DataManage.printAttributes = true;
        }

        private void ChBxPrintAttributes_Unchecked(object sender, RoutedEventArgs e)
        {
            DataManage.printAttributes = false;
        }

        private void ChBxPrintHeader_Checked(object sender, RoutedEventArgs e)
        {
            DataManage.printHeader = true;
        }

        private void ChBxPrintHeader_Unchecked(object sender, RoutedEventArgs e)
        {
            DataManage.printHeader = false;
        }
        
        private void ChBxHeaderExists_Checked(object sender, RoutedEventArgs e)
        {
            DataManage.CSVHasHeader = true;
        }

        private void ChBxHeaderExists_Unchecked(object sender, RoutedEventArgs e)
        {
            DataManage.CSVHasHeader = false;
        }
    }
}
