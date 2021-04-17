using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using static eKasa.Core.Settings;

namespace eKasa.Core.UserControls
{
	public partial class HomeUserControl : UserControl
	{
		public HomeUserControl()
		{
			InitializeComponent();

			titleLabel.Text = $"Hoşgeldin, {dbSettings.InternalDb.Owner}!";
			entriesDataGrid.ItemsSource = dbSettings.InternalDb.Entries;
			UpdateDbHint();
		}

		private void UpdateDbHint()
		{
			var idb = dbSettings.InternalDb;

			titleLabel.Text = $"Hoşgeldin, {idb.Owner}!";
			var modifiedDate = Convert.ToDateTime(idb.ModifiedDate).ToLocalTime().ToShortDateString();
			var modifiedTime = Convert.ToDateTime(idb.ModifiedDate).ToLocalTime().ToShortTimeString();
			dbDataLabel.Content = $"DB: {idb.Name}, Son değişiklik: {modifiedDate} - {modifiedTime}";

			entriesDataGrid.SelectedIndex = -1;
		}

		private void OptionsButton_Click(object sender, RoutedEventArgs e)
		{
			if (entriesDataGrid.SelectedIndex == -1) entriesDataGrid.SelectedIndex = 0;

			var canvas = (Canvas)Parent;
			var ouc = new OptionsUserControl();
			canvas.Children.Add(ouc);
		}

		private void EditButton_Click(object sender, RoutedEventArgs e)
		{
			if (entriesDataGrid.SelectedIndex == -1) entriesDataGrid.SelectedIndex = 0;
			var oldEntry = (EntryModel)entriesDataGrid.SelectedItem;
			if (oldEntry != null) {
				ManageDbWindow.edituc.idPreview.Text = oldEntry.Id.ToString();
				ManageDbWindow.edituc.namePreview.Text = oldEntry.Name;
				ManageDbWindow.edituc.usernamePreview.Text = oldEntry.Username;
				ManageDbWindow.edituc.pwdPreview.Password = oldEntry.Password;
				ManageDbWindow.edituc.tagPreview.Text = oldEntry.Tag;
				ManageDbWindow.edituc.nameInput.Text = oldEntry.Name;
				ManageDbWindow.edituc.usernameInput.Text = oldEntry.Username;
				ManageDbWindow.edituc.tagInput.Text = oldEntry.Tag;

				var parent = (Canvas)Parent;
				parent.Children.Clear();
				parent.Children.Add(ManageDbWindow.edituc);
			}
		}

		private void DeleteButton_Click(object sender, RoutedEventArgs e)
		{
			if (entriesDataGrid.SelectedIndex == -1) entriesDataGrid.SelectedIndex = 0;

			dbSettings.InternalDb.Entries.Remove((EntryModel)entriesDataGrid.SelectedItem);
			entriesDataGrid.ItemsSource = dbSettings.InternalDb.Entries;
			entriesDataGrid.Items.Refresh();

			tooltipLabel.Content = "Kayıt silindi!";
		}

		private void ReloadButton_Click(object sender, RoutedEventArgs e)
		{
			try {
				Database.Restore(ref dbSettings.InternalDb, dbSettings.FilePath);
				entriesDataGrid.ItemsSource = dbSettings.InternalDb.Entries;
				entriesDataGrid.Items.Refresh();

				UpdateDbHint();
				entriesDataGrid.SelectedIndex = -1;
				tooltipLabel.Content = "Veritabanı yenilendi!";
			} catch (Exception ex) { logger.Error(ex); }
		}

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			try {
				dbSettings.InternalDb.Entries = (List<EntryModel>)entriesDataGrid.ItemsSource;
				dbSettings.InternalDb.ModifiedDate = DateTime.UtcNow.ToString();
				Database.Save(dbSettings.InternalDb, dbSettings.FilePath);
				tooltipLabel.Content = "Veritabanı kaydedildi!";
			} catch (Exception ex) { logger.Error(ex); }
		}

		private void LockButton_Click(object sender, RoutedEventArgs e)
		{
			var canvas = (Canvas)Parent;
			var grid = (Grid)(canvas.Parent);
			var window = (Window)(grid.Parent);

			canvas.Children.Clear();
			window.Close();

			MainWindow w = new();
			w.Show();
		}
	}
}