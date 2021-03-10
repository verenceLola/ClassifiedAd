using System;
using System.Threading.Tasks;
using MarketPlace.Domain.Interfaces;
using MarketPlace.Domain.Entities;
using MarketPlace.Domain.ValueObjects;

namespace Marketplace.Infrastructure
{
    public class ClassifiedAdRepository : IClassifiedAdRepository
    {
        private readonly ClassifiedAdDbContext _dbContext;
        public ClassifiedAdRepository(ClassifiedAdDbContext dbContext) =>
            _dbContext = dbContext;

        public async Task Add(ClassfiedAd entity) =>
             await _dbContext.ClassfiedAds.AddAsync(entity);
        public async Task<bool> Exists(ClassfiedAdId id) =>
             await _dbContext.ClassfiedAds.FindAsync(id.Value) != null;
        public async Task<ClassfiedAd> Load(ClassfiedAdId id) =>
            await _dbContext.ClassfiedAds.FindAsync(id.Value);
    }
}
