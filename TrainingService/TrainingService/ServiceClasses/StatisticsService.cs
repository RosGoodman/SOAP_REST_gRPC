
using System.Runtime.Serialization;
using TrainingService.Interfaces;

namespace TrainingService.ServiceClasses
{
    [DataContract]
    public class StatisticsService : IStatisticsService
    {
        [DataMember]
        public int SuccessTacts { get; set; }

        [DataMember]
        public int ErrorTacts { get; set; }

        [DataMember]
        public int AllTacts { get; set; }
    }
}