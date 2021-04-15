using System;
using System.IO;
using System.Windows;

namespace eKasa.Core
{
	public class Logger
	{
		public string Filepath { get; }

		public Logger(string filepath)
		{ Filepath = filepath; }

		private static void ShowMessage(string msg, string title, MessageBoxImage img)
		{ MessageBox.Show(msg, title, MessageBoxButton.OK, img); }

		public void Log(Exception ex)
		{
			File.AppendAllText(Filepath, $"Error date/time: {DateTime.UtcNow.ToLocalTime()}\n" +
				$"Error message: {ex.Message}\n" +
				$"Error stacktrace: {ex.StackTrace}\n" +
				$"Error inner exception: {ex.InnerException}\n\n\n");
		}

		public void Error(Exception ex, string msg, string title)
		{
			ShowMessage(msg, title, MessageBoxImage.Error);
			Log(ex);
		}

		public void Error(Exception ex)
		{
			string msg = "Beklenmedik bir hata oluştu!\nLütfen kayıtlara göz atın.";
			string title = "Hata!";
			Error(ex, msg, title);
		}

		public void Info(Exception ex, string msg, string title)
		{
			ShowMessage(msg, title, MessageBoxImage.Information);
			Log(ex);
		}

		public void Info(Exception ex)
		{
			string msg = "Beklenmedik bir hata oluştu!\nLütfen kayıtlara göz atın.";
			string title = "Hata!";
			Info(ex, msg, title);
		}

		public void Warning(Exception ex, string msg, string title)
		{
			ShowMessage(msg, title, MessageBoxImage.Warning);
			Log(ex);
		}

		public void Warning(Exception ex)
		{
			string msg = "Beklenmedik bir hata oluştu!\nLütfen kayıtlara göz atın.";
			string title = "Hata!";
			Warning(ex, msg, title);
		}
	}
}