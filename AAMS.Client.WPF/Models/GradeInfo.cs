using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AAMS.Client.WPF.Models
{
    public class GradeInfo : NotificationObject
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
        private string className;

        public string ClassName
        {
            get { return className; }
            set
            {
                className = value;
                OnPropertyChanged(nameof(ClassName));
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
