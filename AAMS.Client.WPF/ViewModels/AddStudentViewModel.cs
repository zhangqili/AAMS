using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using AAMS.Client.WPF;
using AAMS.Client.WPF.Lib;
using AAMS.Client.WPF.Models;

namespace AAMS.Client.WPF.ViewModels
{
    class AddStudentViewModel : NotificationObject
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
        private List<Department> departments;

        public List<Department> Departments
        {
            get { return departments; }
            set
            {
                departments = value;
                OnPropertyChanged(nameof(Departments));
            }
        }
        private Department selectedDepartment;

        public Department SelectedDepartment
        {
            get { return selectedDepartment; }
            set
            {
                selectedDepartment = value;
                OnPropertyChanged(nameof(SelectedDepartment));
            }
        }

        public DelegateCommand ConfirmCommand { get; set; }
        private void Confirm(object parameter)
        {
            StudentInfo d = new StudentInfo();
            d.Name = Name;
            d.ID = ID;
            StudentRelation relation = new StudentRelation();
            relation.StudentID = ID;
            relation.DepartmentID = SelectedDepartment.ID;
            StaticVariables.AAMSConnect.AddStudent(d, relation);
        }
        public AddStudentViewModel()
        {
            ConfirmCommand = new DelegateCommand(Confirm);
            Departments = StaticVariables.AAMSConnect.GetDepartmentList();
        }

    }
}
