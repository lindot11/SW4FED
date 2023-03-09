using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheDebtBook.Models
{
    public class DebtTrack : BindableBase
    {
        public DebtTrack() { }

        public DebtTrack(string debt)
        {
            this.Date = DateTime.Now.ToString("d");
            this.Value = debt;
        }

        private string _date;
        public string Date
        {
            get { return _date; }
            set { SetProperty(ref _date, value); }
        }

        private string _value;
        public string Value
        {
            get { return _value; }
            set { SetProperty(ref _value, value); }
        }
    }
}
