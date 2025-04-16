using Newtonsoft.Json;
using System.Net.NetworkInformation;
using ToastNotification.Helper;

namespace ToastNotification;

/// <summary>
/// MainForm (Windows Form) automatically manages connection and reconnection 
/// to a ZKTeco Access Controller via plcommpro.dll over TCP.
/// It uses toast notifications for real-time status updates.
/// </summary>
public partial class MainForm : Form
{
    // === Fields ===
    // Handle to the connected device (returned from Connect method in plcommpro.dll)
    private IntPtr hCommPro = IntPtr.Zero;

    // Connection configuration
    private const int MaxConnectionAttempts = 5;         // Max attempts per cycle
    private const int RetryDelayMs = 5000;               // Delay between attempts (5 sec)
    private const int RetryCycleDelayMinutes = 1;        // Delay between full cycles

    // Flag to detect form closing to stop loops
    private bool _isClosing = false;

    // Instance of AccessPanel class (handles actual plcommpro interactions)
    private AccessPanel AccessPanel;

    // IP address of the access panel (set via config or manually)
    private string accessPanelIP = string.Empty;

    private string ip;


    // === Constructor ===
    public MainForm()
    {
        InitializeComponent();
        LoadConfig();
        AccessPanel = new AccessPanel();
        Opacity = 0;
        ShowInTaskbar = false;

    }

    // === Form Load ===
    private async void MainForm_Load(object sender, EventArgs e)
    {
        // If the IP was successfully read, attempt the connection
        if (!string.IsNullOrEmpty(ip))
        {
            await ContinuousConnectionAttempts(ip);
        }
        else
        {
            ShowToast("ERROR", "Failed to load IP from configuration file.");
        }
    }

    // === Persistent Reconnect Loop ===
    private async Task ContinuousConnectionAttempts(string ipAddress)
    {
        bool connected = await AttemptConnectionCycle(ipAddress);
        if (!connected)
        {
            ShowToast("WARNING", $"Will retry in {RetryCycleDelayMinutes} minutes...");
            await Task.Delay(TimeSpan.FromMinutes(RetryCycleDelayMinutes));
            await ContinuousConnectionAttempts(ipAddress); // Retry continuously
        }
        else
        {
            ShowToast("SUCCESS", "Connected to access controller");
        }
    }

    // === Attempt to Connect Up to N Times ===
    private async Task<bool> AttemptConnectionCycle(string ipAddress)
    {
        for (int attempt = 1; attempt <= MaxConnectionAttempts; attempt++)
        {
            if (_isClosing) return false;

            hCommPro = AccessPanel.Connect(ipAddress);

            if (hCommPro != IntPtr.Zero && IsDeviceReachable(ipAddress))
            {
                ShowToast("SUCCESS", "Connection established!");
                trglog.Enabled = true;
                return true;
            }

            ShowToast("TRYCON", $"Attempt {attempt} failed: {-1}");
            await Task.Delay(RetryDelayMs);
        }

        ShowToast("ERROR", $"All {MaxConnectionAttempts} attempts failed");
        return false;
    }

    // === Check Device Availability via Ping ===
    private bool IsDeviceReachable(string ipAddress)
    {
        try
        {
            using (Ping ping = new Ping())
            {
                PingReply reply = ping.Send(ipAddress);
                return reply.Status == IPStatus.Success;
            }
        }
        catch
        {
            return false; // If ping fails, assume the device is unreachable
        }
    }

    // === Timer Tick to Poll Logs or Reconnect if Disconnected ===
    private async void trglog_Tick(object sender, EventArgs e)
    {
        if (!AccessPanel.IsConnected())
        {
            trglog.Enabled = false;

            ShowToast("ERROR", "Connection lost. Attempting to reconnect...");

            bool reconnected = await AttemptConnectionCycle(accessPanelIP);

            if (reconnected)
            {
                ShowToast("SUCCESS", "Reconnected to access controller.");
                trglog.Enabled = true;
            }
            else
                ShowToast("ERROR", "Failed to reconnect.");
                      
            return;
        }

        AccessPanelEvent? result = AccessPanel.GetEventLog(accessPanelIP);
        if (result != null)
        {
            foreach (var item in result.Events)
            {
                ShowToast("Info", $"event :{item}");
            }
        }
    }

    private void LoadConfig()
    {
        try
        {
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"config.json");
            if (!File.Exists(configPath))
            {
                throw new FileNotFoundException("config.json not found");
            }

            string json = File.ReadAllText(configPath);
            var config = JsonConvert.DeserializeObject<Config>(json);
            ip = config.ip;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    // === Display Toast Notifications Using Custom Form ===
    public void ShowToast(string type, string message)
    {
        ErrorForm errorForm = new ErrorForm(type, message);
        errorForm.ShowInTaskbar = false;
        errorForm.TopMost = true;
        errorForm.Show();
    }

    // === Gracefully Handle Form Closing ===
    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        _isClosing = true; // Flag will stop all retry loops
        base.OnFormClosing(e);
    }
}
