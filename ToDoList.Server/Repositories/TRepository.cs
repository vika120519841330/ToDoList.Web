﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ToDoList.Interfaces;
using ToDoList.Server.Data.Context;
using ToDoList.Server.Helpers;

namespace ToDoList.Repositories;
public class TRepository<T> : ITRepository<T>
    where T : class
{
    private readonly IDbContextFactory<AppDbContext> contextFactory;

    public TRepository(IDbContextFactory<AppDbContext> contextFactory) => this.contextFactory = contextFactory;

    public string Message { get; set; } = string.Empty;

    public async Task<List<T>> GetAsync(CancellationToken token = default)
        => await (await contextFactory.CreateDbContextAsync(token)).Set<T>().AsNoTracking().ToListAsync(token);

    public async Task<T> GetAsync(int id, CancellationToken token = default)
        => await (await contextFactory.CreateDbContextAsync(token)).Set<T>().FindAsync(new object[] { id }, token);

    public async Task<T> CreateAsync(T entity, CancellationToken token = default)
    {
        T result;
        var context = await contextFactory.CreateDbContextAsync(token);
        try
        {
            result = (await context.AddAsync(entity, token)).Entity;
            await context.SaveChangesAsync(token);
            Message = $"Операция успешно завершена. Данные добавлены в систему. ";
        }
        catch (Exception exc)
        {
            exc.LogError(GetType().Name, nameof(CreateAsync));
            Message = $"Запись не добавлена в систему, произошла ошибка на уровне базы данных ! " +
                      $"Подробности: {exc.GetExeceptionMessages()} ! ";
            throw;
        }

        return result;
    }

    public async  Task<T> UpdateAsync(T entity, CancellationToken token = default)
    {
        T result; 
        var context = await contextFactory.CreateDbContextAsync(token);
        try
        {
            result = context.Update(entity).Entity;
            await context.SaveChangesAsync(token);
            Message = $"Операция успешно завершена. Данные обновлены в системе. ";
        }
        catch (Exception exc)
        {
            exc.LogError(GetType().Name, nameof(UpdateAsync));
            Message = $"Запись не обновлена в системе, произошла ошибка на уровне базы данных ! " +
                      $"{exc.GetExeceptionMessages()} ! ";
            throw;
        }

        return result;
    }

    public async Task<bool> RemoveAsync(int id, CancellationToken token = default)
    {
        var result = false;
        var context = await contextFactory.CreateDbContextAsync(token);

        try
        {
            var item = await context.Set<T>().FindAsync(new object[] { id }, token);
            if (item == null)
            {
                Message = $"Запись c идентификатором «{id}» отсутствует в системе. ";
                return false;
            }

            var removedItemsCount = context.Remove(item);
            await context.SaveChangesAsync(token);
            result = true;
        }
        catch (Exception exc)
        {
            exc.LogError(GetType().FullName, nameof(RemoveAsync));
            Message = $"Запись не удалена из системы, произошла ошибка на уровне базы данных ! " +
                      $"{exc.GetExeceptionMessages()} ! ";
            throw;
        }

        return result;
    }
}
