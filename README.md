# Toast Notification For ZKTeco Access Controller 

## Overview

**ToastNotificationForAccessController** is a .NET application designed to manage access controllers, providing real-time status updates and error handling via toast notifications.

This application simplifies the monitoring and management of access panel events by establishing continuous connections, processing real-time logs, and alerting users of important events or connection issues.

---

## Features

1. **Real-Time Event Monitoring:**
   - Captures and displays access panel events like door status, card authentication, and alarm triggers.

2. **Toast Notifications:**
   - Displays categorized notifications such as Success, Error, and Warning, ensuring users are promptly informed of critical events.

3. **Robust Connection Handling:**
   - Implements retry mechanisms for connection attempts with configurable delays and maximum retries.

4. **Configurable IP Address:**
   - Reads the access controller IP from an external configuration file.

---

## Configuration

### File: `config.txt`

**Location:**
   - The `config.txt` file must be placed in the `Config` folder within the project directory.
   - The file will be copied to the output directory (e.g., `bin/Debug/Config/`).

**Content Format:**
```
192.168.1.1
```
- Replace `192.168.1.1` with the IP address of your access controller.

---

## Usage

1. **Setup the Configuration File:**
   - Add the IP of your access controller to `Config/config.txt`.

2. **Run the Application:**
   - Launch the application. It will attempt to connect to the access controller using the IP specified in the configuration file.

3. **Monitor Events:**
   - Receive toast notifications for connection status, door events, and alarms.

---

## Code Overview

### Key Components

1. **`MainForm`:**
   - Handles connection attempts, periodic retries, and event log processing.

2. **`ShowToast`:**
   - Displays categorized notifications for user feedback.

3. **`config.txt`:**
   - Provides flexibility to configure the access controller's IP address without modifying code.

### Key Methods

#### `MainForm_Load`
Initializes the application, loads the configuration file, and starts connection attempts.

#### `ContinuousConnectionAttempts`
Implements a loop to retry connections with delays between cycles.

#### `AttemptConnectionCycle`
Attempts multiple connection retries within a single cycle and displays appropriate notifications.

#### `ShowToast`
Creates and displays a notification form for user messages.

---

## Deployment

1. Build the project in Visual Studio.
2. Ensure the `Config` folder (with `config.txt`) is included in the output directory.
3. Deploy the application folder to the target environment.

---

## Future Enhancements

- Add support for multiple IP configurations.
- Enhance UI for better event visualization.
- Introduce detailed logs and report export functionality.

---

## License

This project is licensed under the MIT License. See the `LICENSE` file for details.

---

## Contributions

Contributions are welcome! Please submit a pull request or open an issue to suggest improvements or report bugs.

---

## Contact

For questions or support, please contact:
- Email: [mohamed.basher.daoud@gmail.com](mailto:mohamed.basher.daoud@gmail.com)

