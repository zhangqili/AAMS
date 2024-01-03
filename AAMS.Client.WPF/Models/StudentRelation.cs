namespace AAMS.Client.WPF.Models
{
    public class StudentRelation : NotificationObject
    {
		private int departmentID;

		public int DepartmentID
		{
			get { return departmentID; }
			set
			{
				departmentID = value;
				OnPropertyChanged(nameof(DepartmentID));
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
    }
}
