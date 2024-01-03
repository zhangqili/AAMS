using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AAMS.Client.WPF;
using AAMS.Client.WPF.Lib;
using AAMS.Client.WPF.Models;

namespace AAMS.Client.WPF.ViewModels
{
    class AddDepartmentViewModel : NotificationObject
    {
		private int iD;

		public int ID
		{
			get { return iD; }
			set
			{
				iD = value;
				OnPropertyChanged(nameof(ID));
			}
		}

		private string name;

		public string Name
		{
			get { return name; }
			set
			{
				name = value;
				OnPropertyChanged(nameof(Name));
			}
		}
        public DelegateCommand ConfirmCommand { get; set; }
        private void Confirm(object parameter)
        {
            Department d = new Department();
            d.Name = Name;
            d.ID = ID;
			StaticVariables.AAMSConnect.AddDepartment(d);

        }
        public AddDepartmentViewModel()
        {
            ConfirmCommand = new DelegateCommand(Confirm);
        }

	}
}
