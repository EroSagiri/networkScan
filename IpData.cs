using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace networkScan
{
    public class IpData : INotifyPropertyChanged
    {
        private string start_ip = "";
        private string end_ip = "";

        public string Start_ip
        {
            get
            {
                return start_ip;
            }

            set
            {
                start_ip = value;
                OnPropertyChanged();
            }
        }

        public string End_ip
        {
            get
            {
                return end_ip;
            }
            set
            {
                end_ip = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
