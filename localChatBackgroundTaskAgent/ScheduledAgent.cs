using System.Diagnostics;
using System.Windows;
using Microsoft.Phone.Scheduler;

using System.Linq;
using Microsoft.Phone.Shell;
using Microsoft.Phone.Shell.Interop;

namespace localChatBackgroundTaskAgent
{
    public class ScheduledAgent : ScheduledTaskAgent
    {
        /// <remarks>
        /// ScheduledAgent constructor, initializes the UnhandledException handler
        /// </remarks>
        static ScheduledAgent()
        {
            // Subscribe to the managed exception handler
            Deployment.Current.Dispatcher.BeginInvoke(delegate
            {
                Application.Current.UnhandledException += UnhandledException;
            });
        }

        /// Code to execute on Unhandled Exceptions
        private static void UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                Debugger.Break();
            }
        }

        /// <summary>
        /// Agent that runs a scheduled task
        /// </summary>
        /// <param name="task">
        /// The invoked task
        /// </param>
        /// <remarks>
        /// This method is called when a periodic or resource intensive task is invoked
        /// </remarks>
        protected override void OnInvoke(ScheduledTask task)
        {
            int newMsgThisTime = 3;
            //Perform your task in background

            updateTost(newMsgThisTime);
            updateLiveTile(newMsgThisTime);

            NotifyComplete();
        }

        private void updateLiveTile(int msgCount)
        {
            /* Update title contents */
            var mainTile = ShellTile.ActiveTiles.FirstOrDefault();

            if (null != mainTile)
            {
                FlipTileData tileData = new FlipTileData()
                {
                    Count = msgCount
                };

                switch (msgCount)
                {
                    case 0:
                        tileData.BackContent = string.Empty;
                        break;
                    case 1:
                        tileData.BackContent = "You have " + msgCount + " post you have not read!";
                        break;
                    default:
                        tileData.BackContent = "You have " + msgCount + " posts you have not read!";
                        break;
                }

                mainTile.Update(tileData);
            }
        }

        private void updateTost(int msgCount)
        {
           ShellToast toast = new ShellToast();
           toast.Title = "New Message Received";
           switch (msgCount)
           {
               case 0:
                   toast.Content = "No new post came in this time.";
                   break;
               case 1:
                   toast.Content = "You have " + msgCount + " post you have not read!";
                   break;
               default:
                   toast.Content = "You have " + msgCount + " posts you have not read!";
                   break;
           }

           toast.Show();
        }

    }
}