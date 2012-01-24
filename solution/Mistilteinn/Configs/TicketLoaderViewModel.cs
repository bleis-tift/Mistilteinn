using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mistilteinn.ToolWindows;
using System.ComponentModel;
using Mistilteinn.Models;
using System.Windows.Media.Imaging;
using System.Reflection;
using Microsoft.Win32;
using System.Xml.Linq;

namespace Mistilteinn.Configs
{
    public abstract class TicketLoaderViewModel : ViewModelBase<TicketLoaderViewModel>
    {
        public string DisplayName { get; set; }
        public Type Type { get; set; }

        BitmapImage icon;
        public BitmapImage Icon { get { return icon; } set 
        {

            if (icon == value) return;

            icon = value;
            OnPropertyChanged(() => Icon);
        } }
        
        public TicketLoaderViewModel(Type type)
        {
            this.Type = type;
            var dn = (DisplayNameAttribute)this.Type.GetCustomAttributes(false).Single(ca => ca is DisplayNameAttribute);
            DisplayName = dn.DisplayName;
        }

        public abstract XElement ToXml();
    }

    public class RedmineTicketLoaderViewModel : TicketLoaderViewModel
    {
        public string AccessKey { get; set; }
        public string BaseUrl { get; set; }
        public string ProjectId { get; set; }

        public static readonly RedmineTicketLoaderViewModel Instance = new RedmineTicketLoaderViewModel();
        RedmineTicketLoaderViewModel()
            : base(typeof(RedmineTicketLoader))
        {
            var logo = new BitmapImage(new Uri("pack://application:,,,/Mistilteinn;component/Resources/redmine.png"));
            Icon = logo;
        }

        public override XElement ToXml()
        {
            return new XElement("loader",
                new XElement("name", "Redmine"),
                new XElement("args",
                    new XElement("arg", new XAttribute("name", "access-key"), AccessKey),
                    new XElement("arg", new XAttribute("name", "base-url"), BaseUrl),
                    new XElement("arg", new XAttribute("name", "project-id"), ProjectId)));
        }
    }

    public class LocalTicketLoaderViewModel : TicketLoaderViewModel
    {
        string ticketFilePath;
        public string TicketFilePath { get { return ticketFilePath; } set 
        {
            if (ticketFilePath == value) return;

            ticketFilePath = value;
            OnPropertyChanged(() => TicketFilePath);
        } }
        public RelayCommand<object> Browse {
            get
            {
                return new RelayCommand<object>(delegate
                    
                    {

                        var ofd = new OpenFileDialog();
                        ofd.FileName = "";
                        ofd.DefaultExt = "*.txt";
                        if (ofd.ShowDialog() == true)
                        {
                            TicketFilePath = ofd.FileName;
                        }
                    });
            }
        }

        public static readonly LocalTicketLoaderViewModel Instance = new LocalTicketLoaderViewModel();
        LocalTicketLoaderViewModel()
            : base(typeof(LocalTicketLoader))
        {
            var logo = new BitmapImage(new Uri("pack://application:,,,/Mistilteinn;component/Resources/local.png"));
            Icon = logo;
        }

        public override XElement ToXml()
        {
            return new XElement("loader",
                new XElement("name", "Local"),
                new XElement("args",
                    new XElement("arg", new XAttribute("name", "file-path"), TicketFilePath)));
        }
    }

    public class GithubTicketLoaderViewModel : TicketLoaderViewModel
    {
        public string ProjectId { get; set; }

        public static readonly GithubTicketLoaderViewModel Instance = new GithubTicketLoaderViewModel();
        GithubTicketLoaderViewModel()
            : base(typeof(GithubTicketLoader))
        {
            var logo = new BitmapImage(new Uri("pack://application:,,,/Mistilteinn;component/Resources/github.png"));
            Icon = logo;
        }

        public override XElement ToXml()
        {
            return new XElement("loader",
                new XElement("name", "Github"),
                new XElement("args",
                    new XElement("arg", new XAttribute("name", "project-id"), ProjectId)));
        }
    }
}
