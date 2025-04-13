namespace ToastNotification.Helper;

public class AccessPanelDoorsStatus
{
    readonly int _door;
    readonly int _alarm;

    public AccessPanelDoorsStatus(string door, string alarm)
    {
        this._door = int.TryParse(door, out var parsedDoor) ? parsedDoor : 0;
        this._alarm = int.TryParse(alarm, out var parsedAlarm) ? parsedAlarm : 0;
    }

    public bool IsDoorClosed(int i) => ((_door >> (i * 8)) & 255) == 1;

    public bool IsDoorOpen(int i) => ((_door >> (i * 8)) & 255) == 2;

    public bool IsDoorSensorWorking(int i) => ((_door >> (i * 8)) & 255) != 0;

    public bool IsAlarmOn(int i) => ((_alarm >> (i * 8)) & 255) != 0;

    public string ToString(int i)
    {
        var alarmStatus = IsAlarmOn(i) ? ", إنذار!" : "";
        if (IsDoorClosed(i)) return "مغلق" + alarmStatus;
        if (IsDoorOpen(i)) return "مفتوح" + alarmStatus;
        if (!IsDoorSensorWorking(i)) return "المستشعر لا يعمل" + alarmStatus;

        return "الكود " + ((_door >> (i * 8)) & 255) + alarmStatus;
    }

}