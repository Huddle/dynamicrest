namespace DynamicRest
{
    public interface IBuildDynamicResults
    {
        object CreateResult(string responseText, RestService serviceType);
    }
}