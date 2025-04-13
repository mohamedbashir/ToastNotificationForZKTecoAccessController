namespace ToastNotification.Helper;

public class AccessPanelDoorsStatus
{
    readonly int _door;
    readonly int _alarm;

    public AccessPanelDoorsStatus(string door, string alarm)
    {
        this._door = int.Parse(door);
        this._alarm = int.Parse(alarm);
    }

    public bool IsDoorClosed(int i)
    {
        return ((_door >> (i * 8)) & 255) == 1;
    }

    public bool IsDoorOpen(int i)
    {
        return ((_door >> (i * 8)) & 255) == 2;
    }

    public bool IsDoorSensorWorking(int i)
    {
        return ((_door >> (i * 8)) & 255) != 0;
    }

    public bool IsAlarmOn(int i)
    {
        return ((_alarm >> (i * 8)) & 255) != 0;
    }

    public string ToString(int i)
    {
        string a = (IsAlarmOn(i) ? ", إنذار!" : "");
        if (IsDoorClosed(i))
        {
            return "مغلق" + a;
        }

        if (IsDoorOpen(i))
        {
            return "مفتوح" + a;
        }

        if (!IsDoorSensorWorking(i))
        {
            return "المستشعر لا يعمل" + a;
        }

        return "الكود " + ((_door >> (i * 8)) & 255) + a;
    }

}