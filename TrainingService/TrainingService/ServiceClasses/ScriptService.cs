
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System;
using TrainingService.Interfaces;
using System.Threading;
using System.Text;

namespace TrainingService.ServiceClasses
{
    public class ScriptService : IScriptService
    {
        CompilerResults compilerResults;

        private readonly IStatisticsService _statisticsService;
        private readonly ISettingsService _serviceSettings;
        private readonly ITrainingServiceCallback _trainigServiceCallback;

        public ScriptService(
            ISettingsService serviceSettings,
            IStatisticsService statisticsService,
            ITrainingServiceCallback pumpServiceCallback)
        {
            _serviceSettings = serviceSettings;
            _statisticsService = statisticsService;
            _trainigServiceCallback = pumpServiceCallback;
        }


        public bool Compile()
        {
            try
            {
                CompilerParameters compilerParameters = new CompilerParameters();
                compilerParameters.GenerateInMemory = true;

                //подключение библиотек
                compilerParameters.ReferencedAssemblies.Add("System.dll");
                compilerParameters.ReferencedAssemblies.Add("System.Core.dll");
                compilerParameters.ReferencedAssemblies.Add("System.Data.dll");
                compilerParameters.ReferencedAssemblies.Add("Microsoft.CSharp.dll");
                //ссылка на текущую сборку
                compilerParameters.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly().Location);

                FileStream fileStream = new FileStream(_serviceSettings.FileName, FileMode.Open);

                byte[] buffer;
                try
                {
                    int length = (int)fileStream.Length;
                    buffer = new byte[length];
                    int count;
                    int sum = 0;
                    while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
                        sum += count;
                }
                finally
                {
                    fileStream.Close();
                }

                //компиляция скрипта с указанными параметрами (compilerParameters)
                CSharpCodeProvider cSharpCodeProvider = new CSharpCodeProvider();
                compilerResults = cSharpCodeProvider.CompileAssemblyFromSource(compilerParameters, Encoding.UTF8.GetString(buffer));

                //проверка наличия ошибок при компиляции
                if (compilerResults.Errors is null && compilerResults.Errors.Count > 0)
                {
                    string compileErrors = string.Empty;
                    for (int i = 0; i < compilerResults.Errors.Count; i++)
                    {
                        if (compileErrors != string.Empty)
                        {
                            compileErrors += "\n";
                        }
                        compileErrors += compilerResults.Errors[i];
                    }
                    return false;
                }
                return true;
            }
            catch (Exception ex) { return false; }
        }

        public void Run()
        {
            if (compilerResults is null || (compilerResults.Errors != null && compilerResults.Errors.Count > 0))
                if (!Compile()) return;

            //получение класса точки входа. Рефлексия
            Type type = compilerResults.CompiledAssembly.GetType("Sample.SampleScript");
            if (type is null) return;

            //получение метода точки входа. Рефлексия
            MethodInfo entryPointMethod = type.GetMethod("EntryPoint");
            if (entryPointMethod is null) return;

            Task.Run(() =>
            {
                try
                {
                    for (int i = 0; i < 10; i++)
                    {
                        //Activator тут создает экземпляр класса точки входа
                        //вторым параметром, в массиве объектов, можно передать параметры для конструктора класса точки входа
                        //пример: new object[] { 1, 2} - два параметра int
                        if ((bool)entryPointMethod.Invoke(Activator.CreateInstance(type), new object[] { }))
                            _statisticsService.SuccessTacts++;
                        else
                            _statisticsService.ErrorTacts++;

                        _statisticsService.AllTacts++;
                        _trainigServiceCallback.UpdateStatistics((StatisticsService)_statisticsService);
                        Thread.Sleep(1000);
                    }
                }
                catch (Exception) { }
            });
        }
    }
}