using NSubstitute.Routing.Handlers;

namespace LinqTests
{
    internal class Employee
    {
        public string Name { get; set; }
        public RoleType Role { get; set; }
        public int MonthSalary { get; set; }
        public int Age { get; set; }
        public double WorkingYear { get; set; }
    }
}