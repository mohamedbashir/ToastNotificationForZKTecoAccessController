using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace ToastNotification.Helper;

public class AccessPanel
{
    [DllImport("plcommpro.dll", EntryPoint = "GetRTLog")]
    private static extern int GetRTLog(IntPtr handle, ref byte buffer, int len);
    [DllImport("plcommpro.dll", EntryPoint = "Connect")]
    private static extern IntPtr Connect(string parameters);

    [DllImport("plcommpro.dll", EntryPoint = "Disconnect")]
    private static extern void Disconnect(IntPtr handle);


    private const int LargeBufferSize = 1024 * 1024 * 2; // 2MB buffer for reading logs
    private IntPtr _handle = IntPtr.Zero;


    /**
     * Checks if the device is connected
     */
    [MethodImpl(MethodImplOptions.Synchronized)]
    public bool IsConnected()
    {
        if (_handle == IntPtr.Zero)
            return false;

        byte dummy = 0;
        int result = GetRTLog(_handle, ref dummy, 1);

        if (result == -1)
        {
            _handle = IntPtr.Zero;
            return false;
        }

        return true;
    }

    /**
     * Connects to a device using the TCP protocol
     */
    [MethodImpl(MethodImplOptions.Synchronized)]
    public IntPtr Connect(string ip, int port = 4370, int timeout = 5000)
    {
        if (IsConnected())
            return _handle;  // Already connected, return the handle

        // Construct the connection string with parameters
        string connStr = $"protocol=TCP,ipaddress={ip},port={port},timeout={timeout},passwd={""}";
        _handle = Connect(connStr);

        // Return the handle if successfully connected
        return _handle != IntPtr.Zero ? _handle : IntPtr.Zero;
    }

    /**
     * Reads the latest events only
     */
    [MethodImpl(MethodImplOptions.Synchronized)]
    public AccessPanelEvent? GetEventLog(string ip)
    {
        if (!IsConnected())
            return null;

        byte[] buf = new byte[LargeBufferSize];
        if (GetRTLog(_handle, ref buf[0], buf.Length) > -1)
        {
            string[] events = Encoding.ASCII.GetString(buf).Replace("\0", "").Trim()
                .Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

            List<AccessPanelRtEvent> rtEvents = new List<AccessPanelRtEvent>();
            AccessPanelDoorsStatus? doorsStatus = null;

            foreach (var evtStr in events)
            {
                if (evtStr.Length == 0 || evtStr[0] == '\0')
                    continue;

                var values = evtStr.Split(',');

                if (values.Length != 7)
                    continue;

                if (values[4].Equals("255"))
                {
                    doorsStatus = new AccessPanelDoorsStatus(values[1], values[2]);
                }
                else
                {
                    int.TryParse(values[2], out var card);
                    rtEvents.Add(new AccessPanelRtEvent(values[0], values[1], card, values[3],
                        int.Parse(values[4]), int.Parse(values[5])));
                }
            }

            return new AccessPanelEvent(doorsStatus, rtEvents);
        }

        return null;
    }

    public void Disconnect()
    {
        if (_handle != IntPtr.Zero)
        {
            Disconnect(_handle);
            _handle = IntPtr.Zero;
        }
    }


}
