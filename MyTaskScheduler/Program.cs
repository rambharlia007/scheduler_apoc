using Microsoft.CSharp.RuntimeBinder;
using Microsoft.Win32.TaskScheduler;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace MyTaskScheduler
{   
    internal class Program
    {

        private static void Main(string[] args)
        {
            Program.ExecuteJob();
        }

        public static void ExecuteJob()
        {
            try
            {
                Program.ScheduleWindowsJob();
                Program.DoTask();
            }
            catch (Exception ex) { }
        }

        private static void ScheduleWindowsJob()
        {
            using (TaskService taskService = new TaskService())
            {
                if (taskService.RootFolder.SubFolders.Exists("SharbedgeEmailNotification") && taskService.GetFolder("SharbedgeEmailNotification").Tasks.Exists("PaymentEmailNotification")) { }
                else
                {
                    TaskDefinition taskDefinition = taskService.NewTask();
                    taskDefinition.RegistrationInfo.Author = "JobAuthor";
                    taskDefinition.RegistrationInfo.Date = DateTime.Now;
                    taskDefinition.RegistrationInfo.Description = "JobDescription";
                    taskDefinition.RegistrationInfo.Documentation = "JobDocumentation";
                    taskDefinition.RegistrationInfo.Version = new Version(0, 1);
                    taskDefinition.Triggers.AddRange(GetTaskTriggers());
                    taskDefinition.Actions.AddRange(GetTaskActions());
                    if (!taskService.RootFolder.SubFolders.Exists("SharbedgeEmailNotification"))
                        taskService.RootFolder.CreateFolder("SharbedgeEmailNotification", (string)null, true);
                    taskService.GetFolder("SharbedgeEmailNotification").RegisterTaskDefinition("PaymentEmailNotification", taskDefinition);
                }
            }
        }

        public static void DoTask()
        {
            Console.WriteLine("Hi ram bharlia");
        }

        public static IList<Trigger> GetTaskTriggers()
        {
            return new List<Trigger>() {
    new DailyTrigger {
     DaysInterval = 1, Enabled = true, Repetition = new RepetitionPattern(TimeSpan.FromMinutes(1), TimeSpan.Zero), StartBoundary = DateTime.UtcNow.Date
    }
   };
        }


        public static IList<Microsoft.Win32.TaskScheduler.Action> GetTaskActions()
        {

            return new List<Microsoft.Win32.TaskScheduler.Action>() {
    new ExecAction(Path.GetFileName(Assembly.GetEntryAssembly().Location), null, Directory.GetCurrentDirectory())
   };
        }
    }
}