using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack;
using Classifieds.ServiceModel;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace Classifieds.ServiceInterface
{
    public class ListingServices : Service
    {
        public object Get(GetListings request)
        {
          
            List<Listing> listings = new List<Listing>();
            int pageSize = 10;
            int pageNr = request.PageNr ?? 1;

            int totalRecords = 0;
            using (IDbConnection db = DbFactory.Open())
            {
                listings = db.Select<Listing>().Where(ar => 
                    (ar.CategoryId == request.CategoryId || request.CategoryId == null) &&
                    (ar.Id == request.Id || request.Id == null) &&
                    (ar.Price >= request.PriceFrom || request.PriceFrom == null) &&
                    (ar.Price <= request.PriceTo || request.PriceTo == null))
                    .OrderByDescending(ar => ar.LastModified).Skip((pageNr -1) * pageSize).Take(pageSize).ToList();
                totalRecords = db.Select<Listing>().Count;
            }

            // add categories
            var categoryService = this.ResolveService<CategoryServices>();
            var categores = ((GetCategoriesResponse)categoryService.Get(new GetCategories())).Result;
            foreach (var l in listings)
                l.Category = categores.First(ar => ar.Id == l.CategoryId);
            

            //build page hierarchy
            var pageHierarchy = new List<PageHierarchy>();
            int rank = 0;

            if (request.CategoryId != null)
            {
               var level = categores.First(ar => ar.Id == request.CategoryId);
                pageHierarchy.Add(new PageHierarchy(rank, new Link { DisplayName = level.Name, Type = "Breadcrumb", Url = (new GetListings { CategoryId = level.Id }).ToGetUrl() }));
                bool hasParent = level.ParentCategoryId.HasValue;
                while (hasParent)
                {
                    rank++;
                    level = categores.First(ar => ar.Id == level.ParentCategoryId);
                    pageHierarchy.Add(new PageHierarchy(rank, new Link {DisplayName = level.Name, Type = "Breadcrumb", Url = (new GetListings {CategoryId = level.Id}).ToGetUrl()}));
                    hasParent = level.ParentCategoryId.HasValue;
                }
            }
            if (request.Id != null)
                pageHierarchy.Add(new PageHierarchy(rank, new Link { DisplayName = listings.First().Title, Type = "Breadcrumb", Url = "" }));


            var response = new GetListingsResponse();
            response.PageHierarchy = pageHierarchy.OrderByDescending(ar => ar.Rank).ToList();
            response.Result = listings.ToList();
            response.TotalPageCount = totalRecords/pageSize;
            response.CurrentPageNr = pageNr;
            response.TotalRecords = totalRecords;
            return response;
        }
        public object Get(GetListing request)
        {
            var listing = new Listing();

            using (IDbConnection db = DbFactory.Open())
            {
                listing = db.Select<Listing>().Where(ar =>
                    (ar.Id == request.Id || request.Id == null)).First();
            }

            // add categories
            var categoryService = this.ResolveService<CategoryServices>();
            var categores = ((GetCategoriesResponse)categoryService.Get(new GetCategories())).Result;
            listing.Category = categores.First(ar => ar.Id == listing.CategoryId);
            

            //build page hierarchy
            var pageHierarchy = new List<PageHierarchy>();
            int rank = 0;

            //if (request.CategoryId != null)
            {
                var level = categores.First(ar => ar.Id == listing.CategoryId);
                pageHierarchy.Add(new PageHierarchy(rank, new Link { DisplayName = level.Name, Type = "Breadcrumb", Url = (new GetListings { CategoryId = level.Id }).ToGetUrl() }));
                bool hasParent = level.ParentCategoryId.HasValue;
                while (hasParent)
                {
                    rank++;
                    level = categores.First(ar => ar.Id == level.ParentCategoryId);
                    pageHierarchy.Add(new PageHierarchy(rank, new Link { DisplayName = level.Name, Type = "Breadcrumb", Url = (new GetListings { CategoryId = level.Id }).ToGetUrl() }));
                    hasParent = level.ParentCategoryId.HasValue;
                }
            }
            if (request.Id != null)
                pageHierarchy.Add(new PageHierarchy(rank, new Link { DisplayName = listing.Title, Type = "Breadcrumb", Url = "" }));



            return new GetListingResponse { PageHierarchy = pageHierarchy.OrderByDescending(ar => ar.Rank).ToList(), Result = listing };
        }

        public object Post(CreateListing request)
        {
            Listing newAd = new Listing();
            newAd = request.ConvertTo<Listing>();
            using (IDbConnection db = DbFactory.Open())
            {
                var imageUrls = request.Files.Split(';');
                foreach (var url in imageUrls)
                {if (url!= "")
                        newAd.ImageUrls.Add(new ImageModel {IsPrimary = false, Name = url, Url = url});
                }
                db.Save(newAd);
            }
            return new CreateListingResponse { Result = true };
        }
    }
}
