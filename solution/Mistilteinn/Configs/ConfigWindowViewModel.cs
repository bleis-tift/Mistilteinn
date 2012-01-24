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
using System.Xml.Linq;
using System.Xml.XPath;

namespace Mistilteinn.Configs
{
    public class ConfigViewModel : ViewModelBase<ConfigViewModel>
    {
        public readonly TicketLoaderConfigViewModel ticketLoaderConfig;

        public ConfigViewModel(XElement config)
        {
            ticketLoaderConfig = new TicketLoaderConfigViewModel();
            ticketLoaderConfig.SelectedLoader = ParseLoader(config);
        }

        static TicketLoaderViewModel ParseLoader(XElement config)
        {
            switch (config.XPathSelectElement("//loader/name").Value)
            {
                case "Local":
                    return MakeLocalTicketLoader(config);
                case "Redmine":
                    return MakeRedmineTicketLoader(config);
                case "Github":
                    return MakeGithubTicketLoader(config);
            }
            throw new Exception(string.Format("{0} is invalid loader.", config.XPathSelectElement("//loader/name")));
        }

        static TicketLoaderViewModel MakeGithubTicketLoader(XElement config)
        {
            var res = GithubTicketLoaderViewModel.Instance;
            res.ProjectId = config.XPathSelectElement("//loader/args/arg[@name='project-id']").Value;
            return res;
        }

        static TicketLoaderViewModel MakeRedmineTicketLoader(XElement config)
        {
            var res = RedmineTicketLoaderViewModel.Instance;
            res.AccessKey = config.XPathSelectElement("//loader/args/arg[@name='access-key']").Value;
            res.ProjectId = config.XPathSelectElement("//loader/args/arg[@name='project-id']").Value;
            res.BaseUrl = config.XPathSelectElement("//loader/args/arg[@name='base-url']").Value;
            return res;
        }

        static TicketLoaderViewModel MakeLocalTicketLoader(XElement config)
        {
            var res = LocalTicketLoaderViewModel.Instance;
            res.TicketFilePath = config.XPathSelectElement("//loader/args/arg[@name='file-path']").Value;
            return res;
        }

        public ICommand OKCommand { get { return new RelayCommand<object>(_ => {  }); } }

        public TicketLoaderConfigViewModel TicketLoaderConfig { get { return ticketLoaderConfig; } }

        public XElement TicketLoaderXml
        {
            get { return ticketLoaderConfig.SelectedLoader.ToXml(); }
        }
    }

    public class TicketLoaderConfigViewModel : ViewModelBase<TicketLoaderConfigViewModel>
    {
        public TicketLoaderConfigViewModel()
        {
            TicketLoaders.Add(LocalTicketLoaderViewModel.Instance);
            TicketLoaders.Add(GithubTicketLoaderViewModel.Instance);
            TicketLoaders.Add(RedmineTicketLoaderViewModel.Instance);
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
                OnPropertyChanged(() => SelectedLoader);
            }
        }
    }
}
