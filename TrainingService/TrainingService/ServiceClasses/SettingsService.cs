
using TrainingService.Interfaces;

namespace TrainingService.ServiceClasses
{
    public class SettingsService : ISettingsService
    {
        public SettingsService()
        {
            FileName = @"\Scripts\SampleScript.script";
        }

        public string FileName { get; set; }
    }
}