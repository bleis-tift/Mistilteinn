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

        public TicketLoaderConfigViewModel TicketLoader
        {
            get { return new TicketLoaderConfigViewModel(config.TicketLoader); }
            set { config.TicketLoader = value.Model; OnPropertyChanged(_ => _.TicketLoader); }
        }

        public ICommand OKCommand { get { return new RelayCommand<object>(_ => {  }); } }
    }

    public class TicketLoaderConfigViewModel : ViewModelBase<TicketLoaderConfigViewModel>
    {
        Models.Config.TicketLoaderType ticketLoader;

        public TicketLoaderConfigViewModel(Models.Config.TicketLoaderType ticketLoader)
        {
            this.ticketLoader = ticketLoader ?? new Models.Config.TicketLoaderType();

            if (ticketLoader.Args == null) args = new BindingList<ArgViewModel>();
            args = new BindingList<ArgViewModel>(this.ticketLoader.Args.Select(arg => new ArgViewModel(arg)).ToList());

            args.AllowNew = true;
            args.AddingNew += delegate { ;;;;; };
          
        }

        public Models.Config.TicketLoaderType Model { get { return ticketLoader; } set { ticketLoader = value; } }

        static readonly IEnumerable<Type> types = typeof(TicketLoaderConfigViewModel).Assembly.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(ITicketLoader)));
        public IEnumerable<Type> Types { get { return types; } }

        public Type SelectedType
        {
            get { return ticketLoader.Type; }
            set { if (ticketLoader.Type == value) return; ticketLoader.Type = value; OnPropertyChanged(_ => _.SelectedType); }
        }
        BindingList<ArgViewModel> args;
        public BindingList<ArgViewModel> Args
        {
            get { return args; }
        }
    }

    public class ArgViewModel : ViewModelBase<ArgViewModel>
    {
        Models.Config.TicketLoaderType.ArgType arg;
        public ArgViewModel(Models.Config.TicketLoaderType.ArgType arg)
        {
            this.arg = arg ?? new Models.Config.TicketLoaderType.ArgType();
        }
        public ArgViewModel() : this(new Models.Config.TicketLoaderType.ArgType()) { }

        public string Name
        {
            get { return arg.Name; }
            set { if (arg.Name == value) return; arg.Name = value; OnPropertyChanged(_ => _.Name); }
        }

        public Models.SourceEnum Source
        {
            get { return arg.Source; }
            set { if (arg.Source == value) return; arg.Source = value; OnPropertyChanged(_ => _.Source); }
        }

        public string Value
        {
            get { return arg.Value; }
            set { if (arg.Value == value) return; arg.Value = value; OnPropertyChanged(_ => _.Value); }
        }
    }
}
