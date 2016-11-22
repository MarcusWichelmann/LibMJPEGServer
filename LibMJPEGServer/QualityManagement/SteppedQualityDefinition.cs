using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibMJPEGServer.QualityManagement
{
    public class SteppedQualityDefinition : QualityDefinition
    {
        private readonly int[] _qualitySteps;
        private readonly int _defaultQuality;

        public SteppedQualityDefinition(int[] qualitySteps, int defaultQuality)
        {
            if(qualitySteps.Length <= 1)
                throw new ArgumentException("Please define at least two quality steps.", nameof(qualitySteps));

            if(!qualitySteps.Contains(defaultQuality))
                throw new ArgumentException("Default quality not in array of supported quality steps.", nameof(defaultQuality));

            foreach(int qualityStep in qualitySteps)
                if(!Utils.CheckQualityValue(qualityStep))
                    throw new ArgumentException($"Quality step out of range: {qualityStep}", nameof(qualitySteps));

            if(!Utils.CheckQualityValue(defaultQuality))
                throw new ArgumentException("Default quality out of range.", nameof(defaultQuality));

            _qualitySteps = qualitySteps.OrderBy(q => q).ToArray();
            _defaultQuality = defaultQuality;
        }

        public override int GetDefaultQuality() => _defaultQuality;

        public override int GetQualityForFps(int lastQuality, int frameTransmissionTime)
        {
            if(!_qualitySteps.Contains(lastQuality))
                return _defaultQuality;

            int clientTransmissionFps = (int)Math.Round(1000.0 / frameTransmissionTime);

            int currentStep = Array.IndexOf(_qualitySteps, lastQuality);
            int newStep = currentStep;

            if(clientTransmissionFps < 10 && currentStep > 0)
                newStep = currentStep - 1;
            else if(clientTransmissionFps > 800 && currentStep < _qualitySteps.Count() - 1)
                newStep = currentStep + 1;

            return _qualitySteps[newStep];
        }
    }
}