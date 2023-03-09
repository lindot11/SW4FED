using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;

namespace TheDebtBook.Models
{
    public class Debtor : BindableBase
    {
        public Debtor() { }
        public Debtor(string name, string debt)
        {
            this.Name = name;
            CurrentDebt = debt;
            DebtHistory = new ObservableCollection<DebtTrack>()
            {
                new DebtTrack(debt)
            };
        }

        private string _name;
        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        private string _currentDebt;
        public string CurrentDebt
        {
            get { return _currentDebt; }
            set { SetProperty(ref _currentDebt, value); }
        }

        private ObservableCollection<DebtTrack> _debtHistory;
        public ObservableCollection<DebtTrack> DebtHistory
        {
            get { return _debtHistory; }
            set { SetProperty(ref _debtHistory, value); }
        }

        public Debtor Clone()
        {
            return this.MemberwiseClone() as Debtor;
        }

    }
}
