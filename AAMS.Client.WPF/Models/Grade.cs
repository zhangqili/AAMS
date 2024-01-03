namespace AAMS.Client.WPF.Models
{
    public class Grade : NotificationObject
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
        private int classID;

        public int ClassID
        {
            get { return classID; }
            set
            {
                classID = value;
                OnPropertyChanged(nameof(ClassID));
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
