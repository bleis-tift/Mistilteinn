using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mistilteinn.ToolWindows;
using Mistilteinn.Models;

namespace Mistilteinn.Configs
{
    public class ConfigTicketLoaderViewModel : ViewModelBase<ConfigTicketLoaderViewModel>
    {
        readonly IEnumerable<Type> types = typeof(ConfigTicketLoaderViewModel).Assembly.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(ITicketLoader)));
        public IEnumerable<Type> Types { get { return types; } }

        Type selectedType;
        public Type SelectedType
        {
            get { return selectedType; }
            set { if (selectedType == value) return; selectedType = value; OnPropertyChanged(_ => _.SelectedType); }
        }
        IEnumerable<ArgViewModel> args = new List<ArgViewModel>();
        public IEnumerable<ArgViewModel> Args
        {
            get { return args; }
            set { if (args == value) return; args = value; OnPropertyChanged(_ => _.Args); }
        }
    }

    public class Arg
    {
        public string Name { get; set; }
        public Source Source { get; set; }
        public string Value { get; set; }
    }

    public class ArgViewModel : ViewModelBase<ArgViewModel>
    {
        Arg arg;
        public ArgViewModel(Arg arg) { this.arg = arg; }
        public ArgViewModel() : this(new Arg()) { }
        public string Name
        {
            get { return arg.Name; }
            set { if (arg.Name == value) return; arg.Name = value; OnPropertyChanged(_ => _.Name); }
        }
        public Source Source
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

    public enum Source
    {
        Value,
        File,
        AppData
    }
}
