using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Mistilteinn.ToolWindows
{
    public class TicketViewModel : ViewModelBase<TicketViewModel>
    {
        Ticket ticket;

        public TicketViewModel(Ticket t)
        {
            ticket = t;
        }

        public string ID
        {
            get { return ticket.ID; }
            set
            {
                if (ticket.ID == value) return;
                ticket.ID = value;
                OnPropertyChanged(_ => _.ID);
            }
        }

        public string Summary
        {
            get { return ticket.Summary; }
            set
            {
                if (ticket.Summary == value) return;
                ticket.Summary = value;
                OnPropertyChanged(_ => _.Summary);
            }
        }

        public bool IsCurrentBranch
        {
            get { return ticket.IsCurrentBranch; }
            set
            {
                if (ticket.IsCurrentBranch == value) return;
                ticket.IsCurrentBranch = value;
                OnPropertyChanged(_ => _.Summary);
            }
        }
    }

    public class TicketViewModelCollection : ObservableCollection<TicketViewModel>
    {
        public TicketViewModelCollection() { }
        public TicketViewModelCollection(IEnumerable<TicketViewModel> e) : base(e) { }
        public TicketViewModelCollection(List<TicketViewModel> l) : base(l) { }
    }

    public class TicketListViewModel : ViewModelBase<TicketListViewModel>
    {
        TicketViewModelCollection tickets;

        public TicketViewModelCollection Tickets
        {
            get { return tickets; }
            set
            {
                if (Equals(tickets, value)) return;
                tickets = value;
                OnPropertyChanged(_ => _.Tickets);
            }
        }

        TicketViewModelCollection ticketList =
            new TicketViewModelCollection(new List<TicketViewModel>() { new TicketViewModel(new Ticket() { ID = "1", Summary = "hoge", IsCurrentBranch = true }) });
        public ICommand UpdateCommand { get { return new RelayCommand<object>(_ => { this.Tickets = new TicketViewModelCollection(ticketList); }); } }
        public ICommand DetailCommand { get { return new RelayCommand<string>(_ => { System.Windows.Forms.MessageBox.Show(_); }); } }
    }
}
