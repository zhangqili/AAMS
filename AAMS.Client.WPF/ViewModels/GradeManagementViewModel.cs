using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAMS.Client.WPF;
using AAMS.Client.WPF.Lib;
using AAMS.Client.WPF.Models;

namespace AAMS.Client.WPF.ViewModels
{
    public class GradeManagementViewModel : NotificationObject
    {

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

        private StudentInfo selectedNewStudentInfo;

        public StudentInfo SelectedNewStudentInfo
        {
            get { return selectedNewStudentInfo; }
            set
            {
                selectedNewStudentInfo = value;
                OnPropertyChanged(nameof(SelectedNewStudentInfo));
            }
        }
        private ClassInfo selectedNewClassInfo;

        public ClassInfo SelectedNewClassInfo
        {
            get { return selectedNewClassInfo; }
            set
            {
                selectedNewClassInfo = value;
                OnPropertyChanged(nameof(SelectedNewClassInfo));
            }
        }

        private GradeInfo selectedGradeInfo;

        public GradeInfo SelectedGradeInfo
        {
            get { return selectedGradeInfo; }
            set
            {
                selectedGradeInfo = value;
                IsSelected = selectedGradeInfo != null;
                if (IsSelected)
                {
                    SelectedClassInfo = ClassInfos.First(x => x.ID == SelectedGradeInfo.ClassID);
                    SelectedStudentInfo = StudentInfos.First(x => x.ID == SelectedGradeInfo.StudentID);
                    Score = selectedGradeInfo.ClassGrade;
                }
                OnPropertyChanged(nameof(GradeInfo));
            }
        }


        private ObservableCollection<ClassInfo> classInfos;

        public ObservableCollection<ClassInfo> ClassInfos
        {
            get { return classInfos; }
            set
            {
                classInfos = value;
                OnPropertyChanged(nameof(ClassInfos));
            }
        }
        private ObservableCollection<StudentInfo> studentInfos;

        public ObservableCollection<StudentInfo> StudentInfos
        {
            get { return studentInfos; }
            set
            {
                studentInfos = value;
                OnPropertyChanged(nameof(StudentInfos));
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
        private ObservableCollection<GradeInfo> gradeInfos;

        public ObservableCollection<GradeInfo> GradeInfos
        {
            get { return gradeInfos; }
            set
            {
                gradeInfos = value;
                if (GradeInfos.Count > 0)
                {
                    AverageGrade = GradeInfos.Average(x => x.ClassGrade);
                    PeakGrade = GradeInfos.Max(x => x.ClassGrade);
                }

                OnPropertyChanged(nameof(GradeInfos));
            }
        }

        private float newScore;

        public float NewScore
        {
            get { return newScore; }
            set
            {
                newScore = value;
                OnPropertyChanged(nameof(NewScore));
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
        private string filterText;
        public string FilterText
        {
            get { return filterText; }
            set
            {
                filterText = value;
                GradeInfos = new ObservableCollection<GradeInfo>(StaticVariables.AAMSConnect.GetGradeInfoListLikeOrderBy(FilterText,OrderBy,IsDesc==1));
                OnPropertyChanged(nameof(FilterText));
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
        private int orderBy;

        public int OrderBy
        {
            get { return orderBy; }
            set
            {
                orderBy = value;
                GradeInfos = new ObservableCollection<GradeInfo>(StaticVariables.AAMSConnect.GetGradeInfoListLikeOrderBy(FilterText, OrderBy, IsDesc == 1));
                OnPropertyChanged(nameof(OrderBy));
            }
        }
        private int isDesc;

        public int IsDesc
        {
            get { return isDesc; }
            set
            {
                isDesc = value;
                GradeInfos = new ObservableCollection<GradeInfo>(StaticVariables.AAMSConnect.GetGradeInfoListLikeOrderBy(FilterText, OrderBy, IsDesc == 1));
                OnPropertyChanged(nameof(IsDesc));
            }
        }
        private double averageGrade;

        public double AverageGrade
        {
            get { return averageGrade; }
            set
            {
                averageGrade = value;
                OnPropertyChanged(nameof(AverageGrade));
            }
        }
        private double peakGrade;

        public double PeakGrade
        {
            get { return peakGrade; }
            set
            {
                peakGrade = value;
                OnPropertyChanged(nameof(PeakGrade));
            }
        }

        public DelegateCommand ConfirmCommand { get; set; }
        private void Confirm(object parameter)
        {
            Grade g = new Grade();
            g.ClassID = SelectedNewClassInfo.ID;
            g.StudentID = SelectedNewStudentInfo.ID;
            g.ClassGrade = NewScore;
            StaticVariables.AAMSConnect.AddGrade(g);
            GradeInfos = new ObservableCollection<GradeInfo>(StaticVariables.AAMSConnect.GetGradeInfoListLikeOrderBy(FilterText, OrderBy, IsDesc == 1));
        }

        public void Update()
        {
            StudentInfos = new ObservableCollection<StudentInfo>(StaticVariables.AAMSConnect.GetStudentInfoList());
            //Departments = new ObservableCollection<Department>(StaticVariables.AAMSConnect.GetDepartmentList());
            ClassInfos = new ObservableCollection<ClassInfo>(StaticVariables.AAMSConnect.GetClassInfoList());
            GradeInfos = new ObservableCollection<GradeInfo>(StaticVariables.AAMSConnect.GetGradeInfoListLikeOrderBy(FilterText, OrderBy, IsDesc == 1));
        }
        public DelegateCommand SaveCommand { get; set; }
        private void Save(object parameter)
        {
            StaticVariables.AAMSConnect.ModifyGrade(
                new Grade() { ClassGrade = Score, ClassID = SelectedClassInfo.ID, StudentID = SelectedStudentInfo.ID },
                new Grade() { ClassGrade = SelectedGradeInfo.ClassGrade, ClassID = SelectedGradeInfo.ClassID, StudentID = SelectedGradeInfo.StudentID });
            GradeInfos = new ObservableCollection<GradeInfo>(StaticVariables.AAMSConnect.GetGradeInfoListLikeOrderBy(FilterText, OrderBy, IsDesc == 1));
        }
        public DelegateCommand DeleteCommand { get; set; }
        private void Delete(object parameter)
        {
            StaticVariables.AAMSConnect.DeleteGrade(new Grade(){ClassGrade = SelectedGradeInfo.ClassGrade,ClassID = SelectedGradeInfo.ClassID,StudentID = SelectedGradeInfo.StudentID});
            GradeInfos = new ObservableCollection<GradeInfo>(StaticVariables.AAMSConnect.GetGradeInfoListLikeOrderBy(FilterText, OrderBy, IsDesc == 1));
        }

        public GradeManagementViewModel()
        {
            ConfirmCommand = new DelegateCommand(Confirm);
            SaveCommand = new DelegateCommand(Save);
            DeleteCommand = new DelegateCommand(Delete);
            OrderBy = 1;
            IsDesc = 0;
            Update();
        }
    }
}
