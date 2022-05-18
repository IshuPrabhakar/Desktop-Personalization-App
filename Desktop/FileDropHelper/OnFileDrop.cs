using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Desktop.FileDropHelper
{
    public interface OnFileDrop
    {
        void OnfileDropAction(string[] FilePaths);
    }
}
