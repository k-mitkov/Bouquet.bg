using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Bouquet.CommunicationsApi.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    //������� �� ������ ���� Bouquet.CommunicationsApi.WebApi
                    //��� ���� �� �� ������ ��� ���� ������ �� �� ��������, �� � ������������ �� �� ����. ����� �� ��������� ���������� �� ������� ��� ��� ����� �� ���
                    webBuilder
                    .UseUrls("http://192.168.137.1:5000", "https://192.168.137.1:5001")
                    .UseStartup<Startup>();
                });
    }
}
