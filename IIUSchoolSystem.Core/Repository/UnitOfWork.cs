using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IIUSchoolSystem.Core.Entities;

namespace IIUSchoolSystem.Core.Repository
{
    public class UnitOfWork : IDisposable
    {
        private readonly IIUSchoolSystemDbEntities _context = new IIUSchoolSystemDbEntities();

        public IIUSchoolSystemDbEntities Context
        {
            get { return _context; }
        }

        private GenericRepository<ErrorLog> _errorRepository;
        private GenericRepository<MailLog> _mailLogRepository;
        private GenericRepository<Role> _roleRepository;
        private GenericRepository<PermissionRecord> _permissionRecordRepository;
        private GenericRepository<Setting> _settingRepository;
        private GenericRepository<State> _stateRepository;
        private GenericRepository<User> _userRepository;
        private bool _disposed;

        public GenericRepository<ErrorLog> ErrorRepository
        {
            get
            {
                if (this._errorRepository == null) { this._errorRepository = new GenericRepository<ErrorLog>(_context); }
                return _errorRepository;
            }
        }
        public GenericRepository<MailLog> MailLogRepository
        {
            get
            {
                if (this._mailLogRepository == null) { this._mailLogRepository = new GenericRepository<MailLog>(_context); }
                return _mailLogRepository;
            }
        }
        public GenericRepository<Role> RoleRepository
        {
            get
            {
                if (this._roleRepository == null) { this._roleRepository = new GenericRepository<Role>(_context); }
                return _roleRepository;
            }
        }
        public GenericRepository<PermissionRecord> PermissionRecordRepository
        {
            get
            {
                if (this._permissionRecordRepository == null) { this._permissionRecordRepository = new GenericRepository<PermissionRecord>(_context); }
                return _permissionRecordRepository;
            }
        }
        public GenericRepository<Setting> SettingRepository
        {
            get
            {
                if (this._settingRepository == null) { this._settingRepository = new GenericRepository<Setting>(_context); }
                return _settingRepository;
            }
        }
        public GenericRepository<State> StateRepository
        {
            get
            {
                return this._stateRepository ??
                       (this._stateRepository = new GenericRepository<State>(_context));
            }
        }
        public GenericRepository<User> UserRepository
        {
            get
            {
                return this._userRepository ??
                       (this._userRepository = new GenericRepository<User>(_context));
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
