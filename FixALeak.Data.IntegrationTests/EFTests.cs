using System;
using System.Collections.Generic;
using System.Data.Entity;
using FixALeak.Data.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FixALeak.Data.IntegrationTests
{
    [TestClass]
    public class EFTests
    {
        ExpenseContext ctx;
        /*[TestInitialize]
       public void SetUp()
       {
           ctx = new ExpenseContext();
       }*/

        [TestMethod]
        public void TestUpdateCat()
        {
            int id;
            using (ctx = new ExpenseContext())
            {
                Category cat = new Category()
                {
                    Name = "cat x",
                    SubCategories = new HashSet<CategoryLeaf>() {
                    new CategoryLeaf() {
                        Name = "catleaf x1"
                    },
                    new CategoryLeaf() {
                        Name = "catleaf x2"
                    }
                }
                };
                ctx.Categories.Add(cat);
                ctx.SaveChanges();
                id = cat.ID;
            }

            using (ctx = new ExpenseContext())
            {
                Category cat = new Category()
                {
                    ID = id,
                    Name = "cat x2",

                };

                ctx.Entry(cat).State = EntityState.Modified;
                
                ctx.SaveChanges();
            }
        }

        /*[TestCleanup]
        public void TearDown()
        {
            ctx.Dispose();
        }*/
    }
}
