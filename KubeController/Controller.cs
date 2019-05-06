using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;
using k8s;
using k8s.Models;
using Microsoft.Extensions.Logging;

namespace Engineerd.KubeController
{
    public class Controller<T> where T : CustomResource
    {
        public delegate void Handle(WatchEventType ev, T item);

        private readonly Kubernetes _client;
        private readonly CustomResourceDefinition _crd;
        private readonly Handle _handle;

        public Controller(Kubernetes client, CustomResourceDefinition crd, Handle handle)
        {
            _client = client;
            _crd = crd;
            _handle = handle;
        }

        public async Task StartAsync(CancellationToken token)
        {
            var result = await _client.ListNamespacedCustomObjectWithHttpMessagesAsync(
                group: _crd.ApiVersion.Split('/')[0],
                version: _crd.ApiVersion.Split('/')[1],
                namespaceParameter: _crd.Namespace,
                plural: _crd.PluralName,
                watch: true,
                timeoutSeconds: (int)TimeSpan.FromMinutes(60).TotalSeconds)
                .ConfigureAwait(false);

            while (!token.IsCancellationRequested)
            {
                result.Watch<T>((type, item) => _handle(type, item));
            }
        }
    }
}
