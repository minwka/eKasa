using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using static eKasa.Core.GlobalSettings;

namespace eKasa.Core.UserControls
{
	public partial class HomeView : UserControl
	{
		public HomeView()
		{
			InitializeComponent();

			titleLabel.Text = $"Hoşgeldin, {dbSettings.InternalDb.Owner}!";
			entriesDataGrid.ItemsSource = dbSettings.InternalDb.Entries;
			UpdateDbHint();
		}

		public void UpdateDbHint()
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
			var ouc = new ActionsView();
			canvas.Children.Add(ouc);
		}

		private void EditButton_Click(object sender, RoutedEventArgs e)
		{
			if (entriesDataGrid.SelectedIndex == -1) entriesDataGrid.SelectedIndex = 0;
			var oldEntry = (EntryModel)entriesDataGrid.SelectedItem;
			if (oldEntry != null) {
				HomeWindow.editv.idPreview.Text = oldEntry.Id.ToString();
				HomeWindow.editv.namePreview.Text = oldEntry.Name;
				HomeWindow.editv.usernamePreview.Text = oldEntry.Username;
				HomeWindow.editv.pwdPreview.Password = oldEntry.Password;
				HomeWindow.editv.tagPreview.Text = oldEntry.Tag;
				HomeWindow.editv.nameInput.Text = oldEntry.Name;
				HomeWindow.editv.usernameInput.Text = oldEntry.Username;
				HomeWindow.editv.tagInput.Text = oldEntry.Tag;

				var parent = (Canvas)Parent;
				parent.Children.Clear();
				parent.Children.Add(HomeWindow.editv);
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

				HomeWindow.UpdateAllViews();
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

			LoginWindow w = new();
			w.Show();
		}
	}
}