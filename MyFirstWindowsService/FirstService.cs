using Serilog;
using System;
using System.ServiceProcess;
using System.Timers;

namespace MyFirstWindowsService
{
	public partial class FirstService : ServiceBase
	{
		private ILogger _logger;

		private Timer timer;

		public FirstService()
		{
			InitializeComponent();

			this._logger = Log.Logger;
			_logger.Debug("FirstService log start...");
		}

		protected override void OnStart(string[] args)
		{
			// Setup timer
			timer = new Timer();
			timer.Interval = 60000;//60秒
			timer.Elapsed += new ElapsedEventHandler(this.OnTimer);
			timer.Start();
			_logger.Debug("Start service...");
		}

		protected override void OnStop()
		{
			timer.Stop();
			_logger.Debug("Stop service...");
		}

		protected void OnTimer(object sender, ElapsedEventArgs args)
		{
			try
			{
				_logger.Debug("OnTimer");
			}
			catch (Exception ex)
			{
				_logger.Error("OnTimer exception", ex);
			}
		}
	}
}
