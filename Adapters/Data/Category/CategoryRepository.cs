﻿using Entities = Domain.Entities;

using Domain.Category.Ports;
using Microsoft.EntityFrameworkCore;

namespace Data.Category
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataDbContext _dbContext;

        public CategoryRepository(DataDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Entities.Category> CreateCategory(Entities.Category category)
        {
            _dbContext.Categories.Add(category);
            await _dbContext.SaveChangesAsync();
            return category;
        }

        public async Task<Entities.Category> Get(int id)
        {
            var category = await _dbContext.Categories.FindAsync(id);
            return category;
        }

        public async Task<List<Entities.Category>> List()
        {
            var categories = await _dbContext.Categories.ToListAsync();
            return categories;
        }
    }
}
