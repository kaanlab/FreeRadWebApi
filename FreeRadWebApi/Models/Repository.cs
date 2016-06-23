using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeRadWebApi.Models
{
    public class Repository : IRepository
    {
        private bool disposed;
        private readonly MySqlDbContext _context;

        public Repository(MySqlDbContext context)
        {
            _context = context;

            //_context.Database.Log = s => System.Diagnostics.Debug.WriteLine(s);
        }


        #region User

        public IEnumerable<User> GetAllUsers() => _context.Users.ToList();

        public void AddUser(User newUser)
        {
            _context.Users.Add(newUser);
        }

        public User FindUser(int? userId) => _context.Users.Find(userId);

        public Task<User> FindUserAsync(int? userId) => _context.Users.FindAsync(userId);

        public void EditUser(User user)
        {
            var mySqlParams = new MySqlParameter[]
               {
                    new MySqlParameter("@id", user.Id),
                    new MySqlParameter("@username", user.UserName),
                    new MySqlParameter("@attribute", user.Attribute),
                    new MySqlParameter("@op", user.Op),
                    new MySqlParameter("@value", user.Value),
                    new MySqlParameter("@description", user.Description)
               };

            _context
                .Database
                .ExecuteSqlCommand("UPDATE radius.radcheck SET username = @username, attribute = @attribute, op = @op, value = @value, description = @description WHERE id = @id", mySqlParams);
        }

        public void DeleteUser(User user)
        {
            var mySqlParams = new MySqlParameter("@id", user.Id);

            _context
                .Database
                .ExecuteSqlCommand("DELETE FROM radius.radcheck WHERE id = @id", mySqlParams);
        }

        #endregion


        #region UserAttr

        public IEnumerable<UserAttribute> GetAllUserAttributes() => _context.UserAttributes.ToList();

        public void AddUserAttr(UserAttribute newUserAttr)
        {
            _context.UserAttributes.Add(newUserAttr);
        }

        public UserAttribute FindUserAttr(int? userAttrId) => _context.UserAttributes.Find(userAttrId);

        public Task<UserAttribute> FindUserAttrAsync(int? userAttrId) => _context.UserAttributes.FindAsync(userAttrId);

        public void EditUserAttr(UserAttribute userAttr)
        {
            var mySqlParams = new MySqlParameter[]
                {
                    new MySqlParameter("@id", userAttr.Id),
                    new MySqlParameter("@username", userAttr.UserName),
                    new MySqlParameter("@attribute", userAttr.Attribute),
                    new MySqlParameter("@op", userAttr.Op),
                    new MySqlParameter("@value", userAttr.Value)
                };

            _context
                .Database
                .ExecuteSqlCommand("UPDATE radius.radreply SET username = @username, attribute = @attribute, op = @op, value = @value WHERE id = @id", mySqlParams);
        }

        public void DeleteUserAttr(UserAttribute userAttr)
        {
            var mySqlParams = new MySqlParameter("@id", userAttr.Id);

            _context
               .Database
               .ExecuteSqlCommand("DELETE FROM radius.radreply WHERE id = @id", mySqlParams);
        }

        #endregion


        #region Group

        public IEnumerable<Group> GetAllGroups() => _context.Groups.ToList();

        public void AddGroup(Group newGroup)
        {
            _context.Groups.Add(newGroup);
        }

        public Group FindGroup(int? groupId) => _context.Groups.Find(groupId);

        public Task<Group> FindGroupAsync(int? groupId) => _context.Groups.FindAsync(groupId);

        public void EditGroup(Group group)
        {
            var mySqlParams = new MySqlParameter[]
                    {
                        new MySqlParameter("@id", group.Id),
                        new MySqlParameter("@groupname", group.GroupName),
                        new MySqlParameter("@attribute", group.Attribute),
                        new MySqlParameter("@op", group.Op),
                        new MySqlParameter("@value", group.Value),
                        new MySqlParameter("@description", group.Description)
                    };

            _context
                .Database
                .ExecuteSqlCommand("UPDATE radius.radgroupcheck SET groupname = @groupname, attribute = @attribute, op = @op, value = @value, description = @description WHERE id = @id", mySqlParams);
        }

        public void DeleteGroup(Group group)
        {
            var mySqlParams = new MySqlParameter("@id", group.Id);

            _context
               .Database
               .ExecuteSqlCommand("DELETE FROM radius.radgroupcheck WHERE id = @id", mySqlParams);
        }

        #endregion


        #region GroupAttr

        public IEnumerable<GroupAttribute> GetAllGroupAttributes() => _context.GroupAttributes.ToList();

        public void AddGroupAttr(GroupAttribute newGroupAttr)
        {
            _context.GroupAttributes.Add(newGroupAttr);
        }

        public GroupAttribute FindGroupAttr(int? groupAttrId) => _context.GroupAttributes.Find(groupAttrId);

        public Task<GroupAttribute> FindGroupAttrAsync(int? groupAttrId) => _context.GroupAttributes.FindAsync(groupAttrId);

        public void EditGroupAttr(GroupAttribute groupAttr)
        {
            var mySqlParams = new MySqlParameter[]
                {
                    new MySqlParameter("@id", groupAttr.Id),
                    new MySqlParameter("@groupname", groupAttr.GroupName),
                    new MySqlParameter("@attribute", groupAttr.Attribute),
                    new MySqlParameter("@op", groupAttr.Op),
                    new MySqlParameter("@value", groupAttr.Value)
                };

            _context
                .Database
                .ExecuteSqlCommand("UPDATE radius.radgroupreply SET groupname = @groupname, attribute = @attribute, op = @op, value = @value WHERE id = @id", mySqlParams);
        }

        public void DeleteGroupAttr(GroupAttribute groupAttr)
        {
            var mySqlParams = new MySqlParameter("@id", groupAttr.Id);

            _context
                .Database
                .ExecuteSqlCommand("DELETE FROM radius.radgroupreply WHERE id = @id", mySqlParams);
        }

        #endregion


        #region UserInGroup



        public IEnumerable<UserInGroup> GetAllUsersInGroup() => _context.UserInGroups.ToList();

        public void AddUserToGroup(UserInGroup newUserInGroup)
        {
            _context.UserInGroups.Add(newUserInGroup);
        }

        public UserInGroup FindUserInGroup(int? userInGroupId) => _context.UserInGroups.Find(userInGroupId);

        public Task<UserInGroup> FindUserInGroupAsync(int? userInGroupId) => _context.UserInGroups.FindAsync(userInGroupId);

        public void EditUserInGroup(UserInGroup userInGroup)
        {
            var mySqlParams = new MySqlParameter[]
                {
                    new MySqlParameter("@id", userInGroup.Id),
                    new MySqlParameter("@username", userInGroup.UserName),
                    new MySqlParameter("@groupname", userInGroup.GroupName),
                    new MySqlParameter("@priority", userInGroup.Priority)
                };

            _context
                .Database
                .ExecuteSqlCommand("UPDATE radius.radusergroup SET username = @username, groupname = @groupname, priority = @priority WHERE id = @id", mySqlParams);
        }

        public void DeleteUserFromGroup(UserInGroup userInGroup)
        {
            var mySqlParams = new MySqlParameter("@id", userInGroup.Id);

            _context
                .Database
                .ExecuteSqlCommand("DELETE FROM radius.radusergroup WHERE id = @id", mySqlParams);
        }

        #endregion


        #region Nas

        public IEnumerable<Nas> GetAllNas() => _context.Nases.ToList();

        public void AddNas(Nas newNas)
        {
            _context.Nases.Add(newNas);
        }

        public Nas FindNas(int? nasId) => _context.Nases.Find(nasId);

        public Task<Nas> FindNasAsync(int? nasId) => _context.Nases.FindAsync(nasId);

        public void EditNas(Nas nas)
        {
            var mySqlParams = new MySqlParameter[]
                {
                    new MySqlParameter("@id", nas.Id),
                    new MySqlParameter("@nasname", nas.NasName),
                    new MySqlParameter("@shortname", nas.ShortName),
                    new MySqlParameter("@type", nas.Type),
                    new MySqlParameter("@ports", nas.Ports),
                    new MySqlParameter("@secret", nas.Secret),
                    new MySqlParameter("@server", nas.Server),
                    new MySqlParameter("@community", nas.Community),
                    new MySqlParameter("@description", nas.Description)
                };

            _context
                .Database
                .ExecuteSqlCommand("UPDATE radius.nas SET nasname = @nasname, shortname = @shortname, type = @type, ports = @ports, secret = @secret, server = @server, community = @community, description = @description WHERE id = @id", mySqlParams);
        }

        public void DeleteNas(Nas nas)
        {
            var mySqlParams = new MySqlParameter("@id", nas.Id);

            _context
                .Database
                .ExecuteSqlCommand("DELETE FROM radius.nas WHERE id = @id", mySqlParams);
        }

        #endregion


        #region AccessLog

        public IEnumerable<AccessLog> GetAllLogsOderByIdDes() => _context.AccessLogs.OrderByDescending(o => o.RadAcctId);

        public AccessLog FindLog(int? logId) => _context.AccessLogs.Find(logId);

        public void DeleteLog(int logId)
        {
            _context
                .Database
                .ExecuteSqlCommand("DELETE FROM radius.radacct WHERE radacctid = @id", new MySqlParameter("@id", logId));
        }

        #endregion


        #region AuthLog

        public IEnumerable<AuthLog> GetAllAuthLogsOderByIdDes() => _context.AuthLogs.OrderByDescending(o => o.Id);

        public AuthLog FindAuthLog(int? logId) => _context.AuthLogs.Find(logId);

        public void DeleteAuthLog(int logId)
        {
            _context
                .Database
                .ExecuteSqlCommand("DELETE FROM radius.radpostauth WHERE id = @id", new MySqlParameter("@id", logId));
        }

        #endregion

        public async Task SaveAsync()
        {
           await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);

        }

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            disposed = true;
        }
    }
}