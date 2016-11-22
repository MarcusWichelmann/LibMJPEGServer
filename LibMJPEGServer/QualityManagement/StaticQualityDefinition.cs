using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibMJPEGServer.QualityManagement
{
    public class StaticQualityDefinition : QualityDefinition
    {
        private readonly int _quality;

        public StaticQualityDefinition(int quality)
        {
            if(!Utils.CheckQualityValue(quality))
                throw new ArgumentException("Quality value out of range.", nameof(quality));

            _quality = quality;
        }

        public override int GetDefaultQuality() => _quality;
        public override int GetQualityForFps(int lastQuality, int frameTransmissionTime) => _quality;
    }
}