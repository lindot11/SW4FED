using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using TheDebtBook.Models;

namespace TheDebtBook.ViewModels
{
    public class AddDebtorViewModel : BindableBase, IDialogAware
    {
        public AddDebtorViewModel(Debtor debtor)
        {
            NewDebtor = debtor;
        }

        public event Action<IDialogResult> RequestClose;
            
        public bool CanCloseDialog() { return true; }

        public void OnDialogClosed() {}

        public void OnDialogOpened(IDialogParameters parameters) 
        {
            NewDebtor = parameters.GetValue<Debtor>("debtor");
        }

        #region Properties

        /*********** NewDebtor ***********/
        private Debtor _newDebtor;
        public Debtor NewDebtor
        {
            get { return _newDebtor; }
            set { SetProperty(ref _newDebtor, value); }
        }

        /*********** IsValid ***********/
        private bool _isValid;
        public bool IsValid
        {
            get
            {
                _isValid = true;
                if (string.IsNullOrWhiteSpace(NewDebtor.Name)) 
                    _isValid = false;
                //if (string.IsNullOrWhiteSpace(NewDebtor.CurrentDebt)) 
                //    _isValid = false;
                return _isValid;
            }

        }

        /*********** Title ***********/
        private string _title = "Add Debtor";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        #endregion

        #region Commands

        /*********** SaveAddDebtorCommand ***********/
        private DelegateCommand _saveAddDebtorCommand;
        public DelegateCommand SaveAddDebtorCommand => _saveAddDebtorCommand ??
            (_saveAddDebtorCommand = new DelegateCommand(ExecuteSaveAddDebtorCommand, CanExecuteSaveAddDebtorCommand))
            .ObservesProperty(() => NewDebtor.Name)
            .ObservesProperty(() => NewDebtor.CurrentDebt);

        private bool CanExecuteSaveAddDebtorCommand()
        {
            return IsValid;
        }

        private void ExecuteSaveAddDebtorCommand()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
        }

        /*********** CancelAddDebtorCommand ***********/
        private DelegateCommand _cancelAddDebtorCommand;
        public DelegateCommand CancelAddDebtorCommand => _cancelAddDebtorCommand ?? 
            (_cancelAddDebtorCommand = new DelegateCommand(ExecuteCancelAddDebtorCommand));

        private void ExecuteCancelAddDebtorCommand()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel));
        }

        #endregion
    }
}
