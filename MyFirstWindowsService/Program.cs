using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace MyFirstWindowsService
{
	internal static class Program
	{
		/// <summary>
		/// 應用程式的主要進入點。
		/// </summary>
		static void Main()
		{
			#region Serilog初始設定
			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Debug()
				.WriteTo.File(AppDomain.CurrentDomain.BaseDirectory + "\\logs\\log-.txt", rollingInterval: RollingInterval.Hour)
				.CreateLogger();

			Log.Debug("ServiceBase.Run(ServicesToRun)");
			#endregion

			ServiceBase[] ServicesToRun;
			ServicesToRun = new ServiceBase[]
			{
				new FirstService()
			};
			ServiceBase.Run(ServicesToRun);

			//Console模式的Debug
			//RunInteractive(ServicesToRun);
		}

		/// <summary>
		/// Console模式的Debug
		/// </summary>
		/// <param name="servicesToRun"></param>
		public static void RunInteractive(ServiceBase[] servicesToRun)
		{
			// 利用Reflection取得非公開之 OnStart() 方法資訊
			MethodInfo onStartMethod = typeof(ServiceBase).GetMethod("OnStart",
				BindingFlags.Instance | BindingFlags.NonPublic);

			// 執行 OnStart 方法
			foreach (ServiceBase service in servicesToRun)
			{
				Console.Write("Starting {0}...", service.ServiceName);
				onStartMethod.Invoke(service, new object[] { new string[] { } });
				Console.Write("Started");
			}

			Console.WriteLine("Press any key to stop the services");
			Console.ReadKey();

			// 利用Reflection取得非公開之 OnStop() 方法資訊
			MethodInfo onStopMethod = typeof(ServiceBase).GetMethod("OnStop",
				BindingFlags.Instance | BindingFlags.NonPublic);

			// 執行 OnStop 方法
			foreach (ServiceBase service in servicesToRun)
			{
				Console.Write("Stopping {0}...", service.ServiceName);
				onStopMethod.Invoke(service, null);
				Console.WriteLine("Stopped");
			}
		}
	}
}
