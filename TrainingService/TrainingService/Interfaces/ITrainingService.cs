
using System.ServiceModel;
using TrainingService.Interfaces;

namespace TrainingService
{
    [ServiceContract(SessionMode=SessionMode.Required, CallbackContract = typeof(ITrainingServiceCallback))]
    public interface ITrainingService
    {
        [OperationContract(IsOneWay=true)]
        void RunScript();

        [OperationContract(IsOneWay = true)]
        void UpdateAndCompileScript(string filename);

        [OperationContract(IsOneWay = true)]
        void CompileScript();
    }
}
