namespace AirApi
{
    public class AirStatus 
    {
        public bool On {get;set;}
        public required string Mode {get;set;}
        public required string FanSpeed {get;set;}
    }
}
