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
        public void find_employees_age_lower_25_get_role_and_nametake()
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

        [TestMethod]
        public void take_top_2_employees()
        {
            var employees = RepositoryFactory.GetEmployees();
            var actual = employees.FindResultWithIndex((x, index) => index < 2);

            var expected = new List<Employee>()
            {
                new Employee{Name="Joe", Role=RoleType.Engineer, MonthSalary=100, Age=44, WorkingYear=2.6 } ,
                new Employee{Name="Tom", Role=RoleType.Engineer, MonthSalary=140, Age=33, WorkingYear=2.6} ,
            };

            expected.ToExpectedObject().ShouldEqual(actual.ToList());
        }

        [TestMethod]
        public void take_top_2_employees_new()
        {
            IEnumerable<Employee> employees = RepositoryFactory.GetEmployees();
            var actual = employees.TakeByIndex(index => index < 2);

            var expected = new List<Employee>()
            {
                new Employee{Name="Joe", Role=RoleType.Engineer, MonthSalary=100, Age=44, WorkingYear=2.6 } ,
                new Employee{Name="Tom", Role=RoleType.Engineer, MonthSalary=140, Age=33, WorkingYear=2.6} ,
            };

            expected.ToExpectedObject().ShouldEqual(actual.ToList());
        }

        [TestMethod]
        public void skip_top_6_employees()
        {
            IEnumerable<Employee> employees = RepositoryFactory.GetEmployees();
            var actual = employees.SkipByIndex(5);

            var expected = new List<Employee>()
            {
                new Employee{Name="Frank", Role=RoleType.Engineer, MonthSalary=120, Age=16, WorkingYear=2.6} ,
                new Employee{Name="Joey", Role=RoleType.Engineer, MonthSalary=250, Age=40, WorkingYear=2.6}
            };

            expected.ToExpectedObject().ShouldEqual(actual.ToList());
        }

        [TestMethod]
        public void group_every_3_employee_sum_salary()
        {
            var employees = RepositoryFactory.GetEmployees();
            var actual = YourOwnLinq.CalSalary(employees, 3, x => x.MonthSalary);

            var expected = new List<int>()
             {
                 620,
                 540,
                 370
             };

            expected.ToExpectedObject().ShouldEqual(actual.ToList());
        }

        [TestMethod]
        public void cal_salary()
        {
            var employees = RepositoryFactory.GetEmployees();
            var actual = YourOwnLinq.GetSumSalary(employees, 3, x => x.MonthSalary);
            var expected = new List<int>()
            {
                620,
                540,
                370
            };

            expected.ToExpectedObject().ShouldEqual(actual.ToList());
        }

        [TestMethod]
        public void take_top_2_employee_salary_over_150()
        {
            var employees = RepositoryFactory.GetEmployees();
            var actual =
                YourOwnLinq.GetSalaryOverSettingByNumber(employees, employee => employee.MonthSalary > 150, 2);
            var expected = new List<Employee>()
            {
                new Employee{Name="Kevin", Role=RoleType.Manager, MonthSalary=380, Age=55, WorkingYear=2.6} ,
                new Employee{Name="Bas", Role=RoleType.Engineer, MonthSalary=280, Age=36, WorkingYear=2.6}
            };

            expected.ToExpectedObject().ShouldEqual(actual.ToList());
        }

        [TestMethod]
        public void skip_3_while_salary_lower_150()
        {
            var employee = RepositoryFactory.GetEmployees();
            var actual = YourOwnLinq.SkipWhile(employee, 3, e => e.MonthSalary < 150);

            var expected = new List<Employee>()
            {
                new Employee{Name="Kevin", Role=RoleType.Manager, MonthSalary=380, Age=55, WorkingYear=2.6} ,
                new Employee{Name="Bas", Role=RoleType.Engineer, MonthSalary=280, Age=36, WorkingYear=2.6} ,
                new Employee{Name="Mary", Role=RoleType.OP, MonthSalary=180, Age=26, WorkingYear=2.6} ,
                new Employee{Name="Frank", Role=RoleType.Engineer, MonthSalary=120, Age=16, WorkingYear=2.6} ,
                new Employee{Name="Joey", Role=RoleType.Engineer, MonthSalary=250, Age=40, WorkingYear=2.6},
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

    public static IEnumerable<Employee> TakeByIndex(this IEnumerable<Employee> employees, Func<int, bool> selector)
    {
        var emp = employees.GetEnumerator();
        var index = 0;
        while (emp.MoveNext())
        {
            if (selector(index))
            {
                yield return emp.Current;
            }
            else
            {
                yield break;
            }
            index++;
        }
    }

    public static IEnumerable<Employee> SkipByIndex(this IEnumerable<Employee> employees, int skipNumber)
    {
        var emp = employees.GetEnumerator();
        var index = 0;
        while (emp.MoveNext())
        {
            if (skipNumber < index)
            {
                yield return emp.Current;
            }

            index++;
        }
    }

    public static IEnumerable<int> CalSalary(IEnumerable<Employee> employees, int groupCount, Func<Employee, int> func)
    {
        var sum = 0;
        var index = 1;
        foreach (var employee in employees)
        {
            sum += func(employee);
            if (index % groupCount == 0 && index != 0)
            {
                yield return sum;
                sum = 0;
            }
            index++;
        }
        if (index >= employees.Count())
        {
            yield return sum;
        }
    }

    public static IEnumerable<int> GetSumSalary(IEnumerable<Employee> employees, int groupCount, Func<Employee, int> func)
    {
        var idx = 0;
        while (idx < employees.Count())
        {
            yield return employees.Skip(idx).Take(groupCount).Sum(func);
            idx += groupCount;
        }
    }

    public static IEnumerable<Employee> GetSalaryOverSettingByNumber(IEnumerable<Employee> employees, Func<Employee, bool> func, int takeCount)
    {
        var idx = 0;

        foreach (var employee in employees)
        {
            if (func(employee))
            {
                yield return employee;
                idx++;
            }
            if (idx == takeCount)
            {
                yield break;
            }
        }
    }

    public static IEnumerable<Employee> SkipWhile(IEnumerable<Employee> employee, int count, Func<Employee, bool> func)
    {
        var idx = 0;
        var emp = employee.GetEnumerator();

        while (emp.MoveNext())
        {
            if (func(emp.Current) && idx < count)
            {
                idx++;
            }
            else
            {
                yield return emp.Current;
            }
        }
    }
}

//Func<Product, bool> predicate
//-----> predicate(object)---->will return bool value

//Action
//-----> void xxxxx()

//===================================
//extension method
//1.static class
//    2.static function
//3.針對特定型別讓它有擴充方法可以用 要加上this 並擺在第一個參數
//4.namespace
//===================================