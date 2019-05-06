# Kubernetes Controllers for CRDs in C#

## What is this?
This is a sample C# library for creating lightweight [controllers][controller] for [Kubernetes CRDs][crd], using the [Kubernetes C# client][csharp-client].

It is intended to show you how to get started writing your own controller for CRDs in C#, and it is _not_ suited for production purposes.

## Using this library

Considering a custom resource definition that can be found in `KubeController.Sample/deploy/crd.yaml`, building the C# object is done by creating an instance of the `CustomResource` generic class which mimics the structure of your CRD, and passing a handler function to be executed whenever an event is fired:

```csharp
    var crd = new CustomResourceDefinition()
    {
        ApiVersion = "engineerd.dev/v1alpha1",
        PluralName = "examples",
        Kind = "Example",
        Namespace = "kubecontroller"
    };

    var controller = new Controller<ExampleCRD>(
        new Kubernetes(KubernetesClientConfiguration.BuildConfigFromConfigFile()),
        crd,
        (WatchEventType eventType, ExampleCRD example) =>
            Console.WriteLine("Event type: {0} for {1}", eventType, example.Metadata.Name));

    var cts = new CancellationTokenSource();
    await controller.StartAsync(cts.Token).ConfigureAwait(false);
```

## Building from source

Prerequisites:

- a Kubernetes cluster
- .NET Core 2.2
- VS Code (optionally, for debugging)

To run the sample, you first need to deploy the CRD (the sample uses the `kubecontroller` namespace), then start the console application:

```
$ kubectl create -f KubeController.Sample/deploy/crd.yaml
$ dotnet run KubeController.Sample/
```

At this point, you can start operating on `Example` objects in your namespace, and the handler will get executed.

[controller]: https://kubernetes.io/docs/concepts/workloads/controllers/
[crd]: https://kubernetes.io/docs/concepts/extend-kubernetes/api-extension/custom-resources/
[csharp-client]: https://github.com/kubernetes-client/csharp
