
using System.ServiceModel;
using TrainingService.ServiceClasses;

namespace TrainingService.Interfaces
{
    [ServiceContract]
    public interface ITrainingServiceCallback
    {
        [OperationContract(IsOneWay = true)]
        void UpdateStatistics(StatisticsService statisticsService);

    }
}
