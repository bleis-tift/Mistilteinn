using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mistilteinn.ToolWindows;
using Mistilteinn.Models;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using System.Windows.Data;

namespace Mistilteinn.Configs
{
    public class ConfigViewModel : ViewModelBase<ConfigViewModel>
    {
        Models.Config config;
        public ConfigViewModel(Models.Config config) { this.config = config; }

        public ICommand OKCommand { get { return new RelayCommand<object>(_ => {  }); } }

        public readonly TicketLoaderConfigViewModel ticketLoaderConfig = new TicketLoaderConfigViewModel();
        public TicketLoaderConfigViewModel TicketLoaderConfig { get { return ticketLoaderConfig; } }
    }

    public class TicketLoaderConfigViewModel : ViewModelBase<TicketLoaderConfigViewModel>
    {
        public TicketLoaderConfigViewModel()
        {
            TicketLoaders.Add(new LocalTicketLoaderViewModel());

            TicketLoaders.Add(new GithubTicketLoaderViewModel());

            TicketLoaders.Add(new RedmineTicketLoaderViewModel());

        }

        public readonly ObservableCollection<TicketLoaderViewModel> ticketLoaders = new ObservableCollection<TicketLoaderViewModel>();
        public ObservableCollection<TicketLoaderViewModel> TicketLoaders
        {
            get { return ticketLoaders; }
        }

        TicketLoaderViewModel selectedLoader;
        public TicketLoaderViewModel SelectedLoader
        {
            get { return selectedLoader; }
            set 
            {
                if (selectedLoader == value) return;
                selectedLoader = value;
                OnPropertyChanged(_ => _.SelectedLoader);
            }
        }
    }
}
