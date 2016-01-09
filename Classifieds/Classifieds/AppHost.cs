using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using Funq;
using ServiceStack;
using ServiceStack.Razor;
using Classifieds.ServiceInterface;
using Classifieds.ServiceModel;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace Classifieds
{
    public class AppHost : AppHostBase
    {
        /// <summary>
        /// Default constructor.
        /// Base constructor requires a name and assembly to locate web service classes. 
        /// </summary>
        public AppHost()
            : base("Classifieds", typeof(CategoryServices).Assembly)
        {

        }

        /// <summary>
        /// Application specific configuration
        /// This method should initialize any IoC resources utilized by your web service classes.
        /// </summary>
        /// <param name="container"></param>
        public override void Configure(Container container)
        {
            //Config examples
            //this.Plugins.Add(new PostmanFeature());
            //this.Plugins.Add(new CorsFeature());
            this.Plugins.Add(new RazorFormat());

            // register Db
            var dbFactory = new OrmLiteConnectionFactory(
                ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString,
                PostgreSqlDialect.Provider);

            container.Register<IDbConnectionFactory>(c => dbFactory);

            using (IDbConnection db = container.Resolve<IDbConnectionFactory>().Open())
            {
                db.DropAndCreateTable<Listing>();
                db.DropAndCreateTable<Category>();

                List<Category> categories = new List<Category>();
                categories.Add(new Category { Id = 1, Name = "Houses and flats" });
                categories.Add(new Category { Id = 2, Name = "Houses for sale", ParentCategoryId = 1 });
                categories.Add(new Category { Id = 3, Name = "Flats for sale", ParentCategoryId = 1 });
                categories.Add(new Category { Id = 4, Name = "Houses for rent", ParentCategoryId = 1 });
                categories.Add(new Category { Id = 5, Name = "Flats for rent", ParentCategoryId = 1 });

                categories.Add(new Category { Id = 6, Name = "Vehicles" });
                categories.Add(new Category { ParentCategoryId = 6, Name = "Cars for sale" });
                categories.Add(new Category { ParentCategoryId = 6, Name = "Motorbikes for sale" });
                categories.Add(new Category { ParentCategoryId = 6, Name = "Parts & Accessories" });


                categories.Add(new Category { Id = 7, Name = "Electronics" });
                categories.Add(new Category { ParentCategoryId = 7, Name = "Phones & Tablets" });
                categories.Add(new Category { ParentCategoryId = 7, Name = "Computers" });
                categories.Add(new Category { ParentCategoryId = 7, Name = "Cameras" });
                categories.Add(new Category { ParentCategoryId = 7, Name = "Video games & Consoles" });
                categories.Add(new Category { ParentCategoryId = 7, Name = "TVs, DVDs" });
                categories.Add(new Category { ParentCategoryId = 7, Name = "Music equipment" });
                categories.Add(new Category { ParentCategoryId = 7, Name = "Other" });

                categories.Add(new Category { Id = 8, Name = "Jobs" });

              

                db.SaveAll(categories);

                Listing l = new Listing();
                l.CategoryId = 2;
                l.Description = "asdasdas";
                l.Price = 100;
                l.City = "Windhoek";
                l.EmailAddress = "test@test.com";
                l.Title = "Some title here - very long";
                db.Save(l);

                Listing l2 = new Listing();
                l2.CategoryId = 2;
                l2.Description = "House in Windhieok for sale";
                l2.Price = 10000000;
                l2.City = "Windhoek";
                l2.EmailAddress = "test@test.com";
                l2.Title = "Some title here - very long indeed goes on and on";
                db.Save(l2);
            }

        }
    }
}