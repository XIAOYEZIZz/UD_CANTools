using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using Avalonia.Threading;
using System;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;
using MessageBox = Ursa.Controls.MessageBox;
using Notification = Ursa.Controls.Notification;
using Toast = Ursa.Controls.Toast;
using WindowNotificationManager = Ursa.Controls.WindowNotificationManager;
using WindowToastManager = Ursa.Controls.WindowToastManager;
using canserver3.Models;
using canserver3.ViewModels;
using Ursa.Controls;

namespace canserver3.Services
{
    public sealed class FeedbackService
    {
        // 线程安全的单例实现
        private static readonly Lazy<FeedbackService> _instance =
            new Lazy<FeedbackService>(() => new FeedbackService());

        public static FeedbackService Instance => _instance.Value;
        private Window? _window;
        private WindowNotificationManager? _notificationManager;
        private WindowToastManager? _toastManager;
        public Window? Window
        {
            get => _window;
            set
            {
                _window = value;
                var topLevel = TopLevel.GetTopLevel(value);
                _notificationManager = new WindowNotificationManager(topLevel) { MaxItems = 5 };
                _notificationManager.Position = NotificationPosition.BottomRight;
                _toastManager = new WindowToastManager(topLevel) { MaxItems = 3 };
            }
        }

        public static void ShowInformationNotification(string message, string title = "提示")
        {
            void Show()
            {
                Instance._notificationManager?.Show(new Notification(title, message),
                 classes: ["Light"],
                 type: NotificationType.Information,
                 expiration: TimeSpan.FromSeconds(5));
            }
            ;
            if (Dispatcher.UIThread.CheckAccess())
            {
                Show();
            }
            else
            {
                Dispatcher.UIThread.Invoke(Show);
            }
        }
        public static void ShowSuccessNotification(string message, string title = "成功")
        {
            void Show()
            {
                Instance._notificationManager?.Show(new Notification(title, message),
                 classes: ["Light"],
                 type: NotificationType.Success,
                 expiration: TimeSpan.FromSeconds(5));
            }
            ;
            if (Dispatcher.UIThread.CheckAccess())
            {
                Show();
            }
            else
            {
                Dispatcher.UIThread.Invoke(Show);
            }
        }
        public static void ShowWarningNotification(string message, string title = "警告")
        {
            void Show()
            {
                Instance._notificationManager?.Show(new Notification(title, message),
                 classes: ["Light"],
                 type: NotificationType.Warning,
                 expiration: TimeSpan.FromSeconds(5));
            }
            ;
            if (Dispatcher.UIThread.CheckAccess())
            {
                Show();
            }
            else
            {
                Dispatcher.UIThread.Invoke(Show);
            }
        }
        public static void ShowErrorNotification(string message, string title = "错误")
        {
            void Show()
            {
                Instance._notificationManager?.Show(new Notification(title, message),
                 classes: ["Light"],
                 type: NotificationType.Error,
                 expiration: TimeSpan.FromSeconds(5));
            }
            ;
            if (Dispatcher.UIThread.CheckAccess())
            {
                Show();
            }
            else
            {
                Dispatcher.UIThread.Invoke(Show);
            }
        }
        public static void ShowInformationToast(string message, WindowToastManager? toastManager = null)
        {
            void Show()
            {
                if (toastManager != null)
                {
                    toastManager.Show(new Toast(message),
                    classes: ["Light"],
                    type: NotificationType.Information,
                    expiration: TimeSpan.FromSeconds(3));
                }
                else
                {
                    Instance._toastManager?.Show(new Toast(message),
                     classes: ["Light"],
                     type: NotificationType.Information,
                     expiration: TimeSpan.FromSeconds(3));
                }
            }
            ;
            if (Dispatcher.UIThread.CheckAccess())
            {
                Show();
            }
            else
            {
                Dispatcher.UIThread.Invoke(Show);
            }
        }
        public static void ShowSuccessToast(string message = "下发成功",WindowToastManager? toastManager = null)
        {
            void Show()
            {
                if (toastManager != null)
                {
                    toastManager.Show(new Toast(message),
                    classes: ["Light"],
                    type: NotificationType.Success,
                    expiration: TimeSpan.FromSeconds(3));
                }
                else
                {
                    Instance._toastManager?.Show(new Toast(message),
                     classes: ["Light"],
                     type: NotificationType.Success,
                     expiration: TimeSpan.FromSeconds(3));
                }
            }
            ;
            if (Dispatcher.UIThread.CheckAccess())
            {
                Show();
            }
            else
            {
                Dispatcher.UIThread.Invoke(Show);
            }

        }
        public static void ShowWarningToast(string message, WindowToastManager? toastManager = null)
        {
            void Show()
            {
                if (toastManager != null)
                {
                    toastManager.Show(new Toast(message),
                    classes: ["Light"],
                    type: NotificationType.Warning,
                    expiration: TimeSpan.FromSeconds(3));
                }
                else
                {
                    Instance._toastManager?.Show(new Toast(message),
                     classes: ["Light"],
                     type: NotificationType.Warning,
                     expiration: TimeSpan.FromSeconds(3));
                }
            }
            ;
            if (Dispatcher.UIThread.CheckAccess())
            {
                Show();
            }
            else
            {
                Dispatcher.UIThread.Invoke(Show);
            }
        }
        public static void ShowErrorToast(string message = "下发失败", WindowToastManager? toastManager = null)
        {
            void Show()
            {
                if (toastManager != null)
                {
                    toastManager.Show(new Toast(message),
                    classes: ["Light"],
                    type: NotificationType.Error,
                    expiration: TimeSpan.FromSeconds(3));
                }
                else
                {
                    Instance._toastManager?.Show(new Toast(message),
                     classes: ["Light"],
                     type: NotificationType.Error,
                     expiration: TimeSpan.FromSeconds(3));
                }
            }
            ;
            if (Dispatcher.UIThread.CheckAccess())
            {
                Show();
            }
            else
            {
                Dispatcher.UIThread.Invoke(Show);
            }
        }
        public static async Task ShowOkDiag(string messsage)
        {
            await OverlayMessageBox.ShowAsync(messsage, "提示", icon: Ursa.Controls.MessageBoxIcon.Information);
        }
    }

}
