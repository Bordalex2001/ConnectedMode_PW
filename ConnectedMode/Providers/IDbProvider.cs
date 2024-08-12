using ConnectedMode.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConnectedMode.Providers
{
    internal interface IDbProvider
    {
        void Dispose();
        Task<List<Faculty>> GetFacultiesAsync();
        Task<List<Group>> GetGroupsAsync(int idFaculty);
        Task<List<Student>> GetStudentsAsync(int idGroup);
        Task<int> GetCountOfStudentsAsync(int idGroup);
    }
}
