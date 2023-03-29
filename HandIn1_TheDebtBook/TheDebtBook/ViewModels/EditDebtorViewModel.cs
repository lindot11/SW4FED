using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using Prism.Commands;
using TheDebtBook.Models;

namespace TheDebtBook.ViewModels
{
    public class EditDebtorViewModel : BindableBase, IDialogAware
    {
        public EditDebtorViewModel(Debtor debtor)
        {
            CurrentDebtor = debtor;
        }

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog() { return true; }

        public void OnDialogClosed() { }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            CurrentDebtor = parameters.GetValue<Debtor>("debtor");
        }

        #region Properties

        /*********** CurrentDebtor ***********/
        private Debtor _currentDebtor;
        public Debtor CurrentDebtor
        {
            get { return _currentDebtor; }
            set { SetProperty(ref _currentDebtor, value); }
        }

        /*********** AddValue ***********/
        private string _addValue;
        public string AddValue
        {
            get { return _addValue; }
            set { SetProperty(ref _addValue, value); }
        }

        /*********** IsValid ***********/
        private bool _isValid;
        public bool IsValid
        {
            get
            {
                _isValid = true;
                if (string.IsNullOrWhiteSpace(CurrentDebtor.CurrentDebt))
                    _isValid = false;
                return _isValid;
            }

        }

        /*********** Title ***********/
        private string _title = "Edit Debtor";
        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }

        #endregion

        #region Commands

        /*********** AddValueCommand ***********/
        private DelegateCommand _addValueCommand;
        public DelegateCommand AddValueCommand => _addValueCommand ??
            (_addValueCommand = new DelegateCommand(ExecuteAddValueCommand, CanExecuteAddValueCommand))
            .ObservesProperty(() => CurrentDebtor.CurrentDebt);
        private bool CanExecuteAddValueCommand()
        {
            return IsValid;
        }
        private void ExecuteAddValueCommand()
        {
            int debt = int.Parse(CurrentDebtor.CurrentDebt) + int.Parse(AddValue);

            CurrentDebtor.CurrentDebt = debt.ToString();
            CurrentDebtor.DebtHistory.Add(new DebtTrack(AddValue));
        }

        /*********** CloseDialogCommand ***********/
        private DelegateCommand _closeDialogCommand;
        public DelegateCommand CloseDialogCommand => _closeDialogCommand ??
            (_closeDialogCommand = new DelegateCommand(ExecuteCloseDialogCommand));
        private void ExecuteCloseDialogCommand()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK));
        }

        #endregion
    }
}
