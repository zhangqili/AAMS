using System.Data;
using System.Data.SqlClient;
using System.Windows;
using AAMS.Client.WPF.Models;

namespace AAMS.Client.WPF.Lib
{
    public class AAMSConnect
    {
        public SqlConnection SqlConnection { get; set; }
        public SqlCommand SqlCommand { get; set; }

        public string DatabaseName { get; set; }
        public AAMSConnect(string uid, string pwd,string server="localhost",int port=1433,string name = "wut")
        {
            //声明一个字符串用于储存连接数据库的字符串
            DatabaseName = name;
            string SqlConnectionStatement = $"server={server};database={DatabaseName};uid={uid};pwd={pwd};MultipleActiveResultSets=true";
            //server=localhost代表本机，如果是远程访问可填数据库的IP，端口号默认是1433可以不写
            //database=数据库名;uid=用户名（默认是sa）;pwd=密码（没有密码可以省略）
            SqlConnection = new SqlConnection(SqlConnectionStatement); //声明一个SqlConnection对象
            SqlConnection.Open(); //真正与数据库连接
            SqlCommand = new SqlCommand();
            SqlCommand.Connection = SqlConnection;
        }

        public void CreateTable()
        {
            SqlCommand.CommandText =
                @"
                CREATE TABLE t_department
                (
                	department_id int,
                	department_name varchar(255),
                	PRIMARY KEY (department_id)
                )
                CREATE TABLE t_student_info
                (
                	student_id int,
                	student_name char(100),
                	PRIMARY KEY (student_id)
                )
                CREATE TABLE t_classinfo
                (
                	class_name char(100),
                	class_id int,
                	PRIMARY KEY (class_id)
                )
                CREATE TABLE t_grade
                (
                	class_grade float,
                	class_id int FOREIGN KEY REFERENCES t_classinfo(class_id),
                	student_id int FOREIGN KEY REFERENCES t_student_info(student_id),
                	PRIMARY KEY (class_id, student_id)
                )
                CREATE TABLE t_studentrelation
                (
                	department_id int FOREIGN KEY REFERENCES t_department(department_id),
                	student_id int FOREIGN KEY REFERENCES t_student_info(student_id),
                	PRIMARY KEY (department_id, student_id)
                )
                ";
            SqlCommand.ExecuteNonQuery();
        }

        public SqlDataReader ExecuteReader(string cmd)
        {
            SqlCommand.CommandText = cmd;
            return SqlCommand.ExecuteReader();
        }
        public SqlDataAdapter ExecuteAdapter(string cmd)
        {
            SqlCommand.CommandText = cmd;
            return new SqlDataAdapter(SqlCommand);
        }
        public int ExecuteNonQuery(string cmd)
        {
            int res;
            SqlCommand.CommandText = cmd;
            try
            {
                res = SqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                res = 1;
            }
            return res;
        }
        public int AddStudentInfo(StudentInfo s)
        {
            string sql = @"INSERT INTO t_student_info (student_id,student_name) VALUES";
            sql += string.Format("({0},\'{1}\')", s.ID, s.Name);
            return ExecuteNonQuery(sql);
        }
        public int AddStudent(StudentInfo s, StudentRelation r)
        {
            int res;
            string sql = @"INSERT INTO t_student_info (student_id,student_name) VALUES";
            sql += string.Format("({0},\'{1}\')", s.ID, s.Name);
            SqlCommand.CommandText = sql;
            try
            {
                res = SqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return 1;
            }
            sql = @"INSERT INTO t_studentrelation (department_id,student_id) VALUES";
            sql += string.Format("({0},\'{1}\')", r.DepartmentID, r.StudentID);
            SqlCommand.CommandText = sql;
            try
            {
                res = SqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return 1;
            }
            return 0;
        }
        public int UpdateStudent(StudentInfo s, StudentRelation r)
        {
            int res;
            string sql = $"UPDATE t_student_info SET student_name=\'{s.Name}\' WHERE student_id={s.ID}";
            SqlCommand.CommandText = sql;
            try
            {
                res = SqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return 1;
            }
            sql = $"UPDATE t_studentrelation SET department_id={r.DepartmentID} WHERE student_id={r.StudentID}";
            SqlCommand.CommandText = sql;
            try
            {
                res = SqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return 1;
            }
            return 0;
        }
        public int DeleteStudent(StudentInfo s, StudentRelation r)
        {
            int res;
            string sql = $"DELETE FROM t_studentrelation WHERE student_id = {s.ID}";
            SqlCommand.CommandText = sql;
            try
            {
                res = SqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return 1;
            }
            sql = $"DELETE FROM t_student_info WHERE student_id = {s.ID}";
            SqlCommand.CommandText = sql;
            try
            {
                res = SqlCommand.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return 1;
            }
            return 0;
        }
        public int AddDepartment(Department d)
        {
            string sql = $"INSERT INTO t_department (department_id,department_name) VALUES ({d.ID},\'{d.Name}\')";
            return ExecuteNonQuery(sql);
        }
        public int UpdateDepartment(Department d)
        {
            string sql = $"UPDATE t_department SET department_name=\'{d.Name}\' WHERE department_id={d.ID}";
            return ExecuteNonQuery(sql);
        }
        public int DeleteDepartment(Department d)
        {
            string sql = $"DELETE FROM t_department WHERE department_id = {d.ID}";
            return ExecuteNonQuery(sql);
        }
        public int AddClassInfo(ClassInfo c)
        {
            string sql = @"INSERT INTO t_classinfo VALUES";
            sql += string.Format("(\'{0}\',{1})", c.Name, c.ID);
            return ExecuteNonQuery(sql);
        }
        public int UpdateClassInfo(ClassInfo c)
        {
            string sql = $"UPDATE t_classinfo SET class_name = \'{c.Name}\' WHERE class_id={c.ID}";
            return ExecuteNonQuery(sql);
        }
        public int DeleteClassInfo(ClassInfo c)
        {
            string sql = $"DELETE FROM t_classinfo WHERE class_id = {c.ID}";
            return ExecuteNonQuery(sql);
        }
        public int AddGrade(Grade g)
        {
            string sql = $"INSERT INTO t_grade VALUES ({g.ClassGrade},{g.ClassID},{g.StudentID})";
            return ExecuteNonQuery(sql);
        }
        public int UpdateGrade(Grade g)
        {
            string sql = " UPDATE t_grade ";
            sql += string.Format("SET class_grade = {0} ", g.ClassGrade);
            sql += string.Format("WHERE class_id = {0} AND student_id = {1}", g.ClassID, g.StudentID);
            return ExecuteNonQuery(sql);
        }
        public int ModifyGrade(Grade g,Grade old)
        {
            string sql = $"UPDATE t_grade " +
                         $"SET class_grade = {g.ClassGrade}, class_id = {g.ClassID}, student_id = {g.StudentID}" +
                         $"WHERE class_id = {old.ClassID} AND student_id = {old.StudentID}";
            return ExecuteNonQuery(sql);
        }
        public int DeleteGrade(Grade g)
        {
            string sql = $"DELETE FROM t_grade WHERE class_id = {g.ClassID} AND student_id = {g.StudentID}";
            return ExecuteNonQuery(sql);
        }
        public DataTable GetGradeTable()
        {
            SqlCommand.CommandText = "SELECT * FROM t_grade";
            SqlDataAdapter adapter = new SqlDataAdapter(SqlCommand);
            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            dataTable.Columns[0].ColumnName = "成绩";
            dataTable.Columns[1].ColumnName = "课程";
            dataTable.Columns[2].ColumnName = "课程编号";
            dataTable.Columns[3].ColumnName = "学号";
            return dataTable;
        }
        public List<Department> GetDepartmentList()
        {
            SqlCommand.CommandText = "SELECT department_id,department_name FROM t_department";
            SqlDataAdapter adapter = new SqlDataAdapter(SqlCommand);
            SqlDataReader res = SqlCommand.ExecuteReader();
            List<Department> deptList = new List<Department>();
            for (int i = 0; res.Read(); i++)
            {
                Department dept = new Department();
                dept.ID = res.GetInt32(0);
                dept.Name = res.GetString(1).Trim();
                deptList.Add(dept);
            }
            res.Dispose();
            return deptList;
        }
        public List<Department> GetDepartmentListLike(string filter)
        {
            string sql = $"SELECT department_id,department_name FROM t_department " +
                         $"WHERE CONVERT(varchar(16),department_id) LIKE \'%{filter}%\'" +
                         $"OR department_name LIKE \'%{filter}%\'";
            SqlDataReader res = ExecuteReader(sql);
            List<Department> deptList = new List<Department>();
            for (int i = 0; res.Read(); i++)
            {
                Department dept = new Department();
                dept.ID = res.GetInt32(0);
                dept.Name = res.GetString(1).Trim();
                deptList.Add(dept);
            }
            res.Dispose();
            return deptList;
        }
        public List<ClassInfo> GetClassInfoList()
        {
            SqlCommand.CommandText = "SELECT class_name,class_id FROM t_classinfo";
            SqlDataAdapter adapter = new SqlDataAdapter(SqlCommand);
            SqlDataReader res = SqlCommand.ExecuteReader();
            List<ClassInfo> classInfos = new List<ClassInfo>();
            for (int i = 0; res.Read(); i++)
            {
                ClassInfo classInfo = new ClassInfo();
                classInfo.ID = res.GetInt32(1);
                classInfo.Name = res.GetString(0).Trim();
                classInfos.Add(classInfo);
            }
            res.Dispose();
            return classInfos;
        }
        public List<ClassInfo> GetClassInfoListLike(string filter)
        {
            string sql = $"SELECT class_name,class_id FROM t_classinfo " +
                         $"WHERE CONVERT(varchar(16),class_id) LIKE \'%{filter}%\' " +
                         $"OR class_name LIKE \'%{filter}%\'";
            SqlDataReader res = ExecuteReader(sql);
            List<ClassInfo> classInfos = new List<ClassInfo>();
            for (int i = 0; res.Read(); i++)
            {
                ClassInfo classInfo = new ClassInfo();
                classInfo.ID = res.GetInt32(1);
                classInfo.Name = res.GetString(0).Trim();
                classInfos.Add(classInfo);
            }
            res.Dispose();
            return classInfos;
        }
        public List<StudentInfo> GetStudentInfoList()
        {
            SqlCommand.CommandText = "SELECT student_id,student_name FROM t_student_info";
            SqlDataAdapter adapter = new SqlDataAdapter(SqlCommand);
            SqlDataReader res = SqlCommand.ExecuteReader();
            List<StudentInfo> studentInfos = new List<StudentInfo>();
            for (int i = 0; res.Read(); i++)
            {
                StudentInfo studentInfo = new StudentInfo();
                studentInfo.ID = res.GetInt32(0);
                studentInfo.Name = res.GetString(1).Trim();
                studentInfos.Add(studentInfo);
            }
            res.Dispose();

            return studentInfos;
        }
        public List<StudentInfo> GetStudentInfoListLike(string filter)
        {
            string sql = $"SELECT student_id,student_name FROM t_student_info " +
                                     $"WHERE CONVERT(varchar(16),student_id) LIKE \'%{filter}%\' " +
                                     $"OR student_name LIKE \'%{filter}%\'";
            SqlDataReader res = ExecuteReader(sql);
            List<StudentInfo> studentInfos = new List<StudentInfo>();
            for (int i = 0; res.Read(); i++)
            {
                StudentInfo studentInfo = new StudentInfo();
                studentInfo.ID = res.GetInt32(0);
                studentInfo.Name = res.GetString(1).Trim();
                studentInfos.Add(studentInfo);
            }
            res.Dispose();

            return studentInfos;
        }
        public List<Grade> GetGradeList()
        {
            SqlCommand.CommandText = "SELECT class_grade,class_id FROM t_grade";
            SqlDataAdapter adapter = new SqlDataAdapter(SqlCommand);
            SqlDataReader res = SqlCommand.ExecuteReader();
            List<Grade> grades = new List<Grade>();
            for (int i = 0; res.Read(); i++)
            {
                Grade grade = new();
                grade.ClassGrade = Convert.ToSingle(res.GetDouble(0));
                grade.ClassID = res.GetInt32(1);
                grade.StudentID = res.GetInt32(2);
                grades.Add(grade);
            }
            res.Dispose();
            return grades;
        }
        public List<GradeInfo> GetGradeInfoList()
        {
            string cmd = "USE wut SELECT class_grade,class_name,t_grade.class_id,t_grade.student_id,t_student_info.student_name " +
                         "FROM t_grade " +
                         "JOIN t_classinfo ON t_grade.class_id = t_classinfo.class_id " +
                         "JOIN t_student_info ON t_grade.student_id = t_student_info.student_id ";
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
        public List<GradeInfo> GetGradeInfoListLike(string filter)
        {
            string cmd =
                $"USE wut SELECT class_grade,class_name,t_grade.class_id,t_grade.student_id,t_student_info.student_name " +
                $"FROM t_grade " +
                $"JOIN t_classinfo ON t_grade.class_id = t_classinfo.class_id " +
                $"JOIN t_student_info ON t_grade.student_id = t_student_info.student_id " +
                $"WHERE CONVERT(varchar(16),t_grade.class_id) LIKE \'%{filter}%\' " +
                $"OR CONVERT(varchar(16),class_grade) LIKE \'%{filter}%\'" +
                $"OR CONVERT(varchar(16),t_grade.student_id) LIKE \'%{filter}%\'" +
                $"OR class_name LIKE \'%{filter}%\'" +
                $"OR t_student_info.student_name LIKE \'%{filter}%\'";
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
        public List<GradeInfo> GetGradeInfoListLikeOrderBy(string filter,int order,bool isDesc)
        {
            string cmd =
                $"USE wut SELECT class_grade,class_name,t_grade.class_id,t_grade.student_id,t_student_info.student_name " +
                $"FROM t_grade " +
                $"JOIN t_classinfo ON t_grade.class_id = t_classinfo.class_id " +
                $"JOIN t_student_info ON t_grade.student_id = t_student_info.student_id " +
                $"WHERE CONVERT(varchar(16),t_grade.class_id) LIKE \'%{filter}%\' " +
                $"OR CONVERT(varchar(16),class_grade) LIKE \'%{filter}%\' " +
                $"OR CONVERT(varchar(16),t_grade.student_id) LIKE \'%{filter}%\' " +
                $"OR class_name LIKE \'%{filter}%\' " +
                $"OR t_student_info.student_name LIKE \'%{filter}%\' ";
            switch (order)
            {
                case 0:
                    cmd += $"ORDER BY class_grade ";
                    break;
                case 1:
                    cmd += $"ORDER BY t_grade.class_id ";
                    break;
                case 2:
                    cmd += $"ORDER BY class_name ";
                    break;
                case 3:
                    cmd += $"ORDER BY t_grade.student_id ";
                    break;
                case 4:
                    cmd += $"ORDER BY t_student_info.student_name ";
                    break;
            }
            if(isDesc)
                cmd += $"DESC";
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
        ~AAMSConnect()
        {
            //SqlConnection.Close();
            //SqlConnection.Dispose();
        }
    }
}
