using System;
using System.Threading.Tasks;
using MarketPlace.Domain.Services.Interfaces;
using MarketPlace.Domain.Entities;
using MarketPlace.Domain.ValueObjects;
using MarketPlace.Domain.ValueObjects.ClasifiedAd;
using MarketPlace.Domain.Interfaces;
using MarketPlace.Framework;

namespace Marketplace.Api.ApplicationServices
{
    public class ClassfiedAdApplicationService : Interfaces.IApplicationService
    {
        private readonly IClassifiedAdRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private IcurrencyLookup _currencyLookup;
        public ClassfiedAdApplicationService(IClassifiedAdRepository repository, IUnitOfWork unitOfWork, IcurrencyLookup currencyLookup)
        {
            _currencyLookup = currencyLookup;
            _unitOfWork = unitOfWork;
            _repository = repository;
        }
        public Task Handle(object command) =>
            command switch
            {
                Contracts.ClassifiedAds.V1.Create cmd => HandleCreate(cmd),
                Contracts.ClassifiedAds.V1.SetTitle cmd => HandleUpdate(cmd.Id, c => c.SetTitle(ClassfiedAdTitle.FromString(cmd.Title))),
                Contracts.ClassifiedAds.V1.UpdateText cmd => HandleUpdate(cmd.Id, c => c.UpdateText(ClassfiedAdText.FromString(cmd.Text))),
                Contracts.ClassifiedAds.V1.UpdatePrice cmd => HandleUpdate(cmd.Id, c => c.UpdatePrice(Price.FromDecimal(cmd.Price, cmd.Currency, _currencyLookup))),
                Contracts.ClassifiedAds.V1.RequestToPublish cmd => HandleUpdate(cmd.Id, c => c.RequestToPublish()),
                _ => Task.CompletedTask,
            };
        public async Task HandleCreate(Contracts.ClassifiedAds.V1.Create cmd)
        {
            if (await _repository.Exists(cmd.Id.ToString()))
            {
                throw new InvalidOperationException($"Entity with id {cmd.Id} already exists");
            }
            var classfiedAd = new ClassfiedAd(
                new ClassfiedAdId(cmd.Id),
                new UserId(cmd.OwnerId)
            );

            await _repository.Add(classfiedAd);
            await _unitOfWork.Commit();
        }
        public async Task HandleUpdate(Guid classifiedAdId, Action<ClassfiedAd> operation)
        {
            var classfiedAd = await _repository.Load(classifiedAdId.ToString());
            if (classfiedAd == null)
            {
                throw new InvalidOperationException($"Entity with id {classifiedAdId} cannot be found");
            }
            operation(classfiedAd);

            await _unitOfWork.Commit();
        }
    }
}
