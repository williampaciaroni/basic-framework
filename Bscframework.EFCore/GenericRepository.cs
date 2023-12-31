﻿using Bscframework.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Bscframework.EFCore
{
    public abstract class GenericRepository<TEntity, T, TContext> : IGenericRepository<TEntity, T>
        where TEntity : class, IEntity<T>, new()
        where T : IComparable, IEquatable<T>
        where TContext : DbContext
    {
        private TContext _Context;
        protected readonly DbSet<TEntity> DbSet;

        protected TContext Context { get { return _Context; } }

        public GenericRepository(TContext context)
        {
            ArgumentNullException.ThrowIfNull(context);

            DbSet = context.Set<TEntity>();
            _Context = context;
        }

        public TEntity? GetSingle(T id)
        {
            if (id.Equals(default)) throw new ArgumentNullException(nameof(id));

            return DbSet.SingleOrDefault(x => x.Id.Equals(id));
        }

        public TEntity? GetSingle(Expression<Func<TEntity, bool>> condition)
        {
            ArgumentNullException.ThrowIfNull(condition);

            return DbSet.SingleOrDefault(condition);
        }

        public IEnumerable<TEntity> Fetch(Expression<Func<TEntity, bool>>? condition = null)
        {
            return condition != null ? DbSet.Where(condition).AsEnumerable() : DbSet.AsEnumerable();
        }

        public void Add(TEntity entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            DbSet.Add(entity);
            Context.SaveChanges();
        }

        public void Update(TEntity entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            DbSet.Update(entity);
            Context.SaveChanges();
        }

        public void Delete(TEntity entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            DbSet.Remove(entity);
            Context.SaveChanges();
        }
    }
}
