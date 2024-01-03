
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAMS.Client.WPF;
using AAMS.Client.WPF.Lib;
using AAMS.Client.WPF.Models;

namespace AAMS.Client.WPF.ViewModels
{
    class AddGradeViewModel : NotificationObject
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

        private List<StudentInfo> studentInfos;

        public List<StudentInfo> StudentInfos
        {
            get { return studentInfos; }
            set
            {
                studentInfos = value;
                OnPropertyChanged(nameof(StudentInfos));
            }
        }
        private StudentInfo selectedStudentInfo;

        public StudentInfo SelectedStudentInfo
        {
            get { return selectedStudentInfo; }
            set
            {
                selectedStudentInfo = value;
                OnPropertyChanged(nameof(SelectedStudentInfo));
            }
        }
        private List<ClassInfo> classInfos;

        public List<ClassInfo> ClassInfos
        {
            get { return classInfos; }
            set
            {
                classInfos = value;
                OnPropertyChanged(nameof(ClassInfos));
            }
        }
        private ClassInfo selectedClassInfo;

        public ClassInfo SelectedClassInfo
        {
            get { return selectedClassInfo; }
            set
            {
                selectedClassInfo = value;
                OnPropertyChanged(nameof(SelectedClassInfo));
            }
        }

        private float score;

        public float Score
        {
            get { return score; }
            set
            {
                score = value;
                OnPropertyChanged(nameof(Score));
            }
        }


        public DelegateCommand ConfirmCommand { get; set; }
        private void Confirm(object parameter)
        {
            Grade g = new Grade();
            g.ClassID = SelectedClassInfo.ID;
            g.StudentID = SelectedStudentInfo.ID;
            g.ClassGrade = Score;
            StaticVariables.AAMSConnect.AddGrade(g);

        }
        public AddGradeViewModel()
        {
            ConfirmCommand = new DelegateCommand(Confirm);
            Departments = StaticVariables.AAMSConnect.GetDepartmentList();
            StudentInfos = StaticVariables.AAMSConnect.GetStudentInfoList();
            ClassInfos = StaticVariables.AAMSConnect.GetClassInfoList();
        }

    }
}
