using Microsoft.AspNetCore.Mvc;
using MyMcRealms.Attributes;

// TODO: Add Notification typing class
namespace MyMcRealms.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [RequireMinecraftCookie]
    public class NotificationsController : ControllerBase
    {
        [HttpGet]
        public Object GetNotifications()
        {
            var n1 = new
            {
                NotificationUuid = "8e19f84925bf4efbacec9bed53d74696",
                Dismissable = true,
                Seen = false,
                Type = "visitUrl",
                Url = "https://my-mc.link/",
                ButtonText = new
                {
                    TranslationKey = "mco.notification.visitUrl.message.default",
                    Args = new List<object> { }
                },
                Message = new {
                        TranslationKey = "mco.notification.visitUrl.message.default",
                        Args = new List <object> { }
                    }
            };

            var n2 = new {
                NotificationUuid = "8e116d026aa643b394a2dfbcaaabb7ff",
                Dismissable = true,
                Seen = false,
                Type = "infoPopup",
                Title = new {
                    TranslationKey = "mco.notification.visitUrl.message.default",
                    Args = new List<object> { }
                },
                Message = new {
                    TranslationKey = "mco.notification.visitUrl.message.default",
                    Args = new List<Object> { }
                },
                Image = "notification/1",
                UrlButton = new {
                    Url = "https://my-mc.link/",
                    UrlText = new {
                            TranslationKey = "mco.notification.visitUrl.message.default",
                            Args = new List<Object> { }
                        }
                }
            };

            var notifs = new
            {
                Notifications = /* new List<Object> { n1, n2 } */ new List<Object> { }
            };

            return notifs;
        }

        [HttpPost("seen")]
        public List<object> MarkAsSeenNotifications()
        {
            return [];
        }

        [HttpPost("dissmiss")]
        public List<object> DissmissNotifications()
        {
            return [];
        }
    }
}