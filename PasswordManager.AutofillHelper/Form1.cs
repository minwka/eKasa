using System;
using System.Windows.Forms;

namespace PasswordManager.AutofillHelper
{
	public partial class mainForm : Form
	{
		string[] args = Environment.GetCommandLineArgs();
		public mainForm()
		{
			InitializeComponent();
			type();
		}

		public void type()
		{
			try {
				WindowState = FormWindowState.Minimized;
				switch (args[1]) {
					case "s":
						SendKeys.SendWait($"{args[2]}");
						break;
					case "m":
						SendKeys.SendWait($"{args[2]}{{TAB}}{args[3]}");
						break;
					case "ma":
						SendKeys.SendWait($"{args[2]}{{TAB}}{args[3]}{{ENTER}}");
						break;
				}
				Close();
			} catch {
				Close();
			}
		}
	}
}