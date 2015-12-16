using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinFormDemo
{
   public  class Images
    {
        private string url;

        public string Url
        {
            get { return url; }
            set { url = value; }
        }
        private string desc;

        public string Desc
        {
            get { return desc; }
            set { desc = value; }
        }

        private string fileName;

        public string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        private string sort;

        public string Sort
        {
            get { return sort; }
            set { sort = value; }
        }
    }
}
