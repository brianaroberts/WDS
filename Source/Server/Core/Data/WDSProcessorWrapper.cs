namespace WDS.Data
{
    public class WDSProcessorWrapper
    {
        public IWDSCallProcessor Processor;

        public WDSProcessorWrapper(IWDSCallProcessor processor)
        {
            Processor = processor;
        }   
    }
}
