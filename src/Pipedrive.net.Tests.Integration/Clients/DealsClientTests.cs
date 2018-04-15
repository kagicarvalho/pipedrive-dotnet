﻿using System.Threading.Tasks;
using Xunit;

namespace Pipedrive.Tests.Integration.Clients
{
    public class DealsClientTests
    {
        public class TheGetAllMethod
        {
            [IntegrationTest]
            public async Task ReturnsCorrectCountWithoutStart()
            {
                var pipedrive = Helper.GetAuthenticatedClient();

                var options = new DealFilters
                {
                    PageSize = 3,
                    PageCount = 1
                };

                var deals = await pipedrive.Deal.GetAll(options);
                Assert.Equal(3, deals.Count);
            }

            [IntegrationTest]
            public async Task ReturnsCorrectCountWithStart()
            {
                var pipedrive = Helper.GetAuthenticatedClient();

                var options = new DealFilters
                {
                    PageSize = 2,
                    PageCount = 1,
                    StartPage = 1
                };

                var deals = await pipedrive.Deal.GetAll(options);
                Assert.Equal(2, deals.Count);
            }

            [IntegrationTest]
            public async Task ReturnsDistinctInfosBasedOnStartPage()
            {
                var pipedrive = Helper.GetAuthenticatedClient();

                var startOptions = new DealFilters
                {
                    PageSize = 1,
                    PageCount = 1
                };

                var firstPage = await pipedrive.Deal.GetAll(startOptions);

                var skipStartOptions = new DealFilters
                {
                    PageSize = 1,
                    PageCount = 1,
                    StartPage = 1
                };

                var secondPage = await pipedrive.Deal.GetAll(skipStartOptions);

                Assert.NotEqual(firstPage[0].Id, secondPage[0].Id);
            }
        }

        public class TheCreateMethod
        {
            [IntegrationTest]
            public async Task CanCreate()
            {
                var pipedrive = Helper.GetAuthenticatedClient();
                var fixture = pipedrive.Deal;

                var newDeal = new NewDeal("title");

                var deal = await fixture.Create(newDeal);
                Assert.NotNull(deal);

                var retrieved = await fixture.Get(deal.Id);
                Assert.NotNull(retrieved);
            }
        }

        public class TheEditMethod
        {
            [IntegrationTest]
            public async Task CanEdit()
            {
                var pipedrive = Helper.GetAuthenticatedClient();
                var fixture = pipedrive.Deal;

                var newDeal = new NewDeal("new-title");
                var deal = await fixture.Create(newDeal);

                var editDeal = deal.ToUpdate();
                editDeal.Title = "updated-title";
                editDeal.Status = DealStatus.lost;

                var updatedDeal = await fixture.Edit(deal.Id, editDeal);

                Assert.Equal("updated-title", updatedDeal.Title);
                Assert.Equal(DealStatus.lost, updatedDeal.Status);
            }
        }

        public class TheDeleteMethod
        {
            [IntegrationTest]
            public async Task CanDelete()
            {
                var pipedrive = Helper.GetAuthenticatedClient();
                var fixture = pipedrive.Deal;

                var newDeal = new NewDeal("new-title");
                var deal = await fixture.Create(newDeal);

                var createdDeal = await fixture.Get(deal.Id);

                Assert.NotNull(createdDeal);

                await fixture.Delete(createdDeal.Id);

                var deletedDeal = await fixture.Get(createdDeal.Id);

                Assert.Equal(DealStatus.deleted, deletedDeal.Status);
            }
        }
    }
}