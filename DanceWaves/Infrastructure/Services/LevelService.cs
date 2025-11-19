using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DanceWaves.Application.Ports;
using DanceWaves.Models;
using DanceWaves.Data;
using Microsoft.EntityFrameworkCore;

namespace DanceWaves.Infrastructure.Services
{
    public class LevelService : ILevelService
    {
        private readonly ApplicationDbContext _dbContext;

        public LevelService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Level>> GetAllLevelsAsync()
        {
            return await _dbContext.Levels.ToListAsync();
        }

        public async Task CreateLevelAsync(Level level)
        {
            await _dbContext.Levels.AddAsync(level);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateLevelAsync(Level level)
        {
            var existingLevel = await _dbContext.Levels.FindAsync(level.Id);
            if (existingLevel == null)
            {
                throw new Exception("Level not found");
            }
            existingLevel.Code = level.Code;
            existingLevel.Name = level.Name;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteLevelAsync(int levelId)
        {
            var level = await _dbContext.Levels.FindAsync(levelId);
            if (level == null)
            {
                throw new Exception("Level not found");
            }
            _dbContext.Levels.Remove(level);
            await _dbContext.SaveChangesAsync();
        }
    }
}