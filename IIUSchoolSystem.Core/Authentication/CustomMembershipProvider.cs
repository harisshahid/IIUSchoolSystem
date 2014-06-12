using System;
using System.Web;
using System.Web.Security;
using IIUSchoolSystem.Core.Entities;
using IIUSchoolSystem.Core.Repository;

namespace IIUSchoolSystem.Core.Authentication
{
    public class CustomMembershipProvider : MembershipProvider
    {
        private readonly UnitOfWork _unitOfWork = new UnitOfWork();
        /// <summary>
        /// 	Indicates whether the membership provider is configured to allow users to reset their passwords.
        /// </summary>
        /// <value> </value>
        /// <returns> true if the membership provider supports password reset; otherwise, false. The default is true. </returns>
        public override bool EnablePasswordReset
        {
            get { return true; }
        }

        /// <summary>
        /// 	Indicates whether the membership provider is configured to allow users to retrieve their passwords.
        /// </summary>
        /// <value> </value>
        /// <returns> true if the membership provider is configured to support password retrieval; otherwise, false. The default is false. </returns>
        public override bool EnablePasswordRetrieval
        {
            get { return true; }
        }

        /// <summary>
        /// 	Gets the number of invalid password or password-answer attempts allowed before the membership user is locked out.
        /// </summary>
        /// <value> </value>
        /// <returns> The number of invalid password or password-answer attempts allowed before the membership user is locked out. </returns>
        public override int MaxInvalidPasswordAttempts
        {
            get { return 5; }
        }

        /// <summary>
        /// 	Gets the minimum number of special characters that must be present in a valid password.
        /// </summary>
        /// <value> </value>
        /// <returns> The minimum number of special characters that must be present in a valid password. </returns>
        public override int MinRequiredNonAlphanumericCharacters
        {
            get { return 0; }
        }

        /// <summary>
        /// 	Gets the minimum length required for a password.
        /// </summary>
        /// <value> </value>
        /// <returns> The minimum length required for a password. </returns>
        public override int MinRequiredPasswordLength
        {
            get { return 3; }
        }

        /// <summary>
        /// 	Gets the number of minutes in which a maximum number of invalid password or password-answer attempts are allowed before the membership user is locked out.
        /// </summary>
        /// <value> </value>
        /// <returns> The number of minutes in which a maximum number of invalid password or password-answer attempts are allowed before the membership user is locked out. </returns>
        public override int PasswordAttemptWindow
        {
            get { return 1; }
        }

        /// <summary>
        /// 	Gets a value indicating whether the membership provider is configured to require the user to answer a password question for password reset and retrieval.
        /// </summary>
        /// <value> </value>
        /// <returns> true if a password answer is required for password reset and retrieval; otherwise, false. The default is true. </returns>
        public override bool RequiresQuestionAndAnswer
        {
            get { return false; }
        }

        /// <summary>
        /// 	The name of the application using the custom membership provider.
        /// </summary>
        /// <value> </value>
        /// <returns> The name of the application using the custom membership provider. </returns>
        public override string ApplicationName
        {
            get { return "IIUSchoolSystem"; }
            set { }
        }

        /// <summary>
        /// 	Gets a value indicating whether the membership provider is configured to require a unique e-mail address for each user name.
        /// </summary>
        /// <value> </value>
        /// <returns> true if the membership provider requires a unique e-mail address; otherwise, false. The default is true. </returns>
        public override bool RequiresUniqueEmail
        {
            get { return true; }
        }

        #region "Unsupported methods"

        public override MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException(); }
        }

        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion,
                                                             string newPasswordAnswer)
        {
            throw new NotSupportedException();
        }


        public override string ResetPassword(string username, string answer)
        {
            throw new NotSupportedException();
        }

        public override bool UnlockUser(string userName)
        {
            throw new NotSupportedException();
        }


        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion,
                                                  string passwordAnswer, bool isApproved, object providerUserKey,
                                                  out MembershipCreateStatus status)
        {
            throw new NotSupportedException();
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize,
                                                                  out int totalRecords)
        {
            throw new NotSupportedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize,
                                                                 out int totalRecords)
        {
            throw new NotSupportedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotSupportedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotSupportedException();
        }

        #endregion

        /// <summary>
        /// 	Verifies that the specified user name and password exist in the data source.
        /// </summary>
        /// <param name="username"> The name of the user to validate. </param>
        /// <param name="password"> The password for the specified user. </param>
        /// <returns> true if the specified username and password are valid; otherwise, false. </returns>
        public override bool ValidateUser(string username, string password)
        {
            User user = _unitOfWork.UserRepository.GetSingle(x => x.Username.Equals(username) && x.Password.Equals(password) && x.Active);

            if (user != null && user.Id > 0)
            {
                MembershipContext.Current.User = user;
                user.LastLoginDate = DateTime.Now;
                user.LastIpAddress = HttpContext.Current.Request.UserHostAddress;

                _unitOfWork.UserRepository.Update(user);
                _unitOfWork.Save();

                return true;
            }
            return false;
        }

        /// <summary>
        /// 	Processes a request to update the password for a membership user.
        /// </summary>
        /// <param name="username"> The user to update the password for. </param>
        /// <param name="oldPassword"> The current password for the specified user. </param>
        /// <param name="newPassword"> The new password for the specified user. </param>
        /// <returns> true if the password was updated successfully; otherwise, false. </returns>
        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            var user = _unitOfWork.UserRepository.GetSingle(x => x.Username.Equals(username) && x.Password.Equals(oldPassword) && x.Active);

            if (user != null && user.Password == oldPassword)
            {
                user.Password = newPassword;
                _unitOfWork.UserRepository.Update(user);
                _unitOfWork.Save();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 	Removes a user from the membership data source.
        /// </summary>
        /// <param name="username"> The name of the user to delete. </param>
        /// <param name="deleteAllRelatedData"> true to delete data related to the user from the database; false to leave data related to the user in the database. </param>
        /// <returns> true if the user was successfully deleted; otherwise, false. </returns>
        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            User user = _unitOfWork.UserRepository.GetSingle(t => t.Username == username);
            if (user != null)
            {
                _unitOfWork.UserRepository.Delete(user);
                _unitOfWork.Save();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 	Gets the password for the specified user name from the data source.
        /// </summary>
        /// <param name="username"> The user to retrieve the password for. </param>
        /// <param name="answer"> The password answer for the user. </param>
        /// <returns> The password for the specified user name. </returns>
        public override string GetPassword(string username, string answer)
        {
            //User user = new User(User.Columns.EmailAddress, username);
            //if (user.UserID > 0)
            //{
            //    return user.Password;
            //}
            //else 
            return null;
        }

        /// <summary>
        /// 	Updates information about a user in the data source.
        /// </summary>
        /// <param name="mUser"> A <see cref="T:System.Web.Security.MembershipUser" /> object that represents the user to update and the updated information for the user. </param>
        public override void UpdateUser(MembershipUser mUser)
        {
            User user = _unitOfWork.UserRepository.GetSingle(t => t.Username == mUser.UserName);
            if (user != null)
                if (user.Id > 0)
                {
                    user.LastUpdatedOn = mUser.CreationDate;
                    _unitOfWork.UserRepository.Update(user);
                    _unitOfWork.Save();
                }
        }

        /// <summary>
        /// 	Gets information from the data source for a user. Provides an option to update the last-activity date/time stamp for the user.
        /// </summary>
        /// <param name="username"> The name of the user to get information for. </param>
        /// <param name="userIsOnline"> true to update the last-activity date/time stamp for the user; false to return user information without updating the last-activity date/time stamp for the user. </param>
        /// <returns> A <see cref="T:System.Web.Security.MembershipUser" /> object populated with the specified user's information from the data source. </returns>
        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            User user = _unitOfWork.UserRepository.GetSingle(t => t.Username == username);

            if (user != null && user.Id > 0)
            {
                return new MembershipUser("", user.Username, user.Username, user.Username, null, null, true, false,
                                          user.CreatedOn, DateTime.UtcNow, DateTime.UtcNow, DateTime.UtcNow,
                                          DateTime.MaxValue);
            }
            return null;
        }

        /// <summary>
        /// 	Gets user information from the data source based on the unique identifier for the membership user. Provides an option to update the last-activity date/time stamp for the user.
        /// </summary>
        /// <param name="providerUserKey"> The unique identifier for the membership user to get information for. </param>
        /// <param name="userIsOnline"> true to update the last-activity date/time stamp for the user; false to return user information without updating the last-activity date/time stamp for the user. </param>
        /// <returns> A <see cref="T:System.Web.Security.MembershipUser" /> object populated with the specified user's information from the data source. </returns>
        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            return GetUser(providerUserKey.ToString(), userIsOnline);
        }

        /// <summary>
        /// 	Gets the user name associated with the specified e-mail address.
        /// </summary>
        /// <param name="email"> The e-mail address to search for. </param>
        /// <returns> The user name associated with the specified e-mail address. If no match is found, return null. </returns>
        public override string GetUserNameByEmail(string email)
        {
            var membershipUser = GetUser(email, false);
            return membershipUser != null ? membershipUser.Email : "";
        }
    }
}
