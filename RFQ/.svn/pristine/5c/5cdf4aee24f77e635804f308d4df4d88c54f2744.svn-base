using SSG.Core.Domain.Users;

namespace SSG.Services.Users
{
    public class UserRegistrationRequest
    {
        public User User { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public PasswordFormat PasswordFormat { get; set; }
        public bool IsApproved { get; set; }
        public string EmployeeId { get; set; }
        public int? DepartmentId { get; set; }
        
        public UserRegistrationRequest(User user, string email, string username,
            string password,
            int? departmentId,
            PasswordFormat passwordFormat,
            bool isApproved = true,
            string employeeId = "")
        {
            this.User = user;
            this.Email = email;
            this.Username = username;
            this.Password = password;
            this.PasswordFormat = passwordFormat;
            this.IsApproved = isApproved;
            this.EmployeeId = employeeId;
            this.DepartmentId = departmentId;
        }

        //public bool IsValid  
        //{
        //    get 
        //    {
        //        return (!CommonHelper.AreNullOrEmpty(
        //                    this.Email,
        //                    this.Password
        //                    ));
        //    }
        //}
    }
}
