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


    private const int LargeBufferSize = 1024 * 1024 * 2;
    IntPtr _handle = IntPtr.Zero;


    /**
     * Checks if the device is connected
     */
    [MethodImpl(MethodImplOptions.Synchronized)]
    public bool IsConnected()
    {
        if (_handle == IntPtr.Zero)
            return false;
       
        return _handle != IntPtr.Zero;
    }

    [MethodImpl(MethodImplOptions.Synchronized)]
    public void Disconnect()
    {
        if (IsConnected())
        {
            Disconnect(_handle);
            _handle = IntPtr.Zero;
        }
    }

    /**
     * Connects to a device using the TCP protocol
     */
    [MethodImpl(MethodImplOptions.Synchronized)]
    public nint Connect(string ip, int port = 4370,int timeout = 5000)
    {
        if (IsConnected())
            return 0;    

        string connStr =
            $"protocol=TCP,ipaddress={ip},port={port},timeout={timeout},passwd={""}";
        _handle = Connect(connStr);
        if (_handle != IntPtr.Zero)
            return _handle;    

        return 0;
    }

    /**
     * Reads the latest events only
     */
    [MethodImpl(MethodImplOptions.Synchronized)]
    public AccessPanelEvent? GetEventLog(string ip)
    {
        if (IsConnected())
        {
            byte[] buf = new byte[LargeBufferSize];
            if (GetRTLog(_handle, ref buf[0], buf.Length) > -1)
            {
                // string tempLastEventTime = _lastEventTime;
                string[] events = Encoding.ASCII.GetString(buf).Replace("\0", "").Trim()
                    .Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
                List<AccessPanelRtEvent> rtEvents = new List<AccessPanelRtEvent>();
                AccessPanelDoorsStatus? doorsStatus = null;
                for (int i = 0; i < events.Length; i++)
                {
                    if (events[i][0] == '\0')
                    {
                        continue;
                    }

                    string[] values = events[i].Split(new[] { ',' });
                    if (values.Length != 7)
                    {
                        continue;
                    }

                    // if (String.Compare(values[0], tempLastEventTime, StringComparison.Ordinal) > 0)
                    // {
                    //     tempLastEventTime = values[0];
                    // }

                    // _lastEventTime = values[0];
                    if (values[4].Equals("255"))
                    {
                        doorsStatus = new AccessPanelDoorsStatus(values[1], values[2]);
                    }
                    else
                    {
                        // if (String.Compare(values[0], _lastEventTime, StringComparison.Ordinal) < 0)
                        // {
                        //     continue;
                        // }

                        int.TryParse(values[2], out var card);

                        AccessPanelRtEvent evt = new AccessPanelRtEvent(values[0], values[1], card, values[3],
                            int.Parse(values[4]), int.Parse(values[5]));
                        rtEvents.Add(evt);
                    }
                }

                // _lastEventTime = tempLastEventTime;
                return new AccessPanelEvent(doorsStatus, rtEvents.ToArray());
            }
            else
                return null;
        }
        else
            Connect(ip);
        
        return null;
    }
}
