
using System;
using TrainingClient.TrainingServiceReference;

namespace TrainingClient
{
    public class CallbackHandler : ITrainingServiceCallback
    {
        public void UpdateStatistics(StatisticsService statisticsService)
        {
            Console.Clear();
            Console.WriteLine("Обновление по статистике выполнения скрипта");
            Console.WriteLine($"Всего     тактов: {statisticsService.AllTacts}");
            Console.WriteLine($"Успешных  тактов: {statisticsService.SuccessTacts}");
            Console.WriteLine($"Ошибочных тактов: {statisticsService.ErrorTacts}");
        }
    }
}