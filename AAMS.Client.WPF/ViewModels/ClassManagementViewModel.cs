using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AAMS.Client.WPF.Models;
using AAMS.Client.WPF;
using AAMS.Client.WPF.Lib;
using static AAMS.Client.WPF.ViewModels.GradeManagementViewModel;
using System.Data.SqlClient;
using OxyPlot;
using OxyPlot.Series;
using OxyPlot.Axes;
using OxyPlot.Legends;

namespace AAMS.Client.WPF.ViewModels
{
    public class ClassManagementViewModel : NotificationObject
    {
        private ClassInfo selectedClassInfo;

        public ClassInfo SelectedClassInfo
        {
            get { return selectedClassInfo; }
            set
            {
                selectedClassInfo = value;
                IsSelected = selectedClassInfo != null;
                OnPropertyChanged(nameof(SelectedClassInfo));
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
        private ObservableCollection<ElectiveStudent> electiveStudents;

        public ObservableCollection<ElectiveStudent> ElectiveStudents
        {
            get { return electiveStudents; }
            set
            {
                electiveStudents = value;
                OnPropertyChanged(nameof(ElectiveStudents));
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
        private string filterText;

        public string FilterText
        {
            get { return filterText; }
            set
            {
                filterText = value;
                ClassInfos = new ObservableCollection<ClassInfo>(StaticVariables.AAMSConnect.GetClassInfoListLike(FilterText));
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
        public int GetAverageGrade(int id)
        {
            string cmd = $"SELECT AVG(class_grade) FROM t_grade WHERE class_id={id}";
            SqlDataReader rdr = StaticVariables.AAMSConnect.ExecuteReader(cmd);
            rdr.Read();
            AverageGrade = Convert.ToSingle(rdr.GetDouble(0));
            rdr.Dispose();
            return 0;
        }
        public int GetElectiveStudents(int id)
        {
            string cmd = $"SELECT t_grade.student_id,student_name,class_grade FROM t_grade JOIN t_student_info ON t_grade.student_id=t_student_info.student_id WHERE class_id={id} ORDER BY class_grade DESC";
            SqlDataReader rdr = StaticVariables.AAMSConnect.ExecuteReader(cmd);
            ObservableCollection<ElectiveStudent> es = new ObservableCollection<ElectiveStudent>();
            for (int i = 0; rdr.Read(); i++)
            {
                ElectiveStudent e = new ElectiveStudent();
                e.StudentID = rdr.GetInt32(0);
                e.StudentName = rdr.GetString(1).Trim();
                e.ClassGrade = Convert.ToSingle(rdr.GetDouble(2));
                es.Add(e);
            }
            rdr.Dispose();
            ElectiveStudents = es;
            return 0;
        }
        public void ShowPlot()
        {
            
            PlotModel p = new PlotModel();
            var barSeries = new BarSeries();
            barSeries.ItemsSource = ElectiveStudents.Select(x=>new BarItem(){Value = x.ClassGrade}).ToList();
            barSeries.BarWidth = 5;
            barSeries.LabelPlacement = LabelPlacement.Outside;
            //barSeries.LabelFormatString = "{0.0}%";
            var categorySeries = new CategoryAxis();
            categorySeries.ItemsSource = ElectiveStudents.Select(x => x.StudentName);
            categorySeries.Position = AxisPosition.Left;
            p.Series.Add(barSeries);
            p.Axes.Add(categorySeries);
            p.InvalidatePlot(true);
            p.Transpose();
            PlotModel = p;

        }
        public DelegateCommand ConfirmCommand { get; set; }
        private void Confirm(object parameter)
        {
            ClassInfo d = new ClassInfo();
            d.Name = NewName;
            d.ID = NewID;
            StaticVariables.AAMSConnect.AddClassInfo(d);
            ClassInfos = new ObservableCollection<ClassInfo>(StaticVariables.AAMSConnect.GetClassInfoListLike(FilterText));
        }
        public DelegateCommand DeleteCommand { get; set; }
        private void Delete(object parameter)
        {
            Department d = new Department();
            StaticVariables.AAMSConnect.DeleteClassInfo(SelectedClassInfo);
            ClassInfos = new ObservableCollection<ClassInfo>(StaticVariables.AAMSConnect.GetClassInfoListLike(FilterText));
        }
        public DelegateCommand SaveCommand { get; set; }
        private void Save(object parameter)
        {
            StaticVariables.AAMSConnect.UpdateClassInfo(SelectedClassInfo);
            ClassInfos = new ObservableCollection<ClassInfo>(StaticVariables.AAMSConnect.GetClassInfoListLike(FilterText));
        }
        public DelegateCommand ShowDetailCommand { get; set; }
        private void ShowDetail(object parameter)
        {
            if (SelectedClassInfo != null)
            {
                int count;
                string cmd = $"SELECT COUNT(1) FROM t_grade WHERE class_id={SelectedClassInfo.ID}";
                SqlDataReader rdr = StaticVariables.AAMSConnect.ExecuteReader(cmd);
                rdr.Read();
                count = rdr.GetInt32(0);
                rdr.Dispose();
                if (count > 0)
                {
                    GetAverageGrade(SelectedClassInfo.ID);
                    GetElectiveStudents(SelectedClassInfo.ID);
                    ShowPlot();
                }
                else
                {
                    ElectiveStudents = new();
                    PlotModel = new PlotModel();
                    AverageGrade = 0;
                }

            }
        }
        public void Update()
        {
            ClassInfos = new ObservableCollection<ClassInfo>(StaticVariables.AAMSConnect.GetClassInfoListLike(FilterText));
        }
        public ClassManagementViewModel()
        {
            ConfirmCommand = new DelegateCommand(Confirm);
            ShowDetailCommand = new DelegateCommand(ShowDetail);
            SaveCommand = new DelegateCommand(Save);
            DeleteCommand = new DelegateCommand(Delete);
            Update();
        }

        public class ElectiveStudent : NotificationObject
        {
            private float classGrade;

            public float ClassGrade
            {
                get { return classGrade; }
                set
                {
                    classGrade = value;
                    OnPropertyChanged(nameof(ClassGrade));
                }
            }
            private int studentID;

            public int StudentID
            {
                get { return studentID; }
                set
                {
                    studentID = value;
                    OnPropertyChanged(nameof(StudentID));
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

        }
    }
}
