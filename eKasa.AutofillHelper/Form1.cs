﻿using System;
using System.Windows.Forms;

namespace eKasa.AutofillHelper {
	public partial class MainForm : Form {
		readonly string[] args = Environment.GetCommandLineArgs();
		public MainForm() {
			InitializeComponent();
			Type();
		}

		// TO DO --> Merge this with *.Core
		public void Type() {
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