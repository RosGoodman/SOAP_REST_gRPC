using System;
using System.ServiceModel;
using TrainingService.Interfaces;
using TrainingService.ServiceClasses;

namespace TrainingService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public class TrainingService : ITrainingService
    {
        private readonly IScriptService _scriptService;
        private readonly IStatisticsService _statisticsService;
        private readonly ISettingsService _serviceSettings;

        public TrainingService()
        {
            _statisticsService = new StatisticsService();
            _serviceSettings = new SettingsService();
            _scriptService = new ScriptService(_serviceSettings, _statisticsService, Callback);
        }

        public void CompileScript()
        {
            _scriptService.Compile();
        }

        public void RunScript()
        {
            _scriptService.Run();
        }

        public void UpdateAndCompileScript(string filename)
        {
            _serviceSettings.FileName = filename;
            _scriptService.Compile();
        }

        ITrainingServiceCallback Callback
        {
            get => OperationContext.Current.GetCallbackChannel<ITrainingServiceCallback>();
        }
    }
}
 