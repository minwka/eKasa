using eKasa.Patcher.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace eKasa.Patcher
{
	public partial class Startup : Window
	{
		#region Variables
		string baseUri = "https://raw.githubusercontent.com";
		string repo = "minwka/eKasa-Updates";
		string branch = "stable";

		Uri patchInfoUri;
		PatchModel patchInfo = new();
		PatchModel localBinaries = new();

		string fileBaseUri = "";
		List<FileModel> patchingFiles = new();

		Hashtable Arguments = new();
		#endregion

		public Startup()
		{
			InitializeComponent();

			ParseArguments();
			InitialStatusCheck();
		}

		#region Window Actions
		private void QuitButton_Click(object sender, RoutedEventArgs e)
		{ Application.Current.Shutdown(); }

		private void HostWindow_MouseDown(object sender, MouseButtonEventArgs e)
		{ if (e.ChangedButton == MouseButton.Left) DragMove(); }
		#endregion

		#region Startup
		private void ParseArguments()
		{
			var args = Environment.GetCommandLineArgs();

			if ((args.Length - 1) % 2 == 0) {
				for (int i = 1; i < args.Length; i += 2) {
					Arguments[args[i].Replace("-", "").ToLower()] = args[i + 1];
				}
			}

			// Test Code --> Remove
			//if (Arguments != null) {
			//	foreach (var key in Arguments.Keys) {
			//		MessageBox.Show($"Key: {key} Value: {Arguments[key]}", "", MessageBoxButton.YesNo, MessageBoxImage.Information);
			//	}
			//}
		}

		private void InitialStatusCheck()
		{
			var vL = Assembly.GetExecutingAssembly().GetName().Version.ToString();
			VersionInfo.Text = $"Yüklü sürüm: {vL}";
		}
		#endregion

		#region Update
		async private void CheckUpdatesButton(object sender, RoutedEventArgs e)
		{
			#region Update Status
			PatchStatus.Text = "Kontrol ediliyor!";
			CheckPatchButton.Content = "Kontrol Ediliyor";
			CheckPatchButton.Style = (Style)Application.Current.Resources["OutlineWarningButton"];
			CheckPatchButton.IsEnabled = false;
			QuitButton.IsEnabled = false;
			#endregion

			ConstructUpdateUris();

			await FetchUpdateInfo();

			CheckIsUpdateRequired();
		}

		async private void ApplyUpdatesButton(object sender, RoutedEventArgs e)
		{
			#region Update Status
			PatchStatus.Text = $"Yeni dosyalar indiriliyor...";
			PatchStatus.Visibility = Visibility.Visible;

			CheckPatchButton.Content = "Güncelleniyor";
			CheckPatchButton.IsEnabled = false;
			CheckPatchButton.Style = (Style)Application.Current.Resources["OutlineWarningButton"];
			#endregion

			// Not fully async
			await ListInstalledBinaries();

			CompareInstalledBinaries();

			// Needs directory check
			FetchRemoteBinaries();

			#region Update Status
			PatchStatus.Visibility = Visibility.Hidden;

			QuitButton.IsEnabled = true;
			CheckPatchButton.Content = "Her Şey Güncel";
			CheckPatchButton.IsEnabled = false;
			CheckPatchButton.Style = (Style)Application.Current.Resources["PrimaryButton"];

			UpdateStatus.Text = "Her Şey Güncellendi!";
			VersionInfo.Text = $"Mevcut sürüm: {patchInfo.Version}";
			#endregion
		}
		#endregion

		#region Update Procedure

		private void ConstructUpdateUris()
		{
			var IsRepoPresent = Arguments["repo"] != null ? !string.IsNullOrEmpty(Arguments["repo"].ToString()) : false;
			var IsBranchPresent = Arguments["branch"] != null ? !string.IsNullOrEmpty(Arguments["branch"].ToString()) : false;

			if (IsRepoPresent && IsBranchPresent) {
				patchInfoUri = new Uri($"{baseUri}/{Arguments["repo"]}/updates/{Arguments["branch"]}.json");
				fileBaseUri = $"{baseUri}/{Arguments["repo"]}/files/";
			}
			else {
				patchInfoUri = new Uri($"{baseUri}/{repo}/updates/{branch}.json");
				fileBaseUri = $"{baseUri}/{repo}/files/";
			}
		}

		private async Task FetchUpdateInfo()
		{
			HttpClient client = new();

			var rawJson = await Task.Run(() => client.GetStringAsync(patchInfoUri));
			patchInfo = JsonSerializer.Deserialize<PatchModel>(rawJson);
		}

		private void CheckIsUpdateRequired()
		{
			var vL = Assembly.GetEntryAssembly().GetName().Version;
			var vR = patchInfo.Version.Split('.');

			var IsMajorNewer = vL.Major < Convert.ToInt32(vR[0]);
			var IsMinorNewer = vL.Minor < Convert.ToInt32(vR[1]);
			var IsPatchNewer = vL.Build < Convert.ToInt32(vR[2]);
			var IsBuildNewer = vL.Revision < Convert.ToInt32(vR[3]);

			if (IsMajorNewer || IsMinorNewer || IsPatchNewer || IsBuildNewer) {
				#region Update Status
				UpdateStatus.Text = "Yeni Sürüm Mevcut";
				VersionInfo.Text = $"Yüklü sürüm: {vL} -> Güncel sürüm {patchInfo.Version}";
				ReleaseDescription.Text = patchInfo.Description;

				CheckPatchButton.Content = "Güncelle";
				CheckPatchButton.Style = (Style)Application.Current.Resources["SuccessButton"];

				CheckPatchButton.Click -= CheckUpdatesButton;
				CheckPatchButton.Click += ApplyUpdatesButton;

				CheckPatchButton.IsEnabled = true;
				QuitButton.IsEnabled = true;
				#endregion
			}
			else {
				#region Update Status
				UpdateStatus.Text = "Sürümünüz Güncel";
				ReleaseDescription.Text = patchInfo.Description;

				CheckPatchButton.Content = "Her Şey Güncel";
				CheckPatchButton.Style = (Style)Application.Current.Resources["PrimaryButton"];

				CheckPatchButton.IsEnabled = false;
				QuitButton.IsEnabled = true;
				#endregion
			}
		}

		async private Task ListInstalledBinaries()
		{
			// Create local version info
			localBinaries.Branch = "stable";
			localBinaries.Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
			localBinaries.Description = "Local Binaries";

			// Get files in the directory
			string rootDir = Environment.CurrentDirectory;
			string[] files = await Task.Run(() => Directory.GetFiles(rootDir, "*.*", new EnumerationOptions() {
				RecurseSubdirectories = true
			}));

			// Save file names, versions and hashes
			var fl = new List<FileModel>();
			foreach (var file in files) {
				var f = new FileModel();
				f.Path = file[rootDir.Length..].Trim('\\');
				f.Version = FileVersionInfo.GetVersionInfo(file).FileVersion;

				using (SHA1CryptoServiceProvider scsp = new()) {
					f.Hash = Convert.ToHexString(scsp.ComputeHash(File.ReadAllBytes(file)));
				}

				fl.Add(f);
			}
			localBinaries.FileList = fl;
		}

		private void CompareInstalledBinaries()
		{
			foreach (var file in patchInfo.FileList) {
				var fileMissingOrOld = localBinaries.FileList.Find(local => local.Hash == file.Hash) == null;

				if (fileMissingOrOld) {
					var nf = new FileModel() {
						Source = $"{fileBaseUri}/{file.Path}",
						Path = file.Path,
						Hash = file.Hash,
					};

					patchingFiles.Add(nf);
				}
			}
		}

		private void FetchRemoteBinaries()
		{
			foreach (var file in patchingFiles) {
				WebClient wc = new();
				wc.DownloadFile(file.Source, file.Path);
			}
		}
		#endregion
	}
}