namespace VirtualRamen.Build.Setup
{
    public class SetupInfo
    {
        public string Name;

        public ushort Port;
        public string Address;

        public SetupInfo(string name, string shortname, ushort port, string address)
        {
            Name = name;
            Port = port;
            Address = address;
        }
    }

    
}