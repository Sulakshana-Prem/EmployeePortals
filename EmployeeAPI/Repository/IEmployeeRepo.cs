using EmployeeAPI.Model;

public interface IEmployeeRepo
{
    Task<List<Employee>> GetAll(string Action);
    Task<Employee> GetByID(string id, string Action);

    Task<int> Create(Employee employee, string action);

    Task<int> Update(Employee employee,string action);

    Task<int> Delete(int id, string Action);

    Task<List<Designation>> GetAllRoles();
    Task<UserInfo> GetUser(string username, string password);
}