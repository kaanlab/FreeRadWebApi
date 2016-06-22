using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FreeRadWebApi.Models
{
    public interface IRepository : IDisposable
    {
        IEnumerable<User> GetAllUsers();
        void AddUser(User newUser);
        User FindUser(int? userId);
        Task<User> FindUserAsync(int? userId);
        void EditUser(User user);
        void DeleteUser(User user);

        IEnumerable<UserAttribute> GetAllUserAttributes();
        void AddUserAttr(UserAttribute newUserAttr);
        UserAttribute FindUserAttr(int? userAttrId);
        Task<UserAttribute> FindUserAttrAsync(int? userAttrId);
        void EditUserAttr(UserAttribute userAttr);
        void DeleteUserAttr(UserAttribute userAttr);

        IEnumerable<Group> GetAllGroups();
        void AddGroup(Group newGroup);
        Group FindGroup(int? groupId);
        Task<Group> FindGroupAsync(int? groupId);
        void EditGroup(Group group);
        void DeleteGroup(Group group);

        IEnumerable<GroupAttribute> GetAllGroupAttributes();
        void AddGroupAttr(GroupAttribute newGroupAttr);
        GroupAttribute FindGroupAttr(int? groupAttrId);
        Task<GroupAttribute> FindGroupAttrAsync(int? groupAttrId);
        void EditGroupAttr(GroupAttribute groupAttr);
        void DeleteGroupAttr(GroupAttribute groupAttr);

        IEnumerable<UserInGroup> GetAllUsersInGroup();
        void AddUserToGroup(UserInGroup newUser);
        UserInGroup FindUserInGroup(int? userId);
        Task<UserInGroup> FindUserInGroupAsync(int? userId);
        void EditUserInGroup(UserInGroup user);
        void DeleteUserFromGroup(UserInGroup user);

        IEnumerable<Nas> GetAllNas();
        void AddNas(Nas newNas);
        Nas FindNas(int? nasId);
        Task<Nas> FindNasAsync(int? nasId);
        void EditNas(Nas nas);
        void DeleteNas(Nas nas);

        IEnumerable<AccessLog> GetAllLogsOderByIdDes();
        AccessLog FindLog(int? logId);
        void DeleteLog(int logId);

        IEnumerable<AuthLog> GetAllAuthLogsOderByIdDes();
        AuthLog FindAuthLog(int? logId);
        void DeleteAuthLog(int logId);

        Task SaveAsync();
    }
}
