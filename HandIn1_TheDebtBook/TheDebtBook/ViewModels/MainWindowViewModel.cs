using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.ObjectModel;
using TheDebtBook.Models;
using TheDebtBook.Views;

namespace TheDebtBook.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
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

        #endregion

        #region Commands

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
