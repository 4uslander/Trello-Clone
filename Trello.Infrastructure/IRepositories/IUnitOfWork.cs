using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trello.Domain.Models;

namespace Trello.Infrastructure.IRepositories
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Board> BoardRepository { get; }
        IGenericRepository<BoardMember> BoardMemberRepository { get; }
        IGenericRepository<Card> CardRepository { get; }
        IGenericRepository<CardActivity> CardActivityRepository { get; }
        IGenericRepository<CardLabel> CardLabelRepository { get; }
        IGenericRepository<CardMember> CardMemberRepository { get; }
        IGenericRepository<Comment> CommentRepository { get; }
        IGenericRepository<Label> LabelRepository { get; }
        IGenericRepository<List> ListRepository { get; }
        IGenericRepository<Role> RoleRepository { get; }
        IGenericRepository<Domain.Models.Task> TaskRepository { get; }
        IGenericRepository<ToDo> ToDoRepository { get; }
        IGenericRepository<User> UserRepository { get; }
        IGenericRepository<UserFcmToken> UserFcmTokenRepository { get; }
        IGenericRepository<Notification> NotificationRepository { get; }

        IDbTransaction BeginTransaction();
        System.Threading.Tasks.Task SaveChangesAsync();

    }
}
