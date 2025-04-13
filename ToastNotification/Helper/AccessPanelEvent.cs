namespace ToastNotification.Helper;

public class AccessPanelEvent
{
    public readonly AccessPanelDoorsStatus? DoorsStatus;
    public readonly List<AccessPanelRtEvent> Events;

    public AccessPanelEvent(AccessPanelDoorsStatus? doorsStatus, List<AccessPanelRtEvent> events)
    {
        this.DoorsStatus = doorsStatus;
        this.Events = events;
    }
}