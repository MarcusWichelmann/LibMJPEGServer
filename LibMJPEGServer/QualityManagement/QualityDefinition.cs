using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibMJPEGServer.QualityManagement
{
    public abstract class QualityDefinition
    {
        public abstract int GetDefaultQuality();
        public abstract int GetQualityForFps(int lastQuality, int frameTransmissionTime);
    }
}