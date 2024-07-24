using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trello.Domain.Models;
using Trello.Infrastructure.IRepositories;

namespace Trello.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TrellocloneContext _context;

        private IGenericRepository<Board> _boardRepository;
        private IGenericRepository<BoardMember> _boardMemberRepository;
        private IGenericRepository<Card> _cardRepository;
        private IGenericRepository<CardActivity> _cardActivityRepository;
        private IGenericRepository<CardLabel> _cardLabelRepository;
        private IGenericRepository<CardMember> _cardMemberRepository;
        private IGenericRepository<Comment> _commentRepository;
        private IGenericRepository<Label> _labelRepository;
        private IGenericRepository<List> _listRepository;
        private IGenericRepository<Role> _roleRepository;
        private IGenericRepository<Domain.Models.Task> _taskRepository;
        private IGenericRepository<ToDo> _toDoRepository;
        private IGenericRepository<User> _userRepository;
        private IGenericRepository<UserFcmToken> _userFcmTokenRepository;
        private IGenericRepository<Notification> _notificationRepository;
        public UnitOfWork(TrellocloneContext context)
        {
            _context = context;
        }
        public IGenericRepository<Board> BoardRepository => _boardRepository ??= new GenericRepository<Board>(_context);

        public IGenericRepository<BoardMember> BoardMemberRepository => _boardMemberRepository ??= new GenericRepository<BoardMember>(_context);

        public IGenericRepository<Card> CardRepository => _cardRepository ??= new GenericRepository<Card>(_context);

        public IGenericRepository<CardActivity> CardActivityRepository => _cardActivityRepository ??= new GenericRepository<CardActivity>(_context);

        public IGenericRepository<CardLabel> CardLabelRepository => _cardLabelRepository ??= new GenericRepository<CardLabel>(_context);

        public IGenericRepository<Comment> CommentRepository => _commentRepository ??= new GenericRepository<Comment>(_context);

        public IGenericRepository<Label> LabelRepository => _labelRepository ??= new GenericRepository<Label>(_context);
        public IGenericRepository<CardMember> CardMemberRepository => _cardMemberRepository ??= new GenericRepository<CardMember>(_context);

        public IGenericRepository<List> ListRepository => _listRepository ??= new GenericRepository<List>(_context);

        public IGenericRepository<Role> RoleRepository => _roleRepository ??= new GenericRepository<Role>(_context);

        public IGenericRepository<Domain.Models.Task> TaskRepository => _taskRepository ??= new GenericRepository<Domain.Models.Task>(_context);

        public IGenericRepository<ToDo> ToDoRepository => _toDoRepository ??= new GenericRepository<ToDo>(_context);

        public IGenericRepository<User> UserRepository => _userRepository ??= new GenericRepository<User>(_context);

        public IGenericRepository<UserFcmToken> UserFcmTokenRepository => _userFcmTokenRepository ??= new GenericRepository<UserFcmToken>(_context);

        public IGenericRepository<Notification> NotificationRepository => _notificationRepository ??= new GenericRepository<Notification>(_context);

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public IDbTransaction BeginTransaction()
        {
            var _transaction = _context.Database.BeginTransaction();
            return _transaction.GetDbTransaction();
        }

        public async System.Threading.Tasks.Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
