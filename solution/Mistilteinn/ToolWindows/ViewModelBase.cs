using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Mistilteinn.ToolWindows
{
    public abstract class ViewModelBase<TSelf> : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [Obsolete]
        protected void OnPropertyChanged<TResult>(Expression<Func<TSelf, TResult>> expr)
        {
            var h = PropertyChanged;
            if (h == null) return;

            h(this, new PropertyChangedEventArgs(((MemberExpression)expr.Body).Member.Name));
        }

        protected void OnPropertyChanged<T>(Expression<Func<T>> expr)
        {
            
            var h = PropertyChanged;
            if (h == null) return;

            h(this, new PropertyChangedEventArgs(((MemberExpression)expr.Body).Member.Name));
        }
    }
}
