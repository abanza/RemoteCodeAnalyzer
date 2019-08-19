// BasicServiceHost.cs                                               

#define HOST_TEST

using System;
using System.ServiceModel;
using LocalRepository;
using RemoteRepositoryService;

namespace ServiceHost
{
	class Host
	{
		static System.ServiceModel.ServiceHost CreateChannel(string url)
		{
			BasicHttpBinding binding = new BasicHttpBinding();
			Uri address = new Uri(url);
			Type service = typeof(RemoteRepositoryService.RemoteRepositoryService);
			System.ServiceModel.ServiceHost host = new System.ServiceModel.ServiceHost(service, address);
			host.AddServiceEndpoint(typeof(IRemoteRepositoryService), binding, address);
			return host;
		}

#if HOST_TEST
		static void Main(string[] args)
		{
			Console.Title = "BasicHttp Service Host";
			Console.Write("\n  Starting Programmatic Basic Service");
			Console.Write("\n =====================================\n");

			var repo = new DbRepository();
			//repo.ClearAll();

			System.ServiceModel.ServiceHost host = null;
			try
			{
				host = CreateChannel("http://localhost:8080/RemoteRepositoryService"); // Must match URL specified in client
				host.Open();
				Console.Write("\n  Started RemoteRepositoryService - Press key to exit:\n");
				Console.ReadKey();
			}
			catch (Exception ex)
			{
				Console.Write("\n\n  {0}\n\n", ex.Message);
				return;
			}
			host.Close();
		}

#endif
	}
}