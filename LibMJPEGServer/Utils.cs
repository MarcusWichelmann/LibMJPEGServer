using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibMJPEGServer
{
    public static class Utils
    {
        public static bool CheckQualityValue(int qualityValue) => (qualityValue >= 1 && qualityValue <= 100);
    }
}