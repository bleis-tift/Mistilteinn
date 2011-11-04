using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Mistilteinn.Models;
using System.IO;
using Mistilteinn.Infos;
using System.Diagnostics;
using Mistilteinn.Configs;

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

        public string DetailInfo { get { return ticket.DetailInfo; } }

        bool IsUrl(string str)
        {
            try { new Uri(str); return true; }
            catch { return false; }
        }

        public ICommand DetailCommand { get { return new RelayCommand<object>(_ => { if (IsUrl(DetailInfo)) Process.Start(DetailInfo); else System.Windows.Forms.MessageBox.Show(DetailInfo); }); } }

        public ICommand CheckoutBranch { get { return new RelayCommand<object>(_ => { GitUtil.DoGitCheckout(SolutionInfo.RootDir, "id/" + ID, "master"); }); } }
    }

    public class TicketViewModelCollection : ObservableCollection<TicketViewModel>
    {
        public TicketViewModelCollection() { }
        public TicketViewModelCollection(IEnumerable<TicketViewModel> e) : base(e) { }
        public TicketViewModelCollection(List<TicketViewModel> l) : base(l) { }

        public TicketViewModelCollection(IEnumerable<Ticket> tickets) : base(tickets.Select(t => new TicketViewModel(t))) { }
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

        public ICommand UpdateCommand
        {
            get
            {
                return new RelayCommand<object>(_ =>
                {
                    if (SolutionInfo.RootDir == null)
                    {
                        this.Tickets = new TicketViewModelCollection();
                        return;
                    }

                    //var loader = new LocalTicketLoader(Path.Combine(SolutionInfo.RootDir, "tools-conf", "mistilteinn", "ticketlist"));
                    //var loader = new RedmineTicketLoader(null, null, null);
                    //var loader = new GithubTicketLoader("bleis-tift/Mistilteinn");
                    var loader = Configs.Config.CreateTicketLoader();
                    var tickets = loader.Load(GitUtil.GetCurrentBranch(SolutionInfo.RootDir));
                    this.Tickets = new TicketViewModelCollection(tickets);
                });
            }
        }
    }
}
