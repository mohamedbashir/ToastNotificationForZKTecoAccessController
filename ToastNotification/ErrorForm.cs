using System;
using System.Drawing;
using System.Windows.Forms;

namespace ToastNotification
{
    public partial class ErrorForm : Form
    {
        private int _targetY; // Target Y position for the toast
        private const int AnimationStep = 10; // Animation speed (pixels per tick)
        private const int AnimationInterval = 20; // Timer interval (milliseconds)

        public ErrorForm(string type, string message)
        {
            InitializeComponent();
            lb_meg.Text = message;
            lb_type.Text = type;

            // Set form appearance based on the type
            SetAppearance(type);

            // Position the form off-screen initially
            Position();

            // Set up the timers for animation and auto-close
            InitializeTimers();
        }

        private void SetAppearance(string type)
        {
            switch (type.ToUpper()) // Ensure case-insensitive comparison
            {
                case "SUCCESS":
                    pl_border.BackColor = Color.FromArgb(57, 155, 53);
                    picicon.Image = Properties.Resources.icons8_success_96;
                    break;
                case "ERROR":
                    pl_border.BackColor = Color.FromArgb(227, 50, 45);
                    picicon.Image = Properties.Resources.icons8_cancel_96;
                    break;
                case "INFO":
                    pl_border.BackColor = Color.FromArgb(18, 136, 191);
                    picicon.Image = Properties.Resources.icons8_info_96;
                    break;
                case "WARNING":
                    pl_border.BackColor = Color.FromArgb(245, 171, 35);
                    picicon.Image = Properties.Resources.icons8_warning_96;
                    break;
                case "TRYCON":
                    pl_border.BackColor = Color.FromArgb(124, 179, 66);
                    picicon.Image = Properties.Resources.icons8_loading_96;
                    break;
                default:
                    throw new ArgumentException("Invalid toast type", nameof(type));
            }
        }

        private void Position()
        {
            // Position the form at the top-right corner of the screen (initially off-screen)
            int screenWidth = Screen.PrimaryScreen.WorkingArea.Width;
            int screenHeight = Screen.PrimaryScreen.WorkingArea.Height;

            int toastX = screenWidth - this.Width; // Align to the right
            _targetY = 0; // Target Y position (top of the screen)

            this.StartPosition = FormStartPosition.Manual; // Ensure manual positioning
            this.Location = new Point(toastX, -this.Height); // Start above the screen
        }

        private void InitializeTimers()
        {
            // Timer for slide-in animation
            System.Windows.Forms.Timer slideInTimer = new System.Windows.Forms.Timer();
            slideInTimer.Interval = AnimationInterval;
            slideInTimer.Tick += (sender, e) =>
            {
                // Move the form down
                this.Top += AnimationStep;

                // Stop the timer when the form reaches the target position
                if (this.Top >= _targetY)
                {
                    slideInTimer.Stop();
                    this.Top = _targetY; // Snap to the exact position
                    StartCloseTimer(); // Start the close timer after slide-in
                }
            };
            slideInTimer.Start(); // Start the slide-in animation
        }

        private void StartCloseTimer()
        {
            // Timer for auto-close after 2 seconds
            System.Windows.Forms.Timer closeTimer = new System.Windows.Forms.Timer();
            closeTimer.Interval = 2000; // 2 seconds
            closeTimer.Tick += (sender, e) =>
            {
                closeTimer.Stop();
                StartSlideOutAnimation(); // Start slide-out animation
            };
            closeTimer.Start();
        }

        private void StartSlideOutAnimation()
        {
            // Timer for slide-out animation
            System.Windows.Forms.Timer slideOutTimer = new System.Windows.Forms.Timer();
            slideOutTimer.Interval = AnimationInterval;
            slideOutTimer.Tick += (sender, e) =>
            {
                // Move the form up
                this.Top -= AnimationStep;

                // Stop the timer and close the form when it goes off-screen
                if (this.Top <= -this.Height)
                {
                    slideOutTimer.Stop();
                    this.Close();
                }
            };
            slideOutTimer.Start(); // Start the slide-out animation
        }

        private void ErrorForm_Load(object sender, EventArgs e)
        {
            // No additional logic needed here
        }
    }
}