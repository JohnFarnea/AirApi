using System.IO.Ports;

namespace AirApi

{
    public class SerialCom
    {
        SerialPort _com;

        public SerialCom(string port, int baudRate)
        {
            _com = new SerialPort(port, baudRate, Parity.None, 8, StopBits.One);
            _com.Open();
            _com.ReadTimeout = 5000;
            _com.WriteTimeout = 5000;
        }

        public string Send(string command)
        {
            _com.NewLine = "\r";
            _com.WriteLine(command);
            return _com.ReadLine();
        }

        public void Close()
        {
            _com.Close();
        }

    }
}
