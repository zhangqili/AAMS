using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using AAMS.Client.WPF;
using AAMS.Client.WPF.Lib;
using AAMS.Client.WPF.Models;
using ReactiveUI;
using static AAMS.Client.WPF.ViewModels.GradeManagementViewModel;
using static AAMS.Client.WPF.ViewModels.StudentManagementViewModel;

namespace AAMS.Client.WPF.ViewModels
{
    public class DepartmentManagementViewModel : NotificationObject
    {
        private Department selectedDepartment;

        public Department SelectedDepartment
        {
            get { return selectedDepartment; }
            set
            {
                selectedDepartment = value;
                IsSelected = selectedDepartment != null;
                OnPropertyChanged(nameof(SelectedDepartment));
            }
        }

        private ObservableCollection<Department> departments;

        public ObservableCollection<Department> Departments
        {
            get { return departments; }
            set
            {
                departments = value;
                OnPropertyChanged(nameof(Departments));
            }
        }
        private string newName;

        public string NewName
        {
            get { return newName; }
            set
            {
                newName = value;
                OnPropertyChanged(nameof(NewName));
            }
        }

        private int newID;

        public int NewID
        {
            get { return newID; }
            set
            {
                newID = value;
                OnPropertyChanged(nameof(NewID));
            }
        }
        private int studentCount;

        public int StudentCount
        {
            get { return studentCount; }
            set
            {
                studentCount = value;
                OnPropertyChanged(nameof(StudentCount));
            }
        }
        private string filterText;

        public string FilterText
        {
            get { return filterText; }
            set
            {
                filterText = value;
                Departments = new ObservableCollection<Department>(StaticVariables.AAMSConnect.GetDepartmentListLike(FilterText));
                OnPropertyChanged(nameof(FilterText));
            }
        }
        private bool isSelected;

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }
        private ObservableCollection<string> students;

        public ObservableCollection<string> Students
        {
            get { return students; }
            set
            {
                students = value;
                OnPropertyChanged(nameof(Students));
            }
        }

        public int GetStudents(int id)
        {
            int count;
            string cmd = $"SELECT COUNT(1) " +
                         $"FROM (t_department JOIN t_studentrelation ON  t_studentrelation.department_id=t_department.department_id " +
                         $"JOIN t_student_info ON t_student_info.student_id=t_studentrelation.student_id) " +
                         $"WHERE t_department.department_id = {id}";
            SqlDataReader rdr = StaticVariables.AAMSConnect.ExecuteReader(cmd);
            var vs  = new ObservableCollection<string>();
            rdr.Read();
            count = rdr.GetInt32(0);
            rdr.Dispose();
            if (count > 0)
            {
                cmd = $"SELECT student_name " +
                             $"FROM (t_department JOIN t_studentrelation ON  t_studentrelation.department_id=t_department.department_id " +
                             $"JOIN t_student_info ON t_student_info.student_id=t_studentrelation.student_id) " +
                             $"WHERE t_department.department_id = {id}";
                rdr = StaticVariables.AAMSConnect.ExecuteReader(cmd);
                for (int i = 0; rdr.Read(); i++)
                {
                    vs.Add(rdr.GetString(0).Trim());
                }
                rdr.Dispose();
            }

            Students = vs;
            return 0;
        }
        public DelegateCommand ConfirmCommand { get; set; }
        private void Confirm(object parameter)
        {
            Department d = new Department();
            d.Name = NewName;
            d.ID = NewID;
            StaticVariables.AAMSConnect.AddDepartment(d);
            Departments = new ObservableCollection<Department>(StaticVariables.AAMSConnect.GetDepartmentListLike(FilterText));
        }

        public DelegateCommand SaveCommand { get; set; }
        private void Save(object parameter)
        {
            StaticVariables.AAMSConnect.UpdateDepartment(SelectedDepartment);
            Departments = new ObservableCollection<Department>(StaticVariables.AAMSConnect.GetDepartmentListLike(FilterText));
        }
        public DelegateCommand DeleteCommand { get; set; }
        private void Delete(object parameter)
        {

            StaticVariables.AAMSConnect.DeleteDepartment(SelectedDepartment);
            Departments = new ObservableCollection<Department>(StaticVariables.AAMSConnect.GetDepartmentListLike(FilterText));
        }

        public DelegateCommand ShowDetailCommand { get; set; }
        private void ShowDetail(object parameter)
        {
            if (SelectedDepartment != null) GetStudents(SelectedDepartment.ID);
        }
        public void Update()
        {
            Departments = new ObservableCollection<Department>(StaticVariables.AAMSConnect.GetDepartmentListLike(FilterText));

        }

        public DepartmentManagementViewModel()
        {
            ConfirmCommand = new DelegateCommand(Confirm);
            DeleteCommand = new DelegateCommand(Delete);
            ShowDetailCommand = new DelegateCommand(ShowDetail);
            SaveCommand = new DelegateCommand(Save);
            Update();
        }
    }
}
