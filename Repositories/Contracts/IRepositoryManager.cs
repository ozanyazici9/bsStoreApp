using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repositories.Contracts;

public interface IRepositoryManager
{
    IBookRepository Book { get; }
    void Save();
}
