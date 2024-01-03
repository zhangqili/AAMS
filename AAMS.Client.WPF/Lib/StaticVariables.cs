using AAMS.Client.WPF.ViewModels;
using AAMS.Client.WPF.Views;

namespace AAMS.Client.WPF.Lib
{
    public class StaticVariables
    {
        public static AAMSConnect AAMSConnect { get; set; }
        public static ClassManagementView ClassManagementView { get; set; }
        public static ClassManagementViewModel? ClassManagementViewModel { get; set; }
        public static DepartmentManagementView DepartmentManagementView { get; set; }
        public static DepartmentManagementViewModel? DepartmentManagementViewModel { get; set; }
        public static GradeManagementView GradeManagementView { get; set; }
        public static GradeManagementViewModel? GradeManagementViewModel { get; set; }
        public static StudentManagementView StudentManagementView { get; set; }
        public static StudentManagementViewModel? StudentManagementViewModel { get; set; }
    }
}
