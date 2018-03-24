using ExpectedObjects;
using LinqTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LinqTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void find_products_that_price_between_200_and_500()
        {
            var products = RepositoryFactory.GetProducts();
            var actual = WithoutLinq.FindProductByPrice(products, 200, 500);

            var expected = new List<Product>()
            {
                new Product{Id=2, Cost=21, Price=210, Supplier="Yahoo" },
                new Product{Id=3, Cost=31, Price=310, Supplier="Odd-e" },
                new Product{Id=4, Cost=41, Price=410, Supplier="Odd-e" },
            };

            expected.ToExpectedObject().ShouldEqual(actual.ToList());
        }

        [TestMethod]
        public void find_products_that_price_between_200_and_500_linq()
        {
            var products = RepositoryFactory.GetProducts();
            var actual = products.MyWhere(x => x.Price > 200 && x.Price < 500);

            var expected = new List<Product>()
            {
                new Product{Id=2, Cost=21, Price=210, Supplier="Yahoo" },
                new Product{Id=3, Cost=31, Price=310, Supplier="Odd-e" },
                new Product{Id=4, Cost=41, Price=410, Supplier="Odd-e" },
            };

            expected.ToExpectedObject().ShouldEqual(actual.ToList());
        }

        [TestMethod]
        public void find_products_that_price_between_200_and_500_and_cost_than_30()
        {
            var products = RepositoryFactory.GetProducts();
            var actual = products.MyWhere(x => x.Price > 200 && x.Price < 500 && x.Cost > 30);

            var expected = new List<Product>()
            {
                new Product{Id=3, Cost=31, Price=310, Supplier="Odd-e" },
                new Product{Id=4, Cost=41, Price=410, Supplier="Odd-e" },
            };

            expected.ToExpectedObject().ShouldEqual(actual.ToList());
        }

        [TestMethod]
        public void find_products_by_extension()
        {
            var products = RepositoryFactory.GetProducts();
            var actual = products.FindResult(x => x.FilterProductCondition());

            var expected = new List<Product>()
            {
                new Product{Id=3, Cost=31, Price=310, Supplier="Odd-e" },
                new Product{Id=4, Cost=41, Price=410, Supplier="Odd-e" },
            };

            expected.ToExpectedObject().ShouldEqual(actual.ToList());
        }

        [TestMethod]
        public void find_employee_by_extension()
        {
            var employees = RepositoryFactory.GetEmployees();
            var actual = employees.FindResult(x => x.Age > 30);

            var expected = new List<Employee>()
            {
                new Employee{Name="Joe", Role=RoleType.Engineer, MonthSalary=100, Age=44, WorkingYear=2.6 } ,
                new Employee{Name="Tom", Role=RoleType.Engineer, MonthSalary=140, Age=33, WorkingYear=2.6} ,
                new Employee{Name="Kevin", Role=RoleType.Manager, MonthSalary=380, Age=55, WorkingYear=2.6} ,
                new Employee{Name="Bas", Role=RoleType.Engineer, MonthSalary=280, Age=36, WorkingYear=2.6} ,
                new Employee{Name="Joey", Role=RoleType.Engineer, MonthSalary=250, Age=40, WorkingYear=2.6},
            };

            expected.ToExpectedObject().ShouldEqual(actual.ToList());
        }

        [TestMethod]
        public void find_employee_by_extension_2()
        {
            var employees = RepositoryFactory.GetEmployees();
            var actual = employees.FindResultWithIndex((x, index) => x.Age > 30 && index > 1);

            var expected = new List<Employee>()
            {
                new Employee{Name="Kevin", Role=RoleType.Manager, MonthSalary=380, Age=55, WorkingYear=2.6} ,
                new Employee{Name="Bas", Role=RoleType.Engineer, MonthSalary=280, Age=36, WorkingYear=2.6} ,
                new Employee{Name="Joey", Role=RoleType.Engineer, MonthSalary=250, Age=40, WorkingYear=2.6},
            };

            expected.ToExpectedObject().ShouldEqual(actual.ToList());
        }

        [TestMethod]
        public void replace_url_http_to_https()
        {
            var urls = RepositoryFactory.GetUrls();
            var actual = urls.ReplaceUrl();

            var expected = new List<string>()
            {
                "https://tw.yahoo.com",
                "https://facebook.com",
                "https://twitter.com",
                "https://github.com",
            };

            expected.ToExpectedObject().ShouldEqual(actual.ToList());
        }

        [TestMethod]
        public void replace_url_http_to_https_by_length()
        {
            var urls = RepositoryFactory.GetUrls();
            var actual = urls.MySelect(url => url.Length);

            var expected = new List<int>()
            {
                19,
                20,
                19,
                17,
            };

            expected.ToExpectedObject().ShouldEqual(actual.ToList());
        }

        [TestMethod]
        public void replace_url_http_to_https_linq()
        {
            var urls = RepositoryFactory.GetUrls();
            var actual = urls.Select(x => x.Replace("http:", "https:"));

            var expected = new List<string>()
            {
                "https://tw.yahoo.com",
                "https://facebook.com",
                "https://twitter.com",
                "https://github.com",
            };

            expected.ToExpectedObject().ShouldEqual(actual.ToList());
        }

        [TestMethod]
        public void find_employees_age_lower_25_get_role_and_name()
        {
            var employees = RepositoryFactory.GetEmployees();
            var actual = employees
                .MyWhere(x => x.Age < 25)
                .MySelect(y => $"{y.Role}:{y.Name}");

            foreach (var item in actual)
            {
                Console.WriteLine(item);
            }

            var expected = new List<string>()
            {
                "OP:Andy",
                "Engineer:Frank"
            };

            expected.ToExpectedObject().ShouldEqual(actual.ToList());
        }
    }
}

internal static class WithoutLinq
{
    public static IEnumerable<Product> FindProductByPrice(IEnumerable<Product> products, int lowBoundary, int highBoundary)
    {
        foreach (var product in products)
        {
            if (product.Price > lowBoundary && product.Price < highBoundary)
            {
                yield return product;
            }
        }
    }

    public static IEnumerable<T> FindResult<T>(this IEnumerable<T> resource, Func<T, bool> predicate)
    {
        return resource.MyWhere(product => predicate(product));
    }

    public static IEnumerable<T> FindResultWithIndex<T>(this IEnumerable<T> resources, Func<T, int, bool> predicate)
    {
        return resources.Where((product, index) => predicate(product, index));
    }
}

internal static class YourOwnLinq
{
    public static IEnumerable<string> ReplaceUrl(this IEnumerable<string> urls)
    {
        foreach (var url in urls)
        {
            yield return url.Replace("http:", "https:");
        }
    }

    public static IEnumerable<T> MyWhere<T>(this IEnumerable<T> resources, Predicate<T> predicate)
    {
        var index = 0;
        foreach (var item in resources)
        {
            if (predicate(item))
            {
                yield return item;
            }
            index++;
        }
    }

    public static IEnumerable<TResult> MySelect<TSource, TResult>(this IEnumerable<TSource> sources, Func<TSource, TResult> selector)
    {
        foreach (var item in sources)
        {
            yield return selector(item);
        }
    }
}