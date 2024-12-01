﻿using Microsoft.AspNetCore.Authorization.Infrastructure;
using OMGVA_PoS.Business_layer.Services.Security_and_Authorization;
using OMGVA_PoS.Data_layer.Context;
using OMGVA_PoS.Data_layer.DTOs;
using OMGVA_PoS.Data_layer.Enums;
using OMGVA_PoS.Data_layer.Models;

namespace OMGVA_PoS.Data_layer.Repositories.Business_Management
{
    public class BusinessRepository(OMGVADbContext database, IAuthenticationRepository authenticationRepository) : IBusinessRepository
    {
        private readonly OMGVADbContext _database = database;
        private readonly IAuthenticationRepository _authenticationRepository = authenticationRepository;
        public Business CreateBusiness(CreateBusinessRequest createBusinessRequest) {
            Business business = new() {
                StripeAccId = "/////////////////////////",//TODO: somehow acquire stripe acc id
                Name = createBusinessRequest.Name,
                Address = createBusinessRequest.Address,
                Phone = createBusinessRequest.Phone,
                Email = createBusinessRequest.Email,
            };

            _database.Businesses.Add(business);
            _database.SaveChanges();
            return business;
        }

        public bool UpdateBusiness(long businessId, Business business) {
            var businessToUpdate = _database.Businesses.SingleOrDefault(b => b.Id == businessId);
            if (businessToUpdate != null) {
                businessToUpdate.StripeAccId = business.StripeAccId ?? businessToUpdate.Name;
                businessToUpdate.Name = business.Name ?? businessToUpdate.Name;
                businessToUpdate.Address = business.Address ?? businessToUpdate.Address;
                businessToUpdate.Phone = business.Phone ?? businessToUpdate.Phone;
                businessToUpdate.Email = business.Email ?? businessToUpdate.Email;
                _database.SaveChanges();
                return true;
            }
            else {
                return false;
            }
        }

        public List<Business> GetBusinesses() {
            return [.. _database.Businesses];
        }

        public Business GetBusiness(long businessId) {
            return _database.Businesses.Where(b => b.Id == businessId).FirstOrDefault();
        }
    }
}
