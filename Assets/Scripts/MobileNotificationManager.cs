using System;
using Unity.Notifications.Android;
using UnityEngine;

public class MobileNotificationManager : MonoBehaviour
{
    private AndroidNotificationChannel alerts_channel;
    private AndroidNotificationChannel events_channel;
    private AndroidNotificationChannel remind_channel;

    public const string ChannelID = "game_channel_0";
    public const string NewsChannelID = "news_channel_0";
    public const string ReminderChannelID = "rewind_channel_0";

    public const string default_smallIconName = "small_icon_0";
    public const string default_largeIconName = "large_icon_0";

    public const string retention_smallIconName = "small_icon_retention";
    public const string retention_largeIconName = "large_icon_retention";

    void Start()
    {
        alerts_channel = new AndroidNotificationChannel()
        {
            Id = ChannelID,
            Name = "Alerts",
            Description = "Alerts from game",
            Importance = Importance.Default,
        };

        events_channel = new AndroidNotificationChannel()
        {
            Id = NewsChannelID,
            Name = "News",
            Description = "News from game",
            Importance = Importance.Default,
        };

        remind_channel = new AndroidNotificationChannel() 
        {
            Id = ReminderChannelID,
            Name = "Reminders",
            Description = "Reminders",
            Importance = Importance.Default,
        };

        AndroidNotificationCenter.RegisterNotificationChannel(alerts_channel);
        AndroidNotificationCenter.RegisterNotificationChannel(events_channel);
        AndroidNotificationCenter.RegisterNotificationChannel(remind_channel);

        /*var notification = new AndroidNotification();
        notification.Title = LocalizationManager.instance.StringForKey("Notification_Title");
        notification.Text = LocalizationManager.instance.StringForKey("Notification_Text");
        notification.SmallIcon = "icon_small";
        notification.LargeIcon = "icon_large";
        notification.FireTime = DateTime.Now.AddHours(2);

        var id = AndroidNotificationCenter.SendNotification(notification, "channel_id");
        */
        // AndroidNotificationCenter.CancelAllDisplayedNotifications();
        AndroidNotificationCenter.CancelAllScheduledNotifications();

        CreateNotification("Collect money!", "It is good time to collect all earned money.", DateTime.UtcNow.AddSeconds(43200));

        RetentionReminderNotification(129600);
        RetentionReminderNotification(302400);
        RetentionReminderNotification(1296000);
    }

    public void CreateNotification(string title, string mainText, DateTime timeToFireNotification, string smallIcon = default_smallIconName, string largeIcon = default_largeIconName)
    {
        AndroidNotification notification = new AndroidNotification();
        //notification.Title = LocalizationManager.instance.StringForKey(title);
        notification.Title = title;
        //notification.Text = LocalizationManager.instance.StringForKey(mainText);
        notification.Text = mainText;
        notification.SmallIcon = smallIcon;
        notification.LargeIcon = largeIcon;
        notification.FireTime = timeToFireNotification;

        SendNotification(notification, ChannelID);
    }

    public void SendNotification(AndroidNotification notification, string channelName)
    {
        AndroidNotificationCenter.SendNotification(notification, channelName);
    }

    public void RetentionReminderNotification(int seconds)
    {
        int days = 0;
        if (seconds == 129600)
            days = 3;
        if (seconds == 302400)
            days = 7;
        if (days == 1296000)
            days = 30;

        CreateNotification
            (
                "We miss you!",
                string.Concat("You've been away for ", days, " days"),
                DateTime.UtcNow.AddSeconds(seconds),
                retention_smallIconName,
                retention_largeIconName
            );
    }

    public void TestNotification()
    {
        CreateNotification
            (
                "TEST!",
                string.Concat("TEST"),
                DateTime.UtcNow.AddSeconds(10),
                retention_smallIconName,
                retention_largeIconName
            );
    }
}
