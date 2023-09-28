namespace AirApi
{
    public enum AirZoneStateType { Open, Closed, Climate };

    public class AirZoneStatus
    {
        public required string ZoneName;
        public AirZoneStateType State;
        public decimal CurrentTemp;
        public decimal DesiredTemp;

        public string StateString
        {
            get
            {
                return State.ToString();
            }
        }

        public string ZoneStatusString
        {
            get
            {
                string zs = ZoneName + " " + State.ToString() + " CT:" + CurrentTemp.ToString();
                return zs;
            }
        }
    }
}

