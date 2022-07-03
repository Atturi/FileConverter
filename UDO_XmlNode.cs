using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileConverter
{
    //Represents single XML node
    public class UDO_XmlNode
    {
        //Node nesting level
        private int level;

        //Node name
        private string name = String.Empty;

        //Node attributes
        public Dictionary<string, string> Attributes;
        
        // Inner text of node if it`s not nested node
        private string nodeValue = String.Empty;

        public int Level
        {
            get
            {
                return this.level;
            }

            set
            {
                if (value > 0)
                {
                    this.level = value;
                }
            }
        }

        public string Name
        {
            get
            {
                return this.name;
            }

            set
            {
                if (value != null)
                {
                    this.name = value;
                }
                else
                {
                    throw new NullReferenceException("Node name not specified");
                }
            }
        }

        public string NodeValue
        {
            get
            {
                return this.nodeValue;
            }

            set
            {
                this.nodeValue = value;
            }
        }

        public UDO_XmlNode()
        {
            this.Attributes = new Dictionary<string, string>();
        }
    }
}
