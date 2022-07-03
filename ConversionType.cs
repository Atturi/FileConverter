using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileConverter
{
    /*
     * Направление конвертации.
     * Определяется типами исходного и конечного файлов.
     */
    public enum ConversionType
    { 
        None,
        CSVToXML,
        XMLToCSV
    }
}
