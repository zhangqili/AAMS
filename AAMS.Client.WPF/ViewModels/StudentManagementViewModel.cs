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
using OxyPlot.Axes;
using OxyPlot.Series;
using OxyPlot;
using static AAMS.Client.WPF.ViewModels.ClassManagementViewModel;
using static AAMS.Client.WPF.ViewModels.GradeManagementViewModel;
using System.Windows.Media.Media3D;

namespace AAMS.Client.WPF.ViewModels
{
    public class StudentManagementViewModel : NotificationObject
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
        private ObservableCollection<StudentWithRelation> studentWithRelations;

        public ObservableCollection<StudentWithRelation> StudentWithRelations
        {
            get { return studentWithRelations; }
            set
            {
                studentWithRelations = value;
                OnPropertyChanged(nameof(StudentWithRelations));
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
        private Department selectedNewDepartment;

        public Department SelectedNewDepartment
        {
            get { return selectedNewDepartment; }
            set
            {
                selectedNewDepartment = value;
                OnPropertyChanged(nameof(SelectedNewDepartment));
            }
        }

        private StudentWithRelation selectedStudentWithRelation;

        public StudentWithRelation SelectedStudentWithRelation
        {
            get { return selectedStudentWithRelation; }
            set
            {
                selectedStudentWithRelation = value;
                IsSelected = selectedStudentWithRelation != null;
                OnPropertyChanged(nameof(SelectedStudentWithRelation));
            }
        }
        private string filterText;

        public string FilterText
        {
            get { return filterText; }
            set
            {
                filterText = value;
                GetStudentWithRelationsLike(filterText);
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
        private int classCount;

        public int ClassCount
        {
            get { return classCount; }
            set
            {
                classCount = value;
                OnPropertyChanged(nameof(ClassCount));
            }
        }

        private PlotModel plotModel;

        public PlotModel PlotModel
        {
            get { return plotModel; }
            set
            {
                plotModel = value;
                OnPropertyChanged(nameof(PlotModel));
            }
        }
        private ObservableCollection<GradeInfo> gradeInfos;

        public ObservableCollection<GradeInfo> GradeInfos
        {
            get { return gradeInfos; }
            set
            {
                gradeInfos = value;
                OnPropertyChanged(nameof(GradeInfos));
            }
        }

        public DelegateCommand SaveCommand { get; set; }
        private void Save(object parameter)
        {
            StudentInfo s = new StudentInfo();
            s.Name = SelectedStudentWithRelation.StudentName;
            s.ID = SelectedStudentWithRelation.StudentRelation.StudentID;
            StudentRelation sr = new StudentRelation();
            sr.StudentID = SelectedStudentWithRelation.StudentRelation.StudentID;
            sr.DepartmentID = SelectedDepartment.ID;
            StaticVariables.AAMSConnect.UpdateStudent(s, sr);
            GetStudentWithRelationsLike(FilterText);
        }
        public DelegateCommand DeleteCommand { get; set; }
        private void Delete(object parameter)
        {
            StudentInfo s = new StudentInfo();
            s.Name = SelectedStudentWithRelation.StudentName;
            s.ID = SelectedStudentWithRelation.StudentRelation.StudentID;
            StudentRelation sr = new StudentRelation();
            sr.StudentID = SelectedStudentWithRelation.StudentRelation.StudentID;
            sr.DepartmentID = SelectedDepartment.ID;
            StaticVariables.AAMSConnect.DeleteStudent(s, sr);
            GetStudentWithRelationsLike(FilterText);
        }
        public DelegateCommand ConfirmCommand { get; set; }

        private void Confirm(object parameter)
        {
            StudentInfo d = new StudentInfo();
            d.Name = NewName;
            d.ID = NewID;
            StudentRelation sr = new StudentRelation();
            sr.StudentID = NewID;
            sr.DepartmentID = SelectedNewDepartment.ID;
            StaticVariables.AAMSConnect.AddStudent(d, sr);
            GetStudentWithRelationsLike(FilterText);

        }

        public DelegateCommand ShowDetailCommand { get; set; }
        private void ShowDetail(object parameter)
        {
            if (SelectedStudentWithRelation != null)
            {
                SelectedDepartment = Departments.First(x=>x.ID== SelectedStudentWithRelation.StudentRelation.DepartmentID);
                GetClassCount(SelectedStudentWithRelation.StudentRelation.StudentID);
                if (ClassCount != 0)
                {
                    GetAverageGrade(SelectedStudentWithRelation.StudentRelation.StudentID);
                    GradeInfos = new ObservableCollection<GradeInfo>(GetGradeInfos(SelectedStudentWithRelation.StudentRelation.StudentID));
                    ShowPlot();
                }
                else
                {
                    AverageGrade = 0;
                    ClassCount = 0;
                    GradeInfos = new();
                    //SelectedStudentWithRelation = new();
                    PlotModel = new();
                }
            }
        }
        public void Update()
        {
            StudentInfos = new ObservableCollection<StudentInfo>(StaticVariables.AAMSConnect.GetStudentInfoList());
            GetStudentWithRelationsLike(FilterText);
            Departments = new ObservableCollection<Department>(StaticVariables.AAMSConnect.GetDepartmentList());
        }
        public int GetClassCount(int id)
        {
            string cmd = $"SELECT COUNT(1) FROM t_grade WHERE student_id={id}";
            SqlDataReader rdr = StaticVariables.AAMSConnect.ExecuteReader(cmd);
            rdr.Read();
            ClassCount = rdr.GetInt32(0);
            rdr.Dispose();
            return 0;
        }
        public int GetAverageGrade(int id)
        {
            string cmd = $"SELECT AVG(class_grade) FROM t_grade WHERE student_id={id}";
            SqlDataReader rdr = StaticVariables.AAMSConnect.ExecuteReader(cmd);
            rdr.Read();
            AverageGrade = Convert.ToSingle(rdr.GetDouble(0));
            rdr.Dispose();
            return 0;
        }
        public List<GradeInfo> GetGradeInfos(int id)
        {
            string cmd = "USE wut SELECT class_grade,class_name,t_grade.class_id,t_grade.student_id,t_student_info.student_name " +
                         "FROM t_grade " +
                         "JOIN t_classinfo ON t_grade.class_id = t_classinfo.class_id " +
                         "JOIN t_student_info ON t_grade.student_id = t_student_info.student_id "+
                         $"WHERE t_grade.student_id={id}";
            SqlDataReader res = StaticVariables.AAMSConnect.ExecuteReader(cmd);
            List<GradeInfo> tempGradeInfos = new List<GradeInfo>();
            for (int i = 0; res.Read(); i++)
            {
                GradeInfo g = new GradeInfo();
                g.ClassGrade = Convert.ToSingle(res.GetDouble(0));
                g.ClassName = res.GetString(1).Trim();
                g.ClassID = res.GetInt32(2);
                g.StudentID = res.GetInt32(3);
                g.StudentName = res.GetString(4).Trim();
                tempGradeInfos.Add(g);
            }
            res.Dispose();
            return tempGradeInfos;
        }
        public int ShowPlot()
        {
            PlotModel p = new PlotModel();
            /*
            var barSeries = new BarSeries();
            barSeries.ItemsSource = GradeInfos.Select(x => new BarItem() { Value = x.ClassGrade }).ToList();
            barSeries.BarWidth = 5;
            barSeries.LabelPlacement = LabelPlacement.Outside;
            //barSeries.LabelFormatString = "{0.0}%";
            var categorySeries = new CategoryAxis();
            categorySeries.ItemsSource = GradeInfos.Select(x => x.ClassName);
            categorySeries.Position = AxisPosition.Left;
            p.Series.Add(barSeries);
            p.Axes.Add(categorySeries);
            p.InvalidatePlot(true);
            p.Transpose();
            */

            dynamic seriesP1 = new PieSeries { StrokeThickness = 2.0, InsideLabelPosition = 0.8, AngleSpan = 360, StartAngle = 0 };
            seriesP1.OutsideLabelFormat = "{1:0.0}";
            seriesP1.InsideLabelFormat = "{0:0.0}";
            seriesP1.InsideLabelColor=OxyColors.White;
            foreach (var VARIABLE in GradeInfos)
            {
                seriesP1.Slices.Add(new PieSlice(VARIABLE.ClassName, VARIABLE.ClassGrade) { IsExploded = true });
            }

            p.Series.Add(seriesP1);

            PlotModel = p;
            return 0;
        }
        public int GetStudentWithRelations()
        {
            string cmd = $"SELECT t_student_info.student_id,student_name,t_studentrelation.department_id,department_name " +
                         $"FROM (t_student_info JOIN t_studentrelation ON t_student_info.student_id=t_studentrelation.student_id) " +
                         $"JOIN t_department ON t_studentrelation.department_id=t_department.department_id " +
                         $"";
            SqlDataReader rdr = StaticVariables.AAMSConnect.ExecuteReader(cmd);
            ObservableCollection<StudentWithRelation> swr = new ObservableCollection<StudentWithRelation>();
            for (int i = 0; rdr.Read(); i++)
            {
                StudentWithRelation s = new StudentWithRelation();
                s.StudentRelation.StudentID = rdr.GetInt32(0);
                s.StudentName = rdr.GetString(1).Trim();
                s.StudentRelation.DepartmentID = rdr.GetInt32(2);
                s.DepartmentName = rdr.GetString(3).Trim();
                swr.Add(s);
            }
            rdr.Dispose();
            StudentWithRelations = swr;
            return 0;
        }
        public int GetStudentWithRelationsLike(string filter)
        {
            string cmd = $"SELECT t_student_info.student_id,student_name,t_studentrelation.department_id,department_name " +
                         $"FROM (t_student_info JOIN t_studentrelation ON t_student_info.student_id=t_studentrelation.student_id) " +
                         $"JOIN t_department ON t_studentrelation.department_id=t_department.department_id " +
                         $"WHERE CONVERT(varchar(16),t_student_info.student_id) LIKE \'%{filter}%\' " +
                         $"OR student_name LIKE \'%{filter}%\' " + 
                         $"OR department_name LIKE \'%{filter}%\'";
            SqlDataReader rdr = StaticVariables.AAMSConnect.ExecuteReader(cmd);
            ObservableCollection<StudentWithRelation> swr = new ObservableCollection<StudentWithRelation>();
            for (int i = 0; rdr.Read(); i++)
            {
                StudentWithRelation s = new StudentWithRelation();
                s.StudentRelation = new StudentRelation();
                s.StudentRelation.StudentID = rdr.GetInt32(0);
                s.StudentName = rdr.GetString(1).Trim();
                s.StudentRelation.DepartmentID = rdr.GetInt32(2);
                s.DepartmentName = rdr.GetString(3).Trim();
                swr.Add(s);
            }
            rdr.Dispose();
            StudentWithRelations = swr;
            return 0;
        }
        public StudentManagementViewModel()
        {
            ConfirmCommand = new DelegateCommand(Confirm);
            ShowDetailCommand = new DelegateCommand(ShowDetail);
            SaveCommand = new DelegateCommand(Save);
            DeleteCommand = new DelegateCommand(Delete);
            Update();
        }

        public class StudentWithRelation : NotificationObject
        {
            private StudentRelation studentRelation;

            public StudentRelation StudentRelation
            {
                get { return studentRelation; }
                set
                {
                    studentRelation = value;
                    OnPropertyChanged(nameof(StudentRelation));
                }
            }

            private string studentName;

            public string StudentName
            {
                get { return studentName; }
                set
                {
                    studentName = value;
                    OnPropertyChanged(nameof(StudentName));
                }
            }
            private string departmentName;

            public string DepartmentName
            {
                get { return departmentName; }
                set
                {
                    departmentName = value;
                    OnPropertyChanged(nameof(DepartmentName));
                }
            }

        }
    }
}

