using System;
using System.Runtime.CompilerServices;

namespace AirApi
{
    public class Api
    {
        public enum AirFanSpeedType { Low, Medium, High };
        public enum AirModeType { Cool, Heat, Vent, Auto };

        private IConfiguration _config;

        public Api(IConfiguration config)
        {
            _config = config;
        }

        public AirStatus GetStatusTest()
        {
            var status = new AirStatus {
                On = false,
                FanSpeed = "Low",
                Mode = "Cool"
            };
            return status;
        }

        public AirStatus GetStatus()
        {
            string result = SendCommand("RCS=?");
            var status = new AirStatus {
                On = false,
                FanSpeed = "Low",
                Mode = "Cool"
            };
            status.On = false;
            status.FanSpeed = "Low";
            status.Mode = "Cool";
            if (result.Length > 10)
            {
                status.On = (result[5] == '1');
                switch (result[9])
                {
                    case '1':
                        status.FanSpeed = "Low";
                        break;
                    case '2':
                        status.FanSpeed = "Medium";
                        break;
                    case '3':
                        status.FanSpeed = "High";
                        break;
                }
                switch (result[7])
                {
                    case '1':
                        status.Mode = "Cool";
                        break;
                    case '2':
                        status.Mode = "Heat";
                        break;
                    case '3':
                        status.Mode = "Vent";
                        break;
                    case '4':
                        status.Mode = "Auto";
                        break;
                }
            }
            return status;
        }

        public AirZoneStatus GetZoneStatus(int zone)
        {
            AirZoneStatus zoneStatus = new AirZoneStatus { ZoneName = "Zone " + zone.ToString()};
            string status = SendCommand("ZST=" + zone.ToString());
            if (status.StartsWith("+ZST=" + zone.ToString() + ","))
            {
                try
                {
                    status = status.Substring(7);
                    string[] statii = status.Split(',');
                    if (statii[0] == "1")
                        zoneStatus.State = AirZoneStateType.Climate;
                    else
                    {
                        if (statii[2] == "1")
                            zoneStatus.State = AirZoneStateType.Open;
                        else
                            zoneStatus.State = AirZoneStateType.Closed;
                    }
                    zoneStatus.DesiredTemp = decimal.Parse(statii[1]) / 100;
                    zoneStatus.CurrentTemp = decimal.Round(decimal.Parse(statii[3]) / 10) / 10;
                }
                catch (Exception)
                {
                    zoneStatus.State = AirZoneStateType.Closed;
                    zoneStatus.DesiredTemp = 0;
                    zoneStatus.CurrentTemp = 0;
                }
            }
            return zoneStatus;
        }

        public string SetPower(int state)
        {
            string result = SendCommand($"SRU={state}");
            return result;
        }

        public string SetZone(int zone, AirZoneStateType zoneState, decimal desiredTemp = 23)
        {
            switch (zoneState)
            {
                case AirZoneStateType.Climate:
                    desiredTemp *= 100;
                    return SendCommand("ZSE=" + zone.ToString() + ",1," + desiredTemp.ToString());
                case AirZoneStateType.Open:
                    return SendCommand("ZSE=" + zone.ToString() + ",2,1");
                case AirZoneStateType.Closed:
                    return SendCommand("ZSE=" + zone.ToString() + ",2,0");                    
            }
            return "ERROR";
        }

        public void SetMode(AirModeType mode)
        {
            switch (mode)
            {
                case AirModeType.Auto:
                    SendCommand("SMO=4");
                    break;
                case AirModeType.Cool:
                    SendCommand("SMO=1");
                    break;
                case AirModeType.Heat:
                    SendCommand("SMO=2");
                    break;
                case AirModeType.Vent:
                    SendCommand("SMO=3");
                    break;
            }
        }

        private string SendCommand(string command)
        {
            lock (typeof(Api))
            {
                string port = _config.GetValue<string>("ComPort","COM3") ?? "COM3";
                int baudRate = _config.GetValue<int>("BaudRate",19200);

                SerialCom com = new SerialCom(port, baudRate);
                string returnstring = com.Send(command);
                com.Close();
                return returnstring;
            }
        }

    }
}