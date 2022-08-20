using ClinicService.Protos;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using static ClinicService.Protos.ClientService;
using static ClinicService.Protos.ConsultationService;
using static ClinicService.Protos.PetService;

namespace ClinicClient
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //рекомендуется использовать для работы по незащищенному соединению
            //AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            //канал с указанием порта, который установили на сервере (Program ConfigureCestrel)
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");

            ClinicService.Protos.AuthenticateService.AuthenticateServiceClient authenticateServiceClient = new AuthenticateService.AuthenticateServiceClient(channel);

            var authenticationResponse = authenticateServiceClient.Login(new ClinicService.Protos.AuthenticationRequest
            {
                UserName = "some.mail@gmail.com",
                Password = "12345"
            });

            //проверка статуса аутентификации
            if(authenticationResponse.Status != 0)
            {
                Console.WriteLine("Authentication error.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"Session token: {authenticationResponse.SessionInfo.SessionToken}");

            //перехватчик. В данном случае при каждом запросе на сервер в заголовок вставлялся сессионный токен
            var credentials = CallCredentials.FromInterceptor((c, m) =>
            {
                m.Add("Authorization",
                    $"Bearer {authenticationResponse.SessionInfo.SessionToken}");
                return Task.CompletedTask;
            });

            //создание защищенного канала
            var protectedChannel = GrpcChannel.ForAddress("https://localhost:5001",
                new GrpcChannelOptions { Credentials = ChannelCredentials.Create(new SslCredentials(), credentials) });

            AddClients(protectedChannel);
            Console.WriteLine("========\n=======");
            AddPets(protectedChannel);
            Console.WriteLine("========\n=======");
            AddConsultation(protectedChannel);

            Console.ReadKey();
        }

        private static void AddClients(GrpcChannel channel)
        {
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
        }

        private static void AddPets(GrpcChannel channel)
        {
            PetServiceClient pet = new PetServiceClient(channel);

            var newPet = new CreatePetRequest
            {
                Birthday = Timestamp.FromDateTime(DateTime.UtcNow.AddDays(-100)),
                ClientId = 1,
                Name = "Шарик",
            };

            var createPetResponse = pet.CreatePet(newPet);

            Console.WriteLine($"Pet ({createPetResponse.PetId}) created successfully.");


            var getPetsResponse = pet.GetPets(new GetPetsRequest());
            if (getPetsResponse.ErrCode == 0)
            {
                Console.WriteLine("Pets:");
                Console.WriteLine("========\n");
                foreach (var petDto in getPetsResponse.Pets)
                {
                    Console.WriteLine($"({petDto.PetId}/{petDto.Name}) {petDto.Birthday} {petDto.ClientId}");
                }
            }
        }

        private static void AddConsultation(GrpcChannel channel)
        {
            ConsultationServiceClient consultation = new ConsultationServiceClient(channel);

            var createConsultationResponse = consultation.CreateConsultation(new CreateConsultationRequest
            {
                ClientId = 1,
                ConsultationDate = Timestamp.FromDateTime(DateTime.Now.ToUniversalTime()),
                Description = "description",
                PetId = 1,
            });

            Console.WriteLine($"Consultation ({createConsultationResponse.ConsultationId}) created successfully.");


            var getConsultationsResponse = consultation.GetConsultations(new GetConsultationsRequest());
            if (getConsultationsResponse.ErrCode == 0)
            {
                Console.WriteLine("Consultations:");
                Console.WriteLine("========\n");
                foreach (var consultationsDto in getConsultationsResponse.Consultations)
                {
                    Console.WriteLine($"({consultationsDto.ConsultationId} {consultationsDto.ConsultationDate}) {consultationsDto.Description} {consultationsDto.ClientId} {consultationsDto.PetId}");
                }
            }
        }
    }
}