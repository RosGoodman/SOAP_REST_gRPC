using ClinicService.Protos;
using Grpc.Net.Client;
using static ClinicService.Protos.ClientService;

namespace ClinicClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //рекомендуется использовать для работы по незащищенному соединению
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            //канал с указанием порта, который установили на сервере (Program ConfigureCestrel)
            using var channel = GrpcChannel.ForAddress("http://localhost:5001");

            ClientServiceClient client = new ClientServiceClient(channel);

            var createClientResponse = client.CreateClient(new CreateClientRequest
            {
                Document = "PASS123",
                FirstName = "cтаниcлав",
                Surname = "Байраковcкий",
                Patronymic = "Антонович"
            });

            Console.WriteLine($"Client ({createClientResponse.ClientId}) created successfully.");


            var getClientsResponse = client.GetClients(new GetClientsRequest());
            if (getClientsResponse.ErrCode == 0)
            {
                Console.WriteLine("Clients:");
                Console.WriteLine("========\n");
                foreach (var clientDto in getClientsResponse.Clients)
                {
                    Console.WriteLine($"({clientDto.ClientId}/{clientDto.Document}) {clientDto.Surname} {clientDto.FirstName} {clientDto.Patronymic}");
                }
            }

            Console.ReadKey();
        }
    }
}