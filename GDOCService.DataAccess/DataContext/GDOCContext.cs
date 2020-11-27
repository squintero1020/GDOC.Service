using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GDOCService.DataAccess.EntityConfiguration;
using GDOCService.DataAccess.Models;
using GDOCService.DataAccess.SeedWorks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage;

namespace GDOCService.DataAccess.DataContext
{
    public class GDOCContext : DbContext, IUnitOfWork
    {
        private readonly IMediator _mediator;
        private IDbContextTransaction _currentTransaction;

        public IDbContextTransaction GetCurrentTransaction() => _currentTransaction;
        public bool HasActiveTransaction => _currentTransaction != null;

        public GDOCContext() { }

        public GDOCContext(DbContextOptions opt) : base(opt) { }

        public GDOCContext(DbContextOptions<GDOCContext> options, IMediator mediator) : base(options) =>
            (_mediator) = (mediator ?? throw new ArgumentNullException(nameof(mediator)));



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connectionString = "Server=localhost;Port=3306;Database=gdocservice;Uid=root;Pwd='Amazonas1234.';";
                optionsBuilder.UseMySQL(connectionString);
            }
        }

        public DbSet<Stores> Stores { get; set; }
        public DbSet<Guns> Gunes { get; set; }
        public DbSet<Panes> Panes { get; set; }

        protected override void OnModelCreating(ModelBuilder model)
        {
            model.ApplyConfiguration(new StoresEntityTypeConfiguration());
            model.ApplyConfiguration(new GunsEntityTypeConfiguration());

            base.OnModelCreating(model);
        }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await _mediator.DispatchDomainEventsAsync(this);
            var result = await base.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            if (_currentTransaction != null) return null;

            _currentTransaction = await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

            return _currentTransaction;
        }

        public async Task CommitTransactionAsync(IDbContextTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            if (transaction != _currentTransaction) throw new InvalidOperationException($"Transaction {transaction.TransactionId} is not current");

            try
            {
                await SaveChangesAsync();
                transaction.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }
    }

    public class GDOCContextDesignFactory : IDesignTimeDbContextFactory<GDOCContext>
    {
        public GDOCContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<GDOCContext>()
                .UseMySQL("Server=localhost;Port=3306;Database=gdocservice;Uid=root;Pwd='Amazonas1234.';");

            return new GDOCContext(optionsBuilder.Options, new NoMediator());
        }

        class NoMediator : IMediator
        {
            public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default(CancellationToken)) where TNotification : INotification
            {
                return Task.CompletedTask;
            }

            public Task Publish(object notification, CancellationToken cancellationToken = default)
            {
                return Task.CompletedTask;
            }

            public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult<TResponse>(default(TResponse));
            }

        }
    }
}
