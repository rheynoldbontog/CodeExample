using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using SSG.Core;
using SSG.Core.Caching;
using SSG.Core.Data;
using SSG.Core.Domain.Common;
using SSG.Core.Domain.Users;
using SSG.Services.Common;
using SSG.Services.Events;

namespace SSG.Services.Users
{
    /// <summary>
    /// User service
    /// </summary>
    public partial class UserService : IUserService
    {
        #region Constants

        private const string CUSTOMERROLES_ALL_KEY = "SSG.userrole.all-{0}";
        private const string CUSTOMERROLES_BY_ID_KEY = "SSG.userrole.id-{0}";
        private const string CUSTOMERROLES_BY_SYSTEMNAME_KEY = "SSG.userrole.systemname-{0}";
        private const string CUSTOMERROLES_PATTERN_KEY = "SSG.userrole.";
        #endregion

        #region Fields

        private readonly IRepository<User> _userRepository;
        private readonly IRepository<UserRole> _userRoleRepository;
        private readonly IRepository<GenericAttribute> _gaRepository;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ICacheManager _cacheManager;
        private readonly IEventPublisher _eventPublisher;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="userRepository">User repository</param>
        /// <param name="userRoleRepository">User role repository</param>
        /// <param name="gaRepository">Generic attribute repository</param>
        /// <param name="genericAttributeService">Generic attribute service</param>
        /// <param name="eventPublisher">Event published</param>
        public UserService(ICacheManager cacheManager,
            IRepository<User> userRepository,
            IRepository<UserRole> userRoleRepository,
            IRepository<GenericAttribute> gaRepository,
            IGenericAttributeService genericAttributeService,
            IEventPublisher eventPublisher)
        {
            this._cacheManager = cacheManager;
            this._userRepository = userRepository;
            this._userRoleRepository = userRoleRepository;
            this._gaRepository = gaRepository;
            this._genericAttributeService = genericAttributeService;
            this._eventPublisher = eventPublisher;
        }

        #endregion

        #region Methods

        #region Users

        /// <summary>
        /// Gets all users
        /// </summary>
        /// <param name="registrationFrom">User registration from; null to load all users</param>
        /// <param name="registrationTo">User registration to; null to load all users</param>
        /// <param name="userRoleIds">A list of user role identifiers to filter by (at least one match); pass null or empty list in order to load all users; </param>
        /// <param name="email">Email; null to load all users</param>
        /// <param name="username">Username; null to load all users</param>
        /// <param name="firstName">First name; null to load all users</param>
        /// <param name="lastName">Last name; null to load all users</param>
        /// <param name="dayOfBirth">Day of birth; 0 to load all users</param>
        /// <param name="monthOfBirth">Month of birth; 0 to load all users</param>
        /// <param name="company">Company; null to load all users</param>
        /// <param name="phone">Phone; null to load all users</param>
        /// <param name="zipPostalCode">Phone; null to load all users</param>
        /// <param name="loadOnlyWithShoppingCart">Value indicating whther to load users only with shopping cart</param>
        /// <param name="sct">Value indicating what shopping cart type to filter; userd when 'loadOnlyWithShoppingCart' param is 'true'</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>User collection</returns>
        public virtual IPagedList<User> GetAllUsers(DateTime? registrationFrom,
            DateTime? registrationTo, int[] userRoleIds, string email, string username,
            string firstName, string lastName, int dayOfBirth, int monthOfBirth,
            string company, string phone, string zipPostalCode,
            int pageIndex, int pageSize)
        {
            var query = _userRepository.Table;
            if (registrationFrom.HasValue)
                query = query.Where(c => registrationFrom.Value <= c.CreatedOnUtc);
            if (registrationTo.HasValue)
                query = query.Where(c => registrationTo.Value >= c.CreatedOnUtc);
            query = query.Where(c => !c.Deleted);
            // Interset is not supported in MySql: ignore for now
            //if (userRoleIds != null && userRoleIds.Length > 0)
            //    query = query.Where(c => c.UserRoles.Select(cr => cr.Id).Intersect(userRoleIds).Count() > 0);
            if (userRoleIds != null && userRoleIds.Length > 0)
                query = query.Where(u => u.UserRoles.Any(ur => userRoleIds.Contains(ur.Id)));
            if (!String.IsNullOrWhiteSpace(email))
                query = query.Where(c => c.Email.Contains(email));
            if (!String.IsNullOrWhiteSpace(username))
                query = query.Where(c => c.Username.Contains(username));
            if (!String.IsNullOrWhiteSpace(firstName))
            {
                query = query
                    .Join(_gaRepository.Table, x => x.Id, y => y.EntityId, (x, y) => new { User = x, Attribute = y })
                    .Where((z => z.Attribute.KeyGroup == "User" &&
                        z.Attribute.Key == SystemUserAttributeNames.FirstName &&
                        z.Attribute.Value.Contains(firstName)))
                    .Select(z => z.User);
            }
            if (!String.IsNullOrWhiteSpace(lastName))
            {
                query = query
                    .Join(_gaRepository.Table, x => x.Id, y => y.EntityId, (x, y) => new { User = x, Attribute = y })
                    .Where((z => z.Attribute.KeyGroup == "User" &&
                        z.Attribute.Key == SystemUserAttributeNames.LastName &&
                        z.Attribute.Value.Contains(lastName)))
                    .Select(z => z.User);
            }
            //date of birth is stored as a string into database.
            //we also know that date of birth is stored in the following format YYYY-MM-DD (for example, 1983-02-18).
            //so let's search it as a string
            if (dayOfBirth > 0 && monthOfBirth > 0)
            {
                //both are specified
                string dateOfBirthStr = monthOfBirth.ToString("00", CultureInfo.InvariantCulture) + "-" + dayOfBirth.ToString("00", CultureInfo.InvariantCulture);
                //EndsWith is not supported by SQL Server Compact
                //so let's use the following workaround http://social.msdn.microsoft.com/Forums/is/sqlce/thread/0f810be1-2132-4c59-b9ae-8f7013c0cc00
                
                //we also cannot use Length function in SQL Server Compact (not supported in this context)
                //z.Attribute.Value.Length - dateOfBirthStr.Length = 5
                //dateOfBirthStr.Length = 5
                query = query
                    .Join(_gaRepository.Table, x => x.Id, y => y.EntityId, (x, y) => new { User = x, Attribute = y })
                    .Where((z => z.Attribute.KeyGroup == "User" &&
                        z.Attribute.Key == SystemUserAttributeNames.DateOfBirth &&
                        z.Attribute.Value.Substring(5, 5) == dateOfBirthStr))
                    .Select(z => z.User);
            }
            else if (dayOfBirth > 0)
            {
                //only day is specified
                string dateOfBirthStr = dayOfBirth.ToString("00", CultureInfo.InvariantCulture);
                //EndsWith is not supported by SQL Server Compact
                //so let's use the following workaround http://social.msdn.microsoft.com/Forums/is/sqlce/thread/0f810be1-2132-4c59-b9ae-8f7013c0cc00
                
                //we also cannot use Length function in SQL Server Compact (not supported in this context)
                //z.Attribute.Value.Length - dateOfBirthStr.Length = 8
                //dateOfBirthStr.Length = 2
                query = query
                    .Join(_gaRepository.Table, x => x.Id, y => y.EntityId, (x, y) => new { User = x, Attribute = y })
                    .Where((z => z.Attribute.KeyGroup == "User" &&
                        z.Attribute.Key == SystemUserAttributeNames.DateOfBirth &&
                        z.Attribute.Value.Substring(8, 2) == dateOfBirthStr))
                    .Select(z => z.User);
            }
            else if (monthOfBirth > 0)
            {
                //only month is specified
                string dateOfBirthStr = "-" + monthOfBirth.ToString("00", CultureInfo.InvariantCulture) + "-";
                query = query
                    .Join(_gaRepository.Table, x => x.Id, y => y.EntityId, (x, y) => new { User = x, Attribute = y })
                    .Where((z => z.Attribute.KeyGroup == "User" &&
                        z.Attribute.Key == SystemUserAttributeNames.DateOfBirth &&
                        z.Attribute.Value.Contains(dateOfBirthStr)))
                    .Select(z => z.User);
            }
            //search by company
            if (!String.IsNullOrWhiteSpace(company))
            {
                query = query
                    .Join(_gaRepository.Table, x => x.Id, y => y.EntityId, (x, y) => new { User = x, Attribute = y })
                    .Where((z => z.Attribute.KeyGroup == "User" &&
                        z.Attribute.Key == SystemUserAttributeNames.Company &&
                        z.Attribute.Value.Contains(company)))
                    .Select(z => z.User);
            }
            //search by phone
            if (!String.IsNullOrWhiteSpace(phone))
            {
                query = query
                    .Join(_gaRepository.Table, x => x.Id, y => y.EntityId, (x, y) => new { User = x, Attribute = y })
                    .Where((z => z.Attribute.KeyGroup == "User" &&
                        z.Attribute.Key == SystemUserAttributeNames.Phone &&
                        z.Attribute.Value.Contains(phone)))
                    .Select(z => z.User);
            }
            //search by zip
            if (!String.IsNullOrWhiteSpace(zipPostalCode))
            {
                query = query
                    .Join(_gaRepository.Table, x => x.Id, y => y.EntityId, (x, y) => new { User = x, Attribute = y })
                    .Where((z => z.Attribute.KeyGroup == "User" &&
                        z.Attribute.Key == SystemUserAttributeNames.ZipPostalCode &&
                        z.Attribute.Value.Contains(zipPostalCode)))
                    .Select(z => z.User);
            }
            
            query = query.OrderByDescending(c => c.CreatedOnUtc);

            var users = new PagedList<User>(query, pageIndex, pageSize);
            return users;
        }

        /// <summary>
        /// Gets all users by user format (including deleted ones)
        /// </summary>
        /// <param name="passwordFormat">Password format</param>
        /// <returns>Users</returns>
        public virtual IList<User> GetAllUsersByPasswordFormat(PasswordFormat passwordFormat)
        {
            int passwordFormatId = (int)passwordFormat;

            var query = _userRepository.Table;
            query = query.Where(c => c.PasswordFormatId == passwordFormatId);
            query = query.OrderByDescending(c => c.CreatedOnUtc);
            var users = query.ToList();
            return users;
        }

        /// <summary>
        /// Gets online users
        /// </summary>
        /// <param name="lastActivityFromUtc">User last activity date (from)</param>
        /// <param name="userRoleIds">A list of user role identifiers to filter by (at least one match); pass null or empty list in order to load all users; </param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>User collection</returns>
        public virtual IPagedList<User> GetOnlineUsers(DateTime lastActivityFromUtc,
            int[] userRoleIds, int pageIndex, int pageSize)
        {
            var query = _userRepository.Table;
            query = query.Where(c => lastActivityFromUtc <= c.LastActivityDateUtc);
            query = query.Where(c => !c.Deleted);
            // MySql doesnt support Intersect
            //if (userRoleIds != null && userRoleIds.Length > 0)
            //    query = query.Where(c => c.UserRoles.Select(cr => cr.Id).Intersect(userRoleIds).Count() > 0);
            if (userRoleIds != null && userRoleIds.Length > 0)
                query = query.Where(u => u.UserRoles.Any(ur => userRoleIds.Contains(ur.Id)));
            
            query = query.OrderByDescending(c => c.LastActivityDateUtc);
            var users = new PagedList<User>(query, pageIndex, pageSize);
            return users;
        }

        /// <summary>
        /// Gets all users by user role id
        /// </summary>
        /// <param name="userRoleId">User role identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>User collection</returns>
        public virtual IList<User> GetUsersByUserRoleId(int userRoleId, bool showHidden = false)
        {
            var query = from c in _userRepository.Table
                        from cr in c.UserRoles
                        where (showHidden || c.Active) &&
                            !c.Deleted &&
                            cr.Id == userRoleId
                        orderby c.CreatedOnUtc descending
                        select c;
            
            var users = query.ToList();
            return users;
        }

        /// <summary>
        /// Delete a user
        /// </summary>
        /// <param name="user">User</param>
        public virtual void DeleteUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (user.IsSystemAccount)
                throw new SSGException(string.Format("System user account ({0}) could not be deleted", user.SystemName));

            user.Deleted = true;
            UpdateUser(user);
        }

        /// <summary>
        /// Gets a user
        /// </summary>
        /// <param name="userId">User identifier</param>
        /// <returns>A user</returns>
        public virtual User GetUserById(int userId)
        {
            if (userId == 0)
                return null;
            
            var user = _userRepository.GetById(userId);
            return user;
        }

        /// <summary>
        /// Get users by identifiers
        /// </summary>
        /// <param name="userIds">User identifiers</param>
        /// <returns>Users</returns>
        public virtual IList<User> GetUsersByIds(int[] userIds)
        {
            if (userIds == null || userIds.Length == 0)
                return new List<User>();

            var query = from c in _userRepository.Table
                        where userIds.Contains(c.Id)
                        select c;
            var users = query.ToList();
            //sort by passed identifiers
            var sortedUsers = new List<User>();
            foreach (int id in userIds)
            {
                var user = users.Find(x => x.Id == id);
                if (user != null)
                    sortedUsers.Add(user);
            }
            return sortedUsers;
        }

        /// <summary>
        /// Gets a user by GUID
        /// </summary>
        /// <param name="userGuid">User GUID</param>
        /// <returns>A user</returns>
        public virtual User GetUserByGuid(Guid userGuid)
        {
            if (userGuid == Guid.Empty)
                return null;

            var query = from c in _userRepository.Table
                        where c.UserGuid == userGuid
                        orderby c.Id
                        select c;
            var user = query.FirstOrDefault();
            return user;
        }

        /// <summary>
        /// Get user by email
        /// </summary>
        /// <param name="email">Email</param>
        /// <returns>User</returns>
        public virtual User GetUserByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;

            var query = from c in _userRepository.Table
                        orderby c.Id
                        where c.Email == email
                        select c;
            var user = query.FirstOrDefault();
            return user;
        }
        
        /// <summary>
        /// Get user by email and employeeID
        /// </summary>
        /// <param name="email"></param>
        /// <param name="employeeId"></param>
        /// <returns></returns>
        public User GetUserByEmailAndEmployeeId(string email, string employeeId)
        {
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(employeeId))
                return null;

            var query = from c in _userRepository.Table
                        orderby c.Id
                        where c.Email == email && c.EmployeeId == employeeId
                        select c;
            var user = query.FirstOrDefault();
            return user;
        }

        /// <summary>
        /// Get user by system name
        /// </summary>
        /// <param name="systemName">System name</param>
        /// <returns>User</returns>
        public virtual User GetUserBySystemName(string systemName)
        {
            if (string.IsNullOrWhiteSpace(systemName))
                return null;

            var query = from c in _userRepository.Table
                        orderby c.Id
                        where c.SystemName == systemName
                        select c;
            var user = query.FirstOrDefault();
            return user;
        }

        /// <summary>
        /// Get user by username
        /// </summary>
        /// <param name="username">Username</param>
        /// <returns>User</returns>
        public virtual User GetUserByUsername(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return null;

            var query = from c in _userRepository.Table
                        orderby c.Id
                        where c.Username == username
                        select c;
            var user = query.FirstOrDefault();
            return user;
        }

        /// <summary>
        /// Get users by language identifier
        /// </summary>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Users</returns>
        public virtual IList<User> GetUsersByLanguageId(int languageId)
        {
            var query = _userRepository.Table;
            if (languageId > 0)
                query = query.Where(c => c.LanguageId.HasValue && c.LanguageId.Value == languageId);
            else
                query = query.Where(c => !c.LanguageId.HasValue);
            query = query.OrderBy(c => c.Id);
            var users = query.ToList();
            return users;
        }

        /// <summary>
        /// Get users by currency identifier
        /// </summary>
        /// <param name="currencyId">Currency identifier</param>
        /// <returns>Users</returns>
        public virtual IList<User> GetUsersByCurrencyId(int currencyId)
        {
            var query = _userRepository.Table;
            if (currencyId > 0)
                query = query.Where(c => c.CurrencyId.HasValue && c.CurrencyId.Value == currencyId);
            else
                query = query.Where(c => !c.CurrencyId.HasValue);
            query = query.OrderBy(c => c.Id);
            var users = query.ToList();
            return users;
        }

        /// <summary>
        /// Insert a guest user
        /// </summary>
        /// <returns>User</returns>
        public virtual User InsertGuestUser()
        {
            var user = new User()
            {
                UserGuid = Guid.NewGuid(),
                Active = true,
                CreatedOnUtc = DateTime.UtcNow,
                LastActivityDateUtc = DateTime.UtcNow,
            };

            //add to 'Guests' role
            var guestRole = GetUserRoleBySystemName(SystemUserRoleNames.Guests);
            if (guestRole == null)
                throw new SSGException("'Guests' role could not be loaded");
            user.UserRoles.Add(guestRole);

            _userRepository.Insert(user);

            return user;
        }
        
        /// <summary>
        /// Insert a user
        /// </summary>
        /// <param name="user">User</param>
        public virtual void InsertUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            _userRepository.Insert(user);

            //event notification
            _eventPublisher.EntityInserted(user);
        }
        
        /// <summary>
        /// Updates the user
        /// </summary>
        /// <param name="user">User</param>
        public virtual void UpdateUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            _userRepository.Update(user);

            //event notification
            _eventPublisher.EntityUpdated(user);
        }

       
        /// <summary>
        /// Delete guest user records
        /// </summary>
        /// <param name="registrationFrom">User registration from; null to load all users</param>
        /// <param name="registrationTo">User registration to; null to load all users</param>
        /// <param name="onlyWithoutShoppingCart">A value indicating whether to delete users only without shopping cart</param>
        /// <returns>Number of deleted users</returns>
        public virtual int DeleteGuestUsers(DateTime? registrationFrom,
            DateTime? registrationTo)
        {
            var guestRole = GetUserRoleBySystemName(SystemUserRoleNames.Guests);
            if (guestRole == null)
                throw new SSGException("'Guests' role could not be loaded");

            var query = _userRepository.Table;
            if (registrationFrom.HasValue)
                query = query.Where(c => registrationFrom.Value <= c.CreatedOnUtc);
            if (registrationTo.HasValue)
                query = query.Where(c => registrationTo.Value >= c.CreatedOnUtc);
            query = query.Where(c => c.UserRoles.Select(cr => cr.Id).Contains(guestRole.Id));

            //no user content
            query = query.Where(c => c.UserContent.Count() == 0);
            //ensure that users doesn't have forum posts or topics
            query = query.Where(c => c.ForumTopics.Count() == 0);
            query = query.Where(c => c.ForumPosts.Count() == 0);
            //don't delete system accounts
            query = query.Where(c => !c.IsSystemAccount);
            var users = query.ToList();


            int numberOfDeletedUsers = 0;
            foreach (var c in users)
            {
                try
                {
                    //delete from database
                    _userRepository.Delete(c);
                    numberOfDeletedUsers++;
                }
                catch (Exception exc)
                {
                    Debug.WriteLine(exc);
                }
            }
            return numberOfDeletedUsers;
        }

        #endregion
        
        #region User roles

        /// <summary>
        /// Delete a user role
        /// </summary>
        /// <param name="userRole">User role</param>
        public virtual void DeleteUserRole(UserRole userRole)
        {
            if (userRole == null)
                throw new ArgumentNullException("userRole");

            if (userRole.IsSystemRole)
                throw new SSGException("System role could not be deleted");

            _userRoleRepository.Delete(userRole);

            _cacheManager.RemoveByPattern(CUSTOMERROLES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityDeleted(userRole);
        }

        /// <summary>
        /// Gets a user role
        /// </summary>
        /// <param name="userRoleId">User role identifier</param>
        /// <returns>User role</returns>
        public virtual UserRole GetUserRoleById(int userRoleId)
        {
            if (userRoleId == 0)
                return null;

            string key = string.Format(CUSTOMERROLES_BY_ID_KEY, userRoleId);
            return _cacheManager.Get(key, () =>
            {
                var userRole = _userRoleRepository.GetById(userRoleId);
                return userRole;
            });
        }

        /// <summary>
        /// Gets a user role
        /// </summary>
        /// <param name="systemName">User role system name</param>
        /// <returns>User role</returns>
        public virtual UserRole GetUserRoleBySystemName(string systemName)
        {
            if (String.IsNullOrWhiteSpace(systemName))
                return null;

            string key = string.Format(CUSTOMERROLES_BY_SYSTEMNAME_KEY, systemName);
            return _cacheManager.Get(key, () =>
            {
                var query = from cr in _userRoleRepository.Table
                            orderby cr.Id
                            where cr.SystemName == systemName
                            select cr;
                var userRole = query.FirstOrDefault();
                return userRole;
            });
        }

        /// <summary>
        /// Gets all user roles
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>User role collection</returns>
        public virtual IList<UserRole> GetAllUserRoles(bool showHidden = false)
        {
            string key = string.Format(CUSTOMERROLES_ALL_KEY, showHidden);
            return _cacheManager.Get(key, () =>
            {
                var query = from cr in _userRoleRepository.Table
                            orderby cr.Name
                            where (showHidden || cr.Active)
                            select cr;
                var userRoles = query.ToList();
                return userRoles;
            });
        }
        
        /// <summary>
        /// Inserts a user role
        /// </summary>
        /// <param name="userRole">User role</param>
        public virtual void InsertUserRole(UserRole userRole)
        {
            if (userRole == null)
                throw new ArgumentNullException("userRole");

            _userRoleRepository.Insert(userRole);

            _cacheManager.RemoveByPattern(CUSTOMERROLES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityInserted(userRole);
        }

        /// <summary>
        /// Updates the user role
        /// </summary>
        /// <param name="userRole">User role</param>
        public virtual void UpdateUserRole(UserRole userRole)
        {
            if (userRole == null)
                throw new ArgumentNullException("userRole");

            _userRoleRepository.Update(userRole);

            _cacheManager.RemoveByPattern(CUSTOMERROLES_PATTERN_KEY);

            //event notification
            _eventPublisher.EntityUpdated(userRole);
        }

        #endregion

        #endregion



        public int? DeleteGuestUsers(DateTime? startDateValue, DateTime? endDateValue, bool p)
        {
            throw new NotImplementedException();
        }
    }
}