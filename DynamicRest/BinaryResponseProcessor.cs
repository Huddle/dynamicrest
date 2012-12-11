namespace DynamicRest 
{
    public class BinaryResponseProcessor : ResponseProcessor
    {
        public BinaryResponseProcessor() : base(new StandardResultBuilder(RestService.Binary))
        {
        }
    }
}