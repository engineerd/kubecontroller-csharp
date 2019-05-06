using System;
using System.Threading;
using System.Threading.Tasks;
using k8s;
using k8s.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Engineerd.KubeController.Sample
{
    public static class Program
    {
        public static async Task Main()
        {
            var crd = new CustomResourceDefinition()
            {
                ApiVersion = "engineerd.dev/v1alpha1",
                PluralName = "examples",
                Kind = "Example",
                Namespace = "kubecontroller"
            };

            var controller = new Controller<ExampleCRD>(new Kubernetes(KubernetesClientConfiguration.BuildConfigFromConfigFile()), crd, Handle);
            var cts = new CancellationTokenSource();
            await controller.StartAsync(cts.Token).ConfigureAwait(false);
        }

        public static void Handle(WatchEventType eventType, ExampleCRD example)
        {
            Console.WriteLine("Event: {0} \nName: {1}\nProgramming Language: {2}\nType: {3}\n\n",
                                eventType, example.Metadata.Name, example.Spec.ProgrammingLanguage, example.Spec.ExampleType);
        }
    }
}
