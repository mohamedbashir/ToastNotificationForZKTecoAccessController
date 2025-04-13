namespace ToastNotification.Helper;

public class AccessPanelRtEvent
{
    public readonly string Time;
    public readonly string Pin;
    public readonly string Door;
    public readonly int Card;
    public readonly int EventType;
    public readonly int InOrOut;

    public AccessPanelRtEvent(string time, string pin, string door, int eventType, int inOrOut)
    {
        Time = time;
        Pin = pin;
        Door = door;
        EventType = eventType;
        InOrOut = inOrOut;
    }

    public AccessPanelRtEvent(string time, string pin, int card, string door, int eventType, int inOrOut) : this(time, pin, door, eventType, inOrOut)
    {
        Card = card;
    }

    public int GetDoorId()=> 
        !string.IsNullOrEmpty(Door) && int.TryParse(Door, out var doorId) ? doorId : -1;
    

    public override string ToString()
    {
        string? eventDescription = GetDescription(EventType) ?? "Unknown event";

        if (!string.IsNullOrWhiteSpace(Pin) && !"0".Equals(Pin))
        {
            eventDescription += ", " + (InOrOut == 0 ? "Entry" : (InOrOut == 1 ? "Exit" : "User")) + ": " + Pin;
        }

        if (!string.IsNullOrWhiteSpace(Door) && Door != "0")
        {
            eventDescription += ", Door: " + Door;
        }

        return eventDescription;
    }

    //public static string? GetDescription(int code)
    //{
    //    string[] e =
    //    {
    //        /*00*/ "Normal Punch Open",
    //        /*01*/ "Punch during Normal Open Time Zone",
    //        /*02*/ "First Card Normal Open",
    //        /*03*/ "Multi-Card Open",
    //        /*04*/ "Emergency Password Open",
    //        /*05*/ "Open during Normal Open Time Zone",
    //        /*06*/ "Linkage Event Triggered",
    //        /*07*/ "Alarm Canceled",
    //        /*08*/ "Remote Opening",
    //        /*09*/ "Remote Closing",
    //        /*10*/ "Disable Intraday Normal Open Time Zone",
    //        /*11*/ "Enable Intraday Normal Open Time Zone",
    //        /*12*/ "Open Auxiliary Output",
    //        /*13*/ "Close Auxiliary Output",
    //        /*14*/ "Press Fingerprint Open",
    //        /*15*/ "Multi-Card Open",
    //        /*16*/ "Press Fingerprint during Normal Open Time Zone",
    //        /*17*/ "Card plus Fingerprint Open",
    //        /*18*/ "First Card Normal Open",
    //        /*19*/ "First Card Normal Open",
    //        /*20*/ "Too Short Punch Interval",
    //        /*21*/ "Door Inactive Time Zone",
    //        /*22*/ "llegal Time Zone",
    //        /*23*/ "Access Denied",
    //        /*24*/ "Anti-Passback",
    //        /*25*/ "Interlock",
    //        /*26*/ "Multi-Card Authentication",
    //        /*27*/ "Unregistered Card",
    //        /*28*/ "Opening Timeout",
    //        /*29*/ "Card Expired",
    //        /*30*/ "Password Error",
    //        /*31*/ "Too Short Fingerprint Pressing Interval",
    //        /*32*/ "Multi-Card Authentication",
    //        /*33*/ "Fingerprint Expired",
    //        /*34*/ "Unregistered Fingerprint",
    //        /*35*/ "Door Inactive Time Zone",
    //        /*36*/ "Door Inactive Time Zone",
    //        /*37*/ "Failed to Close during Normal Open Time Zone",
    //    };
    //    if (code < 37 && code > -1)
    //    {
    //        return e[code];
    //    }

    //    switch (code)
    //    {
    //        case 101:
    //            return "Duress Password Open";
    //        case 102:
    //            return "Opened Accidentally";
    //        case 103:
    //            return "Duress Fingerprint Open";
    //        case 200:
    //            return "Door Opened Correctly";
    //        case 204:
    //            return "Normal Open Time Zone Over";
    //        case 205:
    //            return "Remote Normal Opening";
    //        case 206:
    //            return "Device Start";
    //        case 220:
    //            return "Auxiliary Input Disconnected";
    //        case 221:
    //            return "Auxiliary Input Shorted";
    //    }

    //    return null;
    //}

    public static string? GetDescription(int code)
    {
        var descriptions = new Dictionary<int, string>
    {
        { 0, "فتح عادي بالاشتراك" },
        { 1, "تم تمرير الاشتراك خلال فترة الفتح العادي" },
        { 2, "فتح الاشتراك الأول بشكل عادي" },
        { 3, "فتح متعدد الاشتراكات" },
        { 4, "فتح بكلمة مرور الطوارئ" },
        { 5, "فتح خلال فترة الفتح العادي" },
        { 6, "تم تفعيل حدث الربط" },
        { 7, "تم إلغاء الإنذار" },
        { 8, "فتح عن بُعد" },
        { 9, "إغلاق عن بُعد" },
        { 10, "تعطيل فترة الفتح العادي خلال اليوم" },
        { 11, "تمكين فترة الفتح العادي خلال اليوم" },
        { 12, "فتح المخرج المساعد" },
        { 13, "إغلاق المخرج المساعد" },
        { 14, "فتح ببصمة الإصبع" },
        { 15, "فتح متعدد الاشتراكات" },
        { 16, "تم تمرير الاشتراك خلال فترة الفتح العادي" },
        { 17, "فتح بالاشتراك مع بصمة الإصبع" },
        { 18, "فتح الاشتراك الأول بشكل عادي" },
        { 19, "فتح الاشتراك الأول بشكل عادي" },
        { 20, "فترة تمرير الاشتراك قصيرة جدًا" },
        { 21, "الاشتراك خارج نطاق الوقت المسموح" },
        { 22, "الاشتراك خارج الفترة الزمنية المسموحة" },
        { 23, "تم رفض الوصول" },
        { 24, "مخالفة قاعدة الدخول والخروج (Anti-Passback)" },
        { 25, "قفل بيني (Interlock)" },
        { 26, "مصادقة متعددة الاشتراكات" },
        { 27, "اشتراك غير مسجل" },
        { 28, "انتهاء مهلة الفتح" },
        { 29, "الاشتراك منتهي الصلاحية" },
        { 30, "خطأ في كلمة المرور" },
        { 31, "مدة الضغط على البصمة قصيرة جدًا" },
        { 32, "مصادقة متعددة الاشتراكات" },
        { 33, "انتهاء صلاحية البصمة" },
        { 34, "بصمة غير مسجلة" },
        { 35, "الاشتراك خارج نطاق الوقت المسموح" },
        { 36, "الاشتراك خارج نطاق الوقت المسموح" },
        { 37, "فشل في الإغلاق خلال فترة الفتح العادي" },

        // Descriptions that require special cases
        { 101, "فتح بكلمة مرور الإكراه" },
        { 102, "تم الفتح بالخطأ" },
        { 103, "فتح ببصمة الإكراه" },
        { 200, "تم فتح الباب بنجاح" },
        { 204, "انتهاء فترة الفتح العادي" },
        { 205, "فتح عادي عن بُعد" },
        { 206, "بدء تشغيل الجهاز" },
        { 220, "تم فصل الإدخال المساعد" },
        { 221, "تم قصر الإدخال المساعد" }
    };

        if (descriptions.ContainsKey(code))
        {
            return descriptions[code];
        }

        return null;
    }
}