using System.ServiceProcess;

namespace gRPC_Time_Server
{
    public class ServiceRunner: ServiceBase
    {
        private readonly IHost _host;

        public ServiceRunner(IHost host)
        {
            _host = host ?? throw new ArgumentNullException(nameof(host));
        }

        protected override void OnStart(string[] args)
        {
            _host.Start();
        }

        protected override void OnStop()
        {
            _host.StopAsync().Wait();
        }
    }
}
