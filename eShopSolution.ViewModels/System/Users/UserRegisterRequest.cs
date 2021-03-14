using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eShopSolution.ViewModels.System.Users
{
    public class UserRegisterRequest
    {
        string firstName;
        string lastName;
        DateTime dob;
        [Display(Name = "Tên")]
        public string FirstName { get => firstName; set => firstName = value; }
        [Display(Name = "Họ")]
        public string LastName { get => lastName; set => lastName = value; }
        [Display(Name = "Ngày sinh")]
        [DataType(DataType.Date)]
        public DateTime Dob { get => dob; set => dob = value; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Tài khoản")]
        public string UserName { get; set; }
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }
        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { get; set; }
        [Display(Name = "Xác nhận mật khẩu")]
        public string ConfirmPassword { get; set; }
    }
}
