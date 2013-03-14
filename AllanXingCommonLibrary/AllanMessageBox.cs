using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.System.Threading;
using Windows.UI.Core;
using Windows.UI.Popups;

namespace AllanXingCommonLibrary
{
    public class AllanMessageBox
    {
        static Queue<MessageDialog> MessageQueue = new Queue<MessageDialog>(5);
        volatile static bool _isShowing = false;
        /// <summary>
        /// CancelCommandIndex[0]  is  close button;
        /// </summary>
        /// <param name="content"></param>
        /// <param name="title"></param>
        /// <param name="process"></param>
        public static void Show(string content, string title = null, Func<MessageDialog, MessageDialog> process = null)
        {
            MessageDialog md = title == null ? new MessageDialog(content) : new MessageDialog(content, title);
            if (process != null)
            {
                md = process(md);
            }
            md.CancelCommandIndex = 0;
            md.Commands.Add(new UICommand("Close", new UICommandInvokedHandler((i) =>
            {
                innerShow();
            })));

            MessageQueue.Enqueue(md);
            if (!_isShowing)
            {
                _isShowing = true;
                innerShow();
            }
        }

        private static void innerShow()
        {
            var md = MessageQueue.Dequeue();
            if (md != null)
            {
                //Windows.UI.Core.CoreDispatcher disp = CoreWindow.GetForCurrentThread().Dispatcher;
                ThreadPoolTimer.CreateTimer(async (o) =>
                {
                    await md.ShowAsync();
                }, new TimeSpan(0, 0, 0, 1));
            }
            else
            {
                _isShowing = false;
            }
        }
    }
}
