using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using TheDebtBook.Data;
using TheDebtBook.Models;
using TheDebtBook.Views;

namespace TheDebtBook.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private string filePath = "";

        public MainWindowViewModel(IDialogService dialogService)
        {
            DialogService = dialogService;
            Debtors = new ObservableCollection<Debtor>
            {
                new Debtor("Linda", "-100"),
                new Debtor("Nicolai", "1000"),
                new Debtor("Jonas", "10000"),
                new Debtor("Simon", "10")
            };
            CurrentDebtor = Debtors[0];
        }

        #region Properties

        /*********** DialogService ***********/
        private IDialogService _dialogService;
        public IDialogService DialogService
        {
            get { return _dialogService; }
            set { SetProperty(ref _dialogService, value); }
        }

        /*********** Debtors ***********/
        private ObservableCollection<Debtor> _debtors;
        public ObservableCollection<Debtor> Debtors
        {
            get { return _debtors; }
            set { SetProperty(ref _debtors, value); }
        }

        /*********** CurrentDebtor ***********/
        private Debtor _currentDebtor;
        public Debtor CurrentDebtor
        {
            get { return _currentDebtor; }
            set { SetProperty(ref _currentDebtor, value); }
        }

        /*********** Title ***********/
        private string _title = "The Debt Book";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        /*********** Filename ***********/
        private string _filename = "";
        public string Filename
        {
            get { return _filename; }
            set
            {
                SetProperty(ref _filename, value);
                //RaisePropertyChanged("Title");
            }
        }

        #endregion

        #region Commands

        /*********** NewFileCommand ***********/
        private DelegateCommand _newFileCommand;
        public DelegateCommand NewFileCommand => _newFileCommand ?? 
                (_newFileCommand = new DelegateCommand(ExecuteNewFileCommand));

        private void ExecuteNewFileCommand()
        {
            MessageBoxResult res = MessageBox.Show("Any unsaved data will be lost. Are you sure you want to initiate a new file?", "Warning",
                MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            if (res == MessageBoxResult.Yes)
            {
                Debtors.Clear();
                Filename = "";
            }
        }

        /*********** OpenFileCommand ***********/
        private DelegateCommand _openFileCommand;
        public DelegateCommand OpenFileCommand => _openFileCommand ?? 
            (_openFileCommand = new DelegateCommand(ExecuteOpenFileCommand));

        private void ExecuteOpenFileCommand()
        {
            var dialog = new OpenFileDialog
            {
                Filter = "Debtbook documents|*.agn|All Files|*.*",
                DefaultExt = "agn"
            };
            if (filePath == "")
                dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            else
                dialog.InitialDirectory = Path.GetDirectoryName(filePath);

            if (dialog.ShowDialog(App.Current.MainWindow) == true)
            {
                filePath = dialog.FileName;
                Filename = Path.GetFileName(filePath);
                try
                {
                    Debtors = Repository.ReadFile(filePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Unable to open file", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        /*********** SaveAsCommand ***********/
        DelegateCommand _saveAsCommand;
        public DelegateCommand SaveAsCommand => _saveAsCommand ?? 
            (_saveAsCommand = new DelegateCommand(ExecuteSaveAsCommand));
        private void ExecuteSaveAsCommand()
        {
            var dialog = new SaveFileDialog
            {
                Filter = "Debtbook documents|*.agn|All Files|*.*",
                DefaultExt = "agn"
            };
            if (filePath == "")
                dialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            else
                dialog.InitialDirectory = Path.GetDirectoryName(filePath);

            if (dialog.ShowDialog(App.Current.MainWindow) == true)
            {
                filePath = dialog.FileName;
                Filename = Path.GetFileName(filePath);
                SaveFile();
            }
        }

        /*********** SaveCommand ***********/
        private DelegateCommand _saveCommand;
        public DelegateCommand SaveCommand
        {
            get
            {
                return _saveCommand ?? (_saveCommand = new DelegateCommand(SaveFileCommand_Execute, SaveFileCommand_CanExecute)
                  .ObservesProperty(() => Debtors.Count));
            }
        }

        private void SaveFileCommand_Execute()
        {
            SaveFile();
        }

        private bool SaveFileCommand_CanExecute()
        {
            return (Filename != "") && (Debtors.Count > 0);
        }

        private void SaveFile()
        {
            try
            {
                Repository.SaveFile(filePath, Debtors);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Unable to save file", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        /*********** AddDebtorCommand ***********/
        private DelegateCommand _addDebtorCommand;
        public DelegateCommand AddDebtorCommand => _addDebtorCommand ?? 
            (_addDebtorCommand = new DelegateCommand(ExecuteAddDebtorCommand));
        private void ExecuteAddDebtorCommand()
        {
            var newDebtor = new Debtor();
            var vm = new AddDebtorViewModel(newDebtor);
            var parameters = new DialogParameters();
            parameters.Add("debtor", newDebtor);
            DialogService.ShowDialog("AddDebtor", parameters, r => 
            {
                if(r.Result == ButtonResult.OK )
                {
                    newDebtor.DebtHistory = new ObservableCollection<DebtTrack>();
                    newDebtor.DebtHistory.Add(new DebtTrack(newDebtor.CurrentDebt));
                    Debtors.Add(newDebtor);
                    CurrentDebtor = newDebtor;
                }
            });    
        }

        /*********** EditDebtorCommand ***********/
        private DelegateCommand _editDebtorCommand;
        public DelegateCommand EditDebtorCommand => _editDebtorCommand ??
            (_editDebtorCommand = new DelegateCommand(ExecuteEditDebtorCommand)
            .ObservesProperty(() => CurrentDebtor));
        private void ExecuteEditDebtorCommand()
        {
            var vm = new EditDebtorViewModel(CurrentDebtor);
            var parameters = new DialogParameters();
            parameters.Add("debtor", CurrentDebtor);
            DialogService.ShowDialog("EditDebtor", parameters, r => {});
        }

        #endregion

    }
}
