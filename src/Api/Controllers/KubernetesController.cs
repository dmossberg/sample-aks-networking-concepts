using k8s;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Api.Controllers
{
    [Route("api/kubernetes")]
    [ApiController]
    public class KubernetesController : ControllerBase
    {
        private readonly IHostEnvironment _env;
        private readonly ILogger<KubernetesController> _logger;
        private readonly Kubernetes _kubernetes;

        public KubernetesController(ILogger<KubernetesController> logger, IHostEnvironment env)
        {
            _env = env;
            _logger = logger;
            KubernetesClientConfiguration config;

            Debugger.Log(0, null, $"Directory: {Directory.GetCurrentDirectory()} | Files: " + string.Join("\r\n", Directory.GetFiles(Directory.GetCurrentDirectory())));

            try
            {
                if (_env.IsDevelopment())
                {
                    config = KubernetesClientConfiguration.BuildConfigFromConfigFile("/app/kube-config");
                }
                else
                {
                    config = KubernetesClientConfiguration.InClusterConfig();
                }

                _kubernetes = new Kubernetes(config);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, ex.ToString());
                throw;
            }   
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return new OkObjectResult("Hello from Kubernetes API controller");
        }


        [HttpGet("namespaces")]
        public async Task<IActionResult> GetNamespaces()
        {
            var namespaces = await _kubernetes.ListNamespaceAsync();
            return new OkObjectResult(namespaces.Items);
        }

        [HttpGet("services")]
        public async Task<IActionResult> GetServices()
        {
            var namespaces = await _kubernetes.ListServiceForAllNamespacesAsync();
            return new OkObjectResult(namespaces.Items);
        }

        [HttpGet("deployments")]
        public async Task<IActionResult> GetDeployments()
        {
            var namespaces = await _kubernetes.ListDeploymentForAllNamespacesAsync();
            return new OkObjectResult(namespaces.Items);
        }

        [HttpGet("pods")]
        public async Task<IActionResult> GetPods()
        {
            var namespaces = await _kubernetes.ListPodForAllNamespacesAsync();
            return new OkObjectResult(namespaces.Items);
        }
    }
}
