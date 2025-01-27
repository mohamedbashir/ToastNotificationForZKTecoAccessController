using System.Net;
using ToastNotification.Helper;
namespace ToastNotification;

public partial class MainForm : Form
{

    private IntPtr hCommPro = IntPtr.Zero; // Connection handle
    private const int MaxConnectionAttempts = 5;
    private const int RetryDelayMs = 1000;
    private const int RetryCycleDelayMinutes = 10;
    private bool _isClosing = false;
    AccessPanel AccessPanel = new AccessPanel();
    private string accessPanelIP = string.Empty;

    public MainForm()
    {
        InitializeComponent();
        this.Opacity = 0;
        this.ShowInTaskbar = false;

    }

    private async void MainForm_Load(object sender, EventArgs e)
    {
        // Load IP from the file
        accessPanelIP = ReadIpFromFile("config.txt");

        // If the IP was successfully read, attempt the connection
        if (!string.IsNullOrEmpty(accessPanelIP))
        {
           await ContinuousConnectionAttempts(accessPanelIP);
        }
        else
        {
            ShowToast("ERROR", "Failed to load IP from configuration file.");
        }
    }
    private async Task ContinuousConnectionAttempts(string ipAddress)
    {
        while (!_isClosing)
        {
            bool connected = await AttemptConnectionCycle(ipAddress);
            if (!connected)
            {
                ShowToast("WARNING", $"Will retry in {RetryCycleDelayMinutes} minutes...");
                await Task.Delay(TimeSpan.FromMinutes(RetryCycleDelayMinutes));
            }
            else
            {
                ShowToast("Error", "Failed to connect to access controller");
                break; // Exit the loop when successfully connected
            }
        }
    }

    public void ShowToast(string type, string message)
    {
        ErrorForm errorForm = new ErrorForm(type, message);
        errorForm.ShowInTaskbar = false;
        errorForm.TopMost = true;
        errorForm.Show();
    }

    private async Task<bool> AttemptConnectionCycle(string ipAddress)
    {
        for (int attempt = 1; attempt <= MaxConnectionAttempts; attempt++)
        {
            if (_isClosing) return false;

            hCommPro = AccessPanel.Connect(ipAddress);

            if (hCommPro != IntPtr.Zero)
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

    private void trglog_Tick(object sender, EventArgs e)
    {
        var test = AccessPanel.GetEventLog(accessPanelIP);
        if (test != null) 
            ShowToast("Error", $"Door Status :{test.DoorsStatus} ,Event :{test.Events}");
        
        ShowToast("Error", $"Door Status :{0} ,Event :{0}");
    }

    // Method to read IP from the config file
    private string ReadIpFromFile(string fileName)
    {
        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config", fileName);
        if (File.Exists(filePath))
        {
            return File.ReadAllText(filePath).Trim();
        }
        ShowToast("ERROR", $"Configuration file not found at: {filePath}");
        return string.Empty;
    }

}
