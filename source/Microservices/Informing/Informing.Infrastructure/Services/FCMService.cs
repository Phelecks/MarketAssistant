using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Informing.Application.Interfaces;
using LoggerService.Helpers;
using Microsoft.Extensions.Logging;

namespace Informing.Infrastructure.Services;

public class FCMService : IFCMService
{
    private readonly FirebaseApp _fireBaseApp;
    private readonly ILogger<FCMService> _logger;
    private readonly IApplicationDbContext _context;

    public FCMService(IApplicationDbContext context, ILogger<FCMService> logger)
    {
        _context = context;
        _logger = logger;

        if (FirebaseApp.DefaultInstance != null)
            _fireBaseApp = FirebaseApp.DefaultInstance;
        else
            _fireBaseApp = FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FCM.json"))
            });
    }

    public async Task SendMessagePushNotificationAsync(SendMessagePushNotificationDto dto, CancellationToken cancellationToken)
    {
        try
        {
            //Message message;
            //if (platformType == PlatformType.Android)
            //    message = new Message
            //    {
            //        //Fill this property if you want to pass notification directly to application instead of OS
            //        //Data = new Dictionary<string, string>
            //        //{
            //        //    ["type"] = callType == TwilioEnums.CallType.Video
            //        //        ? ((int)NotificationType.Video).ToString()
            //        //        : ((int)NotificationType.Voice).ToString(),
            //        //    ["roomName"] = roomName,
            //        //    ["callerFullname"] = callerFullname,
            //        //    ["callerTwilioIdentity"] = callerTwilioIdentity
            //        //},
            //        //Fill this property if you want to send notification to OS instead of application
            //        // Notification = new FirebaseAdmin.Messaging.Notification
            //        // {
            //        //     Title = "title",
            //        //     Body = "body"
            //        //},
            //        Token = deviceToken,
            //        Android = new AndroidConfig
            //        {
            //            Priority = Priority.High,
            //            //TimeToLive = TimeSpan.FromDays(28), //Default is 4 weeks
            //            CollapseKey = "dilib calls",
            //            RestrictedPackageName = "com.sunnymehr.dilib",
            //            Data = new Dictionary<string, string>
            //            {
            //                ["type"] = callType == TwilioEnums.CallType.Video
            //                    ? ((int)NotificationType.Video).ToString()
            //                    : ((int)NotificationType.Voice).ToString(),
            //                ["roomName"] = roomName,
            //                ["callerFullname"] = callerFullname,
            //                ["callerTwilioIdentity"] = callerTwilioIdentity
            //            }
            //        }
            //    };
            //else if (platformType == PlatformType.Ios)
            //    message = new Message
            //    {
            //        //Fill this property if you want to pass notification directly to application instead of OS
            //        Data = new Dictionary<string, string>
            //        {
            //            ["type"] = callType == TwilioEnums.CallType.Video
            //                ? ((int)NotificationType.Video).ToString()
            //                : ((int)NotificationType.Voice).ToString(),
            //            ["roomName"] = roomName,
            //            ["callerFullname"] = callerFullname,
            //            ["callerTwilioIdentity"] = callerTwilioIdentity
            //        },
            //        //Fill this property if you want to send notification to OS instead of application
            //        Notification = new FirebaseAdmin.Messaging.Notification
            //        {
            //            Title = title,
            //            Body = body
            //        },
            //        Token = deviceToken,
            //        Apns = new ApnsConfig
            //        {
            //            CustomData = new Dictionary<string, object>
            //            {
            //                ["type"] = callType == TwilioEnums.CallType.Video
            //                    ? ((int)NotificationType.Video).ToString()
            //                    : ((int)NotificationType.Voice).ToString(),
            //                ["roomName"] = roomName,
            //                ["callerFullname"] = callerFullname,
            //                ["callerTwilioIdentity"] = callerTwilioIdentity
            //            },
            //            FcmOptions = new ApnsFcmOptions
            //            {

            //            },
            //            Aps = new Aps
            //            {
            //                Sound = "Default"
            //            }
            //        }
            //    };
            //else
            //    message = new Message
            //    {
            //        ////Fill this property if you want to pass notification directly to application instead of OS
            //        //Data = new Dictionary<string, string>
            //        //{
            //        //    ["type"] = callType == TwilioEnums.CallType.Video
            //        //        ? ((int)NotificationType.Video).ToString()
            //        //        : ((int)NotificationType.Voice).ToString(),
            //        //    ["roomName"] = roomName,
            //        //    ["callerFullname"] = callerFullname,
            //        //    ["callerTwilioIdentity"] = callerTwilioIdentity
            //        //},
            //        //////Fill this property if you want to send notification to OS instead of application
            //        //Notification = new FirebaseAdmin.Messaging.Notification
            //        //{
            //        //    Title = title,
            //        //    Body = body
            //        //},
            //        Token = deviceToken,
            //        Webpush = new WebpushConfig
            //        {
            //            Data = new Dictionary<string, string>
            //            {
            //                ["type"] = callType == TwilioEnums.CallType.Video
            //                    ? ((int)NotificationType.Video).ToString()
            //                    : ((int)NotificationType.Voice).ToString(),
            //                ["roomName"] = roomName,
            //                ["callerFullname"] = callerFullname,
            //                ["callerTwilioIdentity"] = callerTwilioIdentity
            //            }
            //        }
            //    };

            var fcmMessage = new Message
            {
                Token = dto.deviceToken,
                Webpush = new WebpushConfig
                {
                    Data = new Dictionary<string, string>
                    {
                        ["conversationTitle"] = dto.conversationTitle,
                        ["message"] = dto.message
                    }
                }
            };

            var messaging = FirebaseMessaging.DefaultInstance;
            var result = await messaging.SendAsync(fcmMessage);
        }
        catch (Exception exception)
        {
            _ = Task.Run(() => _logger.LogError(
                   eventId: EventTool.GetEventInformation(eventType: EventType.Informing, eventName: "Push notification error"), exception,
                   "Cannot send notification with data: {@dto}", dto), cancellationToken);
        }
    }
}