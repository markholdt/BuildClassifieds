using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack;
using Classifieds.ServiceModel;
using ServiceStack.OrmLite;

namespace Classifieds.ServiceInterface
{
    public class CategoryServices : Service
    {
        public object Get(GetCategoryTreeView request)
        {

            using (IDbConnection db = DbFactory.Open())
            {
                var categories = db.Select<Category>().ToList();
                var treeView = new List<CategoryTreeView>();
                foreach (var c in categories)
                {
                    var tree = c.ConvertTo<CategoryTreeView>();
                    treeView.Add(tree);
                }
                // add subcategories
                {
                    foreach (var c in treeView)
                    {
                        if (c.SubCategories == null)
                            c.SubCategories = new List<Category>();

                        if (c.ParentCategoryId.HasValue)
                        {
                            var parnet = treeView.First(ar => ar.Id == c.ParentCategoryId.Value);
                            if (parnet.SubCategories == null)
                                parnet.SubCategories = new List<Category>();
                            parnet.SubCategories.Add(c);
                        }

                    }
                }

                return new GetCategoryTreeViewResponse { Result = treeView.Where(ar => ar.ParentCategoryId == null).ToList() };

            }
        }
        public object Get(GetCategories request)
        {

            using (IDbConnection db = DbFactory.Open())
            {
                var categories = db.Select<Category>().ToList();


                return new GetCategoriesResponse { Result = categories.ToList() };

            }
        }
    }
}
