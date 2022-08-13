using System.ServiceModel;
using System;
using TrainingClient.TrainingServiceReference;

namespace TrainingClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            InstanceContext instanceContext = new InstanceContext(new CallbackHandler());
            TrainingServiceClient trainingServiceClient = new TrainingServiceClient(instanceContext);

            trainingServiceClient.CompileScript();
            trainingServiceClient.RunScript();

            Console.ReadKey();
            trainingServiceClient.Close();
        }
    }
}
