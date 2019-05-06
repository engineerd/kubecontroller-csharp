using k8s;
using k8s.Models;

namespace Engineerd.KubeController.Sample
{
    public class ExampleSpec
    {
        public string ExampleType { get; set; }
        public string ProgrammingLanguage { get; set; }
    }

    public class ExampleStatus
    {
        public int Count { get; set; }
    }

    public class ExampleCRD : CustomResource<ExampleSpec, ExampleStatus>
    {
    }
}
